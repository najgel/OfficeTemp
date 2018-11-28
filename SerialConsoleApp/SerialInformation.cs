using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.IO.Ports;

namespace SerialConsoleApp
{
    class SerialInformation

    {

        public SerialPort serialPort { get; set; }
        string tableConnectionString = "<YOUR STA CONNECTION STRING GOES HERE>";
        CloudStorageAccount storageAccount;
        string tableName = "Environment";


        public SerialInformation()
        {

        }

        public static void GetPorts()
        {
            Console.WriteLine("Serial ports available:");
            Console.WriteLine("-----------------------");

            foreach (var portName in SerialPort.GetPortNames())
            {
                Console.WriteLine(portName);
            }
        }

        public void ReadFromPort(string portName)
        {
            serialPort = new SerialPort(portName)
            {
                BaudRate = 9600,
                Parity = Parity.None,
                StopBits = StopBits.One,
                DataBits = 8,
                Handshake = Handshake.None
            };

            serialPort.DtrEnable = true;
            serialPort.RtsEnable = true;
            serialPort.ReadTimeout = 30 * 1000;

            serialPort.DataReceived += SerialPortDataReceived;
            serialPort.ErrorReceived += SerialPortErrorReceived;
            serialPort.PinChanged += SerialPortPinChanged;


            serialPort.Open();
        }

        private void SerialPortPinChanged(object sender, SerialPinChangedEventArgs e)
        {
            Console.WriteLine($"PinChanged, eventType: {e.EventType.ToString()}");

            //serialPort.Close();
        }

        private void SerialPortErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            Console.WriteLine($"Error received, eventType: {e.EventType.ToString()}");

            //serialPort.Close();

        }

        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialPort = (SerialPort)sender;
            //EnvironmentStat environment;
            storageAccount = CloudStorageAccount.Parse(tableConnectionString);
            var serialdata = "EMPTY";

            // Read the data that's in the serial buffer.
            try
            {
                serialdata = serialPort.ReadLine();

                var environment = JsonConvert.DeserializeObject<EnvironmentStat>(serialdata);
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference(tableName);
                TableOperation insertOperation = TableOperation.Insert(environment);
                table.ExecuteAsync(insertOperation);
                Console.WriteLine($"Temp: {environment.Temp} C, Humidity: {environment.Humidity} %");
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"Timeout Reading Serial port, error message: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error Reading Serial port, error message: {ex.Message}, Serial Data: {serialdata} ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Oh shit something went wrong, error message: {ex.Message}, Serial Data: {serialdata} ");
            }








            //            serialPort.Close();
        }


    }
}
