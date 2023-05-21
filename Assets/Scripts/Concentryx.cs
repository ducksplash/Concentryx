using System.Collections;
using UnityEngine;


public class Concentryx : MonoBehaviour
{
    public int numSegments = 32; // The number of segments in the ring.
    public int numLayers = 4; // The number of segments in the ring.
    public float innerRadius = 1.0f; // The inner radius of the ring.
    public float outerRadius = 2.0f; // The outer radius of the ring.
    public float spriteWidth = 0.1f; // The width of each sprite in the ring.


    public int dropRandomLower = 0;
    public int dropRandomUpper = 50;

    public int dropRandomCutoff = 45;

    public Sprite mysterySprite;
    public Sprite[] spriteSelection;

    public float rotationSpeed = 10f;
    public string liveLevelType = "none";
    public Material segOneMaterial;

    public GameObject[] pillPrefabs;

    private void Start()
    {

        ConcentricRings();
        //ConcentricRingsLumpy();
        //LatticeHell();
        //LittleSquares();

    }

    public void ConcentricRings()
    {
        float innerRadius = 1.3f; // The inner radius of the ring.
        float outerRadius = 1.4f; // The outer radius of the ring.

        int numRings = 6;

        int numSegments = 16; // The number of segments in the ring.
        float rotationSpeed = 10f;


        for (int o = 0; o < numRings; o++)
        {
            // Create a new GameObject to hold the ring.
            GameObject ringObject = new GameObject("Ring " + o);

            Rigidbody2D ringRB = ringObject.AddComponent<Rigidbody2D>();
            ringRB.bodyType = RigidbodyType2D.Kinematic;

            int segmentModifier = (o * 5);

            Sprite SelectedSprite = spriteSelection[Random.Range(0, spriteSelection.Length)];

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

                // add segment handler script

                Segment segmentScript = segmentObject.AddComponent<Segment>();
                // Set the parent of this GameObject to the ring object.
                segmentObject.transform.parent = ringObject.transform;

                // Set the position of this GameObject.
                segmentObject.transform.position = position;

                // Create a new SpriteRenderer component for this segment.
                SpriteRenderer spriteRenderer = segmentObject.AddComponent<SpriteRenderer>();

                float specialRand = Random.Range(dropRandomLower, dropRandomUpper);
                if (specialRand > dropRandomCutoff)
                {
                    segmentScript.isSpecial = true;
                    spriteRenderer.sprite = mysterySprite;
                }
                else
                {
                    segmentScript.isSpecial = false;
                    spriteRenderer.sprite = SelectedSprite;
                }

                segOneMaterial.EnableKeyword("_EMISSION");
                spriteRenderer.material = segOneMaterial;



                // Set the width of this sprite based on the spriteWidth property.
                spriteRenderer.size = new Vector2(spriteWidth, (outerRadius - innerRadius) / numSegments);

                // Set the rotation of this GameObject to match the angle of this segment.
                segmentObject.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, angle - (angleStep / numSegments)));


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



    public void ConcentricRingsLumpy()
    {
        float segmentWidth = (outerRadius - innerRadius) / numSegments;

        for (int o = 0; o < numLayers; o++)
        {
            // Calculate the dimensions of the rectangle for this ring.
            float rectWidth = spriteWidth + segmentWidth;
            float rectHeight = segmentWidth * (o + 1);

            // Calculate the number of segments to distribute within this ring.
            int segmentsInRing = numSegments + (o * 15);

            // Calculate the angle step for each segment.
            float angleStep = 360f / segmentsInRing;

            // Create a new GameObject to hold the ring.
            GameObject ringObject = new GameObject("Ring " + o);

            Rigidbody2D ringRB = ringObject.AddComponent<Rigidbody2D>();
            ringRB.bodyType = RigidbodyType2D.Kinematic;

            if (o % 2 == 0)
            {
                ringRB.angularVelocity = rotationSpeed + (o * 15);
            }
            else
            {
                ringRB.angularVelocity = -(rotationSpeed + (o * 15));
            }

            Sprite SelectedSprite = spriteSelection[Random.Range(0, spriteSelection.Length)];


            for (int i = 0; i < segmentsInRing; i++)
            {
                // Calculate the angle for this segment.
                float angle = i * angleStep;

                // Calculate the position for the center of this segment.
                Vector3 position = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f) * ((innerRadius + outerRadius + o) / 2f);

                // Create a new GameObject to hold the sprite and collider for this segment.
                GameObject segmentObject = new GameObject("Segment " + o + i);

                // Add segment handler script
                Segment segmentScript = segmentObject.AddComponent<Segment>();
                // Set the parent of this GameObject to the ring object.
                segmentObject.transform.parent = ringObject.transform;

                // Set the position of this GameObject.
                segmentObject.transform.localPosition = position;

                // Create a new SpriteRenderer component for this segment.
                SpriteRenderer spriteRenderer = segmentObject.AddComponent<SpriteRenderer>();



                float specialRand = Random.Range(dropRandomLower, dropRandomUpper);
                if (specialRand > dropRandomCutoff)
                {
                    segmentScript.isSpecial = true;
                    spriteRenderer.sprite = mysterySprite;
                }
                else
                {
                    segmentScript.isSpecial = false;
                    spriteRenderer.sprite = SelectedSprite;
                }

                segOneMaterial.EnableKeyword("_EMISSION");
                spriteRenderer.material = segOneMaterial;

                // Set the size of this sprite based on the spriteWidth and segmentWidth properties.
                spriteRenderer.size = new Vector2(spriteWidth, segmentWidth);

                segmentScript.pillPrefabs = pillPrefabs;

                // Set initial health
                segmentScript.health += o;
            }
        }
    }




    public void LatticeHell()
    {
        for (int o = 0; o < 2; o++)
        {
            // Create a new GameObject to hold the square.
            GameObject squareObject = new GameObject("Square " + o);

            Rigidbody2D squareRB = squareObject.AddComponent<Rigidbody2D>();
            squareRB.bodyType = RigidbodyType2D.Kinematic;

            int segmentModifier = o;

            // Calculate the size of the square for this layer.
            float squareSize = 20f;

            // Calculate the number of rows and columns in the square.
            int numRows = numSegments + segmentModifier;
            int numCols = numSegments + segmentModifier;

            // Calculate the size of each segment within the square.
            float segmentWidth = squareSize / numCols;
            float segmentHeight = squareSize / numRows;


            Sprite SelectedSprite = spriteSelection[Random.Range(0, spriteSelection.Length)];

            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    // Calculate the position for this segment within the square.
                    float xPos = (col * segmentWidth) - (squareSize / 2f) + (segmentWidth / 2f);
                    float yPos = (row * segmentHeight) - (squareSize / 2f) + (segmentHeight / 2f);

                    // Create a new GameObject to hold the sprite and collider for this segment.
                    GameObject segmentObject = new GameObject("Segment " + o + row + col);

                    // Add segment handler script.
                    Segment segmentScript = segmentObject.AddComponent<Segment>();
                    // Set the parent of this GameObject to the square object.
                    segmentObject.transform.parent = squareObject.transform;

                    // Set the position of this GameObject within the square.
                    segmentObject.transform.localPosition = new Vector3(xPos, yPos, 0f);

                    // Create a new SpriteRenderer component for this segment.
                    SpriteRenderer spriteRenderer = segmentObject.AddComponent<SpriteRenderer>();

                    // Set the sprite for this SpriteRenderer.

                    float specialRand = Random.Range(dropRandomLower, dropRandomUpper);
                    if (specialRand > dropRandomCutoff)
                    {
                        segmentScript.isSpecial = true;
                        spriteRenderer.sprite = mysterySprite;
                    }
                    else
                    {
                        segmentScript.isSpecial = false;
                        spriteRenderer.sprite = SelectedSprite;
                    }



                    segOneMaterial.EnableKeyword("_EMISSION");
                    spriteRenderer.material = segOneMaterial;

                    // Set the size of this sprite based on the segment dimensions.
                    spriteRenderer.size = new Vector2(segmentWidth, segmentHeight);

                    segmentScript.pillPrefabs = pillPrefabs;

                    // Set initial health.
                    segmentScript.health += o;
                }
            }

            if (o % 2 == 0)
            {
                squareRB.angularVelocity = rotationSpeed + segmentModifier;
            }
            else
            {
                squareRB.angularVelocity = -(rotationSpeed + segmentModifier);
            }
        }
    }



    public void LittleSquares()
    {

        float innerRadius = 1.3f; // The inner radius of the ring.
        float outerRadius = 1.4f; // The outer radius of the ring.

        int numRings = 6;

        float rotationSpeed = 10f;


        for (int o = 0; o < numRings; o++)
        {
            int numSegments = Random.Range(2, 3);
            // Create a new GameObject to hold the square.
            GameObject squareObject = new GameObject("Square" + o);

            float randomX = Random.Range(-4f, 4f);
            float randomY = Random.Range(-3f, 3f);
            squareObject.transform.position = transform.position + new Vector3(randomX, randomY, 0f);

            Rigidbody2D squareRB = squareObject.AddComponent<Rigidbody2D>();
            squareRB.bodyType = RigidbodyType2D.Kinematic;

            // Calculate the size of the square for this layer.
            float squaresizeRand = Random.Range(20f, 20f);
            float squareSize = spriteWidth / squaresizeRand;

            // Calculate the number of rows and columns in the square.
            float numRows = numSegments * numSegments;
            float numCols = numSegments * numSegments;

            // Calculate the size of each segment within the square.
            float segmentWidth = numSegments / numRows;
            float segmentHeight = numSegments / numCols;

            Sprite SelectedSprite = spriteSelection[Random.Range(0, spriteSelection.Length)];
            for (int row = 0; row < numRows; row++)
            {

                for (int col = 0; col < numCols; col++)
                {
                    // Calculate the position for this segment within the square.
                    float xPos = (col * segmentWidth) - (squareSize);
                    float yPos = (row * segmentHeight) - (squareSize);

                    // Create a new GameObject to hold the sprite and collider for this segment.
                    GameObject segmentObject = new GameObject("Segment" + o + row + col);

                    // Set the parent of this GameObject to the square object.
                    segmentObject.transform.parent = squareObject.transform;

                    // Set the position of this GameObject within the square.
                    segmentObject.transform.localPosition = new Vector3(xPos, yPos, 0f);

                    // Add segment handler script.
                    Segment segmentScript = segmentObject.AddComponent<Segment>();
                    // Create a new SpriteRenderer component for this segment.
                    SpriteRenderer spriteRenderer = segmentObject.AddComponent<SpriteRenderer>();



                    float specialRand = Random.Range(dropRandomLower, dropRandomUpper);
                    if (specialRand > dropRandomCutoff)
                    {
                        segmentScript.isSpecial = true;
                        spriteRenderer.sprite = mysterySprite;
                    }
                    else
                    {
                        segmentScript.isSpecial = false;
                        spriteRenderer.sprite = SelectedSprite;
                    }


                    // Set the sprite for this SpriteRenderer.

                    segOneMaterial.EnableKeyword("_EMISSION");
                    spriteRenderer.material = segOneMaterial;

                    // Set the size of this sprite based on the segment dimensions.
                    spriteRenderer.size = new Vector2(segmentWidth / 2, segmentHeight / 2);



                    segmentScript.pillPrefabs = pillPrefabs;

                    // Set initial health.
                    segmentScript.health = o;
                }

            }
        }
    }





}
