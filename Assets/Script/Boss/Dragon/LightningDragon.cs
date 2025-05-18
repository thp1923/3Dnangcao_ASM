using System.Collections;
using UnityEngine;

public class LightningDragon : MonoBehaviour
{
    Transform player;
    public float timeHide = 1.5f;
    ParticleSystem lightning;
    private void Awake()
    {
        lightning = GetComponent<ParticleSystem>();
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.position = player.position;
        lightning.Play();
        StartCoroutine(Hide());
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(timeHide);
        gameObject.SetActive(false);
    }
}
