using AirVinyl.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace AirVinyl.Api.Controllers
{
    public class VinylRecordsController : ODataController
    {
        AirVinylDbContext _ctx = new AirVinylDbContext();

        [ODataRoute("VinylRecords")]
        public IHttpActionResult GetAllVinylRecords()
        {
            return Ok(_ctx.VinylRecords);
        }

        [HttpGet]
        [ODataRoute("VinylRecords({key})")]
        public IHttpActionResult GetOneVinylRecord([FromODataUri]int key)
        {
            var record = _ctx.VinylRecords.FirstOrDefault(p => p.VinylRecordId == key);

            if (record != null)
                return Ok(record);
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