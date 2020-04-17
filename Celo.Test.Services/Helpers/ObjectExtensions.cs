using System;
using System.Collections.Generic;
using System.Text;

namespace Celo.Test.Services.Helpers
{
    public static class CopyProperties
    {        
        /// <summary>
        /// Copies public property values to the destination object.
        /// </summary>
        public static void CopyFrom(this object destination, object source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            var properties =
                source.GetType().GetProperties(
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            foreach (var property in properties)
            {
                property.SetValue(destination, property.GetValue(source));                
            }                
        }
    }
}
