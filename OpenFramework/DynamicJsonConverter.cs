namespace OpenFramework
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Web.Script.Serialization;

    /// <summary>
    /// Implements DynamicJSONConverter class
    /// </summary>
    public sealed class DynamicJsonConverter : JavaScriptConverter
    {
        /// <summary>
        /// Gets supported types
        /// </summary>
        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                return new ReadOnlyCollection<Type>(new List<Type>(new[] { typeof(object) }));
            }
        }

        /// <summary>
        /// Deserialize object
        /// </summary>
        /// <param name="dictionary">Dictionary of data</param>
        /// <param name="type">Type to deserialize</param>
        /// <param name="serializer">Serializer to performs action</param>
        /// <returns>Deserialized object</returns>
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }

            return type == typeof(object) ? new DynamicJsonObject(dictionary) : null;
        }

        /// <summary>
        /// Gets the dictionary of object data
        /// </summary>
        /// <param name="obj">Object to inspect</param>
        /// <param name="serializer">Serializer to performs action</param>
        /// <returns>Serialized object</returns>
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
