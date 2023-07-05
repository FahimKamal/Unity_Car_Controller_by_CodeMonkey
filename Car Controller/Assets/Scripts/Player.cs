using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }

    public event EventHandler<OnPlayerChangedLocationEventArgs> OnPlayerChangedLocation;
    public class OnPlayerChangedLocationEventArgs : EventArgs {
        public Location location;
        public ResourceTypeSO trackResourceType;
    }


    private CarDriver carDriver;

    public enum Location {
        World,
        Track
    }

    private Location location;


    private void Awake() {
        Instance = this;

        location = Location.World;

        carDriver = GetComponent<CarDriver>();
    }

    private void Update() {
        float forwardAmount = Input.GetAxisRaw("Vertical");
        float turnAmount = Input.GetAxisRaw("Horizontal");
        carDriver.SetInputs(forwardAmount, turnAmount);

        if (location == Location.World) {
            // Player is in the world, check fuel distance
            FuelStation fuelStation = FuelManager.Instance.GetClosestFuelStation(GetPosition());
            float distanceToFuel = Vector3.Distance(GetPosition(), fuelStation.GetPosition());

            if (distanceToFuel > 180) {
                // Way too far, teleport back
                Vector3 dirFromFuelStation = (GetPosition() - fuelStation.GetPosition()).normalized;
                Vector3 teleportBackPosition = fuelStation.GetPosition() + dirFromFuelStation * 30f;
                transform.position = teleportBackPosition;
                carDriver.StopCompletely();
            } else {
                if (distanceToFuel > 100) {
                    // Too far, Warning
                    PlayerTooltip.Instance.Show("Too far from Fuel! Go back!\nTip: Place Fuel Stations to increase range", .1f);
                }
                if (distanceToFuel > 140) {
                    // Too far, Warning
                    PlayerTooltip.Instance.Show("Too far from Fuel! GO BACK!!!\nTip: Place Fuel Stations to increase range", .1f);
                }
            }
        }
    }

    public void SetLocation(Location location, ResourceTypeSO trackResourceType = null) {
        this.location = location;
        OnPlayerChangedLocation?.Invoke(this, new OnPlayerChangedLocationEventArgs { location = location, trackResourceType = trackResourceType  });
    }

    public Location GetLocation() {
        return location;
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

}
