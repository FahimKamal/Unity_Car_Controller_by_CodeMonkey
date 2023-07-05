using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractions : MonoBehaviour {


    [SerializeField] private GameObject teleportUIGameObject;
    [SerializeField] private GameObject shopUIGameObject;
    [SerializeField] private GameObject buildUIGameObject;
    [SerializeField] private GameObject resourceNodeUIGameObject;

    [SerializeField] private TextMeshProUGUI resourceGathererAmountTMPro;
    [SerializeField] private UnityEngine.UI.Image resourceNodeTypeImage;

    private Player player;
    private CarDriver carDriver;
    private Vector3 teleportBackToWorldPosition;

    private void Awake() {
        player = GetComponent<Player>();
        carDriver = GetComponent<CarDriver>();

        teleportUIGameObject.SetActive(false);
        shopUIGameObject.SetActive(false);
        buildUIGameObject.SetActive(false);
        resourceNodeUIGameObject.SetActive(false);
    }

    private void Update() {

        float interactDistance = 20f;

        switch (player.GetLocation()) {
            case Player.Location.World:
                // Track Teleporter
                ResourceNode resourceNode = ResourceNode.GetClosestResourceNode(GetPosition());

                if (Vector3.Distance(resourceNode.GetPosition(), GetPosition()) < interactDistance) {
                    teleportUIGameObject.SetActive(true);
                } else {
                    teleportUIGameObject.SetActive(false);
                }

                if (teleportUIGameObject.activeSelf) {
                    if (Input.GetKeyDown(KeyCode.E)) {
                        resourceNode = ResourceNode.GetClosestResourceNode(GetPosition());

                        if (Vector3.Distance(resourceNode.GetPosition(), GetPosition()) < interactDistance) {
                            teleportBackToWorldPosition = transform.position;
                            player.SetLocation(Player.Location.Track, resourceNode.GetResourceTypeSO());
                            resourceNode.GetTrack().TeleportPlayer(transform, resourceNode.GetResourceTypeSO());
                            teleportUIGameObject.SetActive(false);
                            resourceNodeUIGameObject.SetActive(false);
                        }
                    }
                }

                // Resource Node Gatherers
                if (Vector3.Distance(resourceNode.GetPosition(), GetPosition()) < interactDistance) {
                    resourceNodeUIGameObject.SetActive(true);
                    resourceNodeTypeImage.sprite = resourceNode.GetResourceTypeSO().sprite;
                } else {
                    resourceNodeUIGameObject.SetActive(false);
                }

                if (resourceNodeUIGameObject.activeSelf) {
                    resourceGathererAmountTMPro.text = resourceNode.GetGathererAmount().ToString();

                    if (Input.GetKeyDown(KeyCode.T)) {
                        resourceNode.AddGathererAmount();
                    }
                    if (Input.GetKeyDown(KeyCode.G)) {
                        resourceNode.RemoveGathererAmount();
                    }
                }



                // Shop UI
                Shop shop = Building.GetClosestShop(GetPosition());

                if (shop != null && Vector3.Distance(shop.GetPosition(), GetPosition()) < interactDistance) {
                    shopUIGameObject.SetActive(true);
                } else {
                    shopUIGameObject.SetActive(false);
                }

                if (shopUIGameObject.activeSelf) {
                    if (Input.GetKeyDown(KeyCode.T)) {
                        // Queue Car
                        if (ResourceManager.Instance.TrySpend(GameAssets.Instance.carUnitType.resourceTypeAmountList)) {
                            shop.QueueCar();
                        } else {
                            PlayerTooltip.Instance.Show("Cannot afford cost!");
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.G)) {
                        // Queue Truck
                        if (ResourceManager.Instance.TrySpend(GameAssets.Instance.truckUnitType.resourceTypeAmountList)) {
                            shop.QueueTruck();
                        } else {
                            PlayerTooltip.Instance.Show("Cannot afford cost!");
                        }
                    }
                }



                // Build UI
                if (Input.GetKeyDown(KeyCode.Q)) {
                    buildUIGameObject.SetActive(!buildUIGameObject.activeSelf);
                }

                if (buildUIGameObject.activeSelf) {
                    if (Input.GetKeyDown(KeyCode.T)) {
                        // Build Fuel Station
                        if (ResourceManager.Instance.TrySpend(GameAssets.Instance.outpostBuildingType.constructionResourceTypeAmountList)) {
                            BuildingManager.Instance.Create(GameAssets.Instance.outpostBuildingType, GetPosition(), transform.rotation);
                            buildUIGameObject.SetActive(false);
                        } else {
                            PlayerTooltip.Instance.Show("Cannot afford cost!");
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.G)) {
                        // Build Shop
                        if (ResourceManager.Instance.TrySpend(GameAssets.Instance.shopBuildingType.constructionResourceTypeAmountList)) {
                            BuildingManager.Instance.Create(GameAssets.Instance.shopBuildingType, GetPosition(), transform.rotation);
                            buildUIGameObject.SetActive(false);
                        } else {
                            PlayerTooltip.Instance.Show("Cannot afford cost!");
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.B)) {
                        // Build Outpost
                        if (ResourceManager.Instance.TrySpend(GameAssets.Instance.fuelStationBuildingType.constructionResourceTypeAmountList)) {
                            BuildingManager.Instance.Create(GameAssets.Instance.fuelStationBuildingType, GetPosition(), transform.rotation);
                            buildUIGameObject.SetActive(false);
                        } else {
                            PlayerTooltip.Instance.Show("Cannot afford cost!");
                        }
                    }
                }
                break;


            case Player.Location.Track:
                if (Input.GetKeyDown(KeyCode.E)) {
                    carDriver.StopCompletely();
                    transform.position = teleportBackToWorldPosition;
                    player.SetLocation(Player.Location.World);
                }
                break;
        }






        /*
        if (Input.GetKeyDown(KeyCode.Q)) {
            List<CarAI> carAIList = CarAI.GetCarAIList();
            foreach (CarAI carAI in carAIList) {
                carAI.SetPatrolState(transform.position);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            List<CarAI> carAIList = CarAI.GetCarAIList();
            foreach (CarAI carAI in carAIList) {
                carAI.SetNormalState(transform.position);
            }
        }
        */
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

}
