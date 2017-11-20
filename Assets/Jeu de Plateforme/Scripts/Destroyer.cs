using UnityEngine;
using System.Collections;
/* Network Description

Seul un possésseur du client va pouvoir détruire un objet (ex, boite de gun)

Network Description */
public class Destroyer : Photon.MonoBehaviour
{
	public bool destroyOnAwake;			//Savoir si un objet doit se detruire (quand on recommence une partie)
	public float awakeDestroyDelay;		// Le delai pour le detruire sil doit letre
	public bool findChild = false;		// trouver un objet parent et le détruire
	public string namedChild;           // Créer un nouvel gameobjet unity pour qu'il soit prit en compte
    void Awake() // Est-ce que l'objet doit être détruit?
    {
        if (destroyOnAwake)
        {
            if (findChild)
            {
                PhotonNetwork.Destroy(transform.Find(namedChild).gameObject);
            }
            else
            {
                StartCoroutine(Destroy_(awakeDestroyDelay));
            }

        }
    }
    IEnumerator Destroy_(float awakeDestroyDelay)
    {
        yield return new WaitForSeconds(awakeDestroyDelay);
        if (photonView.isMine)
            PhotonNetwork.Destroy(gameObject);
    }
    void DestroyChildGameObject() //Détruire l'objet sur le serveur 
    {
        // Destroy this child gameobject, this can be called from an Animation Event.
        if (transform.Find(namedChild).gameObject != null)
            PhotonNetwork.Destroy(transform.Find(namedChild).gameObject);
    }
    void DisableChildGameObject()//Désactiver l'utilisation possible de cet objet sur le serveur
    {
        if (transform.Find(namedChild).gameObject.activeSelf == true)
            transform.Find(namedChild).gameObject.SetActive(false);
    }
    void DestroyGameObject() // Détruire l'objet même sur notre écran.
    {
       if(photonView.isMine)
        PhotonNetwork.Destroy(gameObject);
    }
}
