using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallShot : MonoBehaviour
{
    Transform target;              // Vị trí mục tiêu ban đầu
    public float speed = 10f;             // Tốc độ bay
    public float lifeTime = 5f;           // Thời gian tồn tại tối đa

    private Vector3 flyDirection;         // Hướng bay đã tính sẵn
    private float timer = 0f;
    private bool isFlying = false;
    public Transform startPos;
    public GameObject Explosion;

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }
    private void OnEnable()
    {
        transform.position = startPos.position;
        isFlying = false;
        timer = 0f;
        StartCoroutine(FireBallStart());
    }

    private void OnDisable()
    {
        isFlying = false;
        
        
    }

    IEnumerator FireBallStart()
    {
        yield return new WaitForSeconds(1f);
        // Tính hướng bay ngay lúc bắt đầu (chỉ một lần)
        flyDirection = (target.position - transform.position).normalized;
        isFlying = true;
    }

    void Update()
    {
        if (!isFlying) return;

        // Di chuyển theo hướng đã tính
        transform.position += flyDirection * speed * Time.deltaTime;

        // Tăng thời gian đếm
        timer += Time.deltaTime;

        if (timer >= lifeTime)
        {
            // Hết thời gian thì ẩn đối tượng
            gameObject.SetActive(false);
            timer = 0f;
        }
    }

    // Khi va chạm thì tắt luôn object
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Ground"))
        {
            Explosion.SetActive(true);
            Explosion.transform.position = gameObject.transform.position;
            gameObject.SetActive(false);
        }
    }
}
