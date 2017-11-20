using UnityEngine;
//Si le joueur est mort et que l'autre est en vie, il faut recommencer la partie. 
public class Remover : Photon.MonoBehaviour
{
	public GameObject splash; //Animation quand le joueur tombe dans rivière

    void OnTriggerEnter2D(Collider2D col)
	{
        if (!col.GetComponent<PhotonView>().isMine || col.name == "inside") return;
        col.name = "inside";
        PhotonNetwork.Instantiate(splash.name, col.transform.position, transform.rotation, 0);
        if (col.gameObject.tag == "Player")//Va falloir repartir une game
        {
            Invoke("Reloading", 2);
        }
        PhotonNetwork.Destroy(col.gameObject);
    }
    void Reloading()//On repart une game
    {
        if (GameObject.FindGameObjectsWithTag("Player").Length <= 1)
            photonView.RPC("Reload", PhotonTargets.All);
    }
    [PunRPC]
    void Reload()//On dit au serveur de tout détruire et de recommencer
    {
        ConnectRoom CR = FindObjectOfType<ConnectRoom>();
        if (CR.player != null)
            PhotonNetwork.Destroy(CR.player);

        CR.OnJoinedRoom();
    }
}
