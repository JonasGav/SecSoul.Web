using Microsoft.EntityFrameworkCore;
using SecSoul.Model.Context;

namespace SecSoul.Model.ContextFactories
{
    public class SecSoulContextFactory
    {
        private readonly DbContextOptions _options;
        public SecSoulContextFactory(DbContextOptions options)
        {
            _options = options;
        }

        public SecSoulContext SecSoulContextCreate()
        {
            return new SecSoulContext(_options);
        }
    }
}