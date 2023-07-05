using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection : MonoBehaviour {

    public static PlayerSelection Instance { get; private set; }

    public event EventHandler OnSelectedCarListChanged;
    public event EventHandler OnGiveAttackMoveOrder;

    [SerializeField] private Transform selectionAreaTransform;

    private List<CarAI> selectedCarAIList;
    private bool isSelectionGrowing;
    private float selectionSize;

    private void Awake() {
        Instance = this;

        selectedCarAIList = new List<CarAI>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Z)) {
            // Start expanding selection area
            isSelectionGrowing = true;
            selectionSize = 0f;
            selectionAreaTransform.gameObject.SetActive(true);
        }
        if (Input.GetKey(KeyCode.Z)) {
            float selectionSizeIncreaseSpeed = 100f;
            selectionSize += selectionSizeIncreaseSpeed * Time.deltaTime;
            float selectionSizeMax = 140f;
            selectionSize = Mathf.Clamp(selectionSize, 0f, selectionSizeMax);
        }
        if (Input.GetKeyUp(KeyCode.Z)) {
            isSelectionGrowing = false;
            selectionAreaTransform.gameObject.SetActive(false);
            UpdateSelectedCarList();
        }

        if (Input.GetKeyUp(KeyCode.X)) {
            foreach (CarAI carAI in CarAI.GetCarAIList()) {
                carAI.SetPatrolState(transform.position);
            }
            Transform selectionAreaVisual = Instantiate(GameAssets.Instance.pfSelectionOrderVisual, transform.position + new Vector3(0, .1f, 0), Quaternion.Euler(90, 0, 0));
            Destroy(selectionAreaVisual.gameObject, 1f);

            OnGiveAttackMoveOrder?.Invoke(this, EventArgs.Empty);
        }

        selectionAreaTransform.localScale = Vector3.one * selectionSize;
    }

    private void UpdateSelectedCarList() {
        foreach (CarAI carAI in selectedCarAIList) {
            carAI.HideSelectionArea();
        }
        selectedCarAIList.Clear();

        foreach (CarAI carAI in CarAI.GetCarAIList()) {
            if (Vector3.Distance(transform.position, carAI.GetPosition()) < selectionSize * .5f) {
                selectedCarAIList.Add(carAI);
            }
        }

        foreach (CarAI carAI in selectedCarAIList) {
            carAI.ShowSelectionArea();
        }

        OnSelectedCarListChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetSelectedCarAIListCount() {
        return selectedCarAIList.Count;
    }

}
