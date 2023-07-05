using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrackUI : MonoBehaviour {


    private Transform introTransform;
    private TextMeshProUGUI resourceTypeText;
    private float wrongCheckpointTimer;
    private Transform wrongCheckpoint;
    private Transform popupContainer;
    private Transform popupTemplate;
    private Transform teleportBack;

    private void Awake() {
        introTransform = transform.Find("intro");
        resourceTypeText = introTransform.Find("resourceTypeText").GetComponent<TextMeshProUGUI>();
        wrongCheckpoint = transform.Find("wrongCheckpoint");
        teleportBack = transform.Find("teleportBack");

        popupContainer = transform.Find("popupContainer");
        popupTemplate = popupContainer.Find("popupTemplate");
        popupTemplate.gameObject.SetActive(false);
    }

    private void Start() {
        Player.Instance.OnPlayerChangedLocation += Instance_OnPlayerChangedLocation;
        Track.OnAnyPassedCheckpoint += Track_OnAnyPassedCheckpoint;
        Track.OnAnyPassedWrongCheckpoint += Track_OnAnyPassedWrongCheckpoint;
        HideWrongCheckpoint();
        HideIntro();
        HideTeleportBack();
    }

    private void Update() {
        if (wrongCheckpointTimer > 0) {
            wrongCheckpointTimer -= Time.deltaTime;
            if (wrongCheckpointTimer <= 0) {
                HideWrongCheckpoint();
            }
        }
    }

    private void Track_OnAnyPassedCheckpoint(object sender, System.EventArgs e) {
        GameWinStats.trackCheckpoints++;
        HideIntro();
        SpawnPopup();
        HideWrongCheckpoint();
    }

    private void Track_OnAnyPassedWrongCheckpoint(object sender, System.EventArgs e) {
        wrongCheckpointTimer = 999f;
        ShowWrongCheckpoint();
    }

    private void Instance_OnPlayerChangedLocation(object sender, Player.OnPlayerChangedLocationEventArgs e) {
        if (e.location == Player.Location.Track) {
            ShowIntro(e.trackResourceType);
            popupTemplate.Find("image").GetComponent<Image>().sprite = e.trackResourceType.sprite;
            ShowTeleportBack();
        }
        if (e.location == Player.Location.World) {
            HideIntro();
            HideTeleportBack();
            HideWrongCheckpoint();
        }
    }

    private void SpawnPopup() {
        Transform popupTransform = Instantiate(popupTemplate, popupContainer);
        popupTransform.GetComponent<RectTransform>().anchoredPosition += new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * Random.Range(0f, 70f);
        popupTransform.gameObject.SetActive(true);
        Destroy(popupTransform.gameObject, 1f);
    }

    private void ShowWrongCheckpoint() {
        wrongCheckpoint.gameObject.SetActive(true);
    }

    private void HideWrongCheckpoint() {
        wrongCheckpoint.gameObject.SetActive(false);
    }

    private void ShowIntro(ResourceTypeSO resourceTypeSO) {
        introTransform.gameObject.SetActive(true);

        resourceTypeText.text = resourceTypeSO.resourceName + " Track";
    }

    private void HideIntro() {
        introTransform.gameObject.SetActive(false);
    }

    private void ShowTeleportBack() {
        teleportBack.gameObject.SetActive(true);
    }

    private void HideTeleportBack() {
        teleportBack.gameObject.SetActive(false);
    }


}
