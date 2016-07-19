using System.IO.Ports;

namespace WsFederation.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var portNames = SerialPort.GetPortNames();
            var serialPort = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }

            serialPort.Open();
            serialPort.DtrEnable = true;
            serialPort.DataReceived += SerialPort_DataReceived;

            System.Console.ReadLine();
        }

        private static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            System.Console.WriteLine("data received");
        }
    }
}
