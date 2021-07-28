using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.DesktopUI.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        public double GetTaxRate()
        {
            string taxRate = ConfigurationManager.AppSettings.Get("taxRate");

            bool isValidRate = Double.TryParse(taxRate, out double result);
            if (isValidRate)
            {
                return result;
            }

            throw new ConfigurationErrorsException("The tax rate is not configured properly. Contact Admin.");

        }


    }
}
