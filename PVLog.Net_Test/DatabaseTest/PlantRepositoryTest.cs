using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PVLog.DataLayer;
using PVLog.Models;
using PVLog.Enums;
using PVLog;
namespace solar_tests.DatabaseTest
{
    [TestFixture]
    public class PlantRepositoryTest
    {

        TestDbSetup testDB;
        PlantRepository _plantRepository;

        [SetUp]
        public void Setup()
        {
            testDB = new TestDbSetup();
            testDB.TruncateAllTables();
            _plantRepository = new PlantRepository();
        }

        [TearDown]
        public void TearDown()
        {
            _plantRepository.Dispose();
        }

        [Test]
        public void InsertDeleteValidateGetUserOwnsSystemTest()
        {
            var db = new PlantRepository();

            var plantId_1 = DatabaseHelpers.CreatePlantGetId();
            var plantId_2 = DatabaseHelpers.CreatePlantGetId();

            //Insert test data
            db.StoreUserPlantRelation(3, plantId_1, E_PlantRole.Owner);
            db.StoreUserPlantRelation(2, plantId_2, E_PlantRole.Guest);

            //validate
            Assert.True(db.ValidateUserUserForPlant(3, plantId_1, E_PlantRole.Owner));
            Assert.True(db.ValidateUserUserForPlant(3, plantId_1, E_PlantRole.Guest));
            Assert.True(db.ValidateUserUserForPlant(2, plantId_2, E_PlantRole.Guest));
            Assert.False(db.ValidateUserUserForPlant(2, plantId_2, E_PlantRole.Owner));

            //get owners
            var actual = db.GetUsersOfSolarPlant(plantId_2, E_PlantRole.Guest);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(2, actual[0]);

            //delete relationship
            db.DeleteUserHasPlantRelation(2, plantId_2, E_PlantRole.Guest);
            Assert.False(db.ValidateUserUserForPlant(2, plantId_2, E_PlantRole.Guest));
        }

        [Test]
        public void GetPlantByIdTest()
        {
            var plantId = DatabaseHelpers.CreatePlantGetId();

            var plant = _plantRepository.GetPlantById(plantId);

            Assert.False(string.IsNullOrWhiteSpace(plant.Name));
            Assert.IsNotNullOrEmpty(plant.Password);
            Assert.That(plant.PlantId, Is.EqualTo(plantId));
        }


        [Test]
        public void UpdatePlantTest()
        {
            // create test plant
            var plant = DatabaseHelpers.CreatePlantWithOneInverter();

            //set new plant name
            SolarPlant plantToUpdate = new SolarPlant()
            {
                Name = "UpdatedPlant",
                PlantId = plant.PlantId,
                AutoCreateInverter = false,
                PeakWattage = 1843,
                PostalCode = "1543"
            };

            //verify old plant first
            Assert.IsTrue(_plantRepository.GetPlantById(plant.PlantId).AutoCreateInverter);

            //update plant
            _plantRepository.UpdatePlant(plantToUpdate);

            //verify plant name was updated
            var actual = _plantRepository.GetPlantById(plant.PlantId);
            Assert.AreEqual(plantToUpdate.Name, actual.Name);
            Assert.IsFalse(actual.AutoCreateInverter);
            Assert.AreEqual("1543", actual.PostalCode);
            Assert.AreEqual(1843, actual.PeakWattage);
        }

        [Test]
        public void ValidatePVPlant()
        {
            string password = "12345";

            var plant = TestdataGenerator.GetPlant();
            plant.Password = password;

            var plantId = _plantRepository.CreatePlant(plant);

            Assert.IsTrue(_plantRepository.IsValidPlant(plantId, password));
            Assert.IsFalse(_plantRepository.IsValidPlant(plantId, password + "test"));

        }

        [Test]
        public void VerifyAutoCreateInverterAfterPlantCreation()
        {
            var plantId = DatabaseHelpers.CreatePlantGetId();

            Assert.IsTrue(_plantRepository.IsAutoCreateInverterActive(plantId));

        }

        [Test]
        public void CreateInvertersPublicIdTest()
        {
            var plantId = DatabaseHelpers.CreatePlantGetId();

            var inverterId_1 = DatabaseHelpers.CreateInverter(plantId);
            var inverterId_2 = DatabaseHelpers.CreateInverter(plantId);

            var actual_1 = _plantRepository.GetInverter(inverterId_1);
            var actual_2 = _plantRepository.GetInverter(inverterId_2);

            Assert.AreEqual(1, actual_1.PublicInverterId);
            Assert.AreEqual(2, actual_2.PublicInverterId);
        }

        [Test]
        public void DeleteInverterTest()
        {
            //create inverter and plant
            var plant = DatabaseHelpers.CreatePlantWithOneInverter();

            // delete inverter
            _plantRepository.DeleteInverterById(plant.InverterId);

            // check that the plant has no inverter anymore
            var actualPlant = _plantRepository.GetPlantById(plant.PlantId);
            Assert.AreEqual(0, actualPlant.Inverters.Count());
        }


        [Test]
        public void GetPrivateInverterId()
        {
            var plantId = DatabaseHelpers.CreatePlantGetId();
            var publicInverterId = 4;

            var expectedInverterId = _plantRepository.CreateInverter(plantId, publicInverterId, 0.2F, "Test Generator");
            var actual = _plantRepository.GetPrivateInverterId(plantId, publicInverterId);

            Assert.AreEqual(expectedInverterId, actual);
        }

        [Test]
        public void IsValidInverter()
        {
            var plantId = DatabaseHelpers.CreatePlantGetId();
            int publicInverterId = 5;

            var privateInverterId = _plantRepository.CreateInverter(plantId, publicInverterId, 0.2F, "IsValidInverter-Test-Gen");

            Assert.IsTrue(_plantRepository.IsValidInverter(plantId, publicInverterId));
            Assert.IsFalse(_plantRepository.IsValidInverter(plantId, 6));
        }

        [Test]
        public void DemoPlantTest()
        {
            Assert.IsFalse(_plantRepository.IsDemoPlantExsting());
            var plant = TestdataGenerator.GetPlant();
            plant.IsDemoPlant = true;

            //create demo plant
            _plantRepository.CreatePlant(plant);
            Assert.IsTrue(_plantRepository.IsDemoPlantExsting());

            var demoPlant = _plantRepository.GetDemoPlant();
            Assert.IsNotNull(demoPlant);
        }

        [Test]
        public void IsUserOwnerOfInverter()
        {
            var userId = 1337;
            var plant = DatabaseHelpers.CreatePlantWithOneInverter();

            //should return false
            Assert.IsFalse(_plantRepository.IsOwnerOfInverter(plant.InverterId, userId));

            //make user the plant owner
            _plantRepository.StoreUserPlantRelation(userId, plant.PlantId, E_PlantRole.Owner);

            //should return true now
            Assert.IsTrue(_plantRepository.IsOwnerOfInverter(plant.InverterId, userId));

            //make user a plant guest
            _plantRepository.StoreUserPlantRelation(userId, plant.PlantId, E_PlantRole.Guest);

            //should return false now
            Assert.IsFalse(_plantRepository.IsOwnerOfInverter(plant.InverterId, userId));
        }

        [Test]
        public void UpdateInverterTest()
        {
            var plant = DatabaseHelpers.CreatePlantWithOneInverter();

            //load inverter
            var expected = _plantRepository.GetInverter(plant.InverterId);

            //edit inverter
            expected.Name = "UpdatedInverterName";
            expected.EuroPerKwh = 0.5423F;
            expected.ACPowerMax = 5000;

            //edit and store inverter 
            _plantRepository.StoreInverter(expected);

            // verify updated inverter
            var actual = _plantRepository.GetInverter(plant.InverterId);
            Assert.IsTrue(actual.PropertiesEqual(expected));

        }

        [Test]
        public void IsPlantOnlineTest()
        {
            var offlinePlant = DatabaseHelpers.CreatePlantWithOneInverter();
            var onlinePlant = DatabaseHelpers.CreatePlantWithOneInverter();
            var plantWithoutInverter = DatabaseHelpers.CreatePlantGetId();

            // create and store measure for online repository
            I_MeasureRepository measureRepo = new MeasureRepository();

            for (int i = 0; i < 5; i++)
            {
                measureRepo.InsertMeasure(new Measure()
                {
                    DateTime = DateTime.Now.AddMinutes(i),
                    PrivateInverterId = onlinePlant.InverterId,
                    OutputWattage = 41234
                });
            }

            _plantRepository.UpdatePlantOnlineStatus();

            //check on/offline state of plants
            Assert.IsFalse(_plantRepository.GetPlantById(offlinePlant.PlantId).IsOnline);
            Assert.IsTrue(_plantRepository.GetPlantById(onlinePlant.PlantId).IsOnline);
            Assert.IsFalse(_plantRepository.GetPlantById(plantWithoutInverter).IsOnline);
        }

    }
}
