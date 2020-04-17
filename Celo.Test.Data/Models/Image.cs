using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;
using Celo.Test.Data.Infrastructure;

namespace Celo.Test.Data.Models
{
    public class Image : TrackableEntity<Guid>
    {
        /// <summary>
        /// The ID of the user that this image is for.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The display order for the images.
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Whether this image is a thumbnail or a full size.
        /// </summary>
        public bool IsThumbnail { get; set; }

        /// <summary>
        /// The image data.
        /// </summary>
        public byte[] Blob { get; set; }

        // Related entities

        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
