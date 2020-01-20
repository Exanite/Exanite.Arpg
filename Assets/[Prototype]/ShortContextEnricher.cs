using System.IO;
using Serilog.Core;
using Serilog.Events;

namespace Prototype
{
    public class ShortContextEnricher : ILogEventEnricher
    {
        public const string SourceContext = "SourceContext";
        public const string ShortContext = "ShortContext";
        public const string DefaultValue = "Default";

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            string shortened = GetShortened(logEvent.Properties[SourceContext]);

            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(ShortContext, shortened));
        }

        public string GetShortened(LogEventPropertyValue property)
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
