using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class BloodView : MonoBehaviour {
    public void Awake() {
        Fade();
    }

    private void Fade() {
        Color color = renderer.material.color;

        TweenParms parms = new TweenParms();
        parms.Prop("color", new Color(color.r, color.g, color.b, 0));
        parms.Ease(EaseType.Linear);
        parms.OnComplete(OnFadeComplete);

        HOTween.To(renderer.material, 10.0f, parms);
    }

    private void OnFadeComplete() {
        GameObject.Destroy(gameObject);
    }
}
