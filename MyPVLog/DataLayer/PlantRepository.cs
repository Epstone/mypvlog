using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PVLog.Utility;
using System.ComponentModel;
using System.Data;
using MySql.Data.MySqlClient;
using PVLog.Models;
using System.Xml;
using PVLog.Enums;
using MySqlRepository;
using System.Configuration;
using MvcMiniProfiler;
using Dapper;

namespace PVLog.DataLayer
{
    public class PlantRepository : MySqlRepositoryBase, I_PlantRepository
    {
        public PlantRepository()
        {
            var connStr = ConfigurationManager.ConnectionStrings["pv_data"].ConnectionString;
            base.Initialize(connStr, connStr);
        }

        public PlantRepository(string _connStr)
        {
            base.Initialize(_connStr, _connStr);
        }

        /* SOLAR PLANT */
        /// <summary>
        /// Creates a new PV System by a given password
        /// </summary>
        /// <returns>The ID of the new pv system</returns>
        //public int CreatePlant(SolarPlant plant)
        //{
        //  return this.CreatePlant(plant, false);
        //}

        public int CreatePlant(SolarPlant plant)
        {
            //Add a new PV System to the plants table
            string insertPVText = @"INSERT INTO plants(Name, Password, IsDemoPlant, PostalCode, PeakWattage) 
                                    VALUES (@Name, @Password, @isDemoPlant, @PostalCode, @PeakWattage);
                                    SELECT LAST_INSERT_ID() AS ID";


            var result = ProfiledWriteConnection.Query(insertPVText, plant).First();

            return (int)result.ID;
        }

        public bool IsDemoPlantExsting()
        {
            long demoPlantCount = ProfiledReadConnection.Query<long>("SELECT COUNT(PlantId) FROM plants WHERE IsDemoPlant = true").First();
            return (demoPlantCount == 1);
        }

        public SolarPlant GetDemoPlant()
        {
            var demoPlant = ProfiledReadConnection.Query<SolarPlant>("SELECT * FROM plants WHERE IsDemoPlant = true").First();
            demoPlant.Inverters = GetAllInvertersByPlant(demoPlant.PlantId);

            return demoPlant;
        }

        public void DeletePlant(int systemID)
        {
            string text = "DELETE FROM plants WHERE (PlantID = ?PlantID);";


            var sqlCom = base.GetWriteCommand(text);

            sqlCom.Parameters.AddWithValue("?PlantID", systemID);
            sqlCom.ExecuteNonQuery();
        }

        public IEnumerable<SolarPlant> GetAllPlants()
        {
            string text = @"
SELECT * FROM plants p
 ORDER BY p.Name";

            //get all plants
            var result = ProfiledReadConnection.Query<SolarPlant>(text);

            //fill inverter info
            foreach (var plant in result)
            {
                plant.Inverters = GetAllInvertersByPlant(plant.PlantId);
            }

            return result;
        }

        public bool IsValidPlant(int systemID, string inputPassword)
        {
            string text = @"SELECT Password FROM plants 
                            WHERE (PlantID = @PlantID);";

            var result = ProfiledReadConnection.Query<string>(text, new { plantID = systemID });

            return (result.Count() == 1 && result.First() == inputPassword);
        }

        public DataTable GetAllPlantsAsDataTable()
        {
            string text = @"SELECT PlantID AS systemID, Name FROM plants";


            var sqlCom = base.GetWriteCommand(text);

            MySqlDataAdapter adapter = new MySqlDataAdapter(sqlCom);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public SolarPlant GetPlantById(int plantId)
        {
            var result = this.GetAllPlants().Where(x => x.PlantId == plantId);

            if (result.Count() == 0)
                throw new ApplicationException("Plant not existing");

            var plantResult = result.First();
            plantResult.Inverters = this.GetAllInvertersByPlant(plantId);

            return plantResult;
        }

        public void UpdatePlant(SolarPlant plantToUpdate)
        {
            ProfiledWriteConnection.Execute(@"UPDATE plants 
                                          SET Name = @name,
                                              AutoCreateInverter = @autoCreateInverter,
                                              PeakWattage = @peakWattage,
                                              PostalCode = @postalCode,
                                              EmailNotificationsEnabled = @emailNotificationsEnabled
                                        WHERE PlantId = @plantId", plantToUpdate);
        }

        /* INVERTER */
        public IEnumerable<Inverter> GetAllInvertersByPlant(int plantId)
        {
            string cmd = @"SELECT * FROM inverter
                            WHERE PlantId = @plantId
                            ORDER BY PublicInverterId ASC";

            return ProfiledReadConnection.Query<Inverter>(cmd, new { plantId });


        }

        internal List<int> GetPrivateInverterIdsByPlant(int systemID)
        {
            return GetAllInvertersByPlant(systemID).Select(x => x.InverterId).ToList();
        }



        /// <summary>
        /// Creates a new inverter
        /// </summary>
        /// <param name="plantId"></param>
        /// <param name="publicInverterId"></param>
        /// <returns>Returns the private inverterId</returns>
        public int CreateInverter(int plantId, int? publicInverterId, float euroPerKwh, string name)
        {
            //get next inverterId if parameter is null
            if (!publicInverterId.HasValue)
            {
                string cmd1 = @"SELECT (COALESCE(MAX(PublicInverterId),0) +1) as result FROM inverter WHERE PlantID = @plantId;";
                publicInverterId = (int)ProfiledReadConnection.Query<long>(cmd1, new { plantId }).First();
            }

            string cmd = @"INSERT INTO inverter 
                                    (PlantId, PublicInverterId, EuroPerKwh, Name)
                                  VALUES
                                    (@plantId, @publicInverterId,@EuroPerKwh, @name);
                            SELECT CAST(LAST_INSERT_ID() AS UNSIGNED INTEGER);";

            var inverterId = base.ProfiledReadConnection.Query<ulong>(cmd, new { plantId, publicInverterId, euroPerKwh, name }).First();
            return (int)inverterId;
        }

        public IEnumerable<Inverter> GetAllInverters()
        {
            var cmd = "SELECT * FROM inverter";
            return ProfiledReadConnection.Query<Inverter>(cmd);
        }

        public int GetPrivateInverterId(int plantId, int publicInverterId)
        {
            var cmd = @"SELECT InverterId FROM inverter 
                        WHERE PublicInverterId = @publicInverterId
                            AND PlantId = @plantId;";

            return ProfiledReadConnection.Query<int>(cmd, new { plantId, publicInverterId }).First();
        }

        public bool IsValidInverter(int plantId, int publicInverterId)
        {
            var cmd = @"SELECT COUNT(InverterId) FROM inverter 
                        WHERE PublicInverterId = @publicInverterId
                            AND PlantId = @plantId;";

            long inverterCount = ProfiledReadConnection.Query<long>(cmd, new { plantId, publicInverterId }).First();
            return (inverterCount == 1);
        }

        public bool IsAutoCreateInverterActive(int plantId)
        {
            string cmd = "SELECT AutoCreateInverter FROM plants WHERE plantID = @plantId;";
            return base.ProfiledReadConnection.Query<bool>(cmd, new { plantId }).First();
        }

        public Inverter GetInverter(int privateInverterId)
        {
            return GetAllInverters().Single(x => x.InverterId == privateInverterId);
        }

        public void StoreInverter(Inverter inverterToUpdate)
        {
            ProfiledWriteConnection.Execute(@"UPDATE inverter 
          SET Name = @name
              , EuroPerKwh = @euroPerKwh 
              , ACPowerMax = @aCPowerMax
          WHERE InverterId = @inverterId;", inverterToUpdate);
        }

        public void DeleteInverterById(int id)
        {
            ProfiledWriteConnection.Execute("DELETE FROM inverter WHERE InverterId = @inverterId", new { inverterId = id });
        }
        /* USER PLANT AUTHORIZATION */
        public void StoreUserPlantRelation(int userID, int systemID, E_PlantRole role)
        {
            string text = @"INSERT INTO user_has_plant (PlantID, UserID, PlantRole)
                            VALUES (?PlantID, ?UserID, ?PlantRole)
                      ON DUPLICATE KEY UPDATE PlantRole = ?PlantRole;";

            var sqlCom = base.GetWriteCommand(text);

            //add parameters
            sqlCom.Parameters.AddWithValue("?PlantID", systemID);
            sqlCom.Parameters.AddWithValue("?UserID", userID);
            sqlCom.Parameters.AddWithValue("?PlantRole", role);

            sqlCom.ExecuteNonQuery();
        }

        public void DeleteUserHasPlantRelation(int userID, int systemID, E_PlantRole role)
        {
            string text = @"DELETE FROM user_has_plant
                            WHERE (PlantID = ?PlantID)
                            AND (UserID = ?UserID)
                            AND (PlantRole = ?PlantRole);";

            var sqlCom = base.GetWriteCommand(text);

            //add parameters
            sqlCom.Parameters.AddWithValue("?PlantID", systemID);
            sqlCom.Parameters.AddWithValue("?UserID", userID);
            sqlCom.Parameters.AddWithValue("?PlantRole", role);

            sqlCom.ExecuteNonQuery();
        }

        public bool ValidateUserUserForPlant(int userID, int systemID, E_PlantRole minimumRequiredRole)
        {
            string text = @"SELECT Count(*) FROM user_has_plant
                            WHERE (PlantID = ?PlantID)
                            AND (UserID = ?UserID)
                            AND (?MinimumRole <= PlantRole);";

            var sqlCom = base.GetReadCommand(text);

            //add parameters
            sqlCom.Parameters.AddWithValue("?PlantID", systemID);
            sqlCom.Parameters.AddWithValue("?UserID", userID);
            sqlCom.Parameters.AddWithValue("?MinimumRole", minimumRequiredRole);

            return (Convert.ToInt32(sqlCom.ExecuteScalar()) >= 1);
        }

        public List<int> GetUsersOfSolarPlant(int plantId, E_PlantRole role)
        {
            string text = @"SELECT UserID, PlantID FROM user_has_plant
                            WHERE (PlantID =?PlantID)
                            AND (PlantRole = ?PlantRole);";

            var sql = base.GetReadCommand(text);

            sql.Parameters.AddWithValue("?PlantID", plantId);
            sql.Parameters.AddWithValue("?PlantRole", role);

            List<int> result = new List<int>();

            using (var rdr = sql.ExecuteReader())
            {
                while (rdr.Read())
                {
                    result.Add(rdr.GetInt32("UserID"));
                }
            }

            return result;
        }

        public bool IsOwnerOfPlant(int CurrentUserId, int p)
        {
            return this.ValidateUserUserForPlant(CurrentUserId, p, E_PlantRole.Owner);
        }

        /// <summary>
        /// Updates the online or offline status for all plants
        /// </summary>
        public void UpdatePlantOnlineStatus()
        {
            var plants = this.GetAllPlants();

            foreach (var plant in plants)
            {

                // get measure count for the laste 24 hours
                string measureCountSql = @"
SELECT COUNT(m.MeasureId) FROM inverter i
  LEFT JOIN measure m
    ON i.inverterId = m.InverterId
    AND (m.DateTime > (NOW() - INTERVAL 1 DAY))
  WHERE i.plantId = @plantId
  GROUP by i.plantId;";

                var result = ProfiledReadConnection.Query<long>(measureCountSql, new { plantId = plant.PlantId });

                bool isPlantOnline = (result.Count() != 0 && result.First() > 4);

                // update plant online status
                ProfiledWriteConnection.Execute("UPDATE plants SET IsOnline = @isPlantOnline WHERE plantId = @plantId", new { plant.PlantId, isPlantOnline });

            }
        }

        public void SetPlantOnline(int plantId, DateTime lastMeasureDate)
        {
            string updateStatement = @"UPDATE plants 
SET 
IsOnline = @isPlantOnline, 
LastMeasureDate = @lastMeasureDate
WHERE plantId = @plantId";
            ProfiledWriteConnection.Execute(updateStatement, new
            {
                plantId = plantId,
                lastMeasureDate = lastMeasureDate,
                isPlantOnline = true
            });
        }

        /// <summary>
        /// Checks the authorization for a user to edit an inverter.
        /// </summary>
        /// <param name="inverterId">The inverter ID</param>
        /// <param name="userId">The user requesting authorization</param>
        /// <returns>The authorization result</returns>
        public bool IsOwnerOfInverter(int inverterId, int userId)
        {
            long result = ProfiledReadConnection.Query<long>(@"
SELECT COUNT(uha.PlantId) FROM user_has_plant uha
  INNER JOIN inverter i
    ON i.PlantId = uha.PlantID
  WHERE uha.UserID = @userId 
    AND i.InverterId = @inverterId
    AND uha.PlantRole = @plantRole;", new { userId, inverterId, plantRole = (int)E_PlantRole.Owner }).First();

            return (result == 1);

        }

        public void Cleanup()
        {
            base.Dispose();
        }

    }
}