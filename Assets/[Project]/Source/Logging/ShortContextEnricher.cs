using System.IO;
using Serilog.Core;
using Serilog.Events;

namespace Exanite.Arpg.Logging
{
    /// <summary>
    /// Adds a shorter non-namespaced property, 'ShortContext', based off of the <see cref="LogEvent"/>'s 'SourceContext' property
    /// </summary>
    public class ShortContextEnricher : ILogEventEnricher
    {
        /// <summary>
        /// SourceContext property id
        /// </summary>
        public const string SourceContext = "SourceContext";
        /// <summary>
        /// ShortContext property id
        /// </summary>
        public const string ShortContext = "ShortContext";
        /// <summary>
        /// Default value if the SourceContext property does not exist
        /// </summary>
        public const string DefaultValue = "Default";

        /// <summary>
        /// Adds a shorter non-namespaced property, 'ShortContext', based off of the <see cref="LogEvent"/>'s 'SourceContext' property
        /// </summary>
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            string shortened = GetShortened(logEvent.Properties[SourceContext]);

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(ShortContext, shortened));
        }

        private string GetShortened(LogEventPropertyValue property)
        {
            if (property == null)
            {
                return DefaultValue;
            }

            using (StringWriter writer = new StringWriter())
            {
                property.Render(writer);

                string original = writer.ToString().Trim('"');
                int startIndex = original.LastIndexOf('.') + 1;

                return original.Substring(startIndex);
            }
        }
    }
}
