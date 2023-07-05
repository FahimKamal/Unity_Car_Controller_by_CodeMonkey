using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BuildingTypeSO : ScriptableObject {

    public string buildingName;
    public Transform prefab;
    public List<ResourceTypeAmount> constructionResourceTypeAmountList;

}
