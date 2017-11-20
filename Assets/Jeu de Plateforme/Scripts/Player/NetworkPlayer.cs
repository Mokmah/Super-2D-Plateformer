using UnityEngine;
using System.Collections.Generic;
//Transmission des mouvements et des positions du joueur au reseau, ainsi que ses caractéristiques(fusil, ammo, nom, sorte)
[RequireComponent(typeof (PhotonView))]
public class NetworkPlayer : Photon.MonoBehaviour
{
    string name_;
    public static Dictionary<string, Sprite> box_weapon = new Dictionary<string, Sprite>(); //S'il a un fusil
    private Vector3 latestCorrectPos;
    private Vector3 onUpdatePos;
    private float fraction; // Quel joueur est visé

    void Awake() // Avoir les informations principales sur son emplacement et ses munitions
    {
        if (box_weapon.Count != 0) return;
        PickupSpawner pickupSpawner = GameObject.Find("pickupManager").GetComponent<PickupSpawner>();
        foreach (GameObject boncr in pickupSpawner.WeaponBox)
            box_weapon.Add(boncr.GetComponent<Weapons>().picture_weapon.name, boncr.GetComponent<Weapons>().picture_weapon);
        latestCorrectPos = transform.position;
        onUpdatePos = transform.position;
    }
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) // Envoyer les informations sérialisés au serveur pour les traiter
    {
        if (stream.isWriting)//si les positions n'ont pas bouger
        {
            if (transform.GetChild(0).GetComponent<SpriteRenderer>().sprite != null)
            {
                name_ = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.name;//Nom
            }
            else
                name_ = "null";
            stream.Serialize(ref name_);
            int sprite = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite != null ? transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder : 0;
            stream.SendNext(sprite);
            stream.SendNext(transform.localScale);//location actuelle
            stream.SendNext(GetComponent<Rigidbody2D>().velocity); //Pour un saut
            Vector3 pos = transform.localPosition;
            Quaternion rot = transform.localRotation;
            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
        }
        else//envoi des nouvelles coordonnees
        {
            stream.Serialize(ref name_);//envoi du nom
            if (box_weapon.ContainsKey(name_))
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = box_weapon[name_];
            else
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
            transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = (int)stream.ReceiveNext();

            transform.localScale = (Vector3)stream.ReceiveNext();
            GetComponent<Rigidbody2D>().velocity = (Vector2)stream.ReceiveNext();//Pour un saut

            // Receive latest state information
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            latestCorrectPos = pos;
            onUpdatePos = transform.localPosition; // On changer la latestCorrectPos avec on UpdatePos
            fraction = 0;                          //Updater le mouvement du bon joueur
            transform.localRotation = rot;
        }
    }

    void Update()//Le serveur va request 20 messages par seconde
    {
        if (photonView.isMine)
        {
            return; // Si c'est notre joueur on envoie pas d'infos.
        }
        fraction = fraction + Time.deltaTime * 9;
        transform.localPosition = Vector3.Lerp(onUpdatePos, latestCorrectPos, fraction); // set our pos between A and B
    }
}