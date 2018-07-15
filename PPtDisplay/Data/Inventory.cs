using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Component;

namespace PPtDisplay.Data
{
    /// <summary>
    /// 表示<see cref="App"/>的目录信息.
    /// </summary>
    public class Inventory : USettingsObject
    {
        protected override string Comment => "表示App的目录信息.";
        public Inventory()
        {
            base.Folder = App.LocalCache;
            base.DisplayName = "Inventory";
        }

        /// <summary>
        /// 表示目录集,用于<see cref="OpenPage"/>.
        /// </summary>
        public List<string> Inventorys { get; set; } = new List<string>
        {
            @"D:\PPt\"
        };
        public long CurrentIndex { get; set; } = 0L;
        public List<PPtUsageRecord> Records { get; set; } = new List<PPtUsageRecord>();
        public PPtUsageRecord CreateNewPPtUsageRecord(string fullName)
        {
            CurrentIndex++;
            PPtUsageRecord record = new PPtUsageRecord() { FullName = fullName, StartTime = DateTime.Now, ID = CurrentIndex };
            if (!Directory.Exists(record.DataCache))
            {
                Directory.CreateDirectory(record.DataCache);
            }
            return record;
        }
    }
    public class PPtUsageRecord
    {
        public PPtUsageRecord()
        {
   
        }

        public string FullName { get; set; } = "";
        public DateTime StartTime { get; set; } = new DateTime();
        public long ID { get; set; } = 0L;
        public string DataCache => App.DataCache + ID + @"\";
    }
}
