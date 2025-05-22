using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
    #region Setup
    public Button displayTab, audioTab, languagesTab;
    public CanvasGroup displayGroup, audioGroup, languagesGroup;
    public Image highlightImage;
    public float fadeDuration = 0.2f; // Hiệu ứng fade in/out

    CanvasGroup currentGroup; // Nhóm hiện tại đang hiển thị
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        displayTab.onClick.AddListener(() => SwitchGroup(displayGroup, displayTab));
        audioTab.onClick.AddListener(() => SwitchGroup(audioGroup, audioTab));
        languagesTab.onClick.AddListener(() => SwitchGroup(languagesGroup, languagesTab));
        SwitchGroup(displayGroup, displayTab); // Mặc định hiển thị nhóm đầu tiên
    }
    
    void SwitchGroup (CanvasGroup newGroup, Button tabButton)
    {
        if (currentGroup == newGroup) return;
        StopAllCoroutines(); // Dừng tất cả các coroutine đang chạy
        if (currentGroup != null)
            StartCoroutine(FadeGroup(currentGroup, 1, 0));
            StartCoroutine(FadeGroup(newGroup,0,1));
        currentGroup = newGroup;
        highlightImage.transform.position = tabButton.transform.position;
    }

    IEnumerator FadeGroup (CanvasGroup group, float from, float to)
    {
        float t = 0;
        group.interactable = false; // Tắt tương tác với nhóm
        group.blocksRaycasts = false;

        while (t < fadeDuration)
        {
            group.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        group.alpha = to;
        group.interactable = to > 0.9f;
        group.blocksRaycasts = to > 0.9f;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
