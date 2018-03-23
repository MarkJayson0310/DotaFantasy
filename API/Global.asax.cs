using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

using System.Web.Mvc;
using System.Web.Optimization;

namespace API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);           
        }
        //public void Application_BeginRequest(object sender, EventArgs e)
        //{
        //    HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept, X-Token");

        //    if (Request.HttpMethod == "OPTIONS")
        //    {
        //        HttpContext.Current.Response.StatusCode = 200;
        //        var httpApplication = sender as HttpApplication;
        //        httpApplication.CompleteRequest();
        //    }
        //}
    }
}
