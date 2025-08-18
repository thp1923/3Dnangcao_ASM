using UnityEngine;

public class AutoPerformanceOptimizer : MonoBehaviour
{
    [Header("Target FPS")]
    public int targetFPS = 140;

    [Header("LOD Settings")]
    public float lodBiasHigh = 1.0f;
    public float lodBiasLow = 0.3f;

    [Header("Shadow Settings")]
    public float shadowDistanceHigh = 50f;
    public float shadowDistanceLow = 20f;

    [Header("Pixel Light Settings")]
    public int pixelLightHigh = 2;
    public int pixelLightLow = 0;

    private float checkInterval = 1f; // kiểm tra mỗi giây
    private float timer;

    void Start()
    {
        Application.targetFrameRate = targetFPS;
        QualitySettings.lodBias = lodBiasHigh;
        QualitySettings.shadowDistance = shadowDistanceHigh;
        QualitySettings.pixelLightCount = pixelLightHigh;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= checkInterval)
        {
            AdjustPerformance();
            timer = 0f;
        }
    }

    void AdjustPerformance()
    {
        if (Time.deltaTime > 1f / (targetFPS - 10)) // FPS thấp hơn ngưỡng
        {
            // Giảm chất lượng để giữ FPS
            QualitySettings.lodBias = lodBiasLow;
            QualitySettings.shadowDistance = shadowDistanceLow;
            QualitySettings.pixelLightCount = pixelLightLow;
            //Debug.Log("Performance Optimized: Lower quality for FPS boost");
        }
        else
        {
            // Trả lại chất lượng cao khi FPS ổn
            QualitySettings.lodBias = lodBiasHigh;
            QualitySettings.shadowDistance = shadowDistanceHigh;
            QualitySettings.pixelLightCount = pixelLightHigh;
            //Debug.Log("Performance Restored: Higher quality");
        }
    }
}
