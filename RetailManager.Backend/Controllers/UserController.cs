using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using RetailManager.DataAccessLibrary.DataAccess;
using RetailManager.DataAccessLibrary.Models;

namespace RetailManager.Backend.Controllers
{
    [Authorize]
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {

        // GET: User/Details/5
        [HttpGet]
        public UserModel GetById()
        {
            string loggedInUserId = RequestContext.Principal.Identity.GetUserId();

            UserData data = new UserData();
            return data.GetUserById(loggedInUserId);
        }

    }
}
