using Microsoft.Extensions.Logging;
using System;
using WebAPI.Models.Context;
using WebAPI.Models.Entity;

namespace WebAPI.Models.Repository
{
    public class SecSoulRepository
    {
        private SecSoulContext _context { get; set; }
        private ILogger _logger { get; set; }
        public SecSoulRepository(SecSoulContext context, ILogger<SecSoulRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public void CreateScanRequest(ScanRequest scanRequest)
        {
            using (var scope = new SecSoulContext())
            {
                try
                {
                    scope.Add(scanRequest);
                    scope.SaveChanges();
                }
                catch (Exception e)
                {
                    _logger.LogError("Error at CreateScanRequest", e);
                }
            }
        }
    }
}
