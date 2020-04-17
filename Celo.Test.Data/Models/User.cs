using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Celo.Test.Data.Infrastructure;

namespace Celo.Test.Data.Models
{
    /// <summary>
    /// The user entity.
    /// </summary>
    public class User : TrackableEntity<Guid>
    {
        [Required]
        [StringLength(256)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(256)]
        public string LastName { get; set; }

        [Required]
        [StringLength(1024)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression("[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,32}")]
        public string Email { get; set; }

        [Required]
        [Range(typeof(DateTimeOffset), "1/1/1500", "31/12/2099")]
        public DateTimeOffset DateOfBirth { get; set; }

        [StringLength(64)]
        [RegularExpression("^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\\s\\./0-9]*$")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The collection of profile images for this user.
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<Image> ProfileImages { get; set; }
    }
}
