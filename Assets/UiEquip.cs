using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiEquip : MonoBehaviour
{
    public List<Image> listA;
    public List<Image> listB;

    [Range(0f, 1f)]
    public float transparentAlpha = 0.1f; // độ trong suốt khi không có sprite

    void Update()
    {
        if (listA.Count != listB.Count)
            return;

        for (int i = 0; i < listA.Count; i++)
        {
            Sprite sourceSprite = listA[i].sprite;

            if (listB[i].sprite != sourceSprite)
            {
                listB[i].sprite = sourceSprite;
            }

            // Kiểm tra nếu sprite null thì giảm alpha, ngược lại thì tăng lên tối đa
            Color color = listB[i].color;
            color.a = (sourceSprite == null) ? transparentAlpha : 1f;
            listB[i].color = color;
        }
    }
}
