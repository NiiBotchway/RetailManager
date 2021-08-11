using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.DesktopUI.Library.Models
{
    public class Sale
    {
        public List<SaleDetails> SaleDetails { get; set; } = new List<SaleDetails>();
    }
}
