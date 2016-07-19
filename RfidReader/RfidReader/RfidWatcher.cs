using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RfidReader
{
    public class RfidWatcher : NativeWindow
    {
        #region Fields

        private static RfidWatcher _rfidWatcher;

        private readonly Dictionary<IntPtr, KeyPressEvent> _deviceList = new Dictionary<IntPtr, KeyPressEvent>();

        private static InputData _rawBuffer;

        #endregion

        #region Constructor

        private RfidWatcher()
        {
            EnumerateDevices();
        }

        #endregion

        #region Public methods

        public static void Start()
        {
            if (_rfidWatcher == null)
            {
                _rfidWatcher = new RfidWatcher();
            }
        }

        #endregion

        #region Private methods

        private void EnumerateDevices()
        {
            _deviceList.Clear();
            uint deviceCount = 0;
            var keyboardNumber = 0;
            var dwSize = (Marshal.SizeOf(typeof(Rawinputdevicelist)));
            if (Win32.GetRawInputDeviceList(IntPtr.Zero, ref deviceCount, (uint)dwSize) == 0)
            {
                var pRawInputDeviceList = Marshal.AllocHGlobal((int)(dwSize * deviceCount));
                Win32.GetRawInputDeviceList(pRawInputDeviceList, ref deviceCount, (uint)dwSize);

                for (var i = 0; i < deviceCount; i++)
                {
                    uint pcbSize = 0;
                    // On Window 8 64bit when compiling against .Net > 3.5 using .ToInt32 you will generate an arithmetic overflow. Leave as it is for 32bit/64bit applications
                    var rid = (Rawinputdevicelist)Marshal.PtrToStructure(new IntPtr((pRawInputDeviceList.ToInt64() + (dwSize * i))), typeof(Rawinputdevicelist));
                    Win32.GetRawInputDeviceInfo(rid.hDevice, RawInputDeviceInfo.RIDI_DEVICENAME, IntPtr.Zero, ref pcbSize);
                    if (pcbSize <= 0)
                    {
                        continue;
                    }

                    var pData = Marshal.AllocHGlobal((int)pcbSize);
                    Win32.GetRawInputDeviceInfo(rid.hDevice, RawInputDeviceInfo.RIDI_DEVICENAME, pData, ref pcbSize);
                    var deviceName = Marshal.PtrToStringAnsi(pData);
                    if (rid.dwType == DeviceType.RimTypekeyboard || rid.dwType == DeviceType.RimTypeHid)
                    {
                        var deviceDesc = Win32.GetDeviceDescription(deviceName);
                        var dInfo = new KeyPressEvent
                        {
                            DeviceName = Marshal.PtrToStringAnsi(pData),
                            DeviceHandle = rid.hDevice,
                            DeviceType = Win32.GetDeviceType(rid.dwType),
                            Name = deviceDesc,
                            Source = keyboardNumber++.ToString(CultureInfo.InvariantCulture)
                        };

                        if (!_deviceList.ContainsKey(rid.hDevice))
                        {
                            _deviceList.Add(rid.hDevice, dInfo);
                        }
                    }
                }
            }
        }
        
        private void ProcessRawInput(IntPtr hdevice)
        {
            if (_deviceList.Count == 0)
            {
                return;
            }

            var dwSize = 0;
            Win32.GetRawInputData(hdevice, DataCommand.RID_INPUT, IntPtr.Zero, ref dwSize, Marshal.SizeOf(typeof(Rawinputheader)));

            if (dwSize != Win32.GetRawInputData(hdevice, DataCommand.RID_INPUT, out _rawBuffer, ref dwSize, Marshal.SizeOf(typeof(Rawinputheader))))
            {
                return;
            }

            int virtualKey = _rawBuffer.data.keyboard.VKey;
            int makeCode = _rawBuffer.data.keyboard.Makecode;
            int flags = _rawBuffer.data.keyboard.Flags;

            if (virtualKey == Win32.KEYBOARD_OVERRUN_MAKE_CODE) return;

            var isE0BitSet = ((flags & Win32.RI_KEY_E0) != 0);

            KeyPressEvent keyPressEvent;

            if (_deviceList.ContainsKey(_rawBuffer.header.hDevice))
            {
                keyPressEvent = _deviceList[_rawBuffer.header.hDevice];
            }
            else
            {
                return;
            }

            var isBreakBitSet = ((flags & Win32.RI_KEY_BREAK) != 0);

            keyPressEvent.KeyPressState = isBreakBitSet ? "BREAK" : "MAKE";
            keyPressEvent.Message = _rawBuffer.data.keyboard.Message;
            // keyPressEvent.VKeyName = KeyMapper.GetKeyName(VirtualKeyCorrection(virtualKey, isE0BitSet, makeCode)).ToUpper();
            keyPressEvent.VKey = virtualKey;
        }

        #endregion

        #region Protected methods

        protected override void WndProc(ref Message message)
        {
            switch (message.Msg)
            {
                case Win32.WM_INPUT:
                {
                    ProcessRawInput(message.LParam);
                }
                break;
                case Win32.WM_USB_DEVICECHANGE:
                {
                    EnumerateDevices();
                }
                break;
            }

            base.WndProc(ref message);
        }

        #endregion
    }
}
