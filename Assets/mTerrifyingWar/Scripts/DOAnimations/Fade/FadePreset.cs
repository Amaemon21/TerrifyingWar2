using UnityEngine;

namespace Code.Global.Animations
{
    [CreateAssetMenu(menuName = "Source/Fade Preset", fileName = "Fade Preset", order = 0)]
    public class FadePreset : ScriptableObject
    {
        [SerializeField] private FadeAnimationPreset _fadeAnimationPreset;

        public FadeAnimationPreset FadeAnimationPreset => _fadeAnimationPreset;
    }
}