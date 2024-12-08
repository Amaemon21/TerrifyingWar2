using UnityEngine;

public class CameraTilt : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;

    [Space]
    [SerializeField] private float _tiltSpeed;
    [SerializeField] private float _tiltAmount;

    private void Update()
    {
        var rotation = Tilt();
        transform.localRotation = Quaternion.Lerp(transform.localRotation, rotation, Time.deltaTime * _tiltSpeed);
    }

    private Quaternion Tilt()
    {
        if (!_playerController.IsWalking)
            return Quaternion.Euler(Vector3.zero);

        var x = Input.GetAxis("Horizontal");

        Vector3 vector = new Vector3(0, 0, -x).normalized * _tiltAmount;

        return Quaternion.Euler(vector);
    }
}