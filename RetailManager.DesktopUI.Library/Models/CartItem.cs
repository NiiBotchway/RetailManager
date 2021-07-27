using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.DesktopUI.Library.Models
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int QuantityInCart { get; set; }
        public string DisplayText
        {
            get
            {
                return $"{Product.ProductName } ({QuantityInCart})";
            }
        }
    }
}
