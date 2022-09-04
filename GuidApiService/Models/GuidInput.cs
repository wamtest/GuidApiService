using System.ComponentModel.DataAnnotations;

namespace GuidApiService.Models
{
    /// <summary>
    /// The model for metadata
    /// </summary>
    public class GuidInput
    {
        [Key]
        public int GuidInputId { get; set; }
        public DateTime Expire { get; set; }
        public string? MetaData { get; set; }
        public Guid GuidInfoId { get; set; }
    }
}
