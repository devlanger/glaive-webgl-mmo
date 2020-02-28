using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Use plugin namespace
using HybridWebSocket;
using GameCoreEngine;

public class WebSocketDemo : Singleton<WebSocketDemo>
{
    [SerializeField]
    private string ip;

    [SerializeField]
    private int port;
    private WebSocket ws;

    public void SendData(byte[] data)
    {
        ws.Send(data);
    }

    private void OnDestroy()
    {
        ws.Close();
    }

    private void Start () {

        PacketsReceivedManager.Initialize();

        // Create WebSocket instance
        ws = WebSocketFactory.CreateInstance(string.Format("ws://{0}:{1}", ip, port));

        // Add OnOpen event listener
        ws.OnOpen += () =>
        {
            Debug.Log("WS connected!");
            Debug.Log("WS state: " + ws.GetState().ToString());
        };

        // Add OnMessage event listener
        ws.OnMessage += (byte[] msg) =>
        {
            PacketsReceivedManager.ReceiveData(msg);
            //ws.Close();
        };

        // Add OnError event listener
        ws.OnError += (string errMsg) =>
        {
            Debug.Log("WS error: " + errMsg);
        };

        // Add OnClose event listener
        ws.OnClose += (WebSocketCloseCode code) =>
        {
            Debug.Log("WS closed with code: " + code.ToString());
        };

        // Connect to the server
        ws.Connect();

    }
}
