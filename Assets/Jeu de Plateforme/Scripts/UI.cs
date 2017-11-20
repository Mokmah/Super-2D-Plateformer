using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
//Quitte la game si on clique sur echap, instanciation de choisir son joueur, chargement de partie.
public class UI : MonoBehaviour
{
    public GameObject loading_panel; //Ecran de chargement
    public Text info_player_text; // Informations sur le joueur
    public Image body_color; // Image de la patate de couleur
    string key_hat; // Choix du chapeau de la patate
    private string key_body_color; // Choix de la couleuir de la patate
    void Update()
    {
        info_player_text.text = PhotonNetwork.connectionStateDetailed.ToString();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    void OnJoinedLobby()
    {
        loading_panel.SetActive(false);
    }
    void OnJoinedRoom()
    {
        loading_panel.SetActive(false);
    }
    void OnLeftRoom()
    {
        loading_panel.SetActive(true);
    }
    public void SetNetworKey(Transform key)
    {
        PhotonNetwork.player.SetCustomProperties(new Hashtable { { key.parent.name, key.name } });
    }
    public void ChangeColor(Image col)
    {
        body_color.color = col.color; // Mettre l'image de patate de la bonne couleur.
    }
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}

