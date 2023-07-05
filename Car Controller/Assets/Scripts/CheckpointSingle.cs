using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSingle : MonoBehaviour {

    private Track track;
    private Transform visual;

    private void Awake() {
        visual = transform.Find("visual");
    }

    private void Start() {
        Hide();
    }

    public void Setup(Track track) {
        this.track = track;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == GameHandler.PLAYER_LAYER) {
            // Hit the player
            track.PlayerPassedCheckpoint(this);
        }
    }

    public void Show() {
        visual.gameObject.SetActive(true);
    }

    public void Hide() {
        visual.gameObject.SetActive(false);
    }

}
