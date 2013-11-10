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
        PlacePlayer();
        AddTrees();

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
        // TODO
    }
}
