using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioPlayerRun : MonoBehaviour
{
    public AudioSource source;

    public List<AudioClip> clip;

    int pos;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playSourceWalk()
    {
        if (clip.Count == 0) return; // Kiểm tra danh sách có clip không

        source.PlayOneShot(clip[pos]); // Phát clip hiện tại

        pos++; // Tăng chỉ mục lên
        if (pos >= clip.Count) pos = 0;
    }
}
