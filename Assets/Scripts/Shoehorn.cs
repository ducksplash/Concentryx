using UnityEngine;

public class Shoehorn : MonoBehaviour
{


    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("shoehorn " + other.gameObject.name);

        if (other.gameObject.name.Contains("Segment"))
        {
            Destroy(other.gameObject);
        }

    }

}