using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Focus.ForDesktop
{
    internal class Storage
    {
        public void SaveIconPositions(IEnumerable<NamedDesktopPoint> iconPositions, IDictionary<string, string> registryValues, string FileName)
        {
            var xDoc = new XDocument(
                new XElement(FileName,
                    new XElement("Icons",
                        iconPositions.Select(p => new XElement("Icon",
                            new XAttribute("x", p.X),
                            new XAttribute("y", p.Y),
                            new XText(p.Name)))),
                    new XElement("Registry",
                        registryValues.Select(p => new XElement("Value",
                            new XElement("Name", new XCData(p.Key)),
                            new XElement("Data", new XCData(p.Value)))))));

            using (var storage = IsolatedStorageFile.GetUserStoreForAssembly())
            {
                if (storage.FileExists(FileName))
                { storage.DeleteFile(FileName); }

                using (var stream = storage.CreateFile(FileName))
                {
                    using (var writer = XmlWriter.Create(stream))
                    {
                        xDoc.WriteTo(writer);
                    }
                }
            }
        }

        public IEnumerable<NamedDesktopPoint> GetIconPositions(string FileName)
        {
            using (var storage = IsolatedStorageFile.GetUserStoreForAssembly())
            {
                if (storage.FileExists(FileName) == false)
                { return new NamedDesktopPoint[0]; }

                using (var stream = storage.OpenFile(FileName, FileMode.Open))
                {
                    using (var reader = XmlReader.Create(stream))
                    {
                        var xDoc = XDocument.Load(reader);

                        return xDoc.Root.Element("Icons").Elements("Icon")
                            .Select(el => new NamedDesktopPoint(el.Value, int.Parse(el.Attribute("x").Value), int.Parse(el.Attribute("y").Value)))
                            .ToArray();
                    }
                }
            }
        }

        public IDictionary<string, string> GetRegistryValues(string FileName)
        {
            using (var storage = IsolatedStorageFile.GetUserStoreForAssembly())
            {
                if (storage.FileExists(FileName) == false)
                { return new Dictionary<string, string>(); }

                using (var stream = storage.OpenFile(FileName, FileMode.Open))
                {
                    using (var reader = XmlReader.Create(stream))
                    {
                        var xDoc = XDocument.Load(reader);

                        return xDoc.Root.Element("Registry").Elements("Value")
                            .ToDictionary(el => el.Element("Name").Value, el => el.Element("Data").Value);
                    }
                }
            }
        }
    }
}
