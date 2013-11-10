using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class MpClient
{
	public string Host { get; set; }
	public int Port { get; set; }
	
	public string Identity { get; private set; }
	
	public delegate void PositionUpdateHandler(string cid, float px, float py, float pz);
    public delegate void SomeoneJoinedHandler(string cid);
    public delegate void SomeoneDroppedHandler(string cid);
	
	public event PositionUpdateHandler OnPositionUpdate;
	public event SomeoneJoinedHandler OnSomeoneJoined;
	public event SomeoneDroppedHandler OnSomeoneDropped;
    
	private TcpClient Client;
	private NetworkStream Stream;
	private Thread ReadThread;
	private bool ContinueRunning;
	
	public MpClient ()
	{
		Host = "127.0.0.1";
		Port = 9000;
	}
	
	public void Start()
	{
		Client = new TcpClient();
		Client.Connect(Host, Port);
		Stream = Client.GetStream();
		ContinueRunning = true;
		ReadThread = new Thread(ReadSync);
		ReadThread.Start();
	}
	
	public void Stop()
	{
		ContinueRunning = false;
		ReadThread.Join();
	}
	
	public void Restart()
	{
		Stop ();
		Start ();
	}
	
	public void SendPosition(float px, float py, float pz) {
		Send ("set-position", px, py, pz);
	}
	
	public void Send(params object[] p)
    {
        string msg = "";
		foreach (object o in p) {
			if (msg.Length > 0) { msg += " "; }
			msg += o.ToString();
		}
        if (!msg.EndsWith("\n")) {
            msg += "\n";
        }
        byte[] encoded = Encoding.ASCII.GetBytes(msg);
		try {
        	Stream.Write(encoded, 0, encoded.Length);
        	Stream.Flush();
		} catch (System.IO.IOException) {
			Restart();
		}
		catch (Exception ex) {
			Debug.LogError (string.Format ("[MpClient] Unhandled exception in MpClient.Send of type {0}: {1}", ex.GetType ().Name, ex.Message));
		}
    }
	
	private void ReadSync()
	{
		string accum = "";
		byte[] buff = new byte[512];
		
		Debug.LogWarning(string.Format("[MpClient] Now reading from connection"));
		while (ContinueRunning)
		{
			try
			{
				IAsyncResult ar = Stream.BeginRead(buff, 0, buff.Length, null, null);
                while (!ar.IsCompleted && ContinueRunning)
                {
                    Thread.Sleep(200);
                }
                if (!ContinueRunning) { return; }
                int read = Stream.EndRead(ar);
                string decoded = Encoding.ASCII.GetString(buff, 0, read);
                accum += decoded;
                while (accum.Contains("\n"))
                {
                    string nextLine = accum.Substring(0, accum.IndexOf("\n"));
                    accum = accum.Substring(accum.IndexOf("\n") + 1);
                    ParseAndDoleOut(nextLine);
                }
			}
			catch (System.IO.IOException)
			{
				Debug.LogWarning(string.Format("[MpClient] Connection broken, no longer read"));
				ContinueRunning = false;
			}
			catch (Exception ex)
			{
				Debug.LogError (string.Format ("[MpClient] Unhandled exception in MpClient.ReadSync of type {0}: {1}", ex.GetType().Name, ex.Message));
				ContinueRunning = false;
			}
		}
	}
	
	private void ParseAndDoleOut(string feed)
	{
		List<string> feedParts = new List<string>(feed.Split(new char[] { ' ' }));
		if (feedParts.Count == 0) { return; }
		feedParts.Reverse ();
		Stack<string> parts = new Stack<string>(feedParts);
		
		string cid = parts.Pop ();
		if (cid == "identity") {
			Identity = parts.Pop();
			Debug.Log(string.Format("[MpClient] Now identity {0}", Identity));
		} else if (cid == "error") {
			Debug.LogWarning(string.Format("[MpClient] Server sent error: {0}", feed));
		} else if (cid == Identity) {
			/* ignore stuff about us, we already know yo */
		} else {
			string cmd = parts.Pop ();
			if (cmd == "position") {
				if (OnPositionUpdate != null) {
					float px = float.Parse (parts.Pop());
					float py = float.Parse (parts.Pop());
					float pz = float.Parse (parts.Pop());
					OnPositionUpdate(cid, px, py, pz);
				}
			} else if (cmd == "join") {
				if (OnSomeoneJoined != null) {
					OnSomeoneJoined(cid);
				}
			} else if (cmd == "drop") {
				if (OnSomeoneDropped != null) {
					OnSomeoneDropped(cid);
				}
			} else {
				Debug.LogError(string.Format("No handler for cmd {0} on cid {1}", cmd, cid));
			}
		}
	}
}

