using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelStation : MonoBehaviour {

    private void Start() {
        FuelManager.Instance.AddFuelStation(this);
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

}
