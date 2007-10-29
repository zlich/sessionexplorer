using System.Collections.Generic;
using System.Data;

namespace SessionExplorer.DataAccess
{
    /// <summary>
    /// Database parameter collection abstraction
    /// </summary>
    public class Parameters : List<Parameter>
    {
        /// <summary>
        /// Adds a parameter using the specified key and value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(string key, object value)
        {
            Add(new Parameter(key, value));
        }

        /// <summary>
        /// Adds a parameter using the specified key, value and direction.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="parameterDirection">The parameter direction.</param>
        public void Add(string key, object value, ParameterDirection parameterDirection)
        {
            Add(new Parameter(key, value, parameterDirection));
        }
    }
}
