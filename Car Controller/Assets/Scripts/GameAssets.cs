using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour {

    public static GameAssets Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }


    public Transform pfResourceNodeSingle;
    public Transform pfFuelLine;
    public Transform pfEnemy;
    public Transform pfSelectionOrderVisual;
    public Transform pfEnemyDieParticles;


    public UnitTypeSO carUnitType;
    public UnitTypeSO truckUnitType;
    public UnitTypeSO enemyUnitType;

    public BuildingTypeSO outpostBuildingType;
    public BuildingTypeSO shopBuildingType;
    public BuildingTypeSO fuelStationBuildingType;


    public ResourceTypeSO rubberResourceType;
    public ResourceTypeSO steelResourceType;
    public ResourceTypeSO aluminiumResourceType;

}
