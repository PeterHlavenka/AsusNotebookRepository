using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace DroidMatika
{
    public class LanguageManager
    {
        public LanguageManager()
        {
            var currentLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            LocalizationResourceManager.Current.CurrentCulture = m_languageMapping.Select(d => d.value).Contains(currentLanguage) ? new CultureInfo(currentLanguage) : new CultureInfo("en-US");
            Strings.Culture = LocalizationResourceManager.Current.CurrentCulture;
        }

        private readonly List<(Func<string> name, string value)> m_languageMapping = new List<(Func<string> name, string value)>()
        {
            (()=> Strings.English, string.Empty),
            (()=> Strings.Czech, "cs"),
            (()=> Strings.Slovak, "sk")
        };
        
        public async Task ChangeLanguage()
        {
            string selectedName = await Application.Current.MainPage.DisplayActionSheet(Strings.SelectLanguage, null, null, m_languageMapping.Select(d => d.name()).ToArray());

            var selectedValue = m_languageMapping.SingleOrDefault(d => d.name() == selectedName).value;

            LocalizationResourceManager.Current.CurrentCulture = string.IsNullOrWhiteSpace(selectedValue)? new CultureInfo("en-US") : new CultureInfo(selectedValue);
            Strings.Culture = LocalizationResourceManager.Current.CurrentCulture;
        }
    }
}