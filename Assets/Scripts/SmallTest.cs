using System.Xml.Serialization;
using UnityEngine;

public class SmallTest : MonoBehaviour
{
    public SpawnPoint spawnPoint;
    public TreeMiniGame treeMiniGame;
    public bool waterIsActive;

    private void Start()
    {
        treeMiniGame = gameObject.GetComponent<TreeMiniGame>();
        spawnPoint = gameObject.GetComponent<SpawnPoint>();
        waterIsActive = true;
        treeMiniGame.SmallTestLocation();
        spawnPoint.SmallTestLocation();
    }
    private void Update()
    {
        if(waterIsActive == false)
        {
            spawnPoint.SpawnWater();
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<TreeMiniGame>())
        {
            treeMiniGame.IsBeingWatered();
            Debug.Log("Collision detected");
        }
    }

    public void WaterActive()
    {
        waterIsActive = false;
    }
}
