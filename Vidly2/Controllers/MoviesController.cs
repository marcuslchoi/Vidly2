using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly2.Models;
using Vidly2.ViewModels;
using System.Data.Entity;

namespace Vidly2.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;// = new ApplicationDbContext();

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Movies
        public ActionResult Index()
        {
            var movies = _context.Movies.Include(m=>m.Genre).ToList(); //GetMovies();
            return View(movies);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        /*
                private IEnumerable<Movie> GetMovies()
                {
                    return new List<Movie>
                    {
                        new Movie { Id = 1, Name = "Shrek" },
                        new Movie { Id = 2, Name = "Wall-e" }
                    };
                }
        */

        public ActionResult MovieDetails(int id)
        {
            var movie = _context.Movies.Include(m=>m.Genre).SingleOrDefault(m => m.Id == id);
            return View(movie);
        }

        public ActionResult Random()
        {

            var movie = new Movie() { Name = "Shrek" };

            var customers = new List<Customer>
            {
                new Customer {Name = "cust12" },
                new Customer {Name = "cust2" }
            };

            var viewModel = new RandomMovieViewModel
            {
                    Movie = movie,
                    Customers = customers
            };

            //every controller has a prop called ViewData of type viewdatadictionary
            //ViewData["Movie"] = movie;

            return View(viewModel);

//            return Content("this is some text");
//            return HttpNotFound();
//            return new EmptyResult();
//            return RedirectToAction("Index", "Home", new { page = 1, sortBy = "name" });   //action, controller, anonymous object to pass arguments to the action
        }

        public ActionResult Edit(int id)
        {
            return Content("id =" + id);
        }
/*
        public ActionResult Index(int? pageIndex, string sortBy)
        {
            if (!pageIndex.HasValue)
                pageIndex = 1;
            if (String.IsNullOrWhiteSpace(sortBy))
                sortBy = "Name";

            return Content(String.Format("pageIndex={0}&sortBy={1}",pageIndex, sortBy));
                 

        }
*/

        //code snippet for action: mvcaction4
        //the regex are attribute route constraints
        [Route("movies/released{year:regex(\\d(4))}/{month:regex(\\d{2}):range(1,12)}")]
        public ActionResult ByReleaseDate(int year, int month)
        {
            return Content(year + "/" + month);
        }

        public ActionResult New()
        {
            var genres = _context.Genres.ToList();
            var viewModel = new MovieFormViewModel
            {
                Genres = genres
            };

            return View("MovieForm",viewModel);
        }

        public ActionResult Save(Movie movie)
        {
            if (movie.Id == 0)
                _context.Movies.Add(movie);
            else
            {
                var movieInDb = _context.Movies.Single(m => m.Id == movie.Id);

                //can use automapper to do this: Mapper.Map(customer, customerInDb);
                movieInDb.Name = movie.Name;
                movieInDb.ReleaseDate = movie.ReleaseDate;
                movieInDb.Genre = movie.Genre;
                //movieInDb.GenreId = customer.GenreId;
            }

            _context.SaveChanges(); //generates sql statements and runs them at run time
            return RedirectToAction("Index", "Movies");
        }

    }
}