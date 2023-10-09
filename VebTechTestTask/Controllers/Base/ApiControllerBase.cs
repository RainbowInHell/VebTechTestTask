namespace VebTechTestTask.Controllers.Base
{
    using System;
    using System.Net;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging.Abstractions;

    [ApiController]
    [Route("[controller]")]
    public class ApiControllerBase : ControllerBase
    {
        private readonly ILogger logger;

        public ApiControllerBase(ILogger logger)
        {
            this.logger = logger;
        }

        public ILogger Logger => logger ?? NullLogger.Instance;

        protected IActionResult CreateErrorResponse(Exception exception, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            logger.LogError(exception, exception.Message);
            return StatusCode((int)statusCode, exception);
        }

        protected IActionResult CreateErrorResponse(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            logger.LogError(message);
            return StatusCode((int)statusCode, message);
        }

        protected IActionResult CreateOkResponse<T>(T obj)
        {
            return StatusCode((int)HttpStatusCode.OK, obj);

        }

        protected IActionResult CreateOkResponse(HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return StatusCode((int)statusCode);
        }
    }
}