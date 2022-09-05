using System.ComponentModel.DataAnnotations;

namespace GuidApiService.Models
{
    /// <summary>
    /// The model for metadata
    /// </summary>
    public class GuidInput
    {
        /// <summary>
        /// The id
        /// </summary>
        [Key]
        public int GuidInputId { get; set; }
        /// <summary>
        /// The expire as datetime
        /// </summary>
        public DateTime Expire { get; set; }
        /// <summary>
        /// Metadata
        /// </summary>
        public string? MetaData { get; set; }
        /// <summary>
        /// foreign back to parent
        /// </summary>
        public Guid GuidInfoId { get; set; }
    }
}
