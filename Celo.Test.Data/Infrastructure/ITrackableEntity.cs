using System;

namespace Celo.Test.Data.Infrastructure
{
    /// <summary>
    /// An entity that implements this interface can track creation/modification.
    /// </summary>
    public interface ITrackableEntity
    {
        /// <summary>
        /// The time that the record was deleted.
        /// </summary>
        DateTimeOffset? DeletedTime { get; set; }

        /// <summary>
        /// The time that the record was created.
        /// </summary>
        DateTimeOffset CreatedTime { get; set; }

        /// <summary>
        /// The time that the record was created or modified.
        /// </summary>
        DateTimeOffset ModifiedTime { get; set; }
    }
}