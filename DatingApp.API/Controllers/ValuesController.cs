using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;
        public ValuesController(DataContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;

        }
        // GET api/values
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var valuesToReturn = await _dbContext.Values.ToListAsync();
            var mappedValues = valuesToReturn.Select(x => _mapper.Map<ValueDTO>(x)).ToList();
            return Ok(mappedValues);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var value = await _dbContext.Values.FirstOrDefaultAsync(x => x.Id == id);
            if (value != null)
                return Ok(_mapper.Map<ValueDTO>(value));
            else
                return NoContent();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
