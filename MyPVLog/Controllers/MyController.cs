namespace PVLog.Controllers
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using DataLayer;
    using Models;
    using OutputProcessing;

    public class MyController : Controller
    {
        public I_MeasureRepository _measureRepository { get; set; }
        public I_PlantRepository _plantRepository { get; set; }
        public IMembershipService MembershipService { get; set; }
        public KwhRepository _kwhRepository { get; set; }
        public UiDataProvider _dataProvider { get; set; }

        public int CurrentUserId => MembershipService.CurrentUserId;

        public bool IsAdmin => User.IsInRole("Admin");

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (MembershipService == null)
            {
                MembershipService = new AccountMembershipService();
            }

            if (_measureRepository == null)
            {
                _measureRepository = new MeasureRepository();
            }
            if (_plantRepository == null)
            {
                _plantRepository = new PlantRepository();
            }
            if (_kwhRepository == null)
            {
                _kwhRepository = new KwhRepository();
            }
            if (_dataProvider == null)
            {
                _dataProvider = new UiDataProvider();
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _measureRepository?.Dispose();
                _plantRepository?.Dispose();
                _kwhRepository?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}