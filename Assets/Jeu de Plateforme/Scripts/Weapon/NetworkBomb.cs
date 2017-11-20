using UnityEngine;
//Classe qui va gérer l'explosion de la bombe même dans le réseau
public class NetworkBomb : Photon.MonoBehaviour
{
    void Awake()
    {
        correctPlayerPos = transform.position;
        correctPlayerRot = transform.rotation;
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) // On va réenvoyer une nouvelle position parce que la bombe nous fait reculer 
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(GetComponent<Rigidbody2D>().velocity);  
        }
        else
        {
            correctPlayerPos = (Vector3)stream.ReceiveNext();
            correctPlayerRot = (Quaternion)stream.ReceiveNext();
            GetComponent<Rigidbody2D>().velocity = (Vector2)stream.ReceiveNext();
        }
    }
    private Vector3 correctPlayerPos;
    private Quaternion correctPlayerRot;
    void Update()
    {
        if (!photonView.isMine)//Ajuster également le recul dans mon écran.
        {
            transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 5f);
            transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 5f);
        }
    }
}