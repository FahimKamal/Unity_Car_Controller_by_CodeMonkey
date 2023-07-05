using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour {

    public static event EventHandler OnAnyPassedCheckpoint;
    public static event EventHandler OnAnyPassedWrongCheckpoint;



    private Transform playerSpawnPosition;
    private List<CheckpointSingle> checkpointSingleList;
    private int nextCheckpointIndex;
    private ResourceTypeSO resourceTypeSO;

    private void Awake() {
        playerSpawnPosition = transform.Find("SpawnPosition");

        checkpointSingleList = new List<CheckpointSingle>();
        Transform checkpoints = transform.Find("Checkpoints");
        foreach (Transform checkpointSingleTransform in checkpoints) {
            CheckpointSingle checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle>();
            checkpointSingle.Setup(this);

            checkpointSingleList.Add(checkpointSingle);
        }
    }

    public void TeleportPlayer(Transform playerTransform, ResourceTypeSO resourceTypeSO) {
        this.resourceTypeSO = resourceTypeSO;
        playerTransform.GetComponent<CarDriver>().StopCompletely();
        playerTransform.position = playerSpawnPosition.position;
        playerTransform.rotation = playerSpawnPosition.rotation;

        foreach (CheckpointSingle checkpointSingle in checkpointSingleList) {
            checkpointSingle.Hide();
        }

        nextCheckpointIndex = 0;
    }

    public void PlayerPassedCheckpoint(CheckpointSingle checkpointSingle) {
        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointIndex) {
            // Correct checkpoint
            CheckpointSingle correctCheckpoint = checkpointSingleList[nextCheckpointIndex];
            correctCheckpoint.Hide();

            // Get Resource
            ResourceManager.Instance.AddResource(resourceTypeSO, 1);
            PlaySound.Play("DingSound");

            nextCheckpointIndex = (nextCheckpointIndex + 1) % checkpointSingleList.Count;

            OnAnyPassedCheckpoint?.Invoke(this, EventArgs.Empty);
        } else {
            // Not the correct checkpoint, highlight correct one!
            CheckpointSingle correctCheckpoint = checkpointSingleList[nextCheckpointIndex];
            correctCheckpoint.Show();

            OnAnyPassedWrongCheckpoint?.Invoke(this, EventArgs.Empty);
        }
    }

}
