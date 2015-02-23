using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DbSelector
{
    class Program
    {
        private static readonly string[] PossibleWebConfigPaths =
        {
            "web.config",
            "C:/vss/2010 Projects/WebApps/ANAGL/Web.config",
            "C:/SourceSafe/2010 Projects/WebApps/ANAGL/Web.config"
        }; 

        static void Main(string[] args)
        {
            var dbNumber = args[0];
            var webConfigPath = PossibleWebConfigPaths.FirstOrDefault(File.Exists);
            if (webConfigPath == null)
                Error("No web.config found.");
            var doc = XDocument.Load(webConfigPath);
            if (doc.Root == null)
                Error("web.config is incompatible.");
            var appSettings = doc.Root.Element("appSettings");
            if (appSettings == null)
                Error("appSettings not found in web.config.");
            var adds = appSettings.Elements("add").ToArray();
            if (!adds.Any())
                Error("No add elements found in web.config.");
            var debugVersion = adds.FirstOrDefault(e => e.Attribute("key").Value == "DebugVersion");
            if (debugVersion == null)
                Error("DebugVersion not found in web.config.");
            debugVersion.Attribute("value").SetValue(dbNumber);
            doc.Save(webConfigPath);
        }

        private static void Error(string errorMessage)
        {
            Console.WriteLine("DebugVersion not found in web.config.");
            Console.ReadKey();
            Environment.Exit(1);
        }
    }
}
