using System;
using System.IO.Ports;
using System.Threading;


namespace SerialConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            //var serialPort = new SerialPort
            SerialInformation.GetPorts();

            var serialInformation = new SerialInformation();

            serialInformation.ReadFromPort("COM3");

            //           Console.WriteLine("Hello World!");
            Console.ReadLine();

            serialInformation.serialPort.Close();
        }
    }
}
