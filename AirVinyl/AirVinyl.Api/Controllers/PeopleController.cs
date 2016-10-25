using AirVinyl.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.OData;

namespace AirVinyl.Api.Controllers
{
    public class PeopleController : ODataController
    {

        private AirVinylDbContext _ctx = new AirVinylDbContext();

        public IHttpActionResult Get()
        {
            return Ok(_ctx.People);
        }

        public IHttpActionResult Get([FromODataUri]int key)
        {
            var person = _ctx.People.FirstOrDefault(p => p.PersonId == key);

            if (person != null)
                return Ok(person);
            else
                return NotFound();
        }

        protected override void Dispose(bool disposing)
        {
            _ctx.Dispose();
            base.Dispose(disposing);
        }
    }
}