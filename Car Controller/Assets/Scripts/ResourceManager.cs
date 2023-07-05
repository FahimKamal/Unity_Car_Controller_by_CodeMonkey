using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {


    public static ResourceManager Instance { get; private set; }

    public event EventHandler OnResourceAmountChanged;

    private Dictionary<ResourceTypeSO, int> resourceTypeAmountDic;

    private void Awake() {
        Instance = this;

        resourceTypeAmountDic = new Dictionary<ResourceTypeSO, int>();

        resourceTypeAmountDic[GameAssets.Instance.rubberResourceType] = 30;
        resourceTypeAmountDic[GameAssets.Instance.steelResourceType] = 10;
        resourceTypeAmountDic[GameAssets.Instance.aluminiumResourceType] = 10;
    }

    public void AddResource(ResourceTypeSO resourceType, int amount) {
        resourceTypeAmountDic[resourceType] += amount;
        GameWinStats.resourcesGathered += amount;
        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
        //DebugLogResourceDictionary();
    }

    public void DebugLogResourceDictionary() {
        foreach (ResourceTypeSO resourceTypeSO in new List<ResourceTypeSO>(resourceTypeAmountDic.Keys)) {
            Debug.Log(resourceTypeSO.resourceName + ": " + resourceTypeAmountDic[resourceTypeSO]);
        }
    }

    public int GetResourceAmount(ResourceTypeSO resourceTypeSO) {
        return resourceTypeAmountDic[resourceTypeSO];
    }

    public bool TrySpend(List<ResourceTypeAmount> resourceTypeAmountList) {
        foreach (ResourceTypeAmount resourceTypeAmount in resourceTypeAmountList) {
            if (GetResourceAmount(resourceTypeAmount.resourceTypeSO) < resourceTypeAmount.amount) {
                return false;
            }
        }

        foreach (ResourceTypeAmount resourceTypeAmount in resourceTypeAmountList) {
            resourceTypeAmountDic[resourceTypeAmount.resourceTypeSO] -= resourceTypeAmount.amount;
        }

        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);

        return true;
    }

}
