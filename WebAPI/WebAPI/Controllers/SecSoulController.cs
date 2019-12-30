using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private ILogger<SecSoulController> _logger;
        public SecSoulController(SecSoulRepository repository, UserManager<ApplicationUser> userManager, ILogger<SecSoulController> logger)
        {
            _repository = repository;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost]
        [Route("ScanWebsite")]
        public async Task<IActionResult> ScanWebsite(ScanWebsiteJson obj)
        {
            try
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
                Response.StatusCode = 200;
                return new EmptyResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception in SecSoulController, exception: ");
                return BadRequest(new { message = "New scan request was unsuccessful, please try again later" });
            }

            
        }
        // GET api/values
        [HttpGet]
        [Route("GetScanWebsiteList")]
        public ActionResult<IList<ScanRequestGridResult>> Get()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var user = _userManager.FindByIdAsync(userId);
            
            var dbResult = _repository.GetScanRequestByUser(user);
            var results = dbResult.Select(item => new ScanRequestGridResult()
                {
                    Id = item.Id,
                    WebsiteUrl = item.WebsiteUrl,
                    WebsiteFtp = item.WebsiteFtp,
                    RequestDate = item.RequestDate,
                    IsProcessed = item.IsProcessed,
                    NmapScanned = item.ScanNmap.Any()
                })
                .ToList();

            var returnResult = new ActionResult<IList<ScanRequestGridResult>>(results);
            return returnResult;
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