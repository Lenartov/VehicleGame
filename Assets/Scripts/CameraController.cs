using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera startScreenView;
    [SerializeField] private CinemachineVirtualCamera gameView;

    [ContextMenu("ScreenView")]
    public void UseStartScreenView()
    {
        startScreenView.enabled = true;
    }

    [ContextMenu("GameView")]
    public void UseStartGameView()
    {
        startScreenView.enabled = false;
    }
}
