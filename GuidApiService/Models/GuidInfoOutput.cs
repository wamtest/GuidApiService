namespace GuidApiService.Models
{
    /// <summary>
    /// Helper class to map db model to output.
    /// TODO: Replace with Automapper or similar
    /// </summary>
    public class GuidInfoOutput
    {
        /// <summary>
        /// The Guid key
        /// </summary>
        public string? Guid { get; set; }
        /// <summary>
        /// Expire as ticks
        /// </summary>
        public long? Expire { get; set; }
        /// <summary>
        /// Metadata
        /// </summary>
        public string? User { get; set; }

        /// <summary>
        /// Map db model to expected output
        /// TODO: Replace with Automapper or similar
        /// </summary>
        /// <param name="guidInfo"></param>
        /// <returns></returns>
        public GuidInfoOutput? GuidInfoToOutput(GuidInfo? guidInfo)
        {
            if (guidInfo == null)
            {
                return null;
            }

            GuidInfoOutput guidInfoOutput = new()
            {
                Guid = guidInfo.GuidInfoId.ToString("N").ToUpper(),
                Expire = guidInfo.GuidInput?.Expire.Ticks,
                User = guidInfo.GuidInput?.MetaData
            };

            return guidInfoOutput;
        }
    }
}
