using UnityEngine;
using Zenject;

public class EntitySpriteSwitcher : MonoBehaviour
{
    [Inject] private readonly AimPoint _aimPoint;
    [Inject] private readonly ShootTransform _shootTransform;

    [Header("Sprites")]
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _enemySprite;
    [SerializeField] private Sprite _npcSprite;

    [Header("Raycast Settings")]
    [SerializeField] private float _raycastDistance = 500f;
    [SerializeField] private LayerMask _hitScanMask;
    
    private void Update()
    {
        Ray ray = new Ray(_shootTransform.transform.position, _shootTransform.transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, _raycastDistance, _hitScanMask))
        {
            if (hit.collider.TryGetComponent(out BodyPart enemy))
            {
                _aimPoint.Image.sprite = _enemySprite;
            }
            else if (hit.collider.CompareTag("NPC"))
            {
                _aimPoint.Image.sprite = _npcSprite;
            }
            else
            {
                _aimPoint.Image.sprite = _defaultSprite;
            }
        }
        else
        {
            _aimPoint.Image.sprite = _defaultSprite;
        }
    }
}