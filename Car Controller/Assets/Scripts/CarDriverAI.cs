using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriverAI : MonoBehaviour {

    private CarDriver carDriver;
    private Vector3 moveToPosition = new Vector3(0, 0, 100);
    private bool hasReachedMoveToPosition;
    [SerializeField] private Transform moveTarget;

    private float reachedTargetDistance = 20f;
    private float reverseDistance = 35f;

    private Vector3 lastPosition;
    private Vector3 lastStuckPosition;
    private float stuckTimer;

    private void Awake() {
        carDriver = GetComponent<CarDriver>();
    }

    private void Update() {
        float forwardAmount = 0f;
        float turnAmount = 0f;

        //moveToPosition = moveTarget.position;

        Vector3 dirToMovePosition = (moveToPosition - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, dirToMovePosition);

        
        float distanceToTarget = Vector3.Distance(transform.position, moveToPosition);
        if (distanceToTarget > reachedTargetDistance) {
            // Still too far, keep going
            hasReachedMoveToPosition = false;
            forwardAmount = 1f;

            /*
            float positionAngle = Utils.GetAngleFromVector(transform.forward);
            float positionAngleTarget = Utils.GetAngleFromVector(dirToMovePosition);
            Debug.Log(positionAngle + " " + positionAngleTarget  + " " + Vector3.SignedAngle(transform.forward, dirToMovePosition, Vector3.up));
            */

            float stoppingDistance = 30f;
            float stoppingSpeed = 80f;
            if (distanceToTarget < stoppingDistance && carDriver.GetSpeed() > stoppingSpeed) {
                // Within stopping distance and moving forward
                forwardAmount = -1f;
            }

            float dontTurnDot = .995f - .015f * (Mathf.Clamp(distanceToTarget, 0f, 100f) / 100f); // Smaller dontTurn angle when closer
            if (dot > dontTurnDot) {
                // Dont turn
                turnAmount = 0f;
                carDriver.ClearTurnSpeed();
            } else {
                float angleToDir = Vector3.SignedAngle(transform.forward, dirToMovePosition, Vector3.up);
                if (angleToDir < 0) {
                    turnAmount = -1f;
                } else {
                    turnAmount = +1f;
                }

                if (dot < 0) {
                    // Target is behind
                    if (distanceToTarget > reverseDistance) {
                        // Too far to reverse
                    } else {
                        forwardAmount = -1f;
                        turnAmount *= -1f;
                    }
                }
            }
        } else {
            // Close enough
            hasReachedMoveToPosition = true;
            if (carDriver.GetSpeed() > 15f) {
                // Hit the brakes!
                forwardAmount = -1f;
            }
        }

        HandleStuckSafety(forwardAmount);

        //Debug.Log(forwardAmount + " " + turnAmount);
        carDriver.SetInputs(forwardAmount, turnAmount);

        lastPosition = transform.position;
    }

    private void HandleStuckSafety(float forwardAmount) {
        if (!hasReachedMoveToPosition) {
            float stuckDistance = .6f;
            if (Vector3.Distance(transform.position, lastStuckPosition) < stuckDistance) {
                stuckTimer += Time.deltaTime;

                if (stuckTimer > 5) {
                    transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                    stuckTimer = 0;
                    lastStuckPosition = lastPosition;
                }
            } else {
                stuckTimer = 0;
                lastStuckPosition = lastPosition;
            }
        }
    }

    public void SetMoveToPosition(Vector3 moveToPosition) {
        this.moveToPosition = moveToPosition;

        float distanceToTarget = Vector3.Distance(transform.position, moveToPosition);
        hasReachedMoveToPosition = distanceToTarget < reachedTargetDistance;
    }

    public bool GetHasReachedMoveToPosition() {
        return hasReachedMoveToPosition;
    }

    public void SetReachedTargetDistance(float reachedTargetDistance) {
        this.reachedTargetDistance = reachedTargetDistance;
    }

    public void SetReverseDistance(float reverseDistance) {
        this.reverseDistance = reverseDistance;
    }

}
