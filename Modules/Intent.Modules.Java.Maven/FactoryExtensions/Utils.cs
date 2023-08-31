using System.Xml.Linq;

namespace Intent.Modules.Java.Maven.FactoryExtensions
{
    internal static class Utils
    {
        public static XElement WithoutNamespaces(this XElement element, XNamespace @namespace)
        {
            foreach (var e in element.DescendantsAndSelf())
            {
                e.Name = @namespace + e.Name.LocalName; // remove namespaces
            }

            return element;
        }
    }
}
