using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using BveEx.PluginHost;
using BveEx.PluginHost.Plugins;
using BveEx.PluginHost.Plugins.Extensions;

namespace BveEx.Plugins.Extensions.Data
{
    public class LoadedExtensions
    {
        private static readonly LoadedExtensions Empty = new LoadedExtensions();

        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(LoadedExtensions));

        private static readonly string FileName = nameof(LoadedExtensions) + ".xml";
        private static readonly string FilePath;

        static LoadedExtensions()
        {
            FilePath = Path.Combine(Path.GetDirectoryName(App.Instance.BveExAssembly.Location), FileName);
        }


        public Extension[] Extensions = new Extension[0];

        internal static LoadedExtensions Load()
        {
            if (!File.Exists(FilePath)) return Empty;

            try
            {
                using (StreamReader sr = new StreamReader(FilePath, Encoding.UTF8))
                {
                    LoadedExtensions result = (LoadedExtensions)Serializer.Deserialize(sr);
                    return result;
                }
            }
            catch
            {
                return Empty;
            }
        }

        internal static LoadedExtensions Create(IEnumerable<PluginBase> extensions)
        {
            LoadedExtensions data = new LoadedExtensions()
            {
                Extensions = extensions.Select(x => new Extension()
                {
                    Path = ExtensionPath.GetRelativePath(x.Location),
                    Class = x.GetType().FullName,
                    IsEnabled = ((ITogglableExtension)x).IsEnabled,
                }).ToArray(),
            };

            return data;
        }

        internal void Save()
        {
            using (StreamWriter sw = new StreamWriter(FilePath, false, Encoding.UTF8))
            {
                Serializer.Serialize(sw, this);
            }
        }
    }
}
