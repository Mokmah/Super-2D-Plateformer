using UnityEngine;
using System.Collections;

public class BackgroundPropSpawner : MonoBehaviour
{
	public Rigidbody2D backgroundProp;		//Objet a etre instancié, prop référant celui-ci
	public float leftSpawnPosX;				// Position des x si il est instancié a gauche de lecran
	public float rightSpawnPosX;			// Position des x si il est instancié à droite de lecran
	public float minSpawnPosY;				// plus petite position possible sur l'axe des y
	public float maxSpawnPosY;				// plus grande position possible sur l'axe des y
	public float minTimeBetweenSpawns;		// plus petit delai entre les spawn
	public float maxTimeBetweenSpawns;		// plus grand delai possible entre les spawns.
	public float minSpeed;					// plus petite vitesse possible de l'objet amovible.
	public float maxSpeed;                  // plus grande vitesse possible de l'objet amovible.

    void Start()
    {
        StartCoroutine("Spawn");
    }

    IEnumerator Spawn ()
	{
        float waitTime = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
		yield return new WaitForSeconds(waitTime);
        bool facingLeft = Random.Range(0,2) == 0;
		float posX = facingLeft ? rightSpawnPosX : leftSpawnPosX;
		float posY = Random.Range(minSpawnPosY, maxSpawnPosY);
		Vector3 spawnPos = new Vector3(posX, posY, transform.position.z);

		Rigidbody2D propInstance = Instantiate(backgroundProp, spawnPos, Quaternion.identity) as Rigidbody2D;
		if(!facingLeft)
		{
			Vector3 scale = propInstance.transform.localScale;
			scale.x *= -1;
			propInstance.transform.localScale = scale;
		}
		float speed = Random.Range(minSpeed, maxSpeed);
		speed *= facingLeft ? -1f : 1f;
		propInstance.velocity = new Vector2(speed, 0);   
		StartCoroutine(Spawn());
		while(propInstance != null)
		{
			if(facingLeft)
			{
				if(propInstance.transform.position.x < leftSpawnPosX - 0.5f)
					Destroy(propInstance.gameObject);
			}
			else
			{
				if(propInstance.transform.position.x > rightSpawnPosX + 0.5f)
					Destroy(propInstance.gameObject);
			}
			yield return null;
		}
	}
}
