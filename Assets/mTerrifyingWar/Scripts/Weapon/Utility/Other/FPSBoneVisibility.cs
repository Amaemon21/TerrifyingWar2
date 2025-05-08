using System.Collections.Generic;
using UnityEngine;

public class FPSBoneVisibility : MonoBehaviour
{
    [SerializeField] private List<Transform> defaultBonesToHide;
    [SerializeField] private List<Transform> bonesToHideByEvent;

    private bool _isVisible = true;

    private void Start()
    {
        foreach (Transform bone in defaultBonesToHide) 
            bone.localScale = Vector3.zero;
    }

    public void SetBoneVisibility(int value)
    {
        _isVisible = value == 1;
    }

    private void LateUpdate()
    {
        foreach (Transform bone in defaultBonesToHide) 
            bone.localScale = Vector3.zero;
        
        foreach (Transform bone in bonesToHideByEvent)
            bone.localScale = _isVisible ? Vector3.one : Vector3.zero;
    }
}