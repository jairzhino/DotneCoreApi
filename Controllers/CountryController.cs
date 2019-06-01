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
    public class CountryController : ControllerBase
    {
        private readonly ContextDB _context;
        public CountryController(ContextDB context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<Country>> Get()
        {
            try
            {
                return await Task.Factory.StartNew(() => { return _context.countries.AsEnumerable(); });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<Country> Get(int id)
        {
            try
            {
                Country country=await _context.countries.FirstOrDefaultAsync(p => p.id.Equals(id));
                if(country==null)
                    throw new Exception($"The element with Id:{id} does not exist");
                return country;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<int> Post([FromBody] Country value)
        {
            try
            {
                if (value.id > 0)
                    throw new Exception("Id property must be 0");
                await _context.countries.AddAsync(value);
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
        public async Task<bool> Put(int id, [FromBody] Country value)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception("Model View is Invalid");
                if (value.id == 0)
                    throw new Exception("Id property must be greater than 0");
                Country country = await _context.countries.FirstAsync(p => p.id.Equals(id));
                country.name = value.name;
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
                Country country = await _context.countries.FirstAsync(p => p.id.Equals(id));
                _context.countries.Remove(country);
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
