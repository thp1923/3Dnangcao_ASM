using UnityEngine;

public class NoFog : MonoBehaviour
{
    public bool enableFog = true; // Bật/tắt fog cho camera này

    void OnPreRender()
    {
        RenderSettings.fog = enableFog;
    }

    void OnPostRender()
    {
        RenderSettings.fog = !enableFog; // Khôi phục lại giá trị cũ sau khi vẽ xong
    }
}
