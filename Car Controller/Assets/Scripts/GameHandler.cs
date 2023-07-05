using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class GameHandler : MonoBehaviour {

    public static GameHandler Instance { get; private set; }

    public const int SOLID_OBJECTS_LAYER = 8;
    public const int PLAYER_LAYER = 9;
    public const int CARS_LAYER = 10;
    public const int ENEMIES_LAYER = 11;


    public event EventHandler OnWin;

    [SerializeField] private CinemachineVirtualCamera worldCinemachineCamera;
    [SerializeField] private CinemachineVirtualCamera trackCinemachineCamera;

    [SerializeField] private Transform hqTransform;
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private List<FuelStation> startingFuelStations;

    private float gameTimer;

    private void Awake() {
        Instance = this;

        GameWinStats.Init();

        gameTimer = 0f;
    }

    private void Start() {
        Player.Instance.OnPlayerChangedLocation += Instance_OnPlayerChangedLocation;
        Enemy.OnAnyEnemyDied += Enemy_OnAnyEnemyDied;
        EnemySpawner.OnAnyEnemySpawnerDied += EnemySpawner_OnAnyEnemySpawnerDied;

        foreach (FuelStation fuelStation in startingFuelStations) {
            fuelStation.gameObject.SetActive(true);
        }
    }

    private void EnemySpawner_OnAnyEnemySpawnerDied(object sender, EventArgs e) {
        TestGameOver();
    }

    private void Enemy_OnAnyEnemyDied(object sender, EventArgs e) {
        TestGameOver();
    }

    private void Update() {
        gameTimer += Time.deltaTime;

        TimeSpan gameTimeSpan = TimeSpan.FromSeconds(gameTimer);

        timerText.text = gameTimeSpan.ToString(@"mm\:ss\.fff");
    }

    private void Instance_OnPlayerChangedLocation(object sender, Player.OnPlayerChangedLocationEventArgs e) {
        worldCinemachineCamera.Priority = 0;
        trackCinemachineCamera.Priority = 0;

        switch (e.location) {
            case Player.Location.World:
                worldCinemachineCamera.Priority = 10;
                break;
            case Player.Location.Track:
                trackCinemachineCamera.Priority = 10;
                break;
        }
    }

    private void TestGameOver() {
        if (EnemySpawner.GetEnemySpawnerCount() == 0 && Enemy.GetAliveEnemyCount() == 0) {
            // All dead, Game Over!
            GameWinStats.gameTime = gameTimer;
            OnWin?.Invoke(this, EventArgs.Empty);
        }
    }

    public Transform GetHQ() {
        return hqTransform;
    }

}
