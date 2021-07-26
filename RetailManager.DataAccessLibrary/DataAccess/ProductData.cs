using RetailManager.DataAccessLibrary.Internal.DataAccess;
using RetailManager.DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.DataAccessLibrary.DataAccess
{
    public class ProductData
    {
        SqlDataAccess dataAccess = new SqlDataAccess();
        public IEnumerable<ProductModel> GetProducts()
        {
            var output = dataAccess.LoadData<ProductModel, dynamic>("spProduct_GetAll", new { }, "RetailManagerDB");

            return output;
        }
    }
}
