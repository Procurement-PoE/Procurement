using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using POEApi.Infrastructure;
using System.ComponentModel;

namespace Procurement.Controls
{
    internal class ForumExportTemplateReader
    {
        private const string templateFileName = "ForumExportTemplate.txt";
        private static string currentTemplateName;

        internal static event PropertyChangedEventHandler OnTemplateReloaded;

        internal static string GetTemplate(string templateName)
        {
            try
            {
                currentTemplateName = templateName ?? templateFileName;
                string template = getTemplateFromDisk(templateName ?? templateFileName);

                if (template != string.Empty)
                {
                    if (OnTemplateReloaded != null)
                        OnTemplateReloaded(template, null);

                    return template;
                }

                template = getDefaultTemplate();
                saveTemplate(template);

                if (OnTemplateReloaded != null)
                    OnTemplateReloaded(template, null);

                return template;
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
                var message = "Failed to load ForumExportTemplate!";
                Logger.Log(message);
                throw new Exception(message);
            }
        }

        internal static void SaveTemplate(string Template)
        {
            saveTemplate(Template);
        }

        private static void saveTemplate(string defaultTemplate)
        {
            try
            {
                File.WriteAllText(currentTemplateName, defaultTemplate);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
                Logger.Log("Failed saving ForumExportTemplate to disk!");
            }
        }

        private static string getDefaultTemplate()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Procurement.ForumExportTemplate.txt"))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private static string getTemplateFromDisk(string templateName)
        {
            if (!File.Exists(templateName))
                return string.Empty;

            return System.IO.File.ReadAllText(templateName);
        }
    }
}
