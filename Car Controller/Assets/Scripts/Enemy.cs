using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable {

    public static event EventHandler OnAnyEnemyDied;
    public static event EventHandler OnAnyEnemySpawned;

    private static List<Enemy> enemyList = new List<Enemy>();

    public static int GetAliveEnemyCount() {
        return enemyList.Count;
    }

    public static Enemy GetClosestEnemy(Vector3 testPosition, float maxRange) {
        Enemy closestEnemy = null;

        foreach (Enemy enemy in enemyList) {
            float distance = Vector3.Distance(testPosition, enemy.GetPosition());
            if (distance < maxRange) {
                // Within range
                if (closestEnemy == null) {
                    closestEnemy = enemy;
                } else {
                    if (distance < Vector3.Distance(testPosition, closestEnemy.GetPosition())) {
                        // Closer
                        closestEnemy = enemy;
                    }
                }
            }
        }

        return closestEnemy;
    }




    private bool isDead;
    private Rigidbody enemyRigidbody;
    private Vector3 spawnPosition;
    private Vector3 targetPosition;
    private EnemySpawner enemySpawner;

    private void Awake() {
        enemyList.Add(this);

        enemyRigidbody = GetComponent<Rigidbody>();

        spawnPosition = transform.position;
        UpdateTargetPosition();
    }

    private void Start() {
        OnAnyEnemySpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update() {
        if (Vector3.Distance(transform.position, targetPosition) > 1f) {
            Vector3 dir = (targetPosition - transform.position).normalized;
            //Debug.Log(dir + " " + Utils.GetAngleFromVector(dir));
            transform.eulerAngles = new Vector3(0, -Utils.GetAngleFromVector(dir), 0);
            float moveSpeed = 1f;
            enemyRigidbody.velocity = dir * moveSpeed;
        } else {
            UpdateTargetPosition();
        }
    }

    private void UpdateTargetPosition() {
        targetPosition = spawnPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(0f, 20f);
    }

    public bool IsDead() {
        return isDead;
    }

    public void SetEnemySpawner(EnemySpawner enemySpawner) {
        this.enemySpawner = enemySpawner;
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    private void OnCollisionEnter(Collision collision) {
        int collisionLayer = collision.gameObject.layer;

        if (collisionLayer == GameHandler.PLAYER_LAYER || collisionLayer == GameHandler.CARS_LAYER) {
            Damage();
        }
    }

    private void Damage() {
        // Die
        isDead = true;
        enemyList.Remove(this);
        enemySpawner.EnemyDied(this);

        Instantiate(GameAssets.Instance.pfEnemyDieParticles, transform.position + Vector3.up * 1f, Quaternion.Euler(-90, 0, 0));

        Destroy(gameObject);

        GameWinStats.zombiesKilled++;
        OnAnyEnemyDied?.Invoke(this, EventArgs.Empty);
    }


}
