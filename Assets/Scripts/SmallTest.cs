using System.Xml.Serialization;
using UnityEngine;

public class SmallTest : MonoBehaviour
{
    public TreeMiniGame treeMiniGame;
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<TreeMiniGame>())
        {
            treeMiniGame.IsBeingWatered();
            //Debug.Log("ik werk");
        }
    }
}
