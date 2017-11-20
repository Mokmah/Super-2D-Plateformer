using UnityEngine;
using System.Collections;
//Classe qui va gérer l'apparition des munitions entre le serveur et le client.
public class PickupSpawner : Photon.MonoBehaviour
{
	public float pickupDeliveryTime = 1f;    // Délai avant apparition
    float[] dropRange = { 2.8f,-1.4f,-5.6f}; // Location des 3 plateformes qui votn pouvoir avoir des munitions dessus
    public GameObject[] WeaponBox;           // Quellesorte de munition
    [Range(0,10)]public int quantity;                     // Combien de boites sur la map
    int different_bonus = 0;                 // Spawn de différentes boites sur la map
    void OnJoinedRoom() // Faire un premier ajout quand le jeu part
	{
        if (PhotonNetwork.isMasterClient)
            InvokeRepeating("test",4, pickupDeliveryTime);
    }
    void OnMasterClientSwitched(PhotonPlayer newMasterClient) // Avoir les mêmes boites dans tous les clients
    {
        InvokeRepeating("test", 4, pickupDeliveryTime);
    }
    void test()
    {
        if(FindObjectsOfType<WeaponBoxPickup>().Length < quantity)
            StartCoroutine(DeliverPickup());
    }
    public IEnumerator DeliverPickup()
	{
        float dropPos_y = dropRange[Random.Range(0, dropRange.Length)];
        Vector3 dropPos = new Vector3(Random.Range(-19, 20), dropPos_y);
        Collider2D[] enemies = Physics2D.OverlapCircleAll(dropPos, 5); // Empecher que plusieurs boites soient collées
        foreach (Collider2D en in enemies)
        {
            if (en.transform.tag == "WeaponBox")
            {
                yield return new WaitForSeconds(0.001f);
                StartCoroutine(DeliverPickup());
                yield break;
            }

        }

        int pickupIndex = Random.Range(0, WeaponBox.Length);
        while (different_bonus == pickupIndex && WeaponBox.Length > 1)
        {
            pickupIndex = Random.Range(0, WeaponBox.Length);
            yield return new WaitForSeconds(0.001f);
        }
        different_bonus = pickupIndex;
        PhotonNetwork.InstantiateSceneObject("Weapon Box/"+WeaponBox[pickupIndex].name, dropPos, Quaternion.identity, 0,null);
        if (FindObjectsOfType<WeaponBoxPickup>().Length < quantity)
            StartCoroutine(DeliverPickup());
    }
}
