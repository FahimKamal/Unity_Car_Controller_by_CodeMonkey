using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour {

    public static BuildingManager Instance { get; private set; }



    private void Awake() {
        Instance = this;
    }

    public void Create(BuildingTypeSO buildingTypeSO, Vector3 position, Quaternion rotation) {
        Transform buildingTransform = Instantiate(buildingTypeSO.prefab, position, rotation);
    }

}
