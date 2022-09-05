using GuidApiService.Models;
using Microsoft.EntityFrameworkCore;

namespace GuidApiService.DataProviders
{
    /// <summary>
    /// The GuidApi data access layer
    /// </summary>
    public class GuidApiRepository : IGuidApiRepository<GuidInfo>
    {
        private readonly GuidServiceContext _dbContext;

        /// <summary>
        /// GuidApiRepository constructor
        /// </summary>
        /// <param name="dbContext"></param>
        public GuidApiRepository(GuidServiceContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Create GuidInfo in the datastore
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<GuidInfo> Create(GuidInfo entity)
        {
            _dbContext.GuidInfoSet.Add(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        /// <summary>
        /// Delete GuidInfo in the datastore
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task<bool> Delete(string guid)
        {
            var didRemove = false;
            var guidInfo = Get(guid);
            if (guidInfo != null)
            {
                _dbContext.GuidInfoSet.Remove(guidInfo);
                await _dbContext.SaveChangesAsync();
                didRemove = true;
            }

            return didRemove;
        }

        /// <summary>
        /// Get GuidInfo from the datastore
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public GuidInfo Get(string guid)
        {
            Guid guidForSearch = new Guid(guid);
            GuidInfo? possibleGuidInfo = _dbContext.GuidInfoSet.
                Include(g => g.GuidInput).
                FirstOrDefault(g => g.GuidInfoId == guidForSearch);

            return possibleGuidInfo!;
        }

        /// <summary>
        /// Update GuidInfo in the datastore
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task Update(GuidInfo entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
