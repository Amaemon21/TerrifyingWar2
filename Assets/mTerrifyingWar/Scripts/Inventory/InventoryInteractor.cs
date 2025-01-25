using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

[RequireComponent(typeof(Inventory))]
public class InventoryInteractor : MonoBehaviour
{
    [Inject] private readonly IInputService _inputService;
    [Inject] private readonly InteractUIModel interactUIModel;
    
    [SerializeField, BoxGroup("Interact")] private LayerMask _hitScanMask;
    [SerializeField, BoxGroup("Interact")] private float _interactRange = 5f;
    [SerializeField, BoxGroup("Interact")] private InputActionReference _interactAction;
    
    private Transform _cameraTransform;
    private Inventory _inventory;
    private InventoryItemObject _currentItem;

    private void Awake()
    {
        _inventory = GetComponent<Inventory>();
        
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        CheckForInteractable();
    }
    
    private void CheckForInteractable()
    {
        Ray ray = new Ray(_cameraTransform.transform.position, _cameraTransform.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, _interactRange, _hitScanMask))
        {
            Collider hitCollider = hit.collider;
            
            if (hitCollider.TryGetComponent(out InventoryItemObject item))
            {
                HandleNewItem(item);

                if (_inputService.IsInteract)
                    PickupItem(item);
            }
            else
                ClearCurrentItem();
        }
        else
            ClearCurrentItem();
    }

    private void HandleNewItem(InventoryItemObject item)
    {
        if (_currentItem != item)
            ClearCurrentItem();

        _currentItem = item;

        ShowInteract();
    }

    private void PickupItem(InventoryItemObject item)
    {
        _inventory.AddItem(item.InventoryItemConfig);
        Destroy(item.gameObject);
        ClearCurrentItem();
    }

    private void ShowInteract()
    {
        if (_currentItem == null) 
            return;
        
        string coloredButton = $"<color=#E78300>{_interactAction.action.bindings[0].ToDisplayString()}</color>";
        string coloredName = $"<color=#E78300>{_currentItem.InventoryItemConfig.ItemName}</color>";

        string text = $"Press [{coloredButton}] to pickup: {coloredName}";
        Sprite icon = _currentItem.InventoryItemConfig.ItemSprite;
        
        interactUIModel.SetupText(text);
        interactUIModel.SetupIcon(icon);
        interactUIModel.Visible(true);
        
        //_currentItem.Outline.enabled = true;
    }

    private void ClearCurrentItem()
    {
        if (_currentItem != null)
        {
            //_currentItem.Outline.enabled = false;
            _currentItem = null;
        }
        
        interactUIModel.Visible(false);
    }
}