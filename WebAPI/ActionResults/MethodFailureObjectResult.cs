using Microsoft.AspNetCore.Mvc;
using System;
using WebAPI.Extension;

namespace WebAPI.ActionResults
{
    public class MethodFailureObjectResult : ObjectResult
    {
        public MethodFailureObjectResult(object value) : base(value)
        {
            StatusCode = 520;
        }

        public MethodFailureObjectResult(Exception ex) : this(ex.ErrorObject())
        {
            StatusCode = 520;
        }
    }
}
