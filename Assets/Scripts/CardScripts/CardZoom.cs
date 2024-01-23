using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardZoom : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject Card;
    public Sprite zoomCardSprite;
    private GameObject zoomCard;
    public bool CardDead = false;


    public void Awake()
    {
        Canvas = GameObject.Find("Main Canvas");
    }

    public void OnHoverEnter()
    {
        if(CardDead)
        {
            return;
        }
        if(gameObject.GetComponent<Image>().sprite != gameObject.GetComponent<CardFlipper>().CardBack)
        {
            zoomCardSprite = gameObject.GetComponent<Image>().sprite;
            Card.GetComponent<Image>().sprite = zoomCardSprite;
            int CardHealth = gameObject.GetComponent<CardDetails>().GetCardHealth();
            int CardAttack = gameObject.GetComponent<CardDetails>().GetCardAttack();
            int DoubloonCost = gameObject.GetComponent<CardDetails>().DoubloonCost;
            foreach (Text child in Card.GetComponentsInChildren<Text>())
            {
                if(child.gameObject.name == "AttackHealthText")
                {
                    child.GetComponent<Text>().text = CardAttack + " / " + CardHealth;
                }
                else if(child.gameObject.name == "DoubloonText")
                {
                    child.GetComponent<Text>().text = DoubloonCost.ToString();
                }
            }
            zoomCard = Instantiate(Card, new Vector2(744, 0), Quaternion.identity);
            zoomCard.transform.SetParent(Canvas.transform, false);
            zoomCard.layer = LayerMask.NameToLayer("Zoom");
        }
    }

    public void OnHoverExit()
    {
        CardDead = false;
        Destroy(zoomCard);
    }
}
