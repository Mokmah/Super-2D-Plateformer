using UnityEngine;
using UnityEngine.UI;

//Classe pour faire l'animation des points dans l'écran d'accueil pour que ça ait l'air de charger
public class LoadingAnimation : MonoBehaviour
{
    float time_ =0.5f;
    int amount_of_points;
    string line_without_points; //La ligne qui va recevoir les points
    void Start()
    {
        line_without_points = GetComponent<Text>().text;
    }
	void Update ()
    {
        time_ += 0.01f; 

        if (time_ >= 1f)
        {
            GetComponent<Text>().text += ".";
            if (amount_of_points == 3)
            {
                GetComponent<Text>().text = line_without_points;
                amount_of_points = -1;
            }
            time_ = 0;
            amount_of_points++;
        }
    }
}
