using GuidApiService.Models;

namespace GuidApiService.Services
{
    /// <summary>
    /// Service with guidInfo operations
    /// </summary>
    public interface IGuidService
    {
        /// <summary>
        /// Is guid valid?
        /// </summary>
        /// <param name="guidInfo"></param>
        /// <returns></returns>
        bool IsValidGuid(string guidInfo);

        /// <summary>
        /// Is user name metadata valid?
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool IsValidUser(string user);

        /// <summary>
        /// Is expire in the past?
        /// </summary>
        /// <param name="expire"></param>
        /// <returns></returns>
        bool IsExpiryInPast(long expire);

        /// <summary>
        /// Is expire in valid format?
        /// </summary>
        /// <param name="expire"></param>
        /// <returns></returns>
        bool IsExpiryValid(long expire);

        /// <summary>
        /// Get GuidInfo
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        Task<GuidInfoOutput?> Get(string guid);

        /// <summary>
        /// Create GuidInfo given all inputs
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="expire"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<GuidInfoOutput> Create(string guid, long expire, string user);

        /// <summary>
        /// Create GuidInfo for user with generated guid and default expire
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<GuidInfoOutput> Create(string user);

        /// <summary>
        /// Delete GuidInfo
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        Task<Boolean> Delete(string guid);

        /// <summary>
        /// Update GuidInfo
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="expire"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<GuidInfoOutput> Update(string guid, long expire, string user);

        /// <summary>
        /// Build GuidInfo for guid guid and guidInput (expire and user)
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="guidInput"></param>
        /// <returns></returns>
        GuidInfo BuildGuid(string guid, GuidInput guidInput);
    }
}
