﻿using Application.Exceptions;
using Application.Wrappers.Concrete;
using FluentValidation;
using FluentValidation.Results;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace WebAPI.Infrastructure.Middleware
{
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception e)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            string message = "Internal Server Error";

            //if (e.GetType() == typeof(ValidationException))
            //{
            //    IEnumerable<ValidationFailure> errors;
            //    errors = ((ValidationException)e).Errors;
            //    httpContext.Response.StatusCode = 400;
            //    var validationerror = JsonConvert.SerializeObject(new ErrorResponse(400, errors.Select(x => x.ErrorMessage).ToList()), new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            //    return httpContext.Response.WriteAsync(validationerror);
            //}


            //if (e.InnerException is ApiException || e.GetType() == typeof(ApiException))
            //{
            //    var ex = e.InnerException != null ? (ApiException)e.InnerException : (ApiException)e;
            //    httpContext.Response.StatusCode = ex.StatusCode;
            //    var apiException = new ErrorResponse(ex.StatusCode, ex.Errors);
            //    var serializedApiError = JsonConvert.SerializeObject(apiException, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            //    _logger.LogInformation("{@ErrorResponse}", apiException);
            //    return httpContext.Response.WriteAsync(serializedApiError);
            //}

            List<string> exceptions = new List<string>();

            if (e.InnerException != null)
            {
                exceptions.Add(e.InnerException.ToString());
                if (e.InnerException.Message != null)
                {
                    exceptions.Add(e.InnerException.Message);
                }
                else if (e.InnerException.InnerException.Message != null)
                {
                    exceptions.Add(e.InnerException.InnerException.Message);
                }
            }
            else if (e.Message != null)
            {
                exceptions.Add(e.Message);
            }
            var errorlogDetail = new
            {
                Errors = exceptions,
            };
            _logger.LogError("Unknown error occurred {@Error}", errorlogDetail);
            var error = JsonConvert.SerializeObject(new ErrorResponse(httpContext.Response.StatusCode, message), new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            return httpContext.Response.WriteAsync(error);
        }
    }
}