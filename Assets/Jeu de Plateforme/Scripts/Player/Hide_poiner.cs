using UnityEngine;
//Classe qui sert a mettre un point au dessus de la tete du joueur quand il rentre dans la partie pour savoir qui il est.
public class Hide_poiner : Photon.MonoBehaviour
{
	void Start ()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        Invoke("hide_pointer", 2f);
	}
	void hide_pointer()
    {
        gameObject.SetActive(false);
	}
}
