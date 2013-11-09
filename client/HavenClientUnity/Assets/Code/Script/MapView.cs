using UnityEngine;
using System.Collections;

public class MapView : MonoBehaviour {
    private GameObject _groundView;
    private WallView _wallView;

    private Map _map;

    public void Awake() {
        _groundView = GameObject.Find("GroundView");
        _wallView = GameObject.Find("WallView").GetComponent<WallView>();
    }

    public void SetModel(Map map) {
        _map = map;

        _groundView.transform.localScale = new Vector3(GameConfig.BLOCK_SIZE * _map.Width, 1, GameConfig.BLOCK_SIZE * _map.Height);

        _wallView.SetModel(_map.Wall);
    }
}
