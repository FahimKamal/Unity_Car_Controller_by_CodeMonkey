using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

    private enum Stage {
        GoToTrack,
        WaitingToLeaveTrack,
        BuildTruck,
        WaitForTruckToFinishConstruction,
        AddResourceNodeTruck,
        BuildCar,
        SelectCar,
        SetAttackPosition,
        DeselectCar,
        BuildFuelStation,
        DestroyEnemySpawner,
        Complete
    }

    private Stage stage;
    private float timer;

    private void Start() {
        Player.Instance.OnPlayerChangedLocation += Instance_OnPlayerChangedLocation;
        Shop.OnAnyCarQueued += Shop_OnAnyCarQueued;
        Shop.OnAnyTruckQueued += Shop_OnAnyTruckQueued;
        Shop.OnAnyTruckConstructed += Shop_OnAnyTruckConstructed;
        ResourceNode.OnAnyGathererAmountChanged += ResourceNode_OnAnyGathererAmountChanged;
        PlayerSelection.Instance.OnSelectedCarListChanged += Instance_OnSelectedCarListChanged;
        PlayerSelection.Instance.OnGiveAttackMoveOrder += Instance_OnGiveAttackMoveOrder;
        FuelManager.Instance.OnFuelStationAdded += Instance_OnFuelStationAdded;
        EnemySpawner.OnAnyEnemySpawnerDied += EnemySpawner_OnAnyEnemySpawnerDied;

        SetupStage(Stage.GoToTrack);
    }

    private void EnemySpawner_OnAnyEnemySpawnerDied(object sender, System.EventArgs e) {
        if (stage == Stage.DestroyEnemySpawner) {
            SetupStage(Stage.Complete);
        }
    }

    private void Update() {
        if (timer > 0) {
            timer -= Time.deltaTime;
            if (timer <= 0f) {
                // Timer complete
                if (stage == Stage.Complete) {
                    WorldPointer.Instance.Hide();
                }
            }
        }
    }

    private void Instance_OnFuelStationAdded(object sender, System.EventArgs e) {
        if (stage == Stage.BuildFuelStation) {
            SetupStage(Stage.DestroyEnemySpawner);
        }
    }

    private void Instance_OnGiveAttackMoveOrder(object sender, System.EventArgs e) {
        if (stage == Stage.SetAttackPosition) {
            SetupStage(Stage.DeselectCar);
        }
    }

    private void Instance_OnSelectedCarListChanged(object sender, System.EventArgs e) {
        if (stage == Stage.SelectCar) {
            if (PlayerSelection.Instance.GetSelectedCarAIListCount() > 0) {
                SetupStage(Stage.SetAttackPosition);
            }
        }

        if (stage == Stage.DeselectCar) {
            if (PlayerSelection.Instance.GetSelectedCarAIListCount() == 0) {
                SetupStage(Stage.BuildFuelStation);
            }
        }
    }

    private void ResourceNode_OnAnyGathererAmountChanged(object sender, System.EventArgs e) {
        if (stage == Stage.AddResourceNodeTruck) {
            SetupStage(Stage.BuildCar);
        }
    }

    private void Shop_OnAnyTruckConstructed(object sender, System.EventArgs e) {
        if (stage == Stage.WaitForTruckToFinishConstruction) {
            SetupStage(Stage.AddResourceNodeTruck);
        }
    }

    private void Shop_OnAnyTruckQueued(object sender, System.EventArgs e) {
        if (stage == Stage.BuildTruck) {
            SetupStage(Stage.WaitForTruckToFinishConstruction);
        }
    }

    private void Shop_OnAnyCarQueued(object sender, System.EventArgs e) {
        if (stage == Stage.BuildCar) {
            SetupStage(Stage.SelectCar);
        }
    }

    private void Instance_OnPlayerChangedLocation(object sender, Player.OnPlayerChangedLocationEventArgs e) {
        if (stage == Stage.GoToTrack) {
            WorldPointer.Instance.Hide();
            SetupStage(Stage.WaitingToLeaveTrack);
        }

        if (e.location == Player.Location.World) {
            if (stage == Stage.WaitingToLeaveTrack) {
                SetupStage(Stage.BuildTruck);
            }
        }
    }

    private void SetupStage(Stage stage) {
        this.stage = stage;

        switch (stage) {
            case Stage.GoToTrack:
                Vector3 steelTrackPosition = new Vector3(43, 0, -86);
                WorldPointer.Instance.Show("Go to\nTrack", () => steelTrackPosition);
                break;
            case Stage.WaitingToLeaveTrack:
                break;
            case Stage.BuildTruck:
                WorldPointer.Instance.Show("Build\nTruck", () => Building.GetFirstShop().GetPosition());
                break;
            case Stage.WaitForTruckToFinishConstruction:
                WorldPointer.Instance.Show("Wait for\nTruck...", () => Building.GetFirstShop().GetPosition());
                break;
            case Stage.AddResourceNodeTruck:
                steelTrackPosition = new Vector3(43, 0, -86);
                WorldPointer.Instance.Show("Add Truck\nResource Node", () => steelTrackPosition);
                break;
            case Stage.BuildCar:
                WorldPointer.Instance.Show("Build\nCar", () => Building.GetFirstShop().GetPosition());
                break;
            case Stage.SelectCar:
                WorldPointer.Instance.Show("Select Car\nPress-Release (Z)", () => CarAI.GetCarAIList()[0].GetPosition());
                break;
            case Stage.SetAttackPosition:
                Vector3 attackOrderPosition = new Vector3(160, 0, 40);
                WorldPointer.Instance.Show("Attack Move Order\nPress (X)", () => attackOrderPosition);
                break;
            case Stage.DeselectCar:
                WorldPointer.Instance.Show("Deselect Car\nPress (Z)", () => GameHandler.Instance.GetHQ().position);
                break;
            case Stage.BuildFuelStation:
                Vector3 buildFuelStationPosition = new Vector3(70, 0, -60);
                WorldPointer.Instance.Show("Build Fuel Station\nPress (Q)", () => buildFuelStationPosition);
                break;
            case Stage.DestroyEnemySpawner:
                Vector3 enemySpawnerPosition = EnemySpawner.GetClosestEnemySpawner(Player.Instance.transform.position, 9999f).GetPosition();
                WorldPointer.Instance.Show("Destroy Enemy Spawner\nSend Cars to Attack", () => enemySpawnerPosition);
                break;
            case Stage.Complete:
                timer = 5f;
                WorldPointer.Instance.Show("Tutorial Complete!\nClear the World of All Enemies!", () => GameHandler.Instance.GetHQ().position);
                break;
        }
    }

}
