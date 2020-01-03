using System.Globalization;
using System.Linq;
using Vnn88.Common.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Vnn88.Common.Infrastructure
{
    public class LanguageManager : FluentValidation.Resources.LanguageManager
    {
        public LanguageManager(IOptions<RequestLocalizationOptions> locOptions)
        {
            var cultures = locOptions.Value.SupportedUICultures.Select(m => m.Name).ToList();
            foreach (var culture in cultures)
            {
                AddTranslation(culture, "NotNullValidator",
                    MessageResource.ResourceManager.GetString("NotNullValidator",
                        new CultureInfo(culture)));
                AddTranslation(culture, "LengthValidator",
                    MessageResource.ResourceManager.GetString("LengthValidator",
                        new CultureInfo(culture)));
                AddTranslation(culture, "MaximumLengthValidator",
                    MessageResource.ResourceManager.GetString("MaximumLengthValidator",
                        new CultureInfo(culture)));
                AddTranslation(culture, "MinimumLengthValidator",
                    MessageResource.ResourceManager.GetString("MinimumLengthValidator",
                        new CultureInfo(culture)));
                AddTranslation(culture, "LessThanValidator",
                    MessageResource.ResourceManager.GetString("LessThanValidator",
                        new CultureInfo(culture)));
                AddTranslation(culture, "LessThanOrEqualValidator",
                    MessageResource.ResourceManager.GetString("LessThanOrEqualValidator",
                        new CultureInfo(culture)));
                AddTranslation(culture, "GreaterThanValidator",
                    MessageResource.ResourceManager.GetString("GreaterThanValidator",
                        new CultureInfo(culture)));
                AddTranslation(culture, "GreaterThanOrEqualValidator",
                    MessageResource.ResourceManager.GetString("GreaterThanOrEqualValidator",
                        new CultureInfo(culture)));
                AddTranslation(culture, "EmailValidator",
                    MessageResource.ResourceManager.GetString("EmailValidator",
                        new CultureInfo(culture)));
            }
        }
    }
}