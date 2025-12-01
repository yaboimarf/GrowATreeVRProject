using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject water;
    public SmallTest smallTest;

    private void Update()
    {
        smallTest = gameObject.GetComponent<SmallTest>();
        if(smallTest.waterIsActive == false)
        {
            Instantiate(water);
            smallTest.waterIsActive = true;
        }
    }
    public void SpawnWater()
    {
        Instantiate(water);
    }
    public void SmallTestLocation()
    {
        smallTest = gameObject.GetComponent<SmallTest>();
    }
}
