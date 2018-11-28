using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace SerialConsoleApp
{
    class EnvironmentStat : TableEntity
    {
        public EnvironmentStat()
        {
            this.PartitionKey = "EnvironmentInfo";
            this.RowKey = Guid.NewGuid().ToString();
        }

        public string Temp { get; set; }
        public string Humidity { get; set; }
      //  public DateTime TimeStamp = DateTime.Now;
    }
}
