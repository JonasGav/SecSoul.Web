﻿using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SecSoul.Model.Entity;
using SecSoul.Model.Repository;

namespace SecSoul.Core.DbService
{
    public class SecSoulService
    {
        private SecSoulRepository _repository;
        private ILogger<SecSoulService> _logger;
        public SecSoulService(SecSoulRepository repository, ILogger<SecSoulService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public IList<ScanRequest> GetUnprocessedScanRequest()
        {
            try
            {
                return _repository.GetUnprocessedScanRequest();
            }
            catch (Exception e)
            {
                _logger.LogError("");
                throw;
            }
        }

        public void UpdateUnprocessedScanRequestList(IList<ScanRequest> unprocessedRequests)
        {
            try
            {
                _repository.UpdateUnprocessedScanRequestList(unprocessedRequests);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public void UpdateUnprocessedScanRequest(ScanRequest unprocessedRequest)
        {
            try
            {
                _repository.UpdateUnprocessedScanRequest(unprocessedRequest);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
