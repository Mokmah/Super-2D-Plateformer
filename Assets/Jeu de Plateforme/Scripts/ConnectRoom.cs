using UnityEngine;
/*
 * Architecture réseau avec Photon Networking 
 * 
 * But : Le premier joueur se connecte au serveur de Photon et les suivants se connectent à lui,
 *  
 */
public class ConnectRoom : Photon.MonoBehaviour
{
    [HideInInspector]
    public GameObject player;
    public AudioClip GameSong;
    float[] dropRange = { 2.8f, -1.4f, -5.6f }; //Positions ou le serveur va pouvoir drop le joueur sur la map
    void Start()
    {
        PhotonNetwork.JoinOrCreateRoom("JeuComm", new RoomOptions() { MaxPlayers = 4 }, null);
    }
    void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause: " + cause);
    }
    public void OnJoinedRoom()
    {
        float dropPos_y = dropRange[Random.Range(0, dropRange.Length)];
        Vector3 dropPos = new Vector3(Random.Range(-19, 20), dropPos_y);
        player = PhotonNetwork.Instantiate(Resources.Load("hero").name, dropPos, Quaternion.identity, 0);
        player.GetComponent<PlayerControl>().enabled = true;
        player.GetComponentInChildren<Hide_poiner>().enabled = true;
    }
}
