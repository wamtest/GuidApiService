using System.ComponentModel.DataAnnotations;

namespace GuidApiService.Models
{
    /// <summary>
    /// The model for guid + metadata
    /// </summary>
    public class GuidInfo
    {
        [Key]
        public Guid GuidInfoId { get; set; }
        public GuidInput? GuidInput { get; set; }
    }
}
