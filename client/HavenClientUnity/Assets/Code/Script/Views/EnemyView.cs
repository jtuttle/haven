using UnityEngine;
using System.Collections;

public class EnemyView : MonoBehaviour {
    public delegate void EnemyDieDelegate(EnemyView enemyView);
    public event EnemyDieDelegate OnEnemyDie = delegate { };

    public GameObject Visual;
    public int BloodCount;

    public AudioSource GrowlSound;
    public AudioSource DieSound;

    private TimeKeeper _growlTimer;
    private TimeKeeper _dieTimer;

    public void Awake() {
        _growlTimer = TimeKeeper.GetTimer(5);
        _growlTimer.transform.parent = transform;
        _growlTimer.OnTimer += OnGrowlTimer;
        _growlTimer.StartTimer();

        _dieTimer = TimeKeeper.GetTimer(DieSound.clip.length, 1);
        _dieTimer.transform.parent = transform;
        _dieTimer.OnTimerComplete += OnDieTimerComplete;
    }

    public void Growl() {
        GrowlSound.Play();
    }

    public void Die() {
        _growlTimer.StopTimer();
        _growlTimer.OnTimer -= OnGrowlTimer;

        GetComponent<Follow>().Target = null;
        Visual.renderer.enabled = false;

        DieSound.Play();
        MakeBlood();

        _dieTimer.StartTimer();
    }

    private void MakeBlood() {
        for(int i = 0; i < BloodCount; i++) {
            GameObject blood = UnityUtils.LoadResource<GameObject>("Prefabs/BloodView", true);

            Vector2 adjust = Vector2.zero;

            if(i > 0)
                adjust = Random.insideUnitCircle * 10.0f;

            blood.transform.position = transform.position + new Vector3(adjust.x, 0.05f, adjust.y);
        }
    }

    private void OnDieTimerComplete(TimeKeeper e) {
        OnEnemyDie(this);
    }

    private void OnGrowlTimer(TimeKeeper e) {
        Growl();
    }
}
