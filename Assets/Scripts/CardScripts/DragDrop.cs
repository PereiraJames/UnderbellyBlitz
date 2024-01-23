using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DragDrop : NetworkBehaviour
{
    public GameManager GameManager;
    public GameObject Canvas;
    public PlayerManager PlayerManager;
    public RectTransform PlayerSlot;

    public bool isDragging = false;
    private bool isOverDropZone = false;
    private bool isDraggable = true;
    private GameObject dropZone;
    private GameObject startParent;
    private Vector2 startPosition;
    private int CardsInPlayLimit = 8;
    

    private void Start()
    {
        PlayerSlot = GameObject.Find("PlayerSlot").GetComponent<RectTransform>();
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Canvas = GameObject.Find("Main Canvas");
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        PlayerManager = networkIdentity.GetComponent<PlayerManager>();
        if (!isOwned)
        {
            isDraggable = false;
        }
    }

    void Update()
    {
        if(isDragging)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            transform.SetParent(Canvas.transform, true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if (collision.gameObject == PlayerManager.PlayerSockets[PlayerManager.cardsPlayed])
        // {
        //     isOverDropZone = true;
        //     dropZone = collision.gameObject;
        // }
        isOverDropZone = true;
        dropZone = collision.gameObject;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOverDropZone = false;
        dropZone = null;
    }

    public void StartDrag()
    {   
        if(transform.parent != PlayerSlot)
        {
            if(!isDraggable || PlayerManager.AttackDisplayOpened || PlayerManager.DestroyDisplayOpen)
            {
                return;
            }
            
            startPosition = transform.position;
            startParent = transform.parent.gameObject;
            isDragging = true;
        }
    }

    public void EndDrag()
    {
        if(!isDraggable)
        {
            return;
        }

        isDragging = false;
        int cardCost = gameObject.GetComponent<CardDetails>().DoubloonCost;
        int currentPlayerDoubloons = GameManager.currentPlayerDoubloons;
        
        int PlayerSlotCardTotal = 0;

        foreach (Transform child in PlayerSlot.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Cards")
            {
                PlayerSlotCardTotal++;
            }
        }

        if (isOverDropZone && PlayerManager.IsMyTurn && cardCost <= currentPlayerDoubloons && PlayerSlotCardTotal < CardsInPlayLimit)
        {
            PlayerManager.CmdUpdateDoubloons(cardCost,false);
            transform.SetParent(dropZone.transform, false);
            isDraggable = false;
            // NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            // PlayerManager = networkIdentity.GetComponent<PlayerManager>();
            
            PlayerManager.PlayCard(gameObject);
        }   
        else
        {
            transform.position = startPosition;
            transform.SetParent(startParent.transform, false);
        }
    }
}
