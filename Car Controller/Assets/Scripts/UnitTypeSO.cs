using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class UnitTypeSO : ScriptableObject {

    public string unitName;
    public Sprite sprite;
    public Transform prefab;
    public float constructionTimerMax = 5f;
    public List<ResourceTypeAmount> resourceTypeAmountList;

}
