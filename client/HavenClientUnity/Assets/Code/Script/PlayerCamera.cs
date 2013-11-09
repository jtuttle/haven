using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {
    public GameObject Target;

    public float Distance;
    public float Height;
    public float _angle;

    public void Start() {
        Distance = 100.0f;
        Height = 100.0f;
        _angle = 0;

        GameManager.Instance.Input.OnCameraInput += OnCameraInput;
    }

    public void Destroy() {
        GameManager.Instance.Input.OnCameraInput -= OnCameraInput;
    }

	// Update is called once per frame
	public void Update () {
        if(Target != null) {
            float x = Mathf.Cos(_angle) * Distance;
            float z = Mathf.Sin(_angle) * Distance;

            Vector3 playerPos = Target.transform.position;

            transform.position = new Vector3(playerPos.x + x, Height, playerPos.z + z);
            transform.LookAt(Target.transform.position);
        }
	}

    private void OnCameraInput(float h) {
        _angle += h * 0.05f;
    }
}
