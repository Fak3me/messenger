using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Messenger.Service {
    public class ExceptionFilter : ExceptionFilterAttribute {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger) {
            this._logger = logger;
        }

        public override Task OnExceptionAsync(ExceptionContext context) {
            switch (context.Exception) {
                default:
                    var errorResult = new ObjectResult("Service is temporary unavailable. Please try later.") {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                    _logger.LogError(context.Exception, "Unhandled exception. " + context.Exception.Message);
                    context.Result = errorResult;
                    break;
            }

            return base.OnExceptionAsync(context);
        }
    }
}
