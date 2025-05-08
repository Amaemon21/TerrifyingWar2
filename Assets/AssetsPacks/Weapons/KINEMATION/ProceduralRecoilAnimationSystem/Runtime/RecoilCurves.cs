using System;
using KINEMATION.KAnimationCore.Runtime.Core;
using UnityEngine;

namespace KINEMATION.ProceduralRecoilAnimationSystem.Runtime
{
    [Serializable]
    public struct RecoilCurves
    {
        public VectorCurve semiRotCurve;
        public VectorCurve semiLocCurve;
        public VectorCurve autoRotCurve;
        public VectorCurve autoLocCurve;
        
        public static float GetMaxTime(AnimationCurve curve)
        {
            return curve[curve.length - 1].time;
        }

        public RecoilCurves(Keyframe[] keyFrame)
        {
            semiRotCurve = new VectorCurve(keyFrame);
            semiLocCurve = new VectorCurve(keyFrame);
            autoRotCurve = new VectorCurve(keyFrame);
            autoLocCurve = new VectorCurve(keyFrame);
        }
    }
}