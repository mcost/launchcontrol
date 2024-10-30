using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace NEAWebApplication
{
    public class DatabaseHelper
    {
        private string _connectionString;

        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<MEMBERCLUB> GetMembers(string name)
        {
            var members = new List<MEMBERCLUB>();
            string query = "SELECT Name, Surname, ID FROM LaunchControlSystem.dbo.MEMBERCLUB WHERE Name LIKE @name";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", $"{name}%");
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            members.Add(new MEMBERCLUB
                            {
                                Name = reader["Name"].ToString(),
                                Surname = reader["Surname"].ToString(),
                                ID = (int)reader["ID"]
                            });
                        }
                    }
                }
            }

            return members;
        }
    }

    public class Pilot
    {
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public int ID { get; set; } // Assuming LastName is used for Membership Number
    }

    public partial class FLIGHT
    {
        public Pilot Pilot1 { get; set; } = new Pilot();
        public Pilot Pilot2 { get; set; } = new Pilot();
        public Pilot Instructor { get; set; } = new Pilot();
    }
}

namespace NEAWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Please enter your launch details for your next flight.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = " ";
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Submitted()
        {
            return View();
        }

        public class ApplicationUser : IdentityUser
        {
        }

        public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
        {
            public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
            {
            }

            public static ApplicationDbContext Create()
            {
                return new ApplicationDbContext();
            }
        }

        public class FlightViewModel
        {
            public int? Pilot1Id { get; set; }
            public int? Pilot2Id { get; set; }
            public int? InstructorId { get; set; }
            public SelectList PilotOptions { get; set; }
        }

        [HttpPost]
        public ActionResult SearchPilot(FLIGHT flight)
        {
            if (flight == null)
            {
                flight = new FLIGHT(); // Initialize to avoid null reference
            }

            if (ModelState.IsValid)
            {
                var dbHelper = new DatabaseHelper(_connectionString);

                // Check Pilot 1
                var pilot1Results = dbHelper.GetMembers(flight.Pilot1.FirstName);
                var isPilot1Confirmed = pilot1Results.Any(p => p.Surname == flight.Pilot1.SurName && p.ID == flight.Pilot1.ID);
                if (!isPilot1Confirmed)
                {
                    ModelState.AddModelError("Pilot1", "Pilot 1 details are incorrect.");
                }

                // Check Pilot 2
                var pilot2Results = dbHelper.GetMembers(flight.Pilot2.FirstName);
                var isPilot2Confirmed = pilot2Results.Any(p => p.Surname == flight.Pilot2.SurName && p.ID == flight.Pilot2.ID);
                if (!isPilot2Confirmed)
                {
                    ModelState.AddModelError("Pilot2", "Pilot 2 details are incorrect.");
                }

                // Check Instructor
                var instructorResults = dbHelper.GetMembers(flight.Instructor.FirstName);
                var isInstructorConfirmed = instructorResults.Any(i => i.Surname == flight.Instructor.SurName && i.ID == flight.Instructor.ID);
                if (!isInstructorConfirmed)
                {
                    ModelState.AddModelError("Instructor", "Instructor details are incorrect.");
                }

                // Pass confirmation status to ViewBag if model state is valid
                ViewBag.Pilot1Confirmed = isPilot1Confirmed;
                ViewBag.Pilot2Confirmed = isPilot2Confirmed;
                ViewBag.InstructorConfirmed = isInstructorConfirmed;

                if (ModelState.IsValid)
                {
                    return View("About", flight);
                }
            }

            return View("About", flight); // Return About view even if ModelState is not valid
        }
    }
}