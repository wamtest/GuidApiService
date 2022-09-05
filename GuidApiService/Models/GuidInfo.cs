using System.ComponentModel.DataAnnotations;

namespace GuidApiService.Models
{
    /// <summary>
    /// The model for guid + metadata
    /// </summary>
    public class GuidInfo
    {
        /// <summary>
        /// The guid key
        /// </summary>
        [Key]
        public Guid GuidInfoId { get; set; }
        /// <summary>
        /// Metadata
        /// </summary>
        public GuidInput? GuidInput { get; set; }
    }
}
