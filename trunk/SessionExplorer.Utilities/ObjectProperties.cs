using System.Collections.Generic;
using System.Reflection;

namespace SessionExplorer.Utilities
{
    public class ObjectProperties : Dictionary<string, object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectProperties"/> class.
        /// </summary>
        /// <param name="o">The o.</param>
        public ObjectProperties(object o)
        {
            foreach (PropertyInfo propertyInfo in o.GetType().GetProperties())
            {
                try
                {
                    Add(propertyInfo.Name, propertyInfo.GetValue(o, null));
                }
                catch
                {
                    Add(propertyInfo.Name, string.Empty);
                }
            }
        }
    }
}
