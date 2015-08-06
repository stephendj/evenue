using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Web.Http;
using Evenue.BackEndAPI.DataObjects;
using Evenue.BackEndAPI.Models;
using Microsoft.WindowsAzure.Mobile.Service;

namespace Evenue.BackEndAPI
{
    public static class WebApiConfig
    {
        public static void Register()
        {
            // Use this class to set configuration options for your mobile service
            ConfigOptions options = new ConfigOptions();

            // Use this class to set WebAPI configuration options
            HttpConfiguration config = ServiceConfig.Initialize(new ConfigBuilder(options));

            // To display errors in the browser during development, uncomment the following
            // line. Comment it out again when you deploy your service for production use.
            // config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            Database.SetInitializer(new MobileServiceInitializer());
        }
    }

    public class MobileServiceInitializer : DropCreateDatabaseIfModelChanges<MobileServiceContext>
    {
        protected override void Seed(MobileServiceContext context)
        {
            List<Event> Events = new List<Event>
            {
                new Event { Id = Guid.NewGuid().ToString(), title = "TechFemme 2015", location = "Bandung", startDate = new DateTime(2015, 9, 12, 9, 0, 0), endDate = new DateTime(2015, 9, 12, 15, 30, 0), desc = "Woman in IT", category = "Technology", imageurl = "none", fee = 0 },
                new Event { Id = Guid.NewGuid().ToString(), title = "TechFemme 2014", location = "Jakarta", startDate = new DateTime(2015, 9, 12, 9, 0, 0), endDate = new DateTime(2015, 9, 12, 15, 30, 0), desc = "Woman in IT", category = "Technology", imageurl = "none", fee = 0 },
            };

            foreach (Event Event in Events)
            {
                context.Set<Event>().Add(Event);
            }

            base.Seed(context);
        }
    }
}

