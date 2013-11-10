using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Follow : MonoBehaviour {
    public Transform Target;

    private float _velocity;
    private float _velocityMax = 40.0f;
    private float _acceleration;
    private float _accelerationMax = 10.0f;
    private float _jerk = 2.0f;

    public void Awake() {
        _velocity = _velocityMax;
        _acceleration = _accelerationMax;
    }

    public void Update() {
        if(Target == null) return;

        _acceleration = Mathf.Min(_acceleration + _jerk, _accelerationMax);
        _velocity = Mathf.Min(_velocity + _acceleration, _velocityMax);

        float angle = AngleToTarget();
        float vx = Mathf.Cos(angle) * _velocity;
        float vz = Mathf.Sin(angle) * _velocity;

        rigidbody.velocity = new Vector3(vx, 0, vz);
	}

    private float AngleToTarget() {
        float distX = Target.position.x - gameObject.transform.position.x;
        float distZ = Target.position.z - gameObject.transform.position.z;
        return Mathf.Atan2(distZ, distX);
    }   

    // TODO: the logic for controlling an enemy's reaction should be on a separate component
    public void React() {
        _acceleration = 0;
        _velocity = -400.0f;
    }
}
