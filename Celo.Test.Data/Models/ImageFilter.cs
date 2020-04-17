using System;

namespace Celo.Test.Data.Models
{
    public class ImageFilter : PageFilter
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public bool? IsThumbnail { get; set; }
    }
}