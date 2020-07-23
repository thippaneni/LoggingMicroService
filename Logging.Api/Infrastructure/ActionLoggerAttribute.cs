using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logging.Api.Infrastructure
{
    public class ActionLoggerAttribute : ActionFilterAttribute
    {       
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            System.Net.IPAddress ipAddress = context.HttpContext.Connection.RemoteIpAddress;
            var user = context.HttpContext.User;
            var request = context.HttpContext.Request;
            var connection = context.HttpContext.Connection;
            string controllerName = ((ControllerActionDescriptor)context.ActionDescriptor).ControllerName;
            string actionName = ((ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            var correlationId = context.HttpContext.Request.Headers["CorrelationId"];
            var host = context.HttpContext.Request.Headers["Host"];

            if (context.Exception == null)
            {
                //Log
                //    .ForContext("IPAddress", ipAddress)
                //    .ForContext("User", user)
                //    .ForContext("ControllerName", controllerName)
                //    .ForContext("ActionName", actionName)
                //    .Information($"Context_Info: {context} - Request Info: {request} - Connection Info: {connection} - User Info: {user} - IP Address: {ipAddress} - Controller Name: {controllerName} - Action Name: {actionName}");
            }
            else
            {
                //Log
                //    .ForContext("IPAddress", ipAddress)
                //    .ForContext("ControllerName", controllerName)
                //    .ForContext("ActionName", actionName)
                //    .ForContext("ErrorMessage", context.Exception.Message)
                //    .Error($"Context_Info: {context} - Request Info: {request} - Connection Info: {connection} - User Info: {user} - IP Address: {ipAddress} - Controller Name: {controllerName} - Action Name: {actionName}");
            }

            base.OnActionExecuted(context);
        }
    }
}
