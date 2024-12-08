using NaughtyAttributes;
using UnityEngine;

public class AnimationClipDuration : MonoBehaviour
{
    public AnimationClip clip;

    public float Time;

    [Button]
    private void Calculate()
    {
        Time = clip.length;
    }
}