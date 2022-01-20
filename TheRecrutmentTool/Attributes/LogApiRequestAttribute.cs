namespace TheRecrutmentTool.Attributes
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class LogApiRequestAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            var request = actionExecutedContext.HttpContext.Request;
            var response = actionExecutedContext.HttpContext.Response;
            var actionContext = actionExecutedContext.HttpContext;

            Console.WriteLine(actionContext.Request.RouteValues.Keys.First().ToString());

            // Log API Call
        }
    }
}
