using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class ResourceManager : MonoBehaviour
{
    //Automation of adding Prefabs to NetworkManager.
    
    [Tooltip("Assets/Resources/[Insert Folder Path]")]
    [SerializeField] private string[] folderPaths = null;
    private bool isDone = false;

    private void Awake()
    {
        if(!isDone)
        {
            int c = 0;

            if(folderPaths == null || folderPaths.Length <= 0)
            {
                Debug.LogWarning("No Folder Paths Given.");
            }
            else
            {
                for (int i = 0; i < folderPaths.Length; i++)
                {
                    List<GameObject> tmp = Resources.LoadAll(folderPaths[i], typeof(GameObject)).Cast<GameObject>().ToList();

                    if(tmp == null || tmp.Count <= 0)
                    {
                        Debug.LogWarning("Nothing found at: \"Assets/Resources/" + folderPaths[i] + "\". Skipping Folder!");
                    }
                    else
                    {
                        for(int j = 0; j < tmp.Count; j++)
                        {
                            gameObject.GetComponent<NetworkManager>().spawnPrefabs.Add(tmp[j]);
                            c++;
                        }
                    }
                }
            }
            isDone = true;
            Debug.Log("Registered " + c + " total prefabs.");
        }
    }
}
