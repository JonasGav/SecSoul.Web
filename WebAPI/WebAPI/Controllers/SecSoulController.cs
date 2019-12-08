using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SecSoul.Model.Entity;
using SecSoul.Model.Models;
using SecSoul.Model.Repository;
using SecSoul.WebAPI.Models;

namespace SecSoul.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecSoulController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private SecSoulRepository _repository;
        public SecSoulController(SecSoulRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("ScanWebsite")]
        public async void ScanWebsite(ScanWebsiteJson obj)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = await _userManager.FindByIdAsync(userId);
            var request = new ScanRequest()
            {
                RequestDate = DateTime.Now,
                WebsiteUrl = obj.WebsiteUrl,
                WebsiteFtp = obj.WebsiteFtp,
                UserId = user.Id
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