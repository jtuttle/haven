using UnityEngine;
using System.Collections;

public class MapView : MonoBehaviour {
    private Map _map;

    public void SetMap(Map map) {
        _map = map;

        GameObject ground = UnityUtils.LoadResource<GameObject>("Prefabs/Grass", true);
        ground.transform.localScale = new Vector3(GameConfig.BLOCK_SIZE * _map.Width, 1, GameConfig.BLOCK_SIZE * _map.Height);
        ground.transform.parent = gameObject.transform;

        //for(int y = 0; y < map.Height; y++) {
        //    for(int x = 0; x < map.Width; x++) {
        //        GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //        tile.transform.position = new Vector3(GameConfig.BLOCK_SIZE * x, 0, GameConfig.BLOCK_SIZE * y);
        //        tile.transform.localScale = new Vector3(GameConfig.BLOCK_SIZE, 1, GameConfig.BLOCK_SIZE);
        //        tile.transform.parent = gameObject.transform;
        //    }
        //}
    }
}
