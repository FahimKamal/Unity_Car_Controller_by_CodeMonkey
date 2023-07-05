using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoalUI : MonoBehaviour {

    private TextMeshProUGUI enemyText;
    private TextMeshProUGUI enemySpawnerText;

    private void Awake() {
        enemyText = transform.Find("enemyText").GetComponent<TextMeshProUGUI>();
        enemySpawnerText = transform.Find("enemySpawnerText").GetComponent<TextMeshProUGUI>();
    }

    private void Start() {
        UpdateVisual();

        Enemy.OnAnyEnemyDied += Enemy_OnAnyEnemyDied;
        Enemy.OnAnyEnemySpawned += Enemy_OnAnyEnemySpawned;
        EnemySpawner.OnAnyEnemySpawnerDied += EnemySpawner_OnAnyEnemySpawnerDied;
    }

    private void Enemy_OnAnyEnemySpawned(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void EnemySpawner_OnAnyEnemySpawnerDied(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void Enemy_OnAnyEnemyDied(object sender, System.EventArgs e) {
        UpdateVisual();
    }

    private void UpdateVisual() {
        enemyText.text = Enemy.GetAliveEnemyCount().ToString();
        enemySpawnerText.text = EnemySpawner.GetEnemySpawnerCount().ToString();
    }


}
