using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFlipper : MonoBehaviour
{
    public Sprite CardFront;
    public Sprite CardBack;

    public void SetSprite(string type)
    {
        // if (type == "cyan")
        // {
        //     CardFront = CyanCardFront;
        //     CardBack = CyanCardBack;
        // }
        // else if (type == "magenta")
        // {
        //     gameObject.GetComponent<Image>().sprite = MagCardFront;
        //     CardFront = MagCardFront;
        //     CardBack = MagCardBack;
        // }
    }
    
    public void Flip()
    {   
        if (gameObject.GetComponent<Image>().sprite == CardFront)
        {
            gameObject.GetComponent<Image>().sprite = CardBack;
            gameObject.GetComponentInChildren<Text>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = CardFront;
            gameObject.GetComponentInChildren<Text>().enabled = true;
        }
    }
}
