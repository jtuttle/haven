using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class PlayerView : MonoBehaviour {
    public Player Model { get; private set; }

    private bool _tweening;

    public void Awake() {
        _tweening = false;
    }

    public void SetModel(Player player) {
        Model = player;
    }

    public void Move(float h, float v, Camera camera) {
        if(_tweening) return;

        Vector3 forward = camera.transform.forward;
        Vector3 velocity = (camera.transform.right * h + new Vector3(forward.x, 0, forward.z) * v) * 100.0f;

        if(Model.OnWall) {
            XY worldCoord = new XY((int)transform.position.x, (int)transform.position.z);
            XY mapCoord = GameManager.Instance.MapView.GetMapCoordFromWorldCoord(worldCoord);

            Wall wall = GameManager.Instance.GameModel.Map.Wall;
            bool hori = wall.OnHorizontal(mapCoord);
            bool vert = wall.OnVertical(mapCoord);
            float wallBound = wall.Radius * GameConfig.BLOCK_SIZE;

            SnapToWall(wall, hori, vert);

            // TODO: there HAS to be a better way to handle this...clean it up
            if(hori && !vert || (velocity.z < 0 && transform.position.z == -wallBound) || (velocity.z > 0 && transform.position.z == wallBound))
                velocity = new Vector3(velocity.x, velocity.y, 0);

            if(vert && !hori || (velocity.x < 0 && transform.position.x == -wallBound) || (velocity.x > 0 && transform.position.x == wallBound))
                velocity = new Vector3(0, velocity.y, velocity.z);
        }

        rigidbody.velocity = velocity;
    }

    private void SnapToWall(Wall wall, bool h, bool v) {
        Vector3 pos = gameObject.transform.position;
        float wallX = (wall.Radius * GameConfig.BLOCK_SIZE) * (pos.x < 0 ? -1 : 1);
        float wallZ = (wall.Radius * GameConfig.BLOCK_SIZE) * (pos.z < 0 ? -1 : 1);

        gameObject.transform.position = new Vector3(v ? wallX : pos.x, pos.y, h ? wallZ : pos.z);
    }

    public void DoAscendWallTween(WallPieceView wallPieceView) {
        _tweening = true;

        rigidbody.velocity = Vector3.zero;
        rigidbody.detectCollisions = false;

        TweenToWallHeight(wallPieceView);
    }

    private void TweenToWallHeight(WallPieceView wallPieceView) {
        Vector3 pos = transform.position;

        float targetY = wallPieceView.transform.position.y + (wallPieceView.transform.localScale.y / 2);// -(transform.localScale.y / 2);

        TweenParms parms = new TweenParms();
        parms.Prop("position", new Vector3(pos.x, targetY, pos.z));
        parms.Ease(EaseType.Linear);
        parms.OnComplete(OnTweenToWallHeightComplete, wallPieceView.transform.position);

        HOTween.To(gameObject.transform, 0.5f, parms);
    }

    private void OnTweenToWallHeightComplete(TweenEvent e) {
        Vector3 wallPiecePos = (Vector3)e.parms[0];
        TweenToWallCenter(wallPiecePos);
    }

    private void TweenToWallCenter(Vector3 wallPieceViewPos) {
        Vector3 pos = transform.position;

        TweenParms parms = new TweenParms();
        parms.Prop("position", new Vector3(wallPieceViewPos.x, pos.y, wallPieceViewPos.z));
        parms.Ease(EaseType.Linear);
        parms.OnComplete(OnTweenToWallCenterComplete);

        HOTween.To(gameObject.transform, 0.5f, parms);
    }

    private void OnTweenToWallCenterComplete(TweenEvent e) {
        rigidbody.detectCollisions = true;
        _tweening = false;
    }

    public void DoDescendWallTween(WallPieceView wallPieceView, Vector3 direction) {
        _tweening = true;

        rigidbody.velocity = Vector3.zero;
        rigidbody.detectCollisions = false;

        TweenOffWall(wallPieceView, direction);
    }

    private void TweenOffWall(WallPieceView wallPieceView, Vector3 direction) {
        float wallPieceScaleY = wallPieceView.transform.localScale.y;

        Vector3 target = (wallPieceView.transform.position + new Vector3(0, wallPieceScaleY / 2, 0)) + (direction * GameConfig.BLOCK_SIZE) * (0.75f);

        Vector3 aboveGroundPos = wallPieceView.transform.position + (direction * GameConfig.BLOCK_SIZE);
        Vector3 groundPos = new Vector3(aboveGroundPos.x, 0, aboveGroundPos.z);

        TweenParms parms = new TweenParms();
        parms.Prop("position", target);
        parms.Ease(EaseType.Linear);
        parms.OnComplete(OnTweenOffWallComplete, groundPos);

        HOTween.To(gameObject.transform, 0.5f, parms);
    }

    private void OnTweenOffWallComplete(TweenEvent e) {
        Vector3 groundPos = (Vector3)e.parms[0];

        TweenToGround(groundPos);
    }

    private void TweenToGround(Vector3 groundPos) {
        TweenParms parms = new TweenParms();
        parms.Prop("position", groundPos);
        parms.Ease(EaseType.Linear);
        parms.OnComplete(OnTweenToGroundComplete);

        HOTween.To(gameObject.transform, 0.5f, parms);
    }

    private void OnTweenToGroundComplete(TweenEvent e) {
        rigidbody.detectCollisions = true;
        _tweening = false;
    }
}
