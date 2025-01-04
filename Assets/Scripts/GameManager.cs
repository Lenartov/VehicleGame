using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Ground ground;
    [SerializeField] private CameraController cameraController;

    private void Start()
    {
        ground.Acceleration();
        cameraController.UseGameView();
    }

}
