using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirVinyl.Model;
using AirVinyl.Web.Models;

namespace AirVinyl.Web.Controllers
{
    public class AirVinylController : Controller
    {
        // GET: AirVinyl
        public ActionResult Index()
        {
            var context = new AirVinylContainer(new Uri("http://localhost:56866/odata"));

            var peopleresponse = context.People.Execute();
            var personresponse = context.People.ByKey(1).GetValue();


            return View(new AirVinylViewModel()
            {
                People = peopleresponse,
                Person = personresponse
            });
        }
    }
}