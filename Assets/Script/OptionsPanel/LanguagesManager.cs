using UnityEngine;
using UnityEngine.Localization.Settings;
using System.Collections;
public class LanguagesManager : MonoBehaviour
{
    private const string LanguagePrefKey = "app_language_index";

    private void Awake()
    {
        DontDestroyOnLoad(this);
        StartCoroutine((ApplySavedLanguageAtStart()));
    }
    private IEnumerator ApplySavedLanguageAtStart()
    {
        yield return LocalizationSettings.InitializationOperation;

        int savedLang = PlayerPrefs.GetInt(LanguagePrefKey, 0);
        var locales = LocalizationSettings.AvailableLocales.Locales;

        if (savedLang >= 0 && savedLang < locales.Count)
        {
            LocalizationSettings.SelectedLocale = locales[savedLang];
        }
        else
        {
             LocalizationSettings.SelectedLocale = locales[0];
        }
    }
}
