using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private GameObject hitPos;
    [SerializeField] private LayerMask layerMask;
    [Space]
    [SerializeField] public UnityEvent OnPresed;
    [SerializeField] public UnityEvent OnRelesed;

    private static Camera mainCamera;
    private bool isPressed;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnPresed?.Invoke();
            isPressed = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnRelesed?.Invoke();
            isPressed = false;
        }

        if (isPressed)
        {
            hitPos.transform.position = GetCursorPos(layerMask);
        }
    }

    public static Vector3 GetCursorPos(LayerMask layerMask)
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layerMask))
        {
            return hit.point;
        }

        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, float.MaxValue, layerMask))
        {
            return hit.point;
        }

        Debug.LogError("No ground hit");
        return Vector3.zero;
    }
}
