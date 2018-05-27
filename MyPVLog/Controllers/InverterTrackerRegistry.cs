using System.Collections.Concurrent;
using System.Runtime.Remoting.Messaging;

namespace PVLog.Controllers
{
    public interface IInverterTrackerRegistry
    {
        InverterTracker CreateOrGetTracker(int inverterId);
    }

    public class InverterTrackerRegistry : IInverterTrackerRegistry
    {
        readonly ConcurrentDictionary<int, InverterTracker> _trackers = new ConcurrentDictionary<int, InverterTracker>();

        public InverterTracker CreateOrGetTracker(int inverterId)
        {
            return this._trackers.GetOrAdd(inverterId, x => new InverterTracker(inverterId));
        }
    }
}