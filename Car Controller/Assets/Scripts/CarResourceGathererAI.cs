using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarResourceGathererAI : MonoBehaviour {

    private static List<CarResourceGathererAI> carResourceGathererAIList = new List<CarResourceGathererAI>();


    public static int GetIdleCount() {
        int idleCount = 0;
        foreach (CarResourceGathererAI carResourceGathererAI in carResourceGathererAIList) {
            if (carResourceGathererAI.IsIdle()) {
                idleCount++;
            }
        }
        return idleCount;
    }


    private enum State {
        Idle,
        GatheringResources,
        DroppingResources,
    }

    private State state;
    private CarDriverAI carDriverAI;
    private Outpost outpost;
    private ResourceNode resourceNode;
    private Vector3 gatheringResourcesTargetPosition;
    private float askForTargetResourceNodeTimer;
    private float askForTargetResourceNodeTimerMax = .25f;


    private Dictionary<ResourceTypeSO, int> resourceTypeAmountDic;
    private int resourceAmountMax = 5;

    [SerializeField] private TextMeshProUGUI resourceAmountText; // TODO: Separate from logic

    private void Awake() {
        carResourceGathererAIList.Add(this);

        carDriverAI = GetComponent<CarDriverAI>();

        state = State.Idle;

        resourceTypeAmountDic = new Dictionary<ResourceTypeSO, int>();

        resourceTypeAmountDic[GameAssets.Instance.rubberResourceType] = 0;
        resourceTypeAmountDic[GameAssets.Instance.steelResourceType] = 0;
        resourceTypeAmountDic[GameAssets.Instance.aluminiumResourceType] = 0;
    }

    private void Start() {
        CarDriver carDriver = GetComponent<CarDriver>();
        carDriver.SetSpeedMax(30f);
        //carDriver.SetTurnSpeedMax();

        carDriverAI.SetReachedTargetDistance(5f);
        carDriverAI.SetReverseDistance(10f);
        resourceAmountText.text = "0";

        carDriverAI.SetMoveToPosition(transform.position + transform.forward * 20f + Utils.GetRandomDir() * Random.Range(0, 10f));

        AskForTargetResourceNode();
    }

    private void Update() {
        switch (state) {
            case State.Idle:
                askForTargetResourceNodeTimer += Time.deltaTime;
                if (askForTargetResourceNodeTimer > askForTargetResourceNodeTimerMax) {
                    askForTargetResourceNodeTimer = 0;
                    AskForTargetResourceNode();
                }
                break;
            case State.GatheringResources:
                if (carDriverAI.GetHasReachedMoveToPosition()) {
                    // Go to another Resource Node Single
                    UpdateGatheringResourcesTargetPosition();
                } else {
                    carDriverAI.SetMoveToPosition(gatheringResourcesTargetPosition);
                }
                break;
            case State.DroppingResources:
                carDriverAI.SetMoveToPosition(outpost.GetPosition());

                if (carDriverAI.GetHasReachedMoveToPosition()) {
                    // Reached Outpost
                    // Drop all resources
                    DropResourceAmount();
                    // Go back to gathering
                    SetStateGatheringResources();
                }
                break;
        }
    }

    private void SetStateGatheringResources() {
        state = State.GatheringResources;
        //SetTargetResourceNode(ResourceNode.GetClosestResourceNode(transform.position));
        UpdateGatheringResourcesTargetPosition();
    }

    private void UpdateGatheringResourcesTargetPosition() {
        ResourceNodeSingle resourceNodeSingle = resourceNode.GetRandomResourceNodeSingle();
        gatheringResourcesTargetPosition = resourceNodeSingle.transform.position;
        carDriverAI.SetMoveToPosition(gatheringResourcesTargetPosition);
    }

    private void SetStateDroppingResources() {
        state = State.DroppingResources;
        outpost = Building.GetClosestOutpost(transform.position);
    }

    private void OnTriggerEnter(Collider other) {
        if (IsFullResourceAmount() || resourceNode == null) return; // Already full, don't grab resource!

        ResourceNodeSingle resourceNodeSingle = other.GetComponent<ResourceNodeSingle>();
        if (resourceNodeSingle != null) {
            // Hit a resource node single
            ResourceTypeSO resourceTypeSO = resourceNodeSingle.GetResourceTypeSO();
            resourceNodeSingle.DestroySelf();

            // Grab Resource
            AddResourceAmount(resourceTypeSO);
            if (IsFullResourceAmount()) {
                // Full! Go back
                SetStateDroppingResources();
            } else {
                // Not full, grab more
                UpdateGatheringResourcesTargetPosition();
            }
        }
    }

    private bool IsFullResourceAmount() {
        return GetTotalResourceAmount() >= resourceAmountMax;
    }

    private int GetTotalResourceAmount() {
        int totalAmount = 0;
        foreach (ResourceTypeSO resourceTypeSO in resourceTypeAmountDic.Keys) {
            totalAmount += resourceTypeAmountDic[resourceTypeSO];
        }
        return totalAmount;
    }

    private void AddResourceAmount(ResourceTypeSO resourceTypeSO) {
        resourceTypeAmountDic[resourceTypeSO]++;
        resourceAmountText.text = GetTotalResourceAmount().ToString();
    }

    private void DropResourceAmount() {
        foreach (ResourceTypeSO resourceTypeSO in resourceTypeAmountDic.Keys) {
            ResourceManager.Instance.AddResource(resourceTypeSO, resourceTypeAmountDic[resourceTypeSO]);
        }
        foreach (ResourceTypeSO resourceTypeSO in new List<ResourceTypeSO>(resourceTypeAmountDic.Keys)) {
            resourceTypeAmountDic[resourceTypeSO] = 0;
        }

        resourceAmountText.text = GetTotalResourceAmount().ToString();
    }

    public void AskForTargetResourceNode() {
        SetTargetResourceNode(ResourceGathererManager.Instance.GetTargetResourceNode(this));

        if (resourceNode != null) {
            SetStateGatheringResources();
        }
    }

    public void SetTargetResourceNode(ResourceNode resourceNode) {
        this.resourceNode = resourceNode;
    }

    public bool IsIdle() {
        return state == State.Idle;
    }

}
