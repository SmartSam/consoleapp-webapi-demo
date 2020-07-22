using System;
using System.Collections.Generic;
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

        [HttpGet("getmostrecent/{timespan_seconds}")]
        public async Task<IActionResult> GetMostRecent(int timespan_seconds)
        {
            var returnValue = new List<recentResponseLog>();
            try
            {
               returnValue = await _context.recentResponseLogs
                    .FromSqlRaw("EXECUTE dbo.spMostRecentResponseLogs {0}", timespan_seconds).AsNoTracking().ToListAsync();

            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return Ok(returnValue);
        }

        [HttpGet("geterrorcodereport")]
        public async Task<IActionResult> GetErrorCodeReport()
        {
            var returnValue = new List<errorCodeLog>();
            try
            {

                returnValue = await _context.errorCodeLogs
                    .FromSqlRaw("SELECT * FROM dbo.ServerResponseErrors").AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return Ok(returnValue);
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
