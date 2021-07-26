using RetailManager.DataAccessLibrary.DataAccess;
using RetailManager.DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RetailManager.Backend.Controllers
{
    //[Authorize]
    public class ProductController : ApiController
    {
        // GET api/values
        public List<ProductModel> GetAllProduts()
        {
            ProductData Products = new ProductData();
            return Products.GetProducts().ToList();
        }

    }
}
