using ICSharpCode.USBlib;
using LibUsbDotNet;
using LibUsbDotNet.DeviceNotify;
using LibUsbDotNet.Main;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Management;
using System.Text;

namespace RfidReader
{
    class Program
    {
        public static UsbDevice MyUsbDevice;

        private static UsbDeviceFinder usbFinder = new UsbDeviceFinder(0x08FF, 0x0009);

        public static IDeviceNotifier UsbDeviceNotifier = DeviceNotifier.OpenDeviceNotifier();

        static void Main(string[] args)
        {
            ConnectToRfid();
            // GetUSBDevices();
            // UseSharpUsb();
            // WatchComPorts();
            Console.ReadLine();
        }

        private static void UseSharpUsb() 
        {
            try
            {
                var busses = Bus.Busses;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void ConnectToRfid()
        {
            var devices = UsbDevice.AllDevices;
            var usbDevice = UsbDevice.OpenUsbDevice(usbFinder);
            if (usbDevice == null)
            {
                Console.WriteLine("USB device cannot be found");
                return;
            }

            var wholeUsbDevice = usbDevice as IUsbDevice;
            if (!ReferenceEquals(wholeUsbDevice, null))
            {
                wholeUsbDevice.SetConfiguration(1);
                wholeUsbDevice.ClaimInterface(0);
            }

            var info = wholeUsbDevice.Info;
            var edps = wholeUsbDevice.ActiveEndpoints;

            var reader = usbDevice.OpenEndpointReader(ReadEndpointID.Ep01);
            reader.DataReceivedEnabled = true;
            reader.DataReceived += DataReceived;
        }

        static void DataReceived(object sender, EndpointDataEventArgs e)
        {
            Console.WriteLine("data received");
        }

        private static List<USBDeviceInfo> GetUSBDevices()
        {
            var devices = new List<USBDeviceInfo>();

            var usbClass = new ManagementClass("Win32_USBDevice");
            var usbCollection = usbClass.GetInstances();
            foreach (var usb in usbCollection)
            {
                var deviceId = usb["deviceid"].ToString();
                int vidIndex = deviceId.IndexOf("VID_");
                string startingAtVid = deviceId.Substring(vidIndex + 4);               
                string vid = startingAtVid.Substring(0, 4);
                int pidIndex = deviceId.IndexOf("PID_");
                string startingAtPid = deviceId.Substring(pidIndex + 4);
                string pid = startingAtPid.Substring(0, 4);
                devices.Add(new USBDeviceInfo
                {
                    Pid = pid,
                    Vid = vid
                });
            }

            return devices;
        }

        private static void WatchComPorts()
        {
            var portNames = SerialPort.GetPortNames();
            foreach (var portName in portNames)
            {
                var serialPort = new SerialPort(portName, 9600, Parity.None, 8, StopBits.One);
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }

                serialPort.Open();
                serialPort.DtrEnable = true;
                serialPort.DataReceived += DataReceived;
            }
        }

        static void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Console.WriteLine("data received");
        }

        private class USBDeviceInfo
        {
            public string Vid { get; set; }

            public string Pid { get; set; }
        }
    }
}
