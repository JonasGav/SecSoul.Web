using System.IO;
using System.Xml;
using System.Xml.Xsl;
using Microsoft.Extensions.Options;
using SecSoul.WebAPI.Options;

namespace SecSoul.WebAPI.Helpers
{
    public class XmlConverter
    {
        private OtherSettings _options;
        public XmlConverter(IOptionsMonitor<OtherSettings> optionsMonitor)
        {
            _options = optionsMonitor.CurrentValue;
        }
        public string TransformXMLToHTML(string inputXml)
        {
            string xsltString = File.ReadAllText(_options.NmapXslFile);
            
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            
            XslCompiledTransform transform = new XslCompiledTransform();
            using(XmlReader reader = XmlReader.Create(new StringReader(xsltString), settings)) {
                transform.Load(reader);
            }
            StringWriter results = new StringWriter();
            using(XmlReader reader = XmlReader.Create(new StringReader(inputXml), settings)) {
                transform.Transform(reader, null, results);
            }
            return results.ToString();
        }
    }
}