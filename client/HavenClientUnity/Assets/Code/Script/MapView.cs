using UnityEngine;
using System.Collections;

public class MapView : MonoBehaviour {
    public WallView WallView { get; private set; }

    private GameObject _groundView;
    
    private Map _map;

    public void Awake() {
        _groundView = GameObject.Find("GroundView");
        WallView = GameObject.Find("WallView").GetComponent<WallView>();
    }

    public void SetModel(Map map) {
        _map = map;

        _groundView.transform.localScale = new Vector3(GameConfig.BLOCK_SIZE * _map.Width, 1, GameConfig.BLOCK_SIZE * _map.Height);

        WallView.SetModel(_map.Wall);
    }

    public XY GetMapCoordFromWorldCoord(XY worldCoord) {
        int blockSize = GameConfig.BLOCK_SIZE;
        int halfBlockSize = GameConfig.BLOCK_SIZE / 2;

        float worldX = (worldCoord.X + halfBlockSize * (worldCoord.X < 0 ? -1 : 1)) / blockSize;
        float worldZ = (worldCoord.Y + halfBlockSize * (worldCoord.Y < 0 ? -1 : 1)) / blockSize;

        return new XY((int)worldX, (int)worldZ);
    }

    public XY GetMapCoordFromWorldCoord(Vector3 worldCoord) {
        int blockSize = GameConfig.BLOCK_SIZE;
        int halfBlockSize = GameConfig.BLOCK_SIZE / 2;

        float worldX = (worldCoord.x + halfBlockSize * (worldCoord.x < 0 ? -1 : 1)) / blockSize;
        float worldZ = (worldCoord.z + halfBlockSize * (worldCoord.z < 0 ? -1 : 1)) / blockSize;

        return new XY((int)worldX, (int)worldZ);
    }
}
