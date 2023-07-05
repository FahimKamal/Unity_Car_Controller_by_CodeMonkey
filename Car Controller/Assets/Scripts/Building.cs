using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

    public static List<Building> buildingList = new List<Building>();


    public static Outpost GetClosestOutpost(Vector3 testPosition) {
        Outpost closestOutpost = null;

        foreach (Building building in buildingList) {
            Outpost outpost = building.GetComponent<Outpost>();
            if (outpost != null) {
                if (closestOutpost == null) {
                    closestOutpost = outpost;
                } else {
                    if (Vector3.Distance(testPosition, outpost.GetPosition()) < Vector3.Distance(testPosition, closestOutpost.GetPosition())) {
                        // Closer
                        closestOutpost = outpost;
                    }
                }
            }
        }

        return closestOutpost;
    }

    public static Shop GetClosestShop(Vector3 testPosition) {
        Shop closest = null;

        foreach (Building building in buildingList) {
            Shop shop = building.GetComponent<Shop>();
            if (shop != null) {
                if (closest == null) {
                    closest = shop;
                } else {
                    if (Vector3.Distance(testPosition, shop.GetPosition()) < Vector3.Distance(testPosition, closest.GetPosition())) {
                        // Closer
                        closest = shop;
                    }
                }
            }
        }

        return closest;
    }

    public static Shop GetFirstShop() {
        foreach (Building building in buildingList) {
            Shop shop = building.GetComponent<Shop>();
            if (shop != null) {
                return shop;
            }
        }
        return null;
    }



    private void Awake() {
        buildingList.Add(this);
    }

}
