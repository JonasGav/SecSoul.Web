using Microsoft.Extensions.Logging;
using SecSoul.Model.Context;
using SecSoul.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using SecSoul.Model.ContextFactories;

namespace SecSoul.Model.Repository
{
    public class SecSoulRepository
    {
        private SecSoulContextFactory _contextFactory;
        private ILogger _logger { get; set; }
        public SecSoulRepository(SecSoulContextFactory contextFactory, ILogger<SecSoulRepository> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }
        public void CreateScanRequest(ScanRequest scanRequest)
        {
            try
            {
                using (var scope = _contextFactory.SecSoulContextCreate())
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
                using (var scope = _contextFactory.SecSoulContextCreate())
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
