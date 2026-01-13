using System.Collections.Generic;
using UnityEngine;

public class TrashCanList : MonoBehaviour
{
    public TreeMiniGame TreeMiniGame;
    public List<GameObject> trashItems = new List<GameObject>();
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Trash"))
        {
            trashItems.Add(collision.gameObject);
            TreeMiniGame.TrashPoints();
        }
    }
    private void Update()
    {
        foreach (GameObject item in trashItems)
        {
            Destroy(item);
        }
        trashItems.Clear();
    }
}
