using Microsoft.AspNet.Identity;
using RetailManager.Backend.Models;
using RetailManager.DataAccessLibrary.DataAccess;
using RetailManager.DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace RetailManager.Backend.Controllers
{
    public class SaleController : ApiController
    {
        // GET: Sale
        [Authorize]

        public void Post(SaleModel sale)
        {
            if (sale == null) throw new ArgumentNullException(nameof(sale));

            string loggedInUserId = RequestContext.Principal.Identity.GetUserId();
            SaleData data = new SaleData();
            data.SaveSale(sale, loggedInUserId);

        }
    }
}