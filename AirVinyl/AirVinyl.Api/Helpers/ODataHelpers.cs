using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.OData;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using Microsoft.OData.Core;
using Microsoft.OData.Core.UriParser;

namespace AirVinyl.Api.Helpers
{
    public static class ODataHelpers
    {
        public static bool HasProperty(this object instance, string propertyName)
        {
            var propertyInfo = instance.GetType().GetProperty(propertyName);
            return (propertyInfo != null);
        }

        public static object GetValue(this object instance, string propertyName)
        {
            var propertyInfo = instance.GetType().GetProperty(propertyName);
            if (propertyInfo == null)
            {
                throw new HttpException("Can't find property with name " + propertyName);
            }
            var propertyValue = propertyInfo.GetValue(instance, new object[] { });
            return propertyValue;
        }

        public static IHttpActionResult CreateOKHttpActionResult (this ODataController controller, object propertyValue)
        {
            var okMethod = default(MethodInfo);

            var methods = controller.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var method in methods)
            {
                if (method.Name == "Ok" && method.GetParameters().Length == 1)
                {
                    okMethod = method;
                    break;
                }
            }

            okMethod = okMethod.MakeGenericMethod(propertyValue.GetType());
            var returnValue = okMethod.Invoke(controller, new object[] { propertyValue });
            return (IHttpActionResult)returnValue;
        }

        public static TKey GetKeyValue<TKey>(this HttpRequestMessage request, Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            //get the odata path Ex: ~/entityset/key/navigation/$ref
            var serviceRoot = GetServiceRoot(request);
            var odataPath = request.ODataProperties().PathHandler.Parse(request.ODataProperties().Model, serviceRoot, uri.LocalPath);

            var keySegment = odataPath.Segments.OfType<KeyValuePathSegment>().FirstOrDefault();
            if (keySegment == null)
            {
                throw new InvalidOperationException("The link does not contain a key.");
            }

            var value = ODataUriUtils.ConvertFromUriLiteral(keySegment.Value, ODataVersion.V4);
            return (TKey)value;
        }

        private static string GetServiceRoot(HttpRequestMessage request)
        {
            var urlHelper = request.GetUrlHelper() ?? new UrlHelper(request);
            return urlHelper.CreateODataLink(request.ODataProperties().RouteName, request.ODataProperties().PathHandler, new List<ODataPathSegment>());
        }

        //public static TKey GetKeyFromUri<TKey>(HttpRequestMessage request, Uri uri)
        //{
        //    if (uri == null)
        //    {
        //        throw new ArgumentNullException("uri");
        //    }

        //    var urlHelper = request.GetUrlHelper() ?? new UrlHelper(request);

        //    string serviceRoot = urlHelper.CreateODataLink(
        //        request.ODataProperties().RouteName,
        //        request.ODataProperties().PathHandler, new List<ODataPathSegment>());
        //    var odataPath = request.ODataProperties().PathHandler.Parse(
        //        request.ODataProperties().Model,
        //        serviceRoot, uri.LocalPath);

        //    var keySegment = odataPath.Segments.OfType<KeyValuePathSegment>().FirstOrDefault();
        //    if (keySegment == null)
        //    {
        //        throw new InvalidOperationException("The link does not contain a key.");
        //    }

        //    var value = ODataUriUtils.ConvertFromUriLiteral(keySegment.Value, ODataVersion.V4);
        //    return (TKey)value;
        //}
    }
}