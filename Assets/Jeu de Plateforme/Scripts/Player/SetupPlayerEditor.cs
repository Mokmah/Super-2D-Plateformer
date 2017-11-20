using System.Collections.Generic;
using UnityEngine;
//Classe qui va gérer le choix physique de la patate avant le début de la partie.
public class SetupPlayerEditor : Photon.MonoBehaviour
{
    public SpriteRenderer body;

    public GameObject[] hats;
    public Color[] body_color;
    Dictionary<string, GameObject> setup_hat = new Dictionary<string, GameObject>();
    public string[] keys_color;
    Dictionary<string, Color> setup_color = new Dictionary<string, Color>();

    void Start ()//Savoir quel a été sélectionné et l'appliquer sur la patate.
    {
        foreach(GameObject hat in hats)
            setup_hat.Add(hat.name, hat);

        for (int i=0; i< keys_color.Length; i++ )
            setup_color.Add(keys_color[i], body_color[i]);

        setup_hat[photonView.owner.CustomProperties["Key_hats"].ToString()].SetActive(true);
        body.color = setup_color[photonView.owner.CustomProperties["Key_colors"].ToString()];
    }
}
