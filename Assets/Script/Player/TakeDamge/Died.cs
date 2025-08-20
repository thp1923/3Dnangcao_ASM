using UnityEngine;

public class Died : MonoBehaviour
{
    //private bool _respawning;

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ResetScene()
    {
        //if (_respawning) return;
        //_respawning = true;

        GameAutoSaveManager.Instance.OnPlayerDie();
    }
}
