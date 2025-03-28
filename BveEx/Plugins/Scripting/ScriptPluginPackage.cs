﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

using UnembeddedResources;

using BveEx.PluginHost.Plugins;
using BveEx.Launching;

namespace BveEx.Plugins.Scripting
{
    internal partial class ScriptPluginPackage : IPluginPackage
    {
        private class ResourceSet
        {
            private readonly ResourceLocalizer Localizer = ResourceLocalizer.FromResXOfType<ScriptPluginPackage>("Core");

            [ResourceStringHolder(nameof(Localizer))] public Resource<string> XmlSchemaValidation { get; private set; }

            public ResourceSet()
            {
                ResourceLoader.LoadAndSetAll(this);
            }
        }

        private static readonly Lazy<ResourceSet> Resources = new Lazy<ResourceSet>();

        protected static readonly XmlSchemaSet SchemaSet = new XmlSchemaSet();

        static ScriptPluginPackage()
        {
#if DEBUG
            _ = Resources.Value;
#endif

            using (Stream schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(typeof(ScriptPluginPackage), "BveExScriptPluginPackageManifestXmlSchema.xsd"))
            {
                XmlSchema schema = XmlSchema.Read(schemaStream, SchemaValidation);
                SchemaSet.Add(schema);
            }
        }

        public Identifier Identifier { get; }
        public ScriptLanguage ScriptLanguage { get; }

        public string Location { get; }
        public string Title { get; }
        public string Version { get; }
        public string Description { get; }
        public string Copyright { get; }

        public string ConstructorScriptPath { get; }
        public string DisposeScriptPath { get; }
        public string OnScenarioCreatedScriptPath { get; }
        public string OnStartedScriptPath { get; }
        public string TickScriptPath { get; }

        protected ScriptPluginPackage(Identifier identifier, ScriptLanguage scriptLanguage, InformationBuilder informationBuilder, ScriptsBuilder scriptsBuilder)
        {
            Identifier = identifier;
            ScriptLanguage = scriptLanguage;

            Location = informationBuilder.Location;
            Title = informationBuilder.Title;
            Version = informationBuilder.Version;
            Description = informationBuilder.Description;
            Copyright = informationBuilder.Copyright;

            ConstructorScriptPath = scriptsBuilder.ConstructorScriptPath;
            DisposeScriptPath = scriptsBuilder.DisposeScriptPath;
            OnScenarioCreatedScriptPath = scriptsBuilder.OnScenarioCreatedScriptPath;
            OnStartedScriptPath = scriptsBuilder.OnStartedScriptPath;
            TickScriptPath = scriptsBuilder.TickScriptPath;
        }

        public static ScriptPluginPackage Load(Identifier identifier, ScriptLanguage scriptLanguage, string path)
        {
            XDocument doc = XDocument.Load(path, LoadOptions.SetLineInfo);

            if (doc.Root.Name.LocalName == "AtsExScriptPluginPackageManifest")
            {
                throw new LaunchModeException();
            }

            doc.Validate(SchemaSet, DocumentValidation);
            XElement root = doc.Element("BveExScriptPluginPackageManifest");

            XElement infoElement = root.Element("Info");
            InformationBuilder informationBuilder = CreateInformationBuilder(infoElement, path);

            XElement scriptsElement = root.Element("Scripts");
            ScriptsBuilder scriptsBuilder = CreateScriptsBuilder(scriptsElement, Path.GetDirectoryName(path));

            return new ScriptPluginPackage(identifier, scriptLanguage, informationBuilder, scriptsBuilder);
        }

        private static InformationBuilder CreateInformationBuilder(XElement infoElement, string path)
        {
            string title = GetElementValue("Title");
            InformationBuilder builder = new InformationBuilder(path, title)
            {
                Version = GetElementValue("Version"),
                Description = GetElementValue("Description"),
                Copyright = GetElementValue("Copyright"),
            };

            return builder;


            string GetElementValue(string name) => (string)infoElement.Element(name);
        }

        private static ScriptsBuilder CreateScriptsBuilder(XElement scriptsElement, string baseDirectory)
        {
            ScriptsBuilder builder = new ScriptsBuilder
            {
                ConstructorScriptPath = GetElementPath("Constructor"),
                DisposeScriptPath = GetElementPath("Dispose"),
                OnScenarioCreatedScriptPath = GetElementPath("OnScenarioCreated"),
                OnStartedScriptPath = GetElementPath("OnStarted"),
                TickScriptPath = GetElementPath("Tick"),
            };

            return builder;

            string GetElementPath(string name)
            {
                string path = (string)scriptsElement.Element(name)?.Attribute("Path");
                return path is null ? null : Path.Combine(baseDirectory, path);
            }
        }

        private static void SchemaValidation(object sender, ValidationEventArgs e) => throw new FormatException(Resources.Value.XmlSchemaValidation.Value, e.Exception);

        private static void DocumentValidation(object sender, ValidationEventArgs e) => throw e.Exception;
    }
}
