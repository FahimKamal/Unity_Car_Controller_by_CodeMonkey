using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCamera : MonoBehaviour {

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private CarDriver carDriver;

    private void Awake() {
        carDriver = GetComponent<CarDriver>();
    }

    private void Update() {
        float speedNormalized = Mathf.Clamp01(carDriver.GetSpeed() / 80f);
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(50, 50) + new Vector3(25, 25) * speedNormalized;
    }

}
