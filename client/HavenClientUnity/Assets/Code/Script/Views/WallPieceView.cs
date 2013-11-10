using UnityEngine;
using System.Collections;

public class WallPieceView : MonoBehaviour {
    public bool Touching { get; private set; }

    public float Height { get { return (collider as BoxCollider).size.y * transform.localScale.y; } }

    public void Awake() {
        Touching = false;
    }

    public void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Player")
            Touching = true;
    }

    public void OnCollisionExit(Collision collision) {
        if(collision.gameObject.tag == "Player")
            Touching = false;
    }
}
