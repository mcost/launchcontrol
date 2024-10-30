using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using static NEAWebApplication.Controllers.HomeController;
namespace NEAWebApplication
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
    public class Flight
    {
        public int FlightID { get; set; }
        public int ClubMemberP1ID { get; set; }
        public int? ClubMemberP2ID { get; set; }
        public TimeSpan TakeoffTime { get; set; }
        public TimeSpan LandingTime { get; set; }
        public int LaunchTypeID { get; set; }
        public int GliderID { get; set; }
        public int PayingMemberID { get; set; }
        public float Cost { get; set; }
    }
    public class FlightViewModel
    {
        public string P1FirstName { get; set; }
        public string P1Surname { get; set; }
        public int P1MembershipNumber { get; set; }

        public string P2FirstName { get; set; }
        public string P2Surname { get; set; }
        public int? P2MembershipNumber { get; set; }
    }
public class FlightController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FlightController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new FlightViewModel());
        }

        [HttpPost]
        public ActionResult Create(FlightViewModel model)
        {
            if (ModelState.IsValid)
            {
                var flight = new Flight
                {
                    ClubMemberP1ID = model.P1MembershipNumber,
                    ClubMemberP2ID = model.P2MembershipNumber,
                };

                _context.Flights.Add(flight);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext() : base("DefaultConnection") { }
            public DbSet<Flight> Flights { get; set; }
        }
    }

}
