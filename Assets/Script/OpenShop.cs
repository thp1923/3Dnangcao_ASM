using UnityEngine;

public class OpenShop : MonoBehaviour
{
    public float range = 3f;
    public LayerMask mask;
    private bool isHere;

    public GameObject interactHintUI;
    public GameObject shopopen; // gán Canvas Shop UI vào đây trong Inspector
    public bool canClick;


    void Update()
    {
        CheckNearby();

        if (isHere)
        {
            // Hiện hint khi shop chưa mở
            if (interactHintUI != null && !shopopen.activeSelf)
                interactHintUI.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (shopopen != null)
                {
                    bool isActive = shopopen.activeSelf;
                    shopopen.SetActive(!isActive);
                    PlayerAttackController.CursorLocked = false;


                    // Luôn tắt hint khi mở shop
                    if (interactHintUI != null)
                        interactHintUI.SetActive(false);
                }

            }
        }
        else
        {
            if (interactHintUI != null)
                interactHintUI.SetActive(false);
            PlayerAttackController.CursorLocked = true;

            if (shopopen != null && shopopen.activeSelf)
                shopopen.SetActive(false);
                PlayerAttackController.CursorLocked = true;
        }
    }

    void CheckNearby()
    {
        Collider[] check = Physics.OverlapSphere(transform.position, range, mask);
        isHere = check.Length > 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
