using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Màu Button")]
    public Color normalColor = Color.white;         
    public Color hoverColor = Color.yellow;        
    public Color pressedColor = Color.green;        
    public float scaleMultiplier = 0.9f; 
    private Vector3 originalScale;
    private Image buttonImage;
    [Header("Âm thanh Click")]
    public AudioClip clickSound;
    private AudioSource audioSource;

    private void Awake()
    {
        originalScale = transform.localScale;
        buttonImage = GetComponent<Image>();

        if (buttonImage != null)
            buttonImage.color = normalColor;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = clickSound;

        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(PlayClickSound);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonImage != null)
            buttonImage.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonImage != null)
            buttonImage.color = normalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = originalScale * scaleMultiplier;
        if (buttonImage != null)
            buttonImage.color = pressedColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = originalScale;
        if (buttonImage != null)
            buttonImage.color = hoverColor; 
    }
    private void PlayClickSound()
    {
        if (clickSound != null)
            audioSource.PlayOneShot(clickSound);
        else
            Debug.LogWarning("⚠ Chưa gán âm thanh click cho " + gameObject.name);
    }
}
