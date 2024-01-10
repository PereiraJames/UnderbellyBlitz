using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Sharpshooter_CS : CardAbilities
{
    public override void OnEntry()
    {
        GameObject EnemySlot = PlayerManager.EnemySlot;
        int amountOfEnemies = EnemySlot.transform.childCount;
        int targetRandom = Random.Range(0, amountOfEnemies);
        int count = 0;
        if(amountOfEnemies <= 3)
        {
            foreach (Transform child in EnemySlot.GetComponentsInChildren<Transform>())
            {
                if (child.gameObject.tag == "Cards")
                {
                    PlayerManager.CmdDealDamage(child.gameObject, 2);
                }
            }
        }
        else
        {
            List<GameObject> Enemies = new List<GameObject>();

            foreach (Transform child in EnemySlot.GetComponentsInChildren<Transform>())
            {
                if (child.gameObject.tag == "Cards")
                {
                    Enemies.Add(child.gameObject);
                }
            }    

            Enemies = Enemies.OrderBy(x => Random.value).ToList();
            List<GameObject> selectedGameObjects = Enemies.Take(3).ToList();

            foreach (GameObject child in selectedGameObjects)
            {
                PlayerManager.CmdDealDamage(child.gameObject, 2);
            }
        }
    }

    public override void OnEndTurn()
    {

    }

    public override void OnHit()
    {

    }
    
    public override void OnLastResort()
    {

    }

    public override void OnSilenced()
    {
        
    }
}
