using Microsoft.Extensions.Logging;
using SecSoul.Model.Context;
using SecSoul.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SecSoul.Model.ContextFactories;
using SecSoul.Model.Models;

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

        public void UpdateUnprocessedScanRequest(IList<ScanRequest> unprocessedRequests)
        {
            try
            {
                using (var scope = _contextFactory.SecSoulContextCreate())
                {
                    scope.ScanRequest.UpdateRange(unprocessedRequests);
                    scope.SaveChanges();
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error at CreateScanRequest", e);
                throw;
            }
        }
        public ScanNmap UpdateUnprocessedScanRequest(int Id)
        {
            try
            {
                using (var scope = _contextFactory.SecSoulContextCreate())
                {
                    return scope.ScanNmap.FirstOrDefault(x => x.Id == Id);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error at CreateScanRequest", e);
                throw;
            }
        }

        public IList<ScanRequest> GetScanRequestByUser(Task<ApplicationUser> user)
        {
            using (var scope = _contextFactory.SecSoulContextCreate())
            {
                return scope.ScanRequest.Where(x => x.UserId == user.Result.Id).Include(x => x.ScanNmap).ToList();
            }
        }
    }
}
