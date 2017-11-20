using UnityEngine;

//Classe fournie par Unity 2D permettant de centrer la caméra du jeu sur l'objectif avec un défilement parallax
//force l'élément à observer à se déplacer pour que la caméra puisse le suivre

public class BackgroundParallax : MonoBehaviour
{
	public Transform[] backgrounds;				// Tableau qui va contenir le fond pour le déplacement parallax
	public float parallaxScale;					// Proportion de la caméra qui va pouvoir être amovible sur le jeu.
	public float parallaxReductionFactor;		// De combien de couches d'images il va falloir utiliser pour faire un défilement
	public float smoothing;						// À quel point la caméra aura un effet naturel sur le jeu


	private Transform cam;						
	private Vector3 previousCamPos;		


	void Awake ()
	{
		//Référence entre la caméra du jeu et la caméra de la classe.
		cam = Camera.main.transform;
	}


	void Start ()
	{
		// Mettre la position de la caméra équitable dans l'engine et dans la classe.
		previousCamPos = cam.position;
	}


	void Update ()
	{
        //Un parallax c'est l'opposé des mouvements de la caméra depuis le dernier déplacement, multiplié par le facteur de réduction.
		float parallax = (previousCamPos.x - cam.position.x) * parallaxScale;

		// Et ce, pour chaque couche
		for(int i = 0; i < backgrounds.Length; i++)
		{
            // Créer un objet (x)) qui aura sa position actuelle plus le parallax * facteur de réduction
			float backgroundTargetPosX = backgrounds[i].position.x + parallax * (i * parallaxReductionFactor + 1);
            //Créer une position qui va etre egale a celle actuelle mais avec la position de x
			Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);
            //Fonction lerp entre la position du fond et la position de la cible.
			backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
		}
		previousCamPos = cam.position;
	}
}
