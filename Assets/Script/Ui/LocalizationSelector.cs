using UnityEngine;
using UnityEngine.Localization.Settings;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class LocalizationSelector : MonoBehaviour
{
    public TMP_Dropdown LanguageDropdown;
    private bool isChangingLanguage = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        yield return new WaitUntil(() => LocalizationSettings.InitializationOperation.IsDone);

List<string> languageList = new List<string>();

foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
{
    languageList.Add(locale.LocaleName);
}

LanguageDropdown.AddOptions(languageList);
int savedLanguage = PlayerPrefs.GetInt("Language", 0);

// เปลี่ยน Locale
LocalizationSettings.SelectedLocale =
    LocalizationSettings.AvailableLocales.Locales[savedLanguage];

// ให้ Dropdown แสดงค่าตรงกัน
LanguageDropdown.value = savedLanguage;
LanguageDropdown.RefreshShownValue();

// โหลดภาษาที่เคยเลือก
int language = PlayerPrefs.GetInt("Language", 0);

LanguageDropdown.value = language;
LanguageDropdown.RefreshShownValue();
    }

    public void ChangeLanguage(int languageIndex)
{
    Debug.Log("Language Index = " + languageIndex);

    if (isChangingLanguage)
        return;

    StartCoroutine(SetLanguage(languageIndex));
}

IEnumerator SetLanguage(int languageIndex)
{
    isChangingLanguage = true;

    LocalizationSettings.SelectedLocale =
        LocalizationSettings.AvailableLocales.Locales[languageIndex];

    PlayerPrefs.SetInt("Language", languageIndex);
    PlayerPrefs.Save();

    yield return null;

    isChangingLanguage = false;
}
}
