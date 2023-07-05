using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelManager : MonoBehaviour {

    public static FuelManager Instance { get; private set; }

    public event EventHandler OnFuelStationAdded;

    private List<FuelStation> fuelStationList;

    private void Awake() {
        Instance = this;

        fuelStationList = new List<FuelStation>();
    }

    public void AddFuelStation(FuelStation fuelStation) {
        if (fuelStationList.Count >= 1) {
            // Connect to closest
            Vector3 closestPosition = GetClosestFuelStation(fuelStation.GetPosition()).GetPosition();

            Vector3 dir = (fuelStation.GetPosition() - closestPosition).normalized;

            Transform fuelLineTransform = Instantiate(GameAssets.Instance.pfFuelLine);
            fuelLineTransform.position = closestPosition + new Vector3(0, .1f, 0);// + dir * (Vector3.Distance(closestPosition, fuelStation.GetPosition()) * .5f);
            fuelLineTransform.localScale = new Vector3(Vector3.Distance(closestPosition, fuelStation.GetPosition()), 10, 1);
            fuelLineTransform.eulerAngles = new Vector3(90, 0, Utils.GetAngleFromVector(dir));
        }

        fuelStationList.Add(fuelStation);

        OnFuelStationAdded?.Invoke(this, EventArgs.Empty);
    }

    public FuelStation GetClosestFuelStation(Vector3 testPosition) {
        FuelStation closest = null;

        foreach (FuelStation fuelStation in fuelStationList) {
            if (closest == null) {
                closest = fuelStation;
            } else {
                if (Vector3.Distance(testPosition, fuelStation.GetPosition()) < Vector3.Distance(testPosition, closest.GetPosition())) {
                    // Closer
                    closest = fuelStation;
                }
            }
        }

        return closest;
    }


}
