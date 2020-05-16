using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace WebStoreGusev.Infrastructure
{
    public class SampleActionFilter : Attribute, IActionFilter
    {
        // постобработка
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        // предобработка
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //throw new NotImplementedException();
        }
    }
}
