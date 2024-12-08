using UnityEngine;
using Zenject;

public class MenuWindow : MonoBehaviour
{
    [Inject] private readonly PlayerController _playerController;
    [Inject] private readonly InputService _inputService;

    [SerializeField] private GameObject _menuPanel;
    
    private bool _isOpen = false;

    private void Awake()
    {
        Action(_isOpen);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _isOpen = !_isOpen;
            Action(_isOpen);
        }
    }

    private void Action(bool isOpen)
    {
        Cursor.visible = isOpen;
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;

        _menuPanel.SetActive(isOpen);

        _playerController.enabled = !isOpen;
        
        _inputService.PlayerInput.enabled = !isOpen;
    }
}