using UnityEngine;

namespace Code.Global.Animations
{
    [CreateAssetMenu(menuName = "Source/Camera/Punch Preset", fileName = "Punch Preset", order = 0)]
    public class PunchPreset : ScriptableObject
    {
        [SerializeField] private PunchAnimationPreset _punchAnimationPreset;

        public PunchAnimationPreset PunchAnimationPreset => _punchAnimationPreset;
    }
}