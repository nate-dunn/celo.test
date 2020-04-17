using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Celo.Test.Data.Infrastructure
{
    /// <summary>
    /// A base for all entities which stores additional fields to track creation and modification.
    /// </summary>
    /// <typeparam name="T">The type of the primary key.</typeparam>
    public abstract class TrackableEntity<T> : ITrackableEntity
    {
        [ScaffoldColumn(false)]
        public T Id { get; set; }

        // In real life I would consider using a different set of models for the API itself
        // For this exercise, I will just hide these values from serialization
        [JsonIgnore]
        [ScaffoldColumn(false)]
        public DateTimeOffset CreatedTime { get; set; }

        [JsonIgnore]
        [ScaffoldColumn(false)]
        public DateTimeOffset ModifiedTime { get; set; }

        [JsonIgnore]
        [ScaffoldColumn(false)]
        public DateTimeOffset? DeletedTime { get; set; }
    }
}