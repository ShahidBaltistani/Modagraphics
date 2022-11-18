using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebApp
{
    public static class Utility
    {
        internal const int SessionTimeout = 20;
      internal const string APIBaseAddress = "http://localhost:59449";
       // internal const string APIBaseAddress = "http://mgapi.vu360solutions.org/";
        private static List<DropdownModel> _countries = null;
        internal static List<DropdownModel> Countries {
            get
            {
                return _countries;
            }
            set
            {
                if(value is null)
                {
                    return;
                }
                else
                {
                    _countries = value;
                    _countries.Insert(0, new DropdownModel { Id = 0, Value = "--Select Country--" });
                }
            }
        }
        static Utility()
        {
            try
            {
                using (var client = new HttpClient() { BaseAddress = new Uri(APIBaseAddress) })
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = Task.Run(async () => await client.GetAsync("Anonymous/Countries")).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Countries = Task.Run(async () => await response.Content.ReadAsAsync<List<DropdownModel>>()).Result;
                    }
                }
            }
            catch { }
        }
    }

    public class ErrorResult : ActionResult
    {
        private ErrorModel Error { get; }
        public ErrorResult(ErrorModel error)
        {
            Error = error;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var errorJsonString = JsonConvert.SerializeObject(Error);
            var errorBytes = Encoding.UTF8.GetBytes(errorJsonString);
            context.HttpContext.Response.StatusCode = 520;
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.ContentEncoding = Encoding.UTF8;
            context.HttpContext.Response.OutputStream.Write(errorBytes, 0, errorBytes.Length);
        }
    }

    public class EmailConfigurationSetup
    {
        private EmailConfigurationSetup() { }

        private static EmailConfigurationsModel _configration;

        public static EmailConfigurationsModel GetConfigration()
        {
            if (_configration == null)
            {
                using (var client = new HttpClient() { BaseAddress = new Uri(Utility.APIBaseAddress) })
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = Task.Run(async () => await client.GetAsync("Anonymous/Get/")).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        _configration = Task.Run(async () => await response.Content.ReadAsAsync<EmailConfigurationsModel>()).Result;
                    }
                    
                }
            }
            return _configration;
        }
    }

}