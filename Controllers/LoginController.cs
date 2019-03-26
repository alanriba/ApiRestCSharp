using SAESA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace SAESA.Controllers
{

        /// <summary>
        /// login controller class for authenticate users
        /// </summary>
        [AllowAnonymous]
        [RoutePrefix("api/v2/login")]
        public class LoginController : ApiController
        {
            [HttpGet]
            [Route("echoping")]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            public IHttpActionResult EchoPing()

            {
                return Ok(true);
            }

            [HttpGet]
            [Route("echouser")]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            public IHttpActionResult EchoUser()

            {
                var identity = Thread.CurrentPrincipal.Identity;
                return Ok($" IPrincipal-user: {identity.Name} - IsAuthenticated: {identity.IsAuthenticated}");
            }

            [HttpPost]
            [Route("authenticate")]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            public IHttpActionResult Authenticate(LoginRequest login)

            {
                if (login == null)
                    throw new HttpResponseException(HttpStatusCode.BadRequest);

                //TODO: Validate credentials Correctly, this code is only for demo !!
                bool isCredentialValid = (login.Password == "123456");
                if (isCredentialValid)
                {
                    var token = TokenGenerator.GenerateTokenJwt(login.Username);
                    return Ok(token);
                }
                else
                {
                    return Unauthorized();
                }
            }
        }
    }

