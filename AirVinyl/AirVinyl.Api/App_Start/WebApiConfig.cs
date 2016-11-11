using AirVinyl.Model;
using Microsoft.OData.Edm;
using Swashbuckle.Application;
using Swashbuckle.OData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;

namespace AirVinyl.Api
{
    public static class WebApiConfig
    {

        public static void Register(HttpConfiguration config)
        {
            config
               .EnableSwagger(c =>
               {
                   c.SingleApiVersion("v1", "A title for your API");
                   c.CustomProvider(defaultProvider => new ODataSwaggerProvider(defaultProvider, c, GlobalConfiguration.Configuration));
               })
               .EnableSwaggerUi();

            config.MapODataServiceRoute("ODataRoute", "odata", GetEdmModel());

            config.EnsureInitialized();
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.Namespace = "AirVinyl";
            builder.ContainerName = "AirVinylContainer";

            builder.EntitySet<Person>("People");
            builder.EntitySet<VinylRecord>("VinylRecords");

            //builder.EntityType<Person>().Action("sticazzi"); //    .Parameter<int>("Rating");
            return builder.GetEdmModel();
        }

    }
}
