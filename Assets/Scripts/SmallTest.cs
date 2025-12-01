using System.Xml.Serialization;
using UnityEngine;

public class SmallTest : MonoBehaviour
{
    public TreeMiniGame treeMiniGame;

    private void Update()
    {
        //this.gameObject.GetComponent<Rigidbody>().useGravity = true;
        //this.gameObject.GetComponent <Rigidbody>().isKinematic = false;
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<TreeMiniGame>())
        {
            treeMiniGame.IsBeingWatered();
            Debug.Log("Collision detected");
        }
    }
    public void ResetGravity()
    {
        this.gameObject.GetComponent<Rigidbody>().useGravity = true;
        this.gameObject.GetComponent <Rigidbody>().isKinematic = false;
    }
}
