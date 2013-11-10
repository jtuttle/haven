using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
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

        float rnd = Random.Range(0, 1.0f);

        string enemyPrefab = (rnd < 0.2f ? "GoreSuckerView" : "SwarmerView");
        float vMax = (rnd < 0.2f ? 20.0f : 30.0f);

        EnemyView enemyView = UnityUtils.LoadResource<GameObject>("Prefabs/" + enemyPrefab, true).GetComponent<EnemyView>();

        Follow ai = enemyView.gameObject.GetComponent<Follow>();
        ai.VelocityMax = vMax;
        ai.Target = GameManager.Instance.PlayerView.transform;

        float angle = Random.Range(0, 2 * Mathf.PI);
        float x = _spawnRadius * Mathf.Cos(angle);
        float z = _spawnRadius * Mathf.Sin(angle);

        enemyView.transform.position = new Vector3(x, 0, z);
        enemyView.transform.parent = transform;

        _enemies.Add(enemyView);
    }

    public void DestroyEnemy(EnemyView enemy) {
        _enemies.Remove(enemy);
        GameObject.Destroy(enemy.gameObject);
    }
}
