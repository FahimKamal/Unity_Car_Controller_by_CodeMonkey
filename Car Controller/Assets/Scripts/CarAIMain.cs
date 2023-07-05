using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAIMain : MonoBehaviour {


    private CarAI carAI;
    private CarAttackAI carAttackAI;
    private CarFuelingAI carFuelingAI;
    private CarDriverAI carDriverAI;

    private void Awake() {
        carAI = GetComponent<CarAI>();
        carAttackAI = GetComponent<CarAttackAI>();
        carFuelingAI = GetComponent<CarFuelingAI>();

        carDriverAI = GetComponent<CarDriverAI>();
    }

    private void Start() {
        carDriverAI.SetReachedTargetDistance(5f);
        carDriverAI.SetReverseDistance(10f);
    }

    public bool TryEnableAttackAI() {
        if (!carFuelingAI.IsEnabled()) {
            // Not Fueling
            carAI.SetIsEnabled(false);
            carAttackAI.SetIsEnabled(true);
            return true;
        } else {
            return false;
        }
    }

    public void ClearActiveFueling() {
        carAI.SetIsEnabled(true);
        carAttackAI.SetIsEnabled(false);
        carFuelingAI.SetIsEnabled(false);
    }

    public void ForceActiveFueling() {
        carAI.SetIsEnabled(false);
        carAttackAI.SetIsEnabled(false);
        carFuelingAI.SetIsEnabled(true);
    }

}
