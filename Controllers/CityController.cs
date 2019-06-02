using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ContextDB _context;
        public CityController(ContextDB context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<City>> Get()
        {
            try
            {
                return await Task.Factory.StartNew(() => { return _context.cities.AsEnumerable(); });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<City> Get(int id)
        {
            try
            {
                City city=await _context.cities.FirstOrDefaultAsync(p => p.id.Equals(id));
                if(city==null)
                    throw new Exception($"The element with Id:{id} does not exist");
                return city;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<int> Post([FromBody] City value)
        {
            try
            {
                if (value.id > 0)
                    throw new Exception("Id property must be 0");
                await _context.cities.AddAsync(value);
                await _context.SaveChangesAsync();
                return value.id;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<bool> Put(int id, [FromBody] City value)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception("Model View is Invalid");
                if (value.id == 0)
                    throw new Exception("Id property must be greater than 0");
                City city = await _context.cities.FirstAsync(p => p.id.Equals(id));
                city.name = value.name;
                city.countryId = value.countryId;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            try
            {
                if (id == 0)
                    throw new Exception("Id property must be greater than 0");
                City city = await _context.cities.FirstAsync(p => p.id.Equals(id));
                _context.cities.Remove(city);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
