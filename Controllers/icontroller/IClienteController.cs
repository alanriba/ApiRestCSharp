using Swashbuckle.Swagger.Annotations;
using System.Web.Http;

namespace SAESA.Controllers
{
    /// <summary>
    /// Interfaz del controlador del cliente
    /// </summary>
    public interface IClienteController
    {
        [SwaggerOperation("BuscadorCliente")]
        [SwaggerResponse(statusCode: 200, description: "Operación Correcta")]
        [SwaggerResponse(statusCode: 401, description: "Informacion no encontrada")]
        [SwaggerResponse(statusCode: 500, description: "Error en el servidor")]
        [HttpGet]
        [AcceptVerbs("GET")]
        [ActionName("search")]
        [Route("{idEmp}/{idServ}", Order = 1, Name = "search")]
        IHttpActionResult SearchClient(int idEmp, int idServ);

        [HttpGet]
        [Route("/test/{i}")]
        IHttpActionResult GetCustomer(int i);

    }
}