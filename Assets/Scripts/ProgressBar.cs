using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image progressBar;

    private void Awake()
    {
        progressBar.fillAmount = 0f;
    }

    public void SetProgress(float progress)
    {
        progressBar.fillAmount = progress;
    }
}
