using System;
using System.Collections.Generic;
using System.Text;

namespace Celo.Test.Data.Models
{
    /// <summary>
    /// Allow filtering of user results.
    /// </summary>
    public class UserFilter : PageFilter
    {
        public Guid? Id { get; set; }
        public string FirstNameEquals { get; set; }
        public string LastNameEquals { get; set; }
        public string NameContains { get; set; }
    }
}
