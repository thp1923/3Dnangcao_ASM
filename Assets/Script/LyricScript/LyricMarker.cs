using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

[System.Serializable]
public class LyricStyle
{
    public bool typewriter = true;
    [Range(0f, 2f)] public float typewriterspeed = 40f;
    public bool clearOnEnd = false; //Chỉ xóa khi đoạn tiếp theo bắt đầu

}
public class LyricMarker : Marker, INotification, INotificationOptionProvider
{
    public LocalizedString localizedText;

    [Tooltip("How long this line should 'own' the screen (for typewriter pacing & auto-clear hints).")]
    public float suggestedDuration = 2.5f;
    [Tooltip("Per-line style overrides.")]
    public LyricStyle style = new LyricStyle();
    public PropertyName id => new PropertyName("LyricMarker");

    [SerializeField]
    private NotificationFlags _flags =
        NotificationFlags.Retroactive | NotificationFlags.TriggerOnce;
    NotificationFlags INotificationOptionProvider.flags => _flags; //Chỉ trigger một lần, không lặp lại
}
