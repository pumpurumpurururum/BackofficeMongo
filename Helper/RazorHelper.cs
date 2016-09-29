using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RazorEngine;
using RazorEngine.Templating;
using RazorEngine.Configuration;
using System.IO;

namespace Helper
{
    public class RazorHelper
    {
        public static string RenderView(string template, object model)
        {
            var html = Engine.Razor.RunCompile(template, null, model);

            return PreMailer.Net.PreMailer.MoveCssInline(html).Html;
        }

        public static void Start(string baseTemplatePath)
        {
            var config = new TemplateServiceConfiguration();
            config.Debug = true;
            config.TemplateManager = new MyTemplateManager(baseTemplatePath);
            Engine.Razor = RazorEngineService.Create(config);
        }

        public class MyTemplateManager : ITemplateManager
        {
            private readonly string baseTemplatePath;
            public MyTemplateManager(string basePath)
            {
                baseTemplatePath = basePath;
            }

            public ITemplateSource Resolve(ITemplateKey key)
            {
                string template = key.Name;
                string path = Path.Combine(baseTemplatePath, string.Format("{0}{1}", template, ".cshtml"));
                string content = File.ReadAllText(path);
                return new LoadedTemplateSource(content, path);
            }

            public ITemplateKey GetKey(string name, ResolveType resolveType, ITemplateKey context)
            {
                return new NameOnlyTemplateKey(name, resolveType, context);
            }

            public void AddDynamic(ITemplateKey key, ITemplateSource source)
            {
                throw new NotImplementedException("dynamic templates are not supported!");
            }
        }
    }
}
