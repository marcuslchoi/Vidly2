using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly2.Models;
using System.Data.Entity;   //for the Include() method (eager loading)

namespace Vidly2.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        //override the dispose method of the base controller class
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
/*
        private IEnumerable<Customer> GetCustomers()
        {
             return new List<Customer>
             {
                 new Customer { Id = 1, Name = "John Smith" },
                 new Customer { Id = 2, Name = "Mary Williams" }
             };
        }
*/
        // GET: Customers
        public ActionResult Index()
        {
            //include loads the objects in the argument as well
            //a db set defined in the db context
            var customers = _context.Customers.Include(c=>c.MembershipType).ToList();   // GetCustomers();
            return View(customers);
        }

        public ActionResult Details(int id)
        {
            var customer = _context.Customers.Include(c => c.MembershipType).ToList().SingleOrDefault(c => c.Id == id);//GetCustomers().SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return Content("customer does not exist");
       
            return View(customer);
        }
        
        public ActionResult New()
        {
            //var membershipTypes = _context.MembershipTypes.ToList();
            return View();
        }
        
    }
}