using UnityEngine;
using Zenject;

public class EntitySpriteSwitcher : MonoBehaviour
{
    [Inject] private readonly DisplayProvider _displayProvider;
    
    [SerializeField] private Transform _cameraTransform;

    [Header("Sprites")]
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _enemySprite;
    [SerializeField] private Sprite _npcSprite;

    [Header("Raycast Settings")]
    [SerializeField] private float _raycastDistance = 500f;
    [SerializeField] private LayerMask _hitScanMask;
    
    private void Update()
    {
        Ray ray = new Ray(_cameraTransform.transform.position, _cameraTransform.transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, _raycastDistance, _hitScanMask))
        {
            if (hit.collider.TryGetComponent(out BodyPart enemy))
            {
                _displayProvider.AimPoint.Image.sprite = _enemySprite;
            }
            else if (hit.collider.CompareTag("NPC"))
            {
                _displayProvider.AimPoint.Image.sprite = _npcSprite;
            }
            else
            {
                _displayProvider.AimPoint.Image.sprite = _defaultSprite;
            }
        }
        else
        {
            _displayProvider.AimPoint.Image.sprite = _defaultSprite;
        }
    }
}