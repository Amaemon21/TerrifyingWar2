using UnityEngine;

public class Cameralog : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private void Awake()
    {
        // Координаты углов в системе Viewport
        Vector3 bottomLeft = _camera.ViewportToWorldPoint(new Vector3(0, 0, _camera.nearClipPlane));
        Vector3 topRight = _camera.ViewportToWorldPoint(new Vector3(1, 1, _camera.nearClipPlane));

        // Вывод координат
        Debug.Log($"Bottom Left: {bottomLeft}, Top Right: {topRight}");
    }
}