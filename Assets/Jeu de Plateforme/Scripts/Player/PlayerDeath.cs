using UnityEngine;
//Classe d'un joueur qui meurt et comment il va réagir qauand il va mourir (Désactiver ses fusils, se pencher et tomber dans la rivière. Plus aucun collider ne pourra l'empêcher de tomber 
public class PlayerDeath : Photon.MonoBehaviour
{	
	public AudioClip[] ouchClips;				
	private Animator anim;						

	void Awake ()
	{
		anim = GetComponent<Animator>();
	}
    [PunRPC]
    void Death(int number_clip)
    {
        if(photonView.isMine && GetComponent<PlayerControl>().enabled)//Désactiver a jouabilité
        {
            GetComponent<PlayerControl>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
            anim.SetTrigger("Die");
        }
        Collider2D[] cols = GetComponents<Collider2D>();
        if (!cols[0].isTrigger)
            AudioSource.PlayClipAtPoint(ouchClips[number_clip], transform.position);
        foreach (Collider2D c in cols)
        {
            c.isTrigger = true;
        }
        //Mettre tous les collider en avant pour éviter que le joueur se plante dessus
        SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer s in spr)
        {
            s.sortingLayerName = "UI";//Revenir a la classe UI pour recommencer une partie.
        }
    }
}
