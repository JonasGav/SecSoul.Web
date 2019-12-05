using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebAPI.Models;
using WebAPI.Models.Entity;
using WebAPI.Models.Repository;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecSoulController : ControllerBase
    {
        private SecSoulRepository _repository;
        public SecSoulController(SecSoulRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("ScanWebsite")]
        public void ScanWebsite(ScanWebsiteJson obj)
        {
            var request = new ScanRequest()
            {
                RequestDate = DateTime.Now,
                WebsiteUrl = obj.WebsiteUrl,
                WebsiteFtp = obj.WebsiteFtp
            };
            _repository.CreateScanRequest(request);
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
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