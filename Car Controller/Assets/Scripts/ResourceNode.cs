using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour {

    public static event EventHandler OnAnyGathererAmountChanged;

    private static List<ResourceNode> resourceNodeList = new List<ResourceNode>();

    public static ResourceNode GetClosestResourceNode(Vector3 testPosition) {
        ResourceNode closest = null;

        foreach (ResourceNode resourceNode in resourceNodeList) {
            if (closest == null) {
                closest = resourceNode;
            } else {
                if (Vector3.Distance(testPosition, resourceNode.GetPosition()) < Vector3.Distance(testPosition, closest.GetPosition())) {
                    // Closer
                    closest = resourceNode;
                }
            }
        }

        return closest;
    }

    public static ResourceNode GetClosestResourceNodeWithGathererSlots(Vector3 testPosition) {
        ResourceNode closest = null;

        foreach (ResourceNode resourceNode in resourceNodeList) {

            if (closest == null) {
                closest = resourceNode;
            } else {
                if (Vector3.Distance(testPosition, resourceNode.GetPosition()) < Vector3.Distance(testPosition, closest.GetPosition())) {
                    // Closer
                    closest = resourceNode;
                }
            }
        }

        return closest;
    }

    [SerializeField] private ResourceTypeSO resourceTypeSO;
    [SerializeField] private Track track;

    private List<ResourceNodeSingle> resourceNodeSingleList;
    private float radius;

    private float resourceRespawnTimer;
    [SerializeField] private float resourceRespawnTimerMax = 1f;
    [SerializeField] private int maxResourceAmount = 20;

    [SerializeField] private int gathererAmount = 0;

    private void Awake() {
        resourceNodeList.Add(this);
        ResourceGathererManager.Instance.AddResourceNode(this);

        resourceNodeSingleList = new List<ResourceNodeSingle>();

        radius = 25;
        for (int i = 0; i < 5; i++) {
            CreateResourceNodeSingle();
        }
    }

    private void Update() {
        resourceRespawnTimer -= Time.deltaTime;
        if (resourceRespawnTimer < 0) {
            resourceRespawnTimer += resourceRespawnTimerMax;

            if (resourceNodeSingleList.Count < maxResourceAmount) {
                CreateResourceNodeSingle();
            }
        }
    }

    private void CreateResourceNodeSingle() {
        Vector3 spawnPosition = transform.position + Utils.GetRandomDir() * UnityEngine.Random.Range(0, radius);

        Transform resourceNodeSingleTransform = Instantiate(resourceTypeSO.prefab, spawnPosition, Quaternion.identity);
        ResourceNodeSingle resourceNodeSingle = resourceNodeSingleTransform.GetComponent<ResourceNodeSingle>();
        resourceNodeSingle.Setup(this, resourceTypeSO);

        resourceNodeSingleList.Add(resourceNodeSingle);
    }

    public void ResourceNodeSingleDestroyed(ResourceNodeSingle resourceNodeSingle) {
        resourceNodeSingleList.Remove(resourceNodeSingle);

        if (resourceNodeSingleList.Count == 0) {
            // No more nodes!
            CreateResourceNodeSingle();
        }
    }

    public ResourceNodeSingle GetRandomResourceNodeSingle() {
        return resourceNodeSingleList[UnityEngine.Random.Range(0, resourceNodeSingleList.Count)];
    }

    public ResourceTypeSO GetResourceTypeSO() {
        return resourceTypeSO;
    }

    public Track GetTrack() {
        return track;
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public void AddGathererAmount() {
        gathererAmount++;
        gathererAmount = Mathf.Clamp(gathererAmount, 0, gathererAmount);
        OnAnyGathererAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveGathererAmount() {
        gathererAmount--;
        gathererAmount = Mathf.Clamp(gathererAmount, 0, gathererAmount);
        OnAnyGathererAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetGathererAmount() {
        return gathererAmount;
    }

}
