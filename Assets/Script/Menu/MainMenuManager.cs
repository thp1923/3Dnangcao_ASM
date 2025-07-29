using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject loadGamePanel;
    public GameObject overwritePanel;

    public TMP_Text[] slotTexts;

    string[] usedSlots = new string[4];
    int usedSlotCount = 0;

    void Start()
    {
        loadGamePanel.SetActive(false);
        overwritePanel.SetActive(false);
    }

    public void OnNewGameClicked()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(),
            result =>
            {
                usedSlotCount = 0;

                for (int i = 1; i <= 4; i++)
                {
                    string key = $"SaveSlot{i}";
                    if (result.Data.ContainsKey(key))
                    {
                        usedSlots[i - 1] = key;
                        usedSlotCount++;
                    }
                    else
                    {
                        StartNewGame(key);
                        return;
                    }
                }

                if (usedSlotCount >= 4)
                {
                    overwritePanel.SetActive(true);
                }
            },
            error => Debug.LogError(error.GenerateErrorReport()));
    }
    void StartNewGame(string slot)
    {
        GameAutoSaveManager.Instance.saveSlot = slot;
        GameAutoSaveManager.Instance.level = 1;
        GameAutoSaveManager.Instance.hp = 100;
        GameAutoSaveManager.Instance.souls = 0;
        GameAutoSaveManager.Instance.playTime = 0;

        SceneManager.LoadScene("CutsceneOpeningFinal");
    }


    public void OnOverwriteSlot(int index)
    {
        string slot = usedSlots[index];
        overwritePanel.SetActive(false);
        StartNewGame(slot);
    }

    public void OnCancelOverwrite()
    {
        overwritePanel.SetActive(false);
    }

    public void OnLoadGameClicked()
    {
        mainMenuPanel.SetActive(false);
        loadGamePanel.SetActive(true);

        PlayFabClientAPI.GetUserData(new GetUserDataRequest(),
            result =>
            {
                for (int i = 1; i <= 4; i++)
                {
                    string key = $"SaveSlot{i}";
                    if (result.Data.ContainsKey(key))
                    {
                        var data = JsonUtility.FromJson<GameStateData>(result.Data[key].Value);
                        slotTexts[i - 1].text = $"Level {data.Level} - {data.LastScene}";
                    }
                    else
                    {
                        slotTexts[i - 1].text = "Trống";
                    }
                }
            },
            error => Debug.LogError(error.GenerateErrorReport()));
    }

    public void OnSelectLoadSlot(int index)
    {
        string slot = $"SaveSlot{index + 1}";

        PlayFabClientAPI.GetUserData(new GetUserDataRequest(),
            result =>
            {
                if (result.Data.ContainsKey(slot))
                {
                    var data = JsonUtility.FromJson<GameStateData>(result.Data[slot].Value);

                    GameAutoSaveManager.Instance.saveSlot = slot;
                    GameAutoSaveManager.Instance.level = data.Level;
                    GameAutoSaveManager.Instance.hp = data.HP;
                    GameAutoSaveManager.Instance.souls = data.Souls;
                    GameAutoSaveManager.Instance.playTime = data.PlayTime;

                    GameAutoSaveManager.Instance.nextPlayerPosition =
                        new Vector3(data.posX, data.posY, data.posZ);

                    SceneManager.LoadScene(data.LastScene);
                }
                else
                {
                    Debug.Log("Slot trống");
                }
            },
            error => Debug.LogError(error.GenerateErrorReport()));
    }

    public void OnCancelLoadGame()
    {
        loadGamePanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
