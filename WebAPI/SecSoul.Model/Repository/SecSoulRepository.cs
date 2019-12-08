using System;
using Microsoft.Extensions.Logging;
using SecSoul.Model.Context;
using SecSoul.Model.Entity;

namespace SecSoul.Model.Repository
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
