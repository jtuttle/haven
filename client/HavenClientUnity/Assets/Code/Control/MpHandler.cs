using System;
using System.Collections.Generic;
using UnityEngine;

public class MpHandler
{
	public MpClient Client { get; private set; }
	private List<Action> DelegatedWork { get; set; }
	
	public Dictionary<string, OtherPlayer> OtherPlayers { get; private set; }
	private Vector3 LastPositionSent = new Vector3(0,0,0);
	
	public MpHandler (MpClient client)
	{		
		OtherPlayers = new Dictionary<string, OtherPlayer>();
		DelegatedWork = new List<Action>();
		
		Client = client;
		Client.OnSomeoneJoined += SomeoneJoined;
		Client.OnSomeoneDropped += SomeoneDropped;;
		Client.OnPositionUpdate += PositionUpdate;
	}

	private void PositionUpdate (string cid, float px, float py, float pz)
	{
		if (!OtherPlayers.ContainsKey(cid)) {
			SomeoneJoined(cid);
		}
		DelegateWork(() => {
			OtherPlayers[cid].Actor.transform.position = new Vector3(px, py, pz);
		});
	}
	
	private void SomeoneJoined (string cid)
    {
		if (!OtherPlayers.ContainsKey (cid)) {
			DelegateWork(() => {
				if (!OtherPlayers.ContainsKey (cid)) {
					OtherPlayer op = new OtherPlayer(cid);
			    	OtherPlayers[cid] = op;
					op.Actor = UnityUtils.LoadResource<GameObject>("Prefabs/OtherPlayer", true);
					MpUtils.Log (MpUtils.LogLevel.Info, "MpHandler",
						"Someone joined {0}, now {1} other players",
						cid, OtherPlayers.Count);
				}
			});
		}
    }
	
	private void SomeoneDropped(string cid)
	{
		if (OtherPlayers.ContainsKey(cid)) {
			DelegateWork (() => {
				if (OtherPlayers.ContainsKey(cid)) {
					OtherPlayer op = OtherPlayers[cid];
					GameObject.Destroy(op.Actor);
					OtherPlayers.Remove(cid);
					MpUtils.Log (MpUtils.LogLevel.Info, "MpHandler",
						"Someone dropped out, was {0}, now {1} other players",
						cid, OtherPlayers.Count);
				}
			});
		}
	}
	
	public void PlayerMoved()
	{
		Vector3 pos = GameManager.Instance.PlayerView.transform.position;
		if (pos.x != LastPositionSent.x ||
			pos.y != LastPositionSent.y ||
			pos.z != LastPositionSent.z) {
			Client.SendPosition(pos.x, pos.y, pos.z);			
			LastPositionSent = new Vector3(pos.x, pos.y, pos.z);
		}
	}
	
	public void DoDelegatedWork()
	{
		List<Action> local = null;
		lock(DelegatedWork) {
			if (DelegatedWork.Count > 0) {
				local = new List<Action>(DelegatedWork);
				DelegatedWork.Clear();
			}
		}
		if (local != null) {
			foreach (Action action in local) {
				action.Invoke();
			}
		}
	}
	
	private void DelegateWork(Action action)
	{
		lock(DelegatedWork) {
			DelegatedWork.Add (action);
		}
	}
}
