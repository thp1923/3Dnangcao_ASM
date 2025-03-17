using UnityEngine;

public class AliveManager : MonoBehaviour
{
    #region Variables 
    [Header("-------------Stats----------")]
    public int heathMax;
    public int def;
    #endregion
    public void TakeDamge(int damge)
    {
        heathMax -= damge;
    }
}
