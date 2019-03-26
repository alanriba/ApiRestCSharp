using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SAESA.Controllers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class CustomerController : ApiController

    {
        /// <summary>
        /// customer controller class for testing security token
        /// </summary>
        [Authorize]
        [RoutePrefix("api/customers")]
        public class CustomersController : ApiController
        {
            [HttpGet]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            public IHttpActionResult GetId(int id)

            {
                var customerFake = "customer-fake";
                return Ok(customerFake);
            }

            [HttpGet]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            public IHttpActionResult GetAll()

            {
                var customersFake = new string[] { "customer-1", "customer-2", "customer-3" };
                return Ok(customersFake);
            }
        }
    }
}
