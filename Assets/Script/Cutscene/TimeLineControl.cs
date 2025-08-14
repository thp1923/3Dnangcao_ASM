using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineControl : MonoBehaviour
{
    public PlayableDirector director;

    public void PauseTimeline()
    {
        if (director != null)
        {
            director.extrapolationMode = DirectorWrapMode.None;
            director.playableGraph.GetRootPlayable(0).SetSpeed(0);
        }
    }

    public void ResumeTimeline()
    {
        if (director != null)
        {
            if (director.state != PlayState.Playing)
            {
                double t = director.time;
                director.Play();
                director.time = t;
            }
            director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }
    }
}
