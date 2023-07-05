using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {


    public static event EventHandler OnAnyCarQueued;
    public static event EventHandler OnAnyTruckQueued;
    public static event EventHandler OnAnyTruckConstructed;

    [SerializeField] private Image constructionProgressImage;
    [SerializeField] private Transform queueTemplate;

    private List<UnitTypeSO> unitTypeQueueList;
    private QueueConstruction queueConstruction;

    private void Awake() {
        unitTypeQueueList = new List<UnitTypeSO>();
        queueConstruction = null;

        queueTemplate.gameObject.SetActive(false);
    }

    private void Update() {
        if (queueConstruction != null) {
            // Something is being constructed
            constructionProgressImage.fillAmount = queueConstruction.GetProgress();
            if (queueConstruction.Update()) {
                // Construction complete
                if (queueConstruction.GetUnitTypeSO() == GameAssets.Instance.truckUnitType) {
                    // Constructed truck
                    OnAnyTruckConstructed?.Invoke(this, EventArgs.Empty);
                }
                GameWinStats.carsBuilt++;
                queueConstruction = null;
                UpdateQueueVisual();
                constructionProgressImage.fillAmount = 0f;
            }
        } else {
            // Nothing is being constructed, something in queue?
            if (unitTypeQueueList.Count > 0) {
                queueConstruction = new QueueConstruction(transform, unitTypeQueueList[0]);
                constructionProgressImage.fillAmount = queueConstruction.GetProgress();
                unitTypeQueueList.RemoveAt(0);
                UpdateQueueVisual();
            }
        }
    }

    public void QueueCar() {
        unitTypeQueueList.Add(GameAssets.Instance.carUnitType);
        UpdateQueueVisual();
        OnAnyCarQueued?.Invoke(this, EventArgs.Empty);
    }

    public void QueueTruck() {
        unitTypeQueueList.Add(GameAssets.Instance.truckUnitType);
        UpdateQueueVisual();
        OnAnyTruckQueued?.Invoke(this, EventArgs.Empty);
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    private void UpdateQueueVisual() {
        float imageWidth = 2.8f;

        foreach (Transform child in queueTemplate.parent) {
            if (child == queueTemplate) continue;
            Destroy(child.gameObject);
        }

        if (queueConstruction != null) {
            Transform queueTransform = Instantiate(queueTemplate, queueTemplate.parent);
            queueTransform.gameObject.SetActive(true);
            queueTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(imageWidth * 0, 0);
            queueTransform.Find("image").GetComponent<Image>().sprite = queueConstruction.GetUnitTypeSO().sprite;
        }
        for (int i=0; i<unitTypeQueueList.Count; i++) {
            UnitTypeSO unitTypeSO = unitTypeQueueList[i];
            Transform queueTransform = Instantiate(queueTemplate, queueTemplate.parent);
            queueTransform.gameObject.SetActive(true);
            queueTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(imageWidth * (i + 1), 0);
            queueTransform.Find("image").GetComponent<Image>().sprite = unitTypeSO.sprite;
        }
    }




    private class QueueConstruction {

        private Transform transform;
        private UnitTypeSO unitTypeSO;
        private float constructionTimer;
        private float constructionTimerMax = 5f;

        public QueueConstruction(Transform transform, UnitTypeSO unitTypeSO) {
            this.transform = transform;
            this.unitTypeSO = unitTypeSO;
            constructionTimerMax = unitTypeSO.constructionTimerMax;
        }

        public bool Update() {
            constructionTimer += Time.deltaTime;
            if (constructionTimer >= constructionTimerMax) {
                Instantiate(unitTypeSO.prefab, transform.position, transform.rotation);
                return true;
            } else {
                return false;
            }
        }

        public float GetProgress() {
            return constructionTimer / constructionTimerMax;
        }

        public UnitTypeSO GetUnitTypeSO() {
            return unitTypeSO;
        }
    }

}
