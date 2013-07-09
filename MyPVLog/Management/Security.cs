using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PVLog.DataLayer;
using System.Web.Security;
namespace PVLog.Management
{
    public static class Security
    {
        //internal static bool IsUserOwnerOfPlant(int plantID)
        //{
        //    var currentUser = Membership.GetUser();
        //    if (Roles.IsUserInRole("admin")) return true;
        //    int userID = Convert.ToInt32(currentUser.ProviderUserKey);

        //    var plantDb = new SolarPlantDatabase();

        //    return plantDb.ValidateUserUserForPlant(userID, plantID, Enums.E_PlantRole.Owner);

        //}
    }
}