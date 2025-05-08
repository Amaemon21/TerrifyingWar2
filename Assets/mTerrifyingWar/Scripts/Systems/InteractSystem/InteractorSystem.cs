using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InteractorSystem : MonoBehaviour
{
    [Inject] private readonly IInputService _inputService;
    [Inject] private readonly PlayerProvider _playerProvider;
    [Inject] private readonly DisplayProvider _displayProvider;
    [Inject] private readonly InteractModel interactModel;
    
    [SerializeField, BoxGroup("Interact")] private LayerMask _hitScanMask;
    [SerializeField, BoxGroup("Interact")] private float _interactRange = 5f;
    [SerializeField, BoxGroup("Interact")] private InputActionReference _interactAction;
    
    private InventoryItemObject _currentItem;
    private InteractObject _interactObject;

    private void Update()
    {
        CheckForInteractable();
    }
    
    private void CheckForInteractable()
    {
        Ray ray = _playerProvider.MainCamera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));

        if (Physics.Raycast(ray, out RaycastHit hit, _interactRange, _hitScanMask))
        {
            Collider hitCollider = hit.collider;
            
            if (hitCollider.TryGetComponent(out InventoryItemObject item))
            {
                HandleNewItem(item);

                if (_inputService.IsInteract)
                    PickupItem(item);
            }
            else if(hitCollider.TryGetComponent(out InteractObject interactObject))
            {
                HandleInteract(interactObject);
                
                if (_inputService.IsInteract)
                    Interect(interactObject);
            }
            else
                Clear();
        }
        else
            Clear();
    }

    private void HandleNewItem(InventoryItemObject item)
    {
        if (_currentItem != item)
            Clear();

        _currentItem = item;

        ShowInteract();
    }
    
    private void HandleInteract(InteractObject interactObject)
    {
        if (_interactObject != interactObject)
            Clear();

        _interactObject = interactObject;

        ShowInteract();
    }

    private void PickupItem(InventoryItemObject item)
    {
        _displayProvider.Inventory.AddItem(item.InventoryItemConfig);
        Destroy(item.gameObject);
        Clear();
    }

    private void Interect(InteractObject interactObject)
    {
        interactObject.Interact();
    }

    private void ShowInteract()
    {
        if (_currentItem != null && _interactObject == null)
        {
            string coloredButton = $"<color=#E78300>{_interactAction.action.bindings[0].ToDisplayString()}</color>";
            string coloredName = $"<color=#E78300>{_currentItem.InventoryItemConfig.ItemName}</color>";

            string text = $"Press [{coloredButton}] to pickup: {coloredName}";
            Sprite icon = _currentItem.InventoryItemConfig.ItemSprite;
            
            interactModel.SetupText(text);
            interactModel.SetupIcon(icon);
        }
        else
        {
            string coloredButton = $"<color=#E78300>{_interactAction.action.bindings[0].ToDisplayString()}</color>";
            string coloredName = $"<color=#E78300>{_interactObject.InteractObjectConfig.InteractName}</color>";
            string text = $"Press [{coloredButton}] {coloredName}";
            
            interactModel.SetupText(text);
            interactModel.SetupIcon(null);
        }
        
        interactModel.Visible(true);
        
        //_currentItem.Outline.enabled = true;
    }

    private void Clear()
    {
        if (_currentItem != null || _interactObject != null)
        {
            //_currentItem.Outline.enabled = false;
            _currentItem = null;
            _interactObject = null;
        }
        
        interactModel.Visible(false);
    }
}