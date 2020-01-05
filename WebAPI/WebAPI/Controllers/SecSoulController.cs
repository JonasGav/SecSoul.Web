using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using SecSoul.Model.Entity;
using SecSoul.Model.Models;
using SecSoul.Model.Repository;
using SecSoul.WebAPI.Helpers;
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
        private XmlConverter _xmlConverter;
        private HtmlHelper _htmlHelper;
        public SecSoulController(SecSoulRepository repository, UserManager<ApplicationUser> userManager, ILogger<SecSoulController> logger, XmlConverter xmlConverter, HtmlHelper htmlHelper)
        {
            _repository = repository;
            _userManager = userManager;
            _logger = logger;
            _xmlConverter = xmlConverter;
            _htmlHelper = htmlHelper;
        }

        [HttpPost]
        [Route("ScanWebsite")]
        public async Task<IActionResult> ScanWebsite(ScanWebsiteJson obj)
        {
            try
            {
                Uri uriResult;
                if (Uri.TryCreate(obj.WebsiteUrl, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                {
                    string userId = User.Claims.First(c => c.Type == "UserID").Value;
                    var user = await _userManager.FindByIdAsync(userId);
                    var request = new ScanRequest()
                    {
                        RequestDate = DateTime.Now,
                        WebsiteUrl = obj.WebsiteUrl,
                        WebsiteFtp = obj.WebsiteFtp,
                        FtpUsername = obj.FtpUsername,
                        FtpPassword = obj.FtpPassword,
                        UserId = user.Id
                    };
                    _repository.CreateScanRequest(request);
                    Response.StatusCode = 200;
                    return new EmptyResult();
                }
                else
                {
                    return BadRequest();
                }
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
                    NmapScanned = item.ScanNmap.Any(),
                    DirbScanned = item.ScanDirb.Any(),
                    VirusTotalScanned = item.ScanVirusTotal.Any()
                })
                .ToList();

            var returnResult = new ActionResult<IList<ScanRequestGridResult>>(results);
            return returnResult;
        }
        [HttpGet]
        [Route("Download")]
        public IActionResult DownloadFile(int requestId)
        {
            var data = _repository.GetScanRequestById(requestId);
            if (data.ScanNmap.FirstOrDefault() == null)
            {
                return BadRequest();
            }
            
            var htmlContent = _xmlConverter.TransformXMLToHTML(data.ScanNmap.FirstOrDefault()?.ScanResult);

            htmlContent = _htmlHelper.AddDirbResults(htmlContent, data);
            htmlContent = _htmlHelper.AddVirusTotalResults(htmlContent, data);
            htmlContent = _htmlHelper.AddHashCheckResults(htmlContent, data);
            

            
            byte[] buff = Encoding.ASCII.GetBytes(htmlContent);

            // get the file and convert it into a bytearray
            return new FileContentResult(buff, new 
                MediaTypeHeaderValue("application/octet"))
            {
                FileDownloadName = "result.html"
            };
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