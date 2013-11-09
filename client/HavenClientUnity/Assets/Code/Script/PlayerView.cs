using UnityEngine;
using System.Collections;

public class PlayerView : MonoBehaviour {
    private Player _model;

    public void SetModel(Player player) {
        _model = player;
    }

    public void Move(float h, float v, Camera camera) {
        Vector3 forward = camera.transform.forward;
        rigidbody.velocity = (camera.transform.right * h + new Vector3(forward.x, 0, forward.z) * v) * 100.0f;
    }
}
