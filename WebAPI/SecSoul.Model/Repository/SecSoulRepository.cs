using Microsoft.Extensions.Logging;
using SecSoul.Model.Context;
using SecSoul.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

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
            try
            {
                using (var scope = new SecSoulContext())
                {

                    scope.Add(scanRequest);
                    scope.SaveChanges();
                }

            }
            catch (Exception e)
            {
                _logger.LogError("Error at CreateScanRequest", e);
            }
        }
        public IList<ScanRequest> GetUnprocessedScanRequest()
        {
            try
            {
                using (var scope = new SecSoulContext())
                {

                    return scope.ScanRequest.Where(x => x.IsProcessed == false).ToList();

                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error at CreateScanRequest", e);
                throw;
            }
            
        }
    }
}
