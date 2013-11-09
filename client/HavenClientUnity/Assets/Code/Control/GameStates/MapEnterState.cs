using UnityEngine;
using System.Collections;
using System;

public class MapEnterState : BaseGameState {
    private Map _map;

    public MapEnterState() 
        : base(GameStates.MapEnter) {

    }

    public override void EnterState() {
        base.EnterState();

        CreateMap();
        SetCamera();
        PlacePlayer();

        ExitState();
    }

    public override void Dispose() {
        _map = null;

        base.Dispose();
    }

    private void CreateMap() {
        _map = new Map(100);

        MapView mapView = UnityUtils.LoadResource<GameObject>("Prefabs/MapView", true).GetComponent<MapView>();
        mapView.SetModel(_map);

        //MapView mapView = UnityUtils.LoadResource<GameObject>("Prefabs/MapView", true).GetComponent<MapView>();
        //mapView.gameObject.name = "MapView";

        //mapView.SetMap(_map);
        //mapView.UpdateRoomBounds(_map.Entrance.Coord);

        //GameManager.Instance.MapView = mapView;
    }

    private void SetCamera() {
        int blockSize = GameConfig.BLOCK_SIZE;

        Camera cam = Camera.main;

        cam.transform.position = new Vector3(0, 200.0f, -100.0f);
        cam.transform.LookAt(Vector3.zero);
    }

    private void PlacePlayer() {
        //PlayerView playerView = UnityUtils.LoadResource<GameObject>("Prefabs/PlayerView", true).GetComponent<PlayerView>();
        //playerView.gameObject.name = "PlayerView";

        //MapView mapView = GameManager.Instance.MapView;
        //Vector2 mapCenter = mapView.RoomBounds.center;
        //playerView.transform.position = new Vector3(mapCenter.x, GameConfig.BLOCK_SIZE, mapCenter.y);

        //GameManager.Instance.PlayerView = playerView;
    }
}
