using CodeChallenge.API.Data;
using CodeChallenge.API.Models;
using DatingApp.API.Controllers;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace NUnitTests
{
    [TestFixture]
    public class Tests
    {
        private ValuesController _valuesController;
        private DataContext _dataContext;
        [SetUp]
        public void Setup()
        {
            //using integration testing against real db instead of sqllite
            _dataContext = GetDbContext(false);
            _valuesController = new ValuesController(_dataContext);      
        }

        [Test]
        public void GetServerTime_Success()
        {
            var result = _valuesController.GetServerTime(1);
            Assert.Pass();
            
        }

        [Test]
        public void GetServerTime_Fail()
        {
            Assert.Throws<InvalidOperationException>(() => { _valuesController.GetServerTime(); });
            
        }

        [Test]
        public void GetTimeout()
        {
            Assert.Throws<TimeoutException>(() => { _valuesController.GetTimeout(); });

        }

        [Test]
        public void TestSqlLite()
        {
            var log = new server_response_log
            {
                Starttime = DateTime.Now,
                Endtime = DateTime.Now.AddSeconds(.001),
                ErrorCode = 1,
                ResponseText = string.Empty,
                HttpStatus = 200

            };
            
            _dataContext.Add(log);
            _dataContext.SaveChanges();
           
        }

        [Test]
        public async Task GetMostRecent_Success()
        {
            //going to have to sqlserver since this is testing a rawsql call to a stored procedure
            var result = await _valuesController.GetMostRecent(100000);
            Assert.Pass();

        }

        [Test]
        public async Task GetErrorCodeReport_Success()
        {
            //going to have to sqlserver since this is testing a rawsql call to a view
            var result = await _valuesController.GetErrorCodeReport();
            Assert.Pass();

        }

        public DataContext GetDbContext(bool useSqlite)
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            //need Sqlite db setup
            if (useSqlite)
            {
                // Use Sqlite DB.
                builder.UseSqlite("DataSource=:memory:", x => { });
            }
            else
            {
                //Use SqlServer DB.
               builder.UseSqlServer("Server=.;Database=ae_code_challenge;Trusted_Connection=True;");
            }

            var DataContext = new DataContext(builder.Options);

            if (useSqlite)
            {
                // SQLite needs to open connection to the DB.
                // Not required for in-memory-database and MS SQL.
                DataContext.Database.OpenConnection();
            }

            DataContext.Database.EnsureCreated();

            return DataContext;
        }


    }
}
