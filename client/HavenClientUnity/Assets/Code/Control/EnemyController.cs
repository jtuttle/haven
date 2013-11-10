using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController {
    private float _spawnRadius;

    private List<EnemyView> _enemies;

    private TimeKeeper _enemyTimer;

    public EnemyController(float spawnRadius) {
        _spawnRadius = spawnRadius;

        _enemies = new List<EnemyView>();

        _enemyTimer = TimeKeeper.GetTimer(5);
        _enemyTimer.OnTimer += SpawnEnemy;
    }

    public void Dispose() {
        _enemyTimer.OnTimer -= SpawnEnemy;
    }

    public void Start() {
        _enemyTimer.StartTimer();
    }

    private void SpawnEnemy(TimeKeeper timer) {
        Debug.Log("FUCK");

        EnemyView enemyView = UnityUtils.LoadResource<GameObject>("Prefabs/SwarmerView", true).GetComponent<EnemyView>();

        float angle = Random.Range(0, 2 * Mathf.PI);

        float x = _spawnRadius * Mathf.Cos(angle);
        float z = _spawnRadius * Mathf.Sin(angle);

        enemyView.transform.position = new Vector3(x, 0, z);
    }
}
