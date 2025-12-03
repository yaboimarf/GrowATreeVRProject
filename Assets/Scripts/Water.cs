using UnityEngine;

public class Water : MonoBehaviour
{
    public TreeMiniGame treeMiniGame;
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
        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }
}
