using Microsoft.AspNetCore.Mvc.Filters;

namespace Server.Behavior
{
    public class LoggingActionFilterBehavior : IActionFilter
    {
        private readonly ILogger<LoggingActionFilterBehavior> _logger;
        public LoggingActionFilterBehavior(ILogger<LoggingActionFilterBehavior> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($"Executed action {context.ActionDescriptor.DisplayName}");
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($"Executing action {context.ActionDescriptor.DisplayName}");
        }
    }
}
