using UnityEngine;
using UnityEngine.Playables;

public class CutsceneBranch : MonoBehaviour
{
    public PlayableDirector directorBadEnding;
    public PlayableDirector directorContinue;

    public void ChooseBadEnding()
    {
        directorBadEnding.Play();
    }

    public void ChooseContinue()
    {
        directorContinue.Play();
    }
}
