using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button StartPlayButton;
    [SerializeField] private Button RestartOnLoseButton;
    [SerializeField] private Button RestartOnWinButton;
    [SerializeField] public ProgressBar ProgressBar;

    public void ShowStart()
    {
        StartPlayButton.gameObject.SetActive(true);
        RestartOnLoseButton.gameObject.SetActive(false);
        RestartOnWinButton.gameObject.SetActive(false);
        ProgressBar.gameObject.SetActive(false);

    }

    public void ShowLoseRestart() 
    {
        StartPlayButton.gameObject.SetActive(false);
        RestartOnLoseButton.gameObject.SetActive(true);
        RestartOnWinButton.gameObject.SetActive(false);
        ProgressBar.gameObject.SetActive(false);
    }

    public void ShowWinRestart()
    {
        StartPlayButton.gameObject.SetActive(false);
        RestartOnLoseButton.gameObject.SetActive(false);
        RestartOnWinButton.gameObject.SetActive(true);
        ProgressBar.gameObject.SetActive(false);
    }

    public void ShowProgressBar()
    {
        StartPlayButton.gameObject.SetActive(false);
        RestartOnLoseButton.gameObject.SetActive(false);
        RestartOnWinButton.gameObject.SetActive(false);
        ProgressBar.gameObject.SetActive(true);
    }
}
