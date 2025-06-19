using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAttack : MonoBehaviour
{
    public string tagEffect;
    public GameObject effect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //void OnCollisionEnter(Collision collision)
    //{
    //    // Lấy danh sách các điểm va chạm
    //    ContactPoint[] contactPoints = collision.contacts;

    //    // Kiểm tra nếu có điểm va chạm
    //    if (contactPoints.Length > 0)
    //    {
    //        // Lấy tọa độ điểm va chạm đầu tiên
    //        Vector3 collisionPoint = contactPoints[0].point;

    //        // Tạo một instance của GameObject tại điểm va chạm
    //        GameObject instance = Instantiate(effect, collisionPoint, Quaternion.identity);

    //        Debug.Log("Điểm va chạm: " + collisionPoint);
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagEffect))
        {
            // Tạo một ray từ đối tượng hiện tại tới đối tượng va chạm
            Ray ray = new Ray(transform.position, (other.transform.position - transform.position).normalized);
            RaycastHit hit;

            // Thực hiện Raycast để xác định điểm va chạm
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // Lấy tọa độ điểm va chạm
                Vector3 collisionPoint = hit.point;

                // Tạo một instance của GameObject tại điểm va chạm
                GameObject instance = Instantiate(effect, collisionPoint, Quaternion.identity);

            }
        }
    }
}
