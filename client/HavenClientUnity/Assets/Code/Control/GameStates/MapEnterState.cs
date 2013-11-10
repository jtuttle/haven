using UnityEngine;
using System.Collections;


public class MapEnterState : BaseGameState {
    private Map _map;

    public MapEnterState() 
        : base(GameStates.MapEnter) {

    }

    public override void EnterState() {
        base.EnterState();

        CreateMap();
        PlacePlayer();
        //AddTrees();

        ExitState();
    }

    public override void Dispose() {
        _map = null;

        base.Dispose();
    }

    private void CreateMap() {
        _map = new Map(100);
        GameManager.Instance.GameModel.Map = _map;

        MapView mapView = UnityUtils.LoadResource<GameObject>("Prefabs/MapView", true).GetComponent<MapView>();
        mapView.SetModel(_map);

        GameManager.Instance.MapView = mapView;
    }

    private void PlacePlayer() {
        Player player = new Player();
        GameManager.Instance.GameModel.Player = player;

        PlayerView playerView = UnityUtils.LoadResource<GameObject>("Prefabs/PlayerView", true).GetComponent<PlayerView>();
        playerView.SetModel(player);

        GameManager.Instance.PlayerView = playerView;
        GameManager.Instance.PlayerCamera.Target = playerView.gameObject;
    }

    private void AddTrees() {
        //float rMin = _map.Wall.Radius * GameConfig.BLOCK_SIZE;
        //float rMax = 400;

        float radius = 500;

        for(int k = 0; k < 100; k++) {
            //float x = Random.Range(rMin, rMax) * (Random.Range(0, 2) == 0 ? 1 : -1);
            //float z = Random.Range(rMin, rMax) * (Random.Range(0, 2) == 0 ? 1 : -1);

            float x = Random.Range(-radius, radius);
            float z = Random.Range(-radius, radius);

            GameObject tree = UnityUtils.LoadResource<GameObject>("Prefabs/TreeView", true);
            tree.transform.position = new Vector3(x, 0, z);
            tree.transform.localScale *= 5.0f;
        }
    }
}
