using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour {
    private List<EnemyView> _enemies;
    private TimeKeeper _enemyTimer;

    private float _spawnRadius = 500.0f;
    private float _spawnMax = 20.0f;

    public void Awake() {
        _enemies = new List<EnemyView>();

        _enemyTimer = TimeKeeper.GetTimer(1);
        _enemyTimer.OnTimer += SpawnEnemy;
    }

    public void Dispose() {
        _enemyTimer.OnTimer -= SpawnEnemy;
    }

    public void Start() {
        
        _enemyTimer.StartTimer();
    }

    private void SpawnEnemy(TimeKeeper timer) {
        if(_enemies.Count >= _spawnMax) return;

        string enemyPrefab = (Random.Range(0, 1.0f) < 0.2f ? "GoreSuckerView" : "SwarmerView");

        EnemyView enemyView = UnityUtils.LoadResource<GameObject>("Prefabs/" + enemyPrefab, true).GetComponent<EnemyView>();
        enemyView.OnEnemyDie += OnEnemyDie;

        Follow ai = enemyView.gameObject.GetComponent<Follow>();
        ai.Target = GameManager.Instance.PlayerView.transform;

        float angle = Random.Range(0, 2 * Mathf.PI);
        float x = _spawnRadius * Mathf.Cos(angle);
        float z = _spawnRadius * Mathf.Sin(angle);

        enemyView.transform.position = new Vector3(x, 0, z);
        enemyView.transform.parent = transform;

        _enemies.Add(enemyView);
    }

    private void OnEnemyDie(EnemyView enemy) {
        _enemies.Remove(enemy);
        GameObject.Destroy(enemy.gameObject);
    }
}
