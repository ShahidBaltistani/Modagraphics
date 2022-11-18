using Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class GeoDataController : BaseController
    {
        public GeoDataController(IHttpContextAccessor httpContext) : base(httpContext) { }

        [HttpGet("Countries")]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await ToListAsync<Country>();
            return Ok(countries.Select(x => new DropdownModel { Id = x.Id, Value = $"{x.ShortName} | {x.Name}" }).ToList());
        }

        [HttpGet("StatesOfCountry/{id}")]
        public async Task<IActionResult> GetStatesOfCountry(uint id)
        {
            var states = await ToListAsync<State>(x => x.CountryId == id);
            return Ok(states.Select(x => new DropdownModel { Id = x.Id, Value = x.Name }).ToList());
        }

        [HttpGet("CitiesOfState/{id}")]
        public async Task<IActionResult> GetCitiesOfState(uint id)
        {
            var cities = await ToListAsync<City>(x => x.StateId == id);
            return Ok(cities.Select(x => new DropdownModel { Id = x.Id, Value = x.Name }).ToList());
        }

        [HttpGet("State/{id}")]
        public async Task<IActionResult> GetState(uint id)
        {
            var city = await FirstOrDefaultAsync<City>(x => x.Id == id);
            return Ok(city.StateId);
        }

        [HttpGet("Country/{id}")]
        public async Task<IActionResult> GetCountry(uint id)
        {
            var state = await FirstOrDefaultAsync<State>(x => x.Id == id);
            return Ok(state.CountryId);
        }
    }
}
