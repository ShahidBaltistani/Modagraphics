using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.ActionResults;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class AnonymousController : ControllerBase
    {
        [HttpGet("Countries")]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                using (var entitites = new RapidusContextFactory().CreateDbContext(null))
                {
                    var countries = await entitites.Country.Select(x => new DropdownModel
                    {
                        Id = x.Id,
                        Value = x.ShortName + " | " + x.Name
                    }).ToListAsync();
                    return Ok(countries);
                }
            }
            catch (System.Exception ex)
            {
                return new MethodFailureObjectResult(ex);
            }
        }

        [HttpGet("Logs")]
        public async Task<IActionResult> GetLogs()
        {
            try
            {
                using (var entitites = new RapidusContextFactory().CreateDbContext(null))
                {
                    var logs = await entitites.Logs.Select(x => new
                    {
                        IPAddress = x.IP,
                        BrowserInfo = x.UserAgent,
                        LoginDateTime = x.EnteredOn.ToString("hh:mm:ss tt - dd/MM/yyyy")
                    }).ToListAsync();
                    return Ok(logs);
                }
            }
            catch (System.Exception ex)
            {
                return new MethodFailureObjectResult(ex);
            }
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            try
            {
                using (var entitites = new RapidusContextFactory().CreateDbContext(null))
                {
                    var Configuration = await entitites.EmailConfigurations.FirstOrDefaultAsync();
                    return Ok(Configuration);
                }
            }
            catch (System.Exception ex)
            {
                return new MethodFailureObjectResult(ex);
            }

        }
    }
}
