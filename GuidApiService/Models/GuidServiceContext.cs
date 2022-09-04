using Microsoft.EntityFrameworkCore;

namespace GuidApiService.Models
{
    /// <summary>
    /// The dbcontext for GuidApiService
    /// </summary>
    public class GuidServiceContext : DbContext
    {
        public GuidServiceContext(DbContextOptions<GuidServiceContext> options) : base(options)
        {
        }

        /// <summary>
        /// The Dbset
        /// </summary>
        public DbSet<GuidInfo> GuidInfoSet { get; set; } = null!;
    }
}
