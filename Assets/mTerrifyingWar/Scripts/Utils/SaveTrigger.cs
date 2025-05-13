using UnityEngine;
using Zenject;

[RequireComponent(typeof(BoxCollider))]
public class SaveTrigger : MonoBehaviour
{
    [Inject] private readonly IStorageService _storageService;
    
    private BoxCollider _boxCollider;
    
    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _boxCollider.isTrigger = true; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMover playerMover))
        {
            _storageService.Save();
            Debug.Log("Save progress");
            gameObject.SetActive(false);
        }          
    }

    private void OnDrawGizmos()
    {
        if (!_boxCollider)
            return;
    
        Gizmos.color = new Color32(30, 200, 30, 130);
        Gizmos.DrawCube(transform.position + _boxCollider.center, _boxCollider.size);
    }
}