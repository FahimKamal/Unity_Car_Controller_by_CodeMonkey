using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour {

    private static List<CarAI> carAIList = new List<CarAI>();

    public static List<CarAI> GetCarAIList() {
        return carAIList;
    }




    private enum State {
        Normal,
        Patrol,
    }

    [SerializeField] private Transform selectionAreaTransform;

    private State state;
    private CarDriverAI carDriverAI;
    private Vector3 targetPosition;
    private Vector3 targetPatrolPosition;
    private bool isEnabled;

    private void Awake() {
        carAIList.Add(this);

        carDriverAI = GetComponent<CarDriverAI>();
        targetPosition = transform.position + transform.forward * 10 + Utils.GetRandomDir() * Random.Range(0, 10f);
        SetPatrolState(targetPosition);

        HideSelectionArea();

        isEnabled = true;
    }

    private void Update() {
        if (isEnabled) {
            switch (state) {
                case State.Normal:
                    carDriverAI.SetMoveToPosition(targetPosition);
                    break;
                case State.Patrol:
                    if (carDriverAI.GetHasReachedMoveToPosition()) {
                        targetPatrolPosition = targetPosition + Utils.GetRandomDir() * Random.Range(0, 20f);
                    }
                    carDriverAI.SetMoveToPosition(targetPatrolPosition);
                    break;
            }
        }
    }

    public void SetPatrolState(Vector3 targetPosition) {
        this.targetPosition = targetPosition;
        state = State.Patrol;
    }

    public void SetNormalState(Vector3 targetPosition) {
        this.targetPosition = targetPosition;
        state = State.Normal;
    }

    public void SetIsEnabled(bool isEnabled) {
        this.isEnabled = isEnabled;
    }

    public void ShowSelectionArea() {
        selectionAreaTransform.gameObject.SetActive(true);
    }

    public void HideSelectionArea() {
        selectionAreaTransform.gameObject.SetActive(false);
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

}
