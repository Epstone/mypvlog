namespace solar_tests.ControllerTest
{
    using System;
    using Moq;
    using NUnit.Framework;
    using PVLog;
    using PVLog.Controllers;
    using PVLog.DataLayer;

    public class LogControllerTest
    {
        private LogController _logController;
        private Mock<IMeasureRepository> _measureRepositoryMock;
        private Mock<I_PlantRepository> _plantRepositoryMock;

        [SetUp]
        public void Setup()
        {
            _plantRepositoryMock = new Mock<I_PlantRepository>();
            _measureRepositoryMock = new Mock<IMeasureRepository>();
            var mock = new Mock<IInverterTrackerRegistry>();
            _logController = new LogController(_measureRepositoryMock.Object,
                _plantRepositoryMock.Object, mock.Object);

            _plantRepositoryMock.Setup(x => x.IsValidPlant(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(true);
            _plantRepositoryMock.Setup(x => x.IsValidInverter(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true);
        }


        [Test]
        public void LogKaco1Measure()
        {
            string data = "26.12.2009;23:53:00;5;158.0;3.20;134;229.6;1.34;150;7";
            _logController.Kaco1(data, 1, "1234", 1);

            _measureRepositoryMock.Verify(m => m.InsertMeasure(It.IsAny<Measure>()), Times.Never());
            _measureRepositoryMock.Verify(m => m.InsertTemporary(It.IsAny<Measure>()), Times.Once());
        }

        [Test]
        public void Kaco2()
        {
            _logController.Kaco2("*020;4;378.2;3.96;1498;228.9;6.55;1438;29;5000;", 2, "fadsd");

            _measureRepositoryMock.Verify(m => m.InsertMeasure(It.IsAny<Measure>()), Times.Never());
            _measureRepositoryMock.Verify(m => m.InsertTemporary(It.IsAny<Measure>()), Times.Once());
        }

        [Test]
        public void LogGeneric()
        {
            _logController.Generic(1, "fsa", 2, 142, 243, 234, 234, null, 432, 3, 32, null);

            _measureRepositoryMock.Verify(m => m.InsertMeasure(It.IsAny<Measure>()), Times.Never());
            _measureRepositoryMock.Verify(m => m.InsertTemporary(It.IsAny<Measure>()), Times.Once());
        }

        [Test]
        public void Given_a_new_log_Then_the_plant_online_status_is_updated()
        {
            int plantId = 1;
            _logController.Generic(plantId, "fsa", 2, 142, 243, 234, 234, null, 432, 3, 32, null);
            _plantRepositoryMock.Verify(x=>x.SetPlantOnline(1,It.IsAny<DateTime>()),Times.Once());
        }
    }
}