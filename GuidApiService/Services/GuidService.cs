using GuidApiService.DataProviders;
using GuidApiService.Models;

namespace GuidApiService.Services
{
    /// <summary>
    /// Service with guidInfo operations
    /// </summary>
    public class GuidService : IGuidService
    {
        private readonly IGuidApiRepository<GuidInfo> _guidApiRepository;

        /// <summary>
        /// GuidService Constructor
        /// </summary>
        /// <param name="guidApiRepository"></param>
        public GuidService(IGuidApiRepository<GuidInfo> guidApiRepository)
        {
            _guidApiRepository = guidApiRepository;
        }

        /// <summary>
        /// Generate GuidInfo from given guid string and metadata
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="guidInput"></param>
        /// <returns></returns>
        public GuidInfo BuildGuid(string guid, GuidInput guidInput)
        {
            return new GuidInfo()
            {
                GuidInfoId = new Guid(guid),
                GuidInput = guidInput
            };
        }

        /// <summary>
        /// Delete guidInfo by guid string
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task<bool> Delete(string guid)
        {
            bool didRemove = false;
            GuidInfo guidInfo = await _guidApiRepository.Get(guid);
            if (guidInfo != null)
            {
                await _guidApiRepository.Delete(guid);
                didRemove = true;
            }

            return didRemove;
        }

        /// <summary>
        /// Get guidInfo by guid string
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task<GuidInfoOutput?> Get(string guid)
        {
            GuidInfo guidInfo = await _guidApiRepository.Get(guid);
            GuidInfoOutput? guidInfoOutput = new GuidInfoOutput().GuidInfoToOutput(guidInfo);
            return guidInfoOutput;
        }

        /// <summary>
        /// Is input ticks in the past?
        /// </summary>
        /// <param name="expire"></param>
        /// <returns></returns>
        public bool IsExpiryInPast(long expire)
        {
            DateTime expireAsDateTime = new DateTime(expire);
            return expireAsDateTime < DateTime.Now;
        }

        /// <summary>
        /// Is input ticks valid
        /// </summary>
        /// <param name="expire"></param>
        /// <returns></returns>
        public bool IsExpiryValid(long expire)
        {
            //TODO: Can do more validation on input tick
            return expire != 0;
        }

        /// <summary>
        /// Is input guid string a valid guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public bool IsValidGuid(string guid)
        {
            const int GuidLength = 32;
            if (guid.Length != GuidLength)
            {
                return false;
            }
            foreach (char c in guid)
            {
                if (!((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F')))
                {
                    return false;
                }
            }

            bool isValid = Guid.TryParse(guid, out _);

            return isValid;
        }

        /// <summary>
        /// Is input user a valid username
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool IsValidUser(string user)
        {
            //TODO: Could add more user validation based on requirement
            return !string.IsNullOrWhiteSpace(user);
        }

        /// <summary>
        /// Save guidInfo given guid, expire and user
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="expire"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<GuidInfoOutput?> Create(string guid, long expire, string user)
        {
            GuidInput guidInput = new()
            {
                Expire = new DateTime(expire),
                MetaData = user
            };

            GuidInfo guidInfo = BuildGuid(guid, guidInput);
            GuidInfo createdGuidInfo = await _guidApiRepository.Create(guidInfo);
            GuidInfoOutput? guidInfoOutput = new GuidInfoOutput().GuidInfoToOutput(createdGuidInfo);

            return guidInfoOutput;
        }

        /// <summary>
        /// Save guidInfo given user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<GuidInfoOutput?> Create(string user)
        {
            string newGuid = Guid.NewGuid().ToString();
            GuidInput guidInput = new()
            {
                Expire = DateTime.Now.AddDays(30),
                MetaData = user
            };
            var possibleGuidInfo = await _guidApiRepository.Get(newGuid);

            if (possibleGuidInfo == null)
            {
                GuidInfo guidInfo = BuildGuid(newGuid, guidInput);
                GuidInfo createdGuidInfo = await _guidApiRepository.Create(guidInfo);
                GuidInfoOutput? guidInfoOutput = new GuidInfoOutput().GuidInfoToOutput(createdGuidInfo);
                return guidInfoOutput;
            }
            else
            {
                //TODO: See README.txt for better options than throwing error on guid collison.
                throw new InvalidOperationException("Error generating guid, please try again");
            }
        }

        /// <summary>
        /// Update guidInfo for given guid string
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="expire"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<GuidInfoOutput> Update(string guid, long expire, string user)
        {
            var toUpdate = false;
            GuidInfo possibleGuidInfo = await _guidApiRepository.Get(guid);
            if (possibleGuidInfo != null)
            {
                if (IsExpiryValid(expire) && !IsExpiryInPast(expire))
                {
                    var expireAsDateTime = new DateTime(expire);
                    possibleGuidInfo.GuidInput!.Expire = expireAsDateTime;
                    toUpdate = true;
                }
                if (IsValidUser(user))
                {
                    possibleGuidInfo.GuidInput!.MetaData = user;
                    toUpdate = true;
                }

                if (toUpdate)
                {
                    await _guidApiRepository.Update(possibleGuidInfo);
                }
            }

            GuidInfo guidInfo = await _guidApiRepository.Get(guid);
            GuidInfoOutput? guidInfoOutput = new GuidInfoOutput().GuidInfoToOutput(guidInfo);


            return guidInfoOutput;
        }
    }
}
