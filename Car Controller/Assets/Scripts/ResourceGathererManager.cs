using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Manages all Resource Gatherers
 * */
public class ResourceGathererManager : MonoBehaviour {

    public static ResourceGathererManager Instance { get; private set; }

    private Dictionary<ResourceNode, List<CarResourceGathererAI>> resourceNodeCareResourceGathererListDic;

    private void Awake() {
        Instance = this;
        resourceNodeCareResourceGathererListDic = new Dictionary<ResourceNode, List<CarResourceGathererAI>>();
    }

    public ResourceNode GetClosestResourceNodeWithGathererSlots(Vector3 testPosition) {
        ResourceNode closest = null;

        foreach (ResourceNode resourceNode in resourceNodeCareResourceGathererListDic.Keys) {
            List<CarResourceGathererAI> carResourceGathererAIList = resourceNodeCareResourceGathererListDic[resourceNode];
            if (carResourceGathererAIList.Count < resourceNode.GetGathererAmount()) {
                if (closest == null) {
                    closest = resourceNode;
                } else {
                    if (Vector3.Distance(testPosition, resourceNode.GetPosition()) < Vector3.Distance(testPosition, closest.GetPosition())) {
                        // Closer
                        closest = resourceNode;
                    }
                }
            }
        }

        return closest;
    }




    public void AddResourceNode(ResourceNode resourceNode) {
        resourceNodeCareResourceGathererListDic[resourceNode] = new List<CarResourceGathererAI>();
    }

    public ResourceNode GetTargetResourceNode(CarResourceGathererAI carResourceGathererAI) {
        ResourceNode resourceNode = GetClosestResourceNodeWithGathererSlots(carResourceGathererAI.transform.position);

        if (resourceNode != null) {
            resourceNodeCareResourceGathererListDic[resourceNode].Add(carResourceGathererAI);
        }

        return resourceNode;
    }

}
