using RetailManager.DataAccessLibrary.Internal.DataAccess;
using RetailManager.DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManager.DataAccessLibrary.DataAccess
{
    public class UserData
    {
        SqlDataAccess dataAccess = new SqlDataAccess();
        public UserModel GetUserById(string Id)
        {
            var p = new { Id };
            var output = dataAccess.LoadData<UserModel, dynamic>("spUserLookup", p, "RetailManagerDB").FirstOrDefault();

            return output;
        }
    }
}
