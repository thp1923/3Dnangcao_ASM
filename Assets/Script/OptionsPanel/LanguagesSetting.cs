using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;

public class LanguagesSetting : MonoBehaviour
{
    public TMP_Dropdown languageDropdown;

    private const string LanguagePrefKey = "app_language_index";

    IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;

        languageDropdown.options.Clear();
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            languageDropdown.options.Add(new TMP_Dropdown.OptionData(locale.Identifier.CultureInfo.NativeName));
        }

        int savedlang = PlayerPrefs.GetInt(LanguagePrefKey, 0);
        languageDropdown.value = savedlang;
        languageDropdown.RefreshShownValue();
        languageDropdown.onValueChanged.AddListener(ChangeLanguage);

        // Set locale on start
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[savedlang];
    }

    void ChangeLanguage(int index)
    {
        PlayerPrefs.SetInt(LanguagePrefKey, index);
        PlayerPrefs.Save();
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
