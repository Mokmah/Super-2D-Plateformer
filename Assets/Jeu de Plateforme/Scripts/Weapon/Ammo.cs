using UnityEngine;
using System.Collections;
//Classe qui va gérer l;a gestion des munitions en jeu. // Upload par Unity
public class Ammo : Photon.MonoBehaviour 
{
    public float ammo_speed;
	public GameObject explosion;		

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(transform.right.x* ammo_speed, 0); //Tirer en direction du mouvement 
    }
    bool flag;

    void OnTriggerEnter2D (Collider2D col) 
	{
        if (flag || !photonView.isMine || col.gameObject.tag == "Player" && col.transform.GetComponent<PhotonView>().isMine) return;
        flag = true;
        if (col.tag == "WeaponBox")
        {
            col.gameObject.GetComponent<PhotonView>().RPC("Explode", PhotonTargets.All, col.transform.position);
        }
        else if (col.gameObject.tag != "Player")
        {
            if (explosion != null) 
                PhotonNetwork.Instantiate(explosion.name, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)), 0);
        }
        else if (col.gameObject.tag == "Player" && !col.transform.GetComponent<PhotonView>().isMine)
        {
            int i = Random.Range(0, col.GetComponent<PlayerDeath>().ouchClips.Length);
            col.gameObject.GetComponent<PhotonView>().RPC("Death", PhotonTargets.All, i);

            if(explosion!=null)
                PhotonNetwork.Instantiate(explosion.name, transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)), 0);
        }

        StartCoroutine(bullet_self_destruct_());
    }

    IEnumerator bullet_self_destruct_() // S'auto détruire avec une bombe
    {       
        GetComponent<BoxCollider2D>().enabled = false;

        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        if (transform.childCount > 1) //Pour les fusées
        {
            transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(2).gameObject.SetActive(false);
        }
        while (GetComponent<AudioSource>().isPlaying == true)
            yield return new WaitForSeconds(0.1f);

        PhotonNetwork.Destroy(gameObject);
    }
}
