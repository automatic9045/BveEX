using BveEx.Launching;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;

using UnembeddedResources;

namespace BveEx.Plugins
{
    internal class VehicleConfig
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<VehicleConfig>("Core");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> XmlSchemaValidation { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        private static readonly XmlSchemaSet SchemaSet = new XmlSchemaSet();

        public static readonly VehicleConfig Default = new VehicleConfig();

        static VehicleConfig()
        {
#if DEBUG
            _ = Resources.Value;
#endif

            using (Stream schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(VehicleConfig), "BveExVehicleConfigXmlSchema.xsd"))
            {
                XmlSchema schema = XmlSchema.Read(schemaStream, SchemaValidation);
                SchemaSet.Add(schema);
            }
        }

        public bool DetectSoundIndexConflict { get; private set; } = false;
        public bool DetectPanelValueIndexConflict { get; private set; } = false;
        public string PluginUsingPath { get; private set; } = null;

        private VehicleConfig()
        {
        }

        public static VehicleConfig Resolve(string vehiclePath)
        {
            string directory = Path.GetDirectoryName(vehiclePath);
            VehicleConfig vehicleConfig = TryLoad(
                Path.Combine(directory, Path.GetFileNameWithoutExtension(vehiclePath) + ".VehicleConfig.xml"),
                Path.Combine(directory, "VehicleConfig.xml"));

            return vehicleConfig;


            VehicleConfig TryLoad(params string[] pathArray)
            {
                foreach (string filePath in pathArray)
                {
                    if (File.Exists(filePath))
                    {
                        return LoadFrom(filePath);
                    }
                }

                return Default;
            }
        }

        public static VehicleConfig LoadFrom(string path)
        {
            XDocument doc = XDocument.Load(path, LoadOptions.SetLineInfo);

            if (doc.Root.Name.LocalName == "AtsExVehicleConfig")
            {
                throw new LaunchModeException();
            }

            doc.Validate(SchemaSet, DocumentValidation);
            XElement root = doc.Element("BveExVehicleConfig");

            VehicleConfig result = new VehicleConfig()
            {
                DetectSoundIndexConflict = (bool?)root.Element("DetectSoundIndexConflict") ?? false,
                DetectPanelValueIndexConflict = (bool?)root.Element("DetectPanelValueIndexConflict") ?? false,
                PluginUsingPath = (string)root.Element("PluginUsingPath"),
            };

            return result;
        }

        private static void SchemaValidation(object sender, ValidationEventArgs e) => throw new FormatException(Resources.Value.XmlSchemaValidation.Value, e.Exception);

        private static void DocumentValidation(object sender, ValidationEventArgs e) => throw e.Exception;
    }
}
