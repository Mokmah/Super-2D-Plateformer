using UnityEngine;
//Classe de l'évènement quand le joueur va prendre une boite de munitions, pour qu'il ait le bon fusil avec la bonne quantité de munitions. 
//S'active uniquement quand le joueur marche sur la boite ou y est très près.
public class WeaponBoxPickup : Photon.MonoBehaviour
{
	public AudioClip pickupClip;		//Quand le joueur prend la boite, émet un son
    void OnTriggerEnter2D (Collider2D other)
	{
        if (other.tag == "Player" && other.GetComponent<PhotonView>().isMine && other.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite == null //Savoir quelle boite il a prit 
            && GetComponent<SpriteRenderer>().enabled
            && other.transform.GetChild(0).GetComponents(GetComponent<Weapon>().GetType()).Length == 0)
        {  
            GetComponent<SpriteRenderer>().enabled = false;
            other.transform.GetChild(0).gameObject.AddComponent(GetComponent<Weapon>().GetType());         
            GetComponent<Weapons>().Initialization(other.transform.GetChild(0).GetComponent<Weapons>(),GetComponent<Weapons>());
            photonView.RPC("destroy_bonus", PhotonTargets.All);
        }
    }
    void OnCollisionEnter2D(Collision2D other) // Savoir s'il a touché une boite
    {
        if (other.transform.tag == "ground" || other.transform.tag=="WeaponBox" && !GetComponent<BoxCollider2D>().isTrigger)
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
    [PunRPC]
    void destroy_bonus()// Detruire la boite apres lavoir prise
    {
        GetComponent<SpriteRenderer>().enabled = false;
        AudioSource.PlayClipAtPoint(pickupClip, transform.position);
        if (PhotonNetwork.isMasterClient)
            PhotonNetwork.Destroy(transform.root.gameObject);
    }
}
