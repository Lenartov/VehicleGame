using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private CameraController cameraController;

    private void Start()
    {
        car.StartMovement();
        cameraController.UseGameView();
    }

}
