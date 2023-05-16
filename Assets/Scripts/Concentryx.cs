using System.Collections;
using UnityEngine;


public class Concentryx : MonoBehaviour
{
    public int numSegments = 32; // The number of segments in the ring.
    public int numRings = 4; // The number of segments in the ring.
    public float innerRadius = 1.0f; // The inner radius of the ring.
    public float outerRadius = 2.0f; // The outer radius of the ring.
    public float spriteWidth = 0.1f; // The width of each sprite in the ring.
    public Sprite sprite; // The sprite to use for each segment.
    public Sprite alt_sprite; // The sprite to use for each segment.
    public float rotationSpeed = 10f;

    public GameObject[] pillPrefabs;

    private void Start()
    {


        for (int o = 0; o < numRings; o++)
        {
            // Create a new GameObject to hold the ring.
            GameObject ringObject = new GameObject("Ring " + o);

            Rigidbody2D ringRB = ringObject.AddComponent<Rigidbody2D>();
            ringRB.bodyType = RigidbodyType2D.Kinematic;

            int segmentModifier = (o * 15);


            // Create a new SpriteRenderer and Collider2D component for each segment of the ring.
            for (int i = 0; i < numSegments + segmentModifier; i++)
            {

                float angleStep = 360.0f / (numSegments + segmentModifier);
                // Calculate the angle for this segment.
                float angle = i * angleStep;

                // Calculate the position for the center of this segment.
                Vector3 position = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0.0f) * ((innerRadius + outerRadius + o) / 2.0f);

                // Create a new GameObject to hold the sprite and collider for this segment.
                GameObject segmentObject = new GameObject("Segment " + o + i);

                // Set the parent of this GameObject to the ring object.
                segmentObject.transform.parent = ringObject.transform;

                // Set the position of this GameObject.
                segmentObject.transform.position = position;

                // Create a new SpriteRenderer component for this segment.
                SpriteRenderer spriteRenderer = segmentObject.AddComponent<SpriteRenderer>();

                // Set the sprite for this SpriteRenderer.
                if (o % 2 == 0)
                {
                    spriteRenderer.sprite = sprite;
                }
                else
                {
                    spriteRenderer.sprite = alt_sprite;
                }


                // Set the width of this sprite based on the spriteWidth property.
                spriteRenderer.size = new Vector2(spriteWidth, (outerRadius - innerRadius) / numSegments);

                // Set the rotation of this GameObject to match the angle of this segment.
                segmentObject.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle - (angleStep / numSegments)));

                // add segment handler script
                segmentObject.AddComponent<Segment>();

                segmentObject.GetComponent<Segment>().pillPrefabs = pillPrefabs;

                // set initial health

                segmentObject.GetComponent<Segment>().health += o;


            }

            if (o % 2 == 0)
            {
                ringRB.angularVelocity = rotationSpeed + segmentModifier;
            }
            else
            {
                ringRB.angularVelocity = -(rotationSpeed + segmentModifier);
            }
        }

    }

}
