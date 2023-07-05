using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNodeSingle : MonoBehaviour {

    private ResourceTypeSO resourceTypeSO;
    private ResourceNode resourceNode;

    public void Setup(ResourceNode resourceNode, ResourceTypeSO resourceTypeSO) {
        this.resourceNode = resourceNode;
        this.resourceTypeSO = resourceTypeSO;
    }

    public ResourceTypeSO GetResourceTypeSO() {
        return resourceTypeSO;
    }

    public void DestroySelf() {
        resourceNode.ResourceNodeSingleDestroyed(this);
        Destroy(gameObject);
    }

}
