using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
    public float SpawnRadius;

    private List<EnemyView> _enemies;
    private TimeKeeper _enemyTimer;

    public void Awake() {
        SpawnRadius = 500.0f;

        _enemies = new List<EnemyView>();

        _enemyTimer = TimeKeeper.GetTimer(1);
        _enemyTimer.OnTimer += SpawnEnemy;
    }

    public void Dispose() {
        _enemyTimer.OnTimer -= SpawnEnemy;
    }

    public void Start() {
        //_enemyTimer.StartTimer();
    }

    private void SpawnEnemy(TimeKeeper timer) {
        EnemyView enemyView = UnityUtils.LoadResource<GameObject>("Prefabs/SwarmerView", true).GetComponent<EnemyView>();

        Follow ai = enemyView.gameObject.GetComponent<Follow>();
        ai.Target = GameManager.Instance.PlayerView.transform;

        float angle = Random.Range(0, 2 * Mathf.PI);
        float x = SpawnRadius * Mathf.Cos(angle);
        float z = SpawnRadius * Mathf.Sin(angle);

        enemyView.transform.position = new Vector3(x, 0, z);
        enemyView.transform.localScale *= 2.0f;
        enemyView.transform.parent = transform;

        _enemies.Add(enemyView);
    }

    public void DestroyEnemy(EnemyView enemy) {
        _enemies.Remove(enemy);
        GameObject.Destroy(enemy.gameObject);
    }
}
