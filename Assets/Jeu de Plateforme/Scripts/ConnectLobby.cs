using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable; 
/*
 * Architecture r�seau avec Photon Networking 
 * 
 * But : Le premier joueur se connecte au serveur de Photon et les suivants se connectent � lui,
 *  
 */
public class ConnectLobby : Photon.MonoBehaviour
{
    public string Version; // Diff�rencie tous les utilisateurs 
    bool InConnectUpdate;

    public virtual void Start()
    {
        PhotonNetwork.autoJoinLobby = true;
    }
    void Update()
    {
        if (!InConnectUpdate && !PhotonNetwork.connected)
        {
            InConnectUpdate = true;
            PhotonNetwork.ConnectUsingSettings(Version);
        }
    }
    void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause: " + cause);
    }
    void OnJoinedLobby()
    {
        PhotonNetwork.player.SetCustomProperties(new Hashtable { { "Key_hats", "DefaultHat" } });
        PhotonNetwork.player.SetCustomProperties(new Hashtable { { "Key_colors", "Default" } });
    }
}
