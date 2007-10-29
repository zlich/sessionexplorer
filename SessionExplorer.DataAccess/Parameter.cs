using System.Data;

namespace SessionExplorer.DataAccess
{
    /// <summary>
    /// Database parameter abstraction
    /// </summary>
    public class Parameter
    {
        private readonly string key;
        private readonly object value;
        private readonly ParameterDirection direction;

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key
        {
            get { return key; }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public object Value
        {
            get { return value; }
        }

        /// <summary>
        /// Gets the parameter direction.
        /// </summary>
        /// <value>The parameter direction.</value>
        public ParameterDirection ParameterDirection
        {
            get { return direction; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Parameter"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="direction">The direction.</param>
        public Parameter(string key, object value, ParameterDirection direction)
        {
            this.key = key;
            this.value = value;
            this.direction = direction;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Parameter"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public Parameter(string key, object value)
            : this(key, value, System.Data.ParameterDirection.Input) { }
    }
}
