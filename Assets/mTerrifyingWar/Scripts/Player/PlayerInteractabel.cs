using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using DG.Tweening; 

public class PlayerInteractable : MonoBehaviour
{
    [Inject] private readonly Inventory _inventory;
    [Inject] private readonly ShootTransform _shootTransform;

    [Header("Interact")]
    [SerializeField] private InteractView _interactView;
    
    [Space]
    [SerializeField] private LayerMask _hitScanMask;

    [Space]
    [SerializeField] private float _interactRange = 5f;
    
    [Space]
    [SerializeField] private InputActionReference _interactAction;

    private InventoryItemObject _currentItem;

    private void Update()
    {
        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        Ray ray = new Ray(_shootTransform.transform.position, _shootTransform.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, _interactRange, _hitScanMask))
        {
            var hitCollider = hit.collider;
            
            if (hitCollider.TryGetComponent(out InventoryItemObject item))
            {
                HandleNewItem(item);

                if (Input.GetKeyUp(KeyCode.F))
                {
                    PickupItem(item);
                }
            }
            else
            {
                ClearCurrentItem();
            }
        }
        else
        {
            ClearCurrentItem();
        }
    }

    private void HandleNewItem(InventoryItemObject item)
    {
        if (_currentItem != item)
        {
            ClearCurrentItem();
        }

        _currentItem = item;

        ShowInteract();
    }

    private void PickupItem(InventoryItemObject item)
    {
        _inventory.AddItem(item.InventoryItemConfig);
        Destroy(item.gameObject);
    }

    private void ShowInteract()
    {
        if (_currentItem == null) return;

        _interactView.CanvasGroup.DOFade(1f, 0.1f);

        //_currentItem.Outline.enabled = true;

        string coloredButton = $"<color=#E78300>{_interactAction.action.bindings[0].ToDisplayString()}</color>";
        string coloredName = $"<color=#E78300>{_currentItem.InventoryItemConfig.ItemName}</color>";

        _interactView.Text.text = $"Press [{coloredButton}] to pickup: {coloredName}";
        _interactView.Icon.sprite = _currentItem.InventoryItemConfig.ItemSprite;
    }

    private void ClearCurrentItem()
    {
        if (_currentItem != null)
        {
            //_currentItem.Outline.enabled = false;
            _currentItem = null;
        }

        _interactView.CanvasGroup.DOFade(0f, 0.2f);
    }
}
