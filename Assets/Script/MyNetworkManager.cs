using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MyNetworkManager : NetworkManager
{


public override void OnClientConnect()
{
   base.OnClientConnect();
   Debug.Log("Me conecte al servidor");
}


public override void OnServerAddPlayer(NetworkConnectionToClient conn)
{
  base.OnServerAddPlayer(conn);
   Debug.Log("Un jugador se ha anadido"+numPlayers);
   PlayerController mfs = conn.identity.GetComponent<PlayerController>();
   mfs.SetHealthPlayer(10);

   float R = Random.Range(0.0f, 1.0f);
   float G = Random.Range(0.0f, 1.0f);
   float B = Random.Range(0.0f, 1.0f);
   mfs.SetColorPlayer(new Color(R, G, B));
}



}


