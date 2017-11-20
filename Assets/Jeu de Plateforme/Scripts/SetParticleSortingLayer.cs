using UnityEngine;
public class SetParticleSortingLayer : MonoBehaviour
{
	public string sortingLayerName;		// Nom de la couche d'image qui va avoir un effet dessus (fog)
	void Start ()
	{
		GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = sortingLayerName;
	}
}
