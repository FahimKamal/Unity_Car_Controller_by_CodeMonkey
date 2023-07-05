using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelLine : MonoBehaviour {

    private Material material;
    private float fuelSpeed;

    private void Awake() {
        material = new Material(transform.Find("visual").GetComponent<MeshRenderer>().material);
        transform.Find("visual").GetComponent<MeshRenderer>().material = material;
    }

    private void Start() {
        material.SetTextureScale("_BaseMap", new Vector2(transform.localScale.x / 10f, 1));
    }

    private void Update() {
        fuelSpeed += Time.deltaTime * .5f;
        material.SetTextureOffset("_BaseMap", new Vector2(-fuelSpeed, 0));
    }


}
