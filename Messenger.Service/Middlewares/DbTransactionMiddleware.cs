using Messenger.Application.Services;

namespace Messenger.Service.Middlewares;

public class DbTransactionMiddleware {
    private readonly RequestDelegate _next;
    private readonly ILogger<DbTransactionMiddleware> _logger;

    public DbTransactionMiddleware(RequestDelegate next, ILogger<DbTransactionMiddleware> logger) {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext, IUnitOfWork unitOfWork) {
        // For HTTP GET opening transaction is not required
        if (httpContext.Request.Method.Equals("GET", StringComparison.CurrentCultureIgnoreCase)) {
            await _next(httpContext);
            return;
        }

        try {
            await unitOfWork.BeginTransaction();
            await _next(httpContext);
            await unitOfWork.CommitTransaction();
        }
        catch (Exception e) {
            _logger.LogError(e, "Transaction failed.");
            await unitOfWork.RollbackTransaction();
            throw;
        }
    }
}