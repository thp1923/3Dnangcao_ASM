using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDragon : MonoBehaviour
{
    public AudioSource[] source;

    public List<AudioClip> clipWalk;

    int pos;

    public List<AudioClip> clipWing;

    int posWing;
    public void playSourceWalk()
    {
        if (clipWalk.Count == 0) return; // Kiểm tra danh sách có clip không

        source[0].PlayOneShot(clipWalk[pos]); // Phát clip hiện tại

        pos++; // Tăng chỉ mục lên
        if (pos >= clipWalk.Count) pos = 0;
    }

    public void playSourceWing()
    {
        if (clipWing.Count == 0) return; // Kiểm tra danh sách có clip không

        source[1].PlayOneShot(clipWing[posWing]); // Phát clip hiện tại

        posWing++; // Tăng chỉ mục lên
        if (posWing >= clipWing.Count) posWing = 0;
    }
}
