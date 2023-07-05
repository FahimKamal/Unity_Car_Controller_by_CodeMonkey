using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarFuelingAI : MonoBehaviour {

    private enum State {
        HasFuel,
        GoingToRefuel,
    }

    [SerializeField] private Image fuelImage;

    private State state;
    private CarAI carAI;
    private CarAttackAI carAttackAI;
    private CarDriverAI carDriverAI;
    private CarAIMain carAIMain;
    private bool isEnabled;
    private float fuelAmount = 1f;

    private void Awake() {
        carAI = GetComponent<CarAI>();
        carAttackAI = GetComponent<CarAttackAI>();
        carDriverAI = GetComponent<CarDriverAI>();
        carAIMain = GetComponent<CarAIMain>();

        isEnabled = false;
        state = State.HasFuel;
    }

    private void Update() {
        if (isEnabled) {
            switch (state) {
                case State.HasFuel:
                    break;
                case State.GoingToRefuel:
                    FuelStation fuelStation = FuelManager.Instance.GetClosestFuelStation(transform.position);
                    carDriverAI.SetMoveToPosition(fuelStation.GetPosition());
                    break;
            }
        }

        HandleFuelAmount();
    }

    private void HandleFuelAmount() {
        float fuelDropSpeedTime = 15f;
        fuelAmount -= Time.deltaTime / fuelDropSpeedTime;
        UpdateFuelAmountImage();
        if (fuelAmount <= .0f) {
            carAIMain.ForceActiveFueling();
            SetStateGoingToRefuel();
        }
    }

    private void OnTriggerEnter(Collider other) {
        FuelStation fuelStation = other.GetComponent<FuelStation>();
        if (fuelStation != null) {
            // Refuel
            fuelAmount = 1f;
            UpdateFuelAmountImage();
            carAIMain.ClearActiveFueling();
            SetStateHasFuel();
        }
    }

    private void SetStateGoingToRefuel() {
        state = State.GoingToRefuel;
    }

    private void SetStateHasFuel() {
        state = State.HasFuel;
    }

    private void UpdateFuelAmountImage() {
        fuelImage.fillAmount = fuelAmount;
    }

    public void SetIsEnabled(bool isEnabled) {
        this.isEnabled = isEnabled;
    }

    public bool IsEnabled() {
        return isEnabled;
    }

}
