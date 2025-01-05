using UnityEngine;

public class AlwaysLookAtCamera : MonoBehaviour
{
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(cam.transform.position);
    }
}
