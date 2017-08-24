namespace PVLog.DataLayer
{
    using System;
    using System.Collections.Generic;

    public interface IUserNotifications : IDisposable
    {
        IEnumerable<PlantNotification> GetPlantNotifications();
    }
}