using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAttackAI : MonoBehaviour {

    private enum State {
        LookingForEnemy,
        Attacking,
    }

    private State state;
    private IDamageable targetIDamageable;
    private CarAI carAI;
    private CarFuelingAI carFuelingAI;
    private CarDriverAI carDriverAI;
    private CarAIMain carAIMain;
    private bool isEnabled;

    private void Awake() {
        carAI = GetComponent<CarAI>();
        carFuelingAI = GetComponent<CarFuelingAI>();
        carDriverAI = GetComponent<CarDriverAI>();
        carAIMain = GetComponent<CarAIMain>();

        state = State.LookingForEnemy;

        isEnabled = false;
    }

    private void Update() {
        switch (state) {
            case State.LookingForEnemy:
                float enemyDetectionRange = 50f;

                Enemy closestEnemy = Enemy.GetClosestEnemy(transform.position, enemyDetectionRange);
                EnemySpawner closestEnemySpawner = EnemySpawner.GetClosestEnemySpawner(transform.position, enemyDetectionRange);

                if (closestEnemySpawner != null) {
                    // Prioritize
                    targetIDamageable = closestEnemySpawner;
                } else {
                    if (closestEnemy != null) {
                        targetIDamageable = closestEnemy;
                    }
                }

                if (targetIDamageable != null) {
                    if (carAIMain.TryEnableAttackAI()) {
                        carDriverAI.SetReachedTargetDistance(5f);
                        carDriverAI.SetReverseDistance(10f);
                        SetStateAttacking();
                    }
                }
                break;
            case State.Attacking:
                if (targetIDamageable.IsDead()) {
                    SetStateLookingForEnemy();
                } else {
                    carDriverAI.SetMoveToPosition(targetIDamageable.GetPosition());
                }
                break;
        }
    }

    private void SetStateAttacking() {
        state = State.Attacking;
    }

    private void SetStateLookingForEnemy() {
        state = State.LookingForEnemy;
    }

    public void SetIsEnabled(bool isEnabled) {
        this.isEnabled = isEnabled;
    }

}
