using UnityEngine;
using Holoville.HOTween;

public class MapWalkState : BaseGameState {
    private PlayerView _playerView;
    private MapView _mapView;

    private EnemySpawner _enemySpawner;

    public MapWalkState() 
        : base(GameStates.MapWalk) {

        _playerView = GameManager.Instance.PlayerView;
        _mapView = GameManager.Instance.MapView;

        _enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
    }

    public override void EnterState() {
        base.EnterState();

        GameManager.Instance.Input.OnAxialLeftInput += OnAxialLeftInput;
        GameManager.Instance.Input.GetButton(ButtonId.Confirm).OnPress += OnConfirmPress;
        GameManager.Instance.Input.OnTriggerRightInput += OnTriggerRightInput;

        _enemySpawner.Start();
    }

    public override void ExitState() {
        GameManager.Instance.Input.OnAxialLeftInput -= OnAxialLeftInput;
        GameManager.Instance.Input.GetButton(ButtonId.Confirm).OnPress -= OnConfirmPress;
        GameManager.Instance.Input.OnTriggerRightInput -= OnTriggerRightInput;

        base.ExitState();
    }

    public override void Update() {
        Vector3 pos = _playerView.transform.position;
        XY mapCoord = _mapView.GetMapCoordFromWorldCoord(pos);
    }

    public override void Dispose() {
        
        base.Dispose();
    }

    private void OnAxialLeftInput(float h, float v) {
        _playerView.Move(h, v, GameManager.Instance.PlayerCamera.camera);
		GameManager.Instance.Multiplayer.PlayerMoved();
    }

    private void OnConfirmPress() {
        if(_playerView.Dead) {
            _playerView.gameObject.transform.position = Vector3.zero;
            _playerView.Resurrect();

            return;
        } 

        WallPieceView touchedPiece = _mapView.WallView.GetTouchedPiece();

        if(!_playerView.Model.OnWall) {
            if(touchedPiece != null) {
                _playerView.DoAscendWallTween(touchedPiece);
                GameManager.Instance.GameModel.Player.OnWall = true;
            }
        } else {
            if(touchedPiece != null) {
                Vector3 pos = _playerView.transform.position;
                XY mapCoord = _mapView.GetMapCoordFromWorldCoord(pos);

                bool onCorner = GameManager.Instance.GameModel.Map.Wall.OnCorner(mapCoord);
                bool onHori = GameManager.Instance.GameModel.Map.Wall.OnHorizontal(mapCoord);
                bool onVert = GameManager.Instance.GameModel.Map.Wall.OnVertical(mapCoord);

                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");

                Camera camera = GameManager.Instance.PlayerCamera.camera;
                Vector3 forward = camera.transform.forward;
                Vector3 direction = camera.transform.right * h + new Vector3(forward.x, 0, forward.z) * v;

                if(onCorner && (direction.z != 0 || direction.x != 0)) {
                    // TODO: ascend corners
                } else if(onHori && direction.z != 0) {
                    DescendFromWall(touchedPiece, new Vector3(0, 0, direction.z < 0 ? -1 : 1));
                } else if(onVert && direction.x != 0) {
                    DescendFromWall(touchedPiece, new Vector3(direction.x < 0 ? -1 : 1, 0, 0));
                }
            }
        }
    }

    private void DescendFromWall(WallPieceView wallPieceView, Vector3 direction) {
        GameManager.Instance.GameModel.Player.OnWall = false;
        _playerView.DoDescendWallTween(wallPieceView, direction);
    }

    private void OnTriggerRightInput() {
        if(_playerView.Dead) return;

        Vector3 arrowOrigin = _playerView.transform.position + new Vector3(0, 5.0f, 0);
        Vector3 rayOrigin = new Vector3(arrowOrigin.x, 5.0f, arrowOrigin.z);
        Vector3 direction = new Vector3(Input.GetAxis("RightHorizontal"), 0, Input.GetAxis("RightVertical"));

        RaycastHit hit;

        if(Physics.Raycast(rayOrigin, direction * 200.0f, out hit)) {
            GameObject target = hit.collider.gameObject;

            // remove enemy from collisions to avoid double shot
            if(target.tag == "Enemy")
                target.collider.enabled = false;

            ShootProjectile(arrowOrigin, hit.point, target);
        } else {
            float angle = (float)Mathf.Atan2(direction.z, direction.x);

            float radius = 100.0f;
            Vector3 target = rayOrigin + new Vector3(radius * (float)Mathf.Cos(angle), 0, radius * (float)Mathf.Sin(angle));

            ShootProjectile(arrowOrigin, target, null);
        }
    }

    private void ShootProjectile(Vector3 from, Vector3 to, GameObject target) {
        GameObject arrowView = UnityUtils.LoadResource<GameObject>("Prefabs/ArrowView", true);
        arrowView.transform.position = from;
        arrowView.transform.LookAt(to);

        TweenParms parms = new TweenParms();
        parms.Prop("position", to);
        parms.Ease(EaseType.Linear);
        parms.OnComplete(OnShootProjectileComplete, arrowView, target);

        float velocity = 200.0f;
        float distance = Vector3.Distance(from, to);
        float time = distance / velocity;
        
        HOTween.To(arrowView.transform, time, parms);
    }

    private void OnShootProjectileComplete(TweenEvent e) {
        GameObject arrowView = (GameObject)e.parms[0];
        GameObject target = (GameObject)e.parms[1];

        GameObject.Destroy(arrowView);

        if(target != null && target.tag == "Enemy")
            _enemySpawner.DestroyEnemy(target.GetComponent<EnemyView>());
    }
}
