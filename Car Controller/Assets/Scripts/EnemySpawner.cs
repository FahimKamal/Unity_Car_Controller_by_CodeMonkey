using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour, IDamageable {

    public static event EventHandler OnAnyEnemySpawnerDied;

    private static List<EnemySpawner> enemySpawnerList = new List<EnemySpawner>();

    public static int GetEnemySpawnerCount() {
        return enemySpawnerList.Count;
    }

    public static Enemy Create(Vector3 position) {
        Transform enemyTransform = Instantiate(GameAssets.Instance.pfEnemy, position, Quaternion.identity);

        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        return enemy;
    }

    public static EnemySpawner GetClosestEnemySpawner(Vector3 testPosition, float maxRange) {
        EnemySpawner closestEnemy = null;

        foreach (EnemySpawner enemySpawner in enemySpawnerList) {
            float distance = Vector3.Distance(testPosition, enemySpawner.GetPosition());
            if (distance < maxRange) {
                // Within range
                if (closestEnemy == null) {
                    closestEnemy = enemySpawner;
                } else {
                    if (distance < Vector3.Distance(testPosition, closestEnemy.GetPosition())) {
                        // Closer
                        closestEnemy = enemySpawner;
                    }
                }
            }
        }

        return closestEnemy;
    }




    [SerializeField] private Image healthBarImage;

    private List<Enemy> enemyList;
    [SerializeField] private int enemySpawnAmountMax = 10;

    private float spawnTimer;
    [SerializeField] private float spawnTimerMax = .2f;
    private int healthAmount;
    private int healthAmountMax = 100;

    private void Awake() {
        enemySpawnerList.Add(this);

        enemyList = new List<Enemy>();
        healthAmount = healthAmountMax;
    }

    private void Start() {
        UpdateHealthBar();
    }

    private void Update() {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer < 0) {
            spawnTimer += spawnTimerMax;

            if (enemyList.Count < enemySpawnAmountMax) {
                Enemy enemy = Create(transform.position + Utils.GetRandomDir() * UnityEngine.Random.Range(0, 50f));
                enemy.SetEnemySpawner(this);
                enemyList.Add(enemy);
            }
        }
    }


    private void OnTriggerEnter(Collider other) {
        int collisionLayer = other.gameObject.layer;

        if (collisionLayer == GameHandler.PLAYER_LAYER || collisionLayer == GameHandler.CARS_LAYER) {
            Damage();
        }
    }

    private void Damage() {
        healthAmount -= 5;
        UpdateHealthBar();
        PlaySound.Play("DamagedSound");

        if (healthAmount <= 0) {
            enemySpawnerList.Remove(this);
            Destroy(gameObject);
            PlaySound.Play("EnemySpawnerDestroyedSound");
            OnAnyEnemySpawnerDied?.Invoke(this, EventArgs.Empty);
        }
    }

    private void UpdateHealthBar() {
        healthBarImage.fillAmount = (float)healthAmount / healthAmountMax;
    }

    public void EnemyDied(Enemy enemy) {
        enemyList.Remove(enemy);
    }

    public bool IsDead() {
        return healthAmount <= 0;
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

}
