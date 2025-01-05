using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [Space]
    [SerializeField] public UnityEvent OnPresed;
    [SerializeField] public UnityEvent OnRelesed;

    [HideInInspector] public bool HandleInput { get; set; }

    private static Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!HandleInput)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            OnPresed?.Invoke();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnRelesed?.Invoke();
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
