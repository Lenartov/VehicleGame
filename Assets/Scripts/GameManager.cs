using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Ground ground;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private Car car;
    [Space]
    [SerializeField] private SlowMotionManager slowMotion;
    [SerializeField] private UIManager UIManager;
    [SerializeField] private PlayerInput playerInput;


    private void Start()
    {
        car.OnDie += Lose;
        car.Deactivate();
        playerInput.HandleInput = false;
        UIManager.ShowStart();
    }

    public void StartGame()
    {
        playerInput.HandleInput = true;
        car.Activate();
        UIManager.HideAll();
        ground.Acceleration();
        cameraController.UseGameView();
        spawner.StartSpawning();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Lose()
    {
        playerInput.HandleInput = false;
        car.Deactivate();
        spawner.CancelSpawning();
        ground.Stop();
        cameraController.UseStartScreenView();
        slowMotion.SlowForDuration(0.3f, 2f);
        UIManager.ShowLoseRestart();
    }

    public void Win()
    {
        playerInput.HandleInput = false;
        UIManager.ShowWinRestart();
    }


}
