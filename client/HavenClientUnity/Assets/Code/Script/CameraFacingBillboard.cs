using UnityEngine;
using System.Collections;

public class CameraFacingBillboard : MonoBehaviour {
    private Camera _camera;

    public void Awake() {
        _camera = GameManager.Instance.PlayerCamera.camera;
    }

    public void Update() {
        transform.LookAt(
            transform.position + _camera.transform.rotation * Vector3.back,
            _camera.transform.rotation * Vector3.up
        );

        // hax, for some reason default script didn't work
        transform.Rotate(new Vector3(90.0f, 0, 0));
    }
}