using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button StartPlayButton;
    [SerializeField] private Button RestartOnLoseButton;
    [SerializeField] private Button RestartOnWinButton;

    public void ShowStart()
    {
        StartPlayButton.gameObject.SetActive(true);
        RestartOnLoseButton.gameObject.SetActive(false);
        RestartOnWinButton.gameObject.SetActive(false);
    }

    public void ShowLoseRestart() 
    {
        StartPlayButton.gameObject.SetActive(false);
        RestartOnLoseButton.gameObject.SetActive(true);
        RestartOnWinButton.gameObject.SetActive(false);
    }

    public void ShowWinRestart()
    {
        StartPlayButton.gameObject.SetActive(false);
        RestartOnLoseButton.gameObject.SetActive(false);
        RestartOnWinButton.gameObject.SetActive(true);
    }

    public void HideAll()
    {
        StartPlayButton.gameObject.SetActive(false);
        RestartOnLoseButton.gameObject.SetActive(false);
        RestartOnWinButton.gameObject.SetActive(false);
    }
}
