using System;
using System.Threading;
using System.Threading.Tasks;
using CodeChallenge.API.Data;
using CodeChallenge.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DatingApp.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _context;
        //private readonly ILogger<ValuesController> _logger;

        public ValuesController(DataContext context)
        {
            _context = context;
         
        }

       [HttpGet]
        public async Task<IActionResult> GetValues()
        {
            var values = await _context.server_response_log.ToListAsync();
            return Ok(values);
        }

        [HttpGet("getservertime/{id?}")]
        public IActionResult GetServerTime(int? id = null)
        {
            //var value = new ServerDateModel
            //{
            //    ServerDate = DateTime.UtcNow
            //};
            string value;
            try
            {
                int idparam = id.Value;
                value = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
            }
            catch(Exception ex)
            {
                throw(ex);
            }
           
            return Ok(value);
        }

        [HttpGet("gettimeout")]
        public IActionResult GetTimeout()
        {

            try
            {
                Thread.Sleep(1000);
                throw new TimeoutException("Outa time");
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

      
        // POST api/values
        [HttpPost]
        public void Post([FromBody] server_response_log  response_Log)
        {
            _context.Add(response_Log);
            _context.SaveChanges();
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
