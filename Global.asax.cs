using AT.Net.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace EmailTest
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            #region [ Email Configuration ]
            Email.Configuration.Display = "Testing Email";
            Email.Configuration.Username = ConfigurationManager.AppSettings["SMTP_DEFAULT_USERNAME"];
            Email.Configuration.From = ConfigurationManager.AppSettings["SMTP_DEFAULT_EMAIL"];
            Email.Configuration.Password = ConfigurationManager.AppSettings["SMTP_DEFAULT_PASSWORD"];
            Email.Configuration.Port = int.Parse(ConfigurationManager.AppSettings["SMTP_DEFAULT_PORT"]);
            Email.Configuration.UseSSL = bool.Parse(ConfigurationManager.AppSettings["SMTP_DEFAULT_USESSL"]);
            Email.Configuration.SMTP = ConfigurationManager.AppSettings["SMTP_DEFAULT_HOST"];
            Email.Configuration.UseDefaultCredentials = true;
            Email.Interval = int.Parse(ConfigurationManager.AppSettings["INTERVAL"]);
            Email.SendTrials = 10;
            
            string _ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            EmailDB.ConnectionString = _ConnectionString;
            string ErrorMessage = string.Empty;
            if (EmailDB.CheckConnection(ref ErrorMessage))
            {
                EmailDB.ConfigureDatabase();
            }
            Email.StartProcess();
            #endregion
        }
    }
}
