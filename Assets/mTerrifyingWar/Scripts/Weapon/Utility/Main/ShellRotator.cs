using UnityEngine;

public class ShellRotator : MonoBehaviour
{
    private void Awake()
    {
        var x = Random.Range(-360, 360);
        var y = Random.Range(-360, 360);
        var z = Random.Range(-360, 360);

        var vector = new Vector3(x, y, z);
        transform.Rotate(vector);
    }
}