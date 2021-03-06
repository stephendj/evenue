﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Evenue.MobileAppService.DataObjects;
using Evenue.MobileAppService.Models;

namespace Evenue.MobileAppService.Controllers
{
    [AllowAnonymous]
    public class UserController : TableController<User>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            EvenueBackEndAPIContext context = new EvenueBackEndAPIContext();
            DomainManager = new EntityDomainManager<User>(context, Request);
        }

        // GET tables/User
        [QueryableExpand("Events")]
        public IQueryable<User> GetAllUser()
        {
            return Query();
        }

        [QueryableExpand("Events")]
        // GET tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<User> GetUser(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<User> PatchUser(string id, Delta<User> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/User
        public async Task<IHttpActionResult> PostEvent(User item)
        {
            User current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteUser(string id)
        {
            return DeleteAsync(id);
        }
    }
}