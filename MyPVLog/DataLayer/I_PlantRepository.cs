using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PVLog.Models;
using PVLog.Enums;

namespace PVLog.DataLayer
{
    public interface I_PlantRepository : IDisposable
    {
        int CreatePlant(SolarPlant plant);
        IEnumerable<SolarPlant> GetAllPlants();
        SolarPlant GetPlantById(int plantId);

        bool IsValidPlant(int plant, string pw);

        int GetPrivateInverterId(int plant, int inverter);
        Inverter GetInverter(int privateInverterId);

        bool IsValidInverter(int plant, int inverter);

        bool IsAutoCreateInverterActive(int plant);

        IEnumerable<Inverter> GetAllInvertersByPlant(int plantId);
        IEnumerable<Inverter> GetAllInverters();
        int CreateInverter(int plant, int? publicInverterId, float euroPerKwh, string name);

        //void CreateDemoPlant(string password, string plantName);
        bool IsDemoPlantExsting();
        SolarPlant GetDemoPlant();

        bool IsOwnerOfPlant(int CurrentUserId, int p);
        void StoreUserPlantRelation(int userID, int systemID, E_PlantRole role);
        bool IsOwnerOfInverter(int inverterId, int userId);

        void UpdatePlant(SolarPlant plantToUpdate);

        void StoreInverter(Inverter inverterToUpdate);

        void DeleteInverterById(int id);
        void Cleanup();

        void SetPlantOnline();
        void SetPlantOnline(int plantId, DateTime lastActivityDate);
    }
}
