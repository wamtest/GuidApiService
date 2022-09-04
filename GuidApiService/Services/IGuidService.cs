using GuidApiService.Models;

namespace GuidApiService.Services
{
    public interface IGuidService
    {
        bool IsValidGuid(string guidInfo);
        bool IsValidUser(string user);
        bool IsExpiryInPast(long expire);
        bool IsExpiryValid(long expire);
        GuidInfo BuildGuid(string guid, GuidInput guidInput);
        Task<GuidInfoOutput?> Get(string guid);
        Task<GuidInfoOutput> Save(string guid, long expire, string user);
        Task<GuidInfoOutput> Save(string user);
        Task<Boolean> Delete(string guid);
        Task<GuidInfoOutput> Update(string guid, long expire, string user);
    }
}
