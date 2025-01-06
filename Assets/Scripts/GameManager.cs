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
        playerInput.HandleInput = false;
        UIManager.ShowStart();
        car.OnDie += Lose;
        car.Deactivate();
        ground.OnDistanceChanged += UIManager.ProgressBar.SetProgress;
        ground.OnWin += Win;
    }

    public void StartGame()
    {
        playerInput.HandleInput = true;
        UIManager.ShowProgressBar();
        car.Activate();
        cameraController.UseGameView();
        spawner.StartSpawning();
        ground.Acceleration();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Lose()
    {
        playerInput.HandleInput = false;
        UIManager.ShowLoseRestart();
        car.Deactivate();
        cameraController.UseStartScreenView();
        spawner.CancelSpawning();
        ground.Stop();
        slowMotion.SlowForDuration(0.3f, 2f);
    }

    public void Win()
    {
        playerInput.HandleInput = false;
        UIManager.ShowWinRestart();
        car.Deactivate();
        cameraController.UseStartScreenView();
        spawner.CancelSpawning();
        ground.Stop();
        slowMotion.SlowForDuration(0.5f, 2f);
    }


}
