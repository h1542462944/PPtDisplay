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
        public List<string> Inventories { get; set; } = new List<string>
        {
            @"D:\PPt\"
        };

        public void RaiseEvent()
        {
            InventoriesChanged?.Invoke(this, new EventArgs());
        }
        public event EventHandler InventoriesChanged;
    }
}
