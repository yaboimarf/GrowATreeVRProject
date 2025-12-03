using System.Collections.Generic;
using UnityEngine;

public class TrashCanList : MonoBehaviour
{
    public List<GameObject> trashItems = new List<GameObject>();
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Trash"))
        {
            trashItems.Add(collision.gameObject);
        }
    }

    public void DeleteTrash()
    {
        foreach (GameObject item in trashItems)
        {
            Destroy(item);
        }
        trashItems.Clear();
    }
}
