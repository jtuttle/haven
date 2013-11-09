using UnityEngine;

public class TimeKeeper : MonoBehaviour {
    public delegate void TimerDelegate(TimeKeeper timer);
    public event TimerDelegate OnTimer = delegate { };

    public delegate void TimerCompleteDelegate(TimeKeeper timer);
    public event TimerCompleteDelegate OnTimerComplete = delegate { };

    public float TargetSeconds { get; set; }
    public float TargetCycles { get; private set; }

    public float ElapsedSeconds { get; private set; }
    public int ElapsedCycles { get; set; }

    public float RemainingSeconds { get { return TargetSeconds - ElapsedSeconds; } }

    public bool Running { get; private set; }
    public bool Paused { get; private set; }

    private TimeKeeper() { }

    public static TimeKeeper GetTimer(float targetSeconds, float targetCycles = 0, string timerName = "TimeKeeper", bool destroyOnLoad = true) {
        GameObject go = new GameObject();
        go.name = timerName;
        if(!destroyOnLoad) DontDestroyOnLoad(go);

        TimeKeeper timer = (TimeKeeper)go.AddComponent("TimeKeeper");
        timer.TargetSeconds = targetSeconds;
        timer.TargetCycles = targetCycles;

        return timer;
    }

    void Update() {
        if(!Running) return;

        ElapsedSeconds += Time.deltaTime;

        if(ElapsedSeconds >= TargetSeconds) {
            OnTimer(this);

            ElapsedCycles++;
            ElapsedSeconds = 0;

            if(TargetCycles > 0 && ElapsedCycles >= TargetCycles) {
                StopTimer();
                OnTimerComplete(this);
            }
        }
    }

    public void StartTimer() {
        Running = true;
        Paused = false;
    }

    public void StopTimer() {
        Running = false;
        Paused = false;
    }

    public void PauseTimer() {
        Running = false;
        Paused = true;
    }

    public void ResumeTimer() {
        StartTimer();
    }

    public void ResetTimer() {
        ElapsedSeconds = 0;
        ElapsedCycles = 0;
    }
}