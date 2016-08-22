using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly2.Models;
using System.Data.Entity;   //for the Include() method (eager loading)
using Vidly2.ViewModels;

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
            var membershipTypes = _context.MembershipTypes.ToList();
            var viewModel = new CustomerFormViewModel
            {
                MembershipTypes = membershipTypes
            };

            //pass customerform as the view name
            return View("CustomerForm", viewModel);
        }

        [HttpPost]
        //mvc framework binds this model to the request data (model binding)
        public ActionResult Save(Customer customer)  
        {
            if (customer.Id == 0)
                _context.Customers.Add(customer);
            else
            {
                var customerInDb = _context.Customers.Single(c => c.Id == customer.Id);

                //this updates anything user requests
                //TryUpdateModel(customerInDb);

                //can use automapper to do this: Mapper.Map(customer, customerInDb);
                customerInDb.Name = customer.Name;
                customerInDb.Birthdate = customer.Birthdate;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;
                customerInDb.MembershipType = customer.MembershipType;
            }

            _context.SaveChanges(); //generates sql statements and runs them at run time

            //redirect user back to list of customers
            return RedirectToAction("Index", "Customers");
        }

        public ActionResult Edit(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return HttpNotFound();

            //changed name to CustomerFormViewModel so works for new and existing customers
            var viewModel = new CustomerFormViewModel
            {
                Customer = customer,
                MembershipTypes = _context.MembershipTypes.ToList()
            };

            //redirect to the "New" action
            return View("CustomerForm", viewModel);
        }
        
    }
}