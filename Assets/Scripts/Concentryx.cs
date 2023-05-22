using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Concentryx : MonoBehaviour
{
    public int numSegments = 32; // The number of segments in the ring.
    public int numLayers = 4; // The number of segments in the ring.
    public float innerRadius = 1.0f; // The inner radius of the ring.
    public float outerRadius = 2.0f; // The outer radius of the ring.
    public float spriteWidth = 0.1f; // The width of each sprite in the ring.

    public GameObject Player;

    public int dropRandomLower = 0;
    public int dropRandomUpper = 50;

    public int dropRandomCutoff = 45;

    public Sprite mysterySprite;
    public Sprite[] spriteSelection;

    public float rotationSpeed = 10f;
    public string liveLevelType = "none";
    public Material segOneMaterial;


    public GameObject[] pillPrefabs;

    public GameObject[] enemyPrefabs;

    public GameObject[] planetPrefabs;

    private void Start()
    {

        //ConcentricRings();
        //LatticeHell();
        LevelPatternMutantCarrot();
        //Pot();

        //ConcentricRingsLumpy();
        //LittleSquares();




    }


    public void LevelPatternMutantCarrot()
    {
        WordPlay("mutant", 5, 0.7f, 7f, 3.5f, 1f); // word, gridsize, segmentwidth, xstart, ystart, outputscale
        WordPlay("carrot", 5, 0.7f, 7f, -3f, 1f); // word, gridsize, segmentwidth, xstart, ystart, outputscale


        // 2 enemies, 1 planet

        CreateEnemyShip();

    }
    public void CreateEnemyShip()
    {
        Vector3 parentPosition = transform.position;
        Vector3 parentScale = transform.localScale;
        Vector3 parentSize = new Vector3(parentScale.x * 2, parentScale.y * 2, parentScale.z * 2);

        Vector3 playerPosition = Player.transform.position;
        Vector3 playerScale = Player.transform.localScale;
        Vector3 playerSize = new Vector3(playerScale.x * 2, playerScale.y * 2, playerScale.z * 2);

        float cellSize = Mathf.Min(parentSize.x, parentSize.y, parentSize.z) / 10f; // Adjust the cell size as needed
        Vector3 cellOffset = new Vector3(cellSize, cellSize, cellSize);

        Vector3 enemyPosition = Vector3.zero;
        bool isValidPosition = false;
        int maxAttempts = 100; // Increase the number of attempts if necessary
        int attempts = 0;

        while (!isValidPosition && attempts < maxAttempts)
        {
            Vector3 randomCell = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
            Vector3 cellPosition = parentPosition + Vector3.Scale(parentSize, randomCell);

            // Check if the cell position overlaps with the player
            if (!CheckOverlapWithPlayer(cellPosition, playerPosition, playerSize))
            {
                enemyPosition = cellPosition;
                isValidPosition = true;
            }

            attempts++;
        }

        if (!isValidPosition)
        {
            Debug.Log("Failed to find a valid position for the enemy ship.");
            return;
        }

        GameObject enemyShip = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], enemyPosition, Quaternion.identity);
        enemyShip.transform.parent = transform;
    }

    private bool CheckOverlapWithPlayer(Vector3 enemyPosition, Vector3 playerPosition, Vector3 playerSize)
    {
        float playerHalfWidth = playerSize.x / 2;
        float playerHalfHeight = playerSize.y / 2;

        if (enemyPosition.x + playerHalfWidth < playerPosition.x - playerHalfWidth ||
            enemyPosition.x - playerHalfWidth > playerPosition.x + playerHalfWidth ||
            enemyPosition.y + playerHalfHeight < playerPosition.y - playerHalfHeight ||
            enemyPosition.y - playerHalfHeight > playerPosition.y + playerHalfHeight)
        {
            return false;
        }

        return true;
    }





    public void Fortress()
    {
        WordPlay("#######", 5, 0.7f, 7f, 3.5f, 0.8f); // word, gridsize, segmentwidth, xstart, ystart, outputscale
        WordPlay("| |    ", 5, 0.7f, 7f, 2f, 0.8f); // word, gridsize, segmentwidth, xstart, ystart, outputscale
        WordPlay("| |    ", 5, 0.7f, 7f, 0.1f, 0.8f); // word, gridsize, segmentwidth, xstart, ystart, outputscale
        WordPlay("| |    ", 5, 0.7f, 7f, -2f, 0.8f); // word, gridsize, segmentwidth, xstart, ystart, outputscale
        WordPlay("#######", 5, 0.7f, 7f, -3.5f, 0.8f); // word, gridsize, segmentwidth, xstart, ystart, outputscale
    }



    public void Pot()
    {
        WordPlay("~  ~  ~", 5, 0.7f, 7f, 3.5f, 0.8f); // word, gridsize, segmentwidth, xstart, ystart, outputscale
        WordPlay(" ~ ~ ~ ", 5, 0.7f, 7f, 1.5f, 0.8f); // word, gridsize, segmentwidth, xstart, ystart, outputscale
        WordPlay("  ~~~  ", 5, 0.7f, 7f, -0.5f, 0.8f); // word, gridsize, segmentwidth, xstart, ystart, outputscale
        WordPlay("~~~~~~~", 5, 0.7f, 7f, -2.5f, 0.8f); // word, gridsize, segmentwidth, xstart, ystart, outputscale
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





    public void WordPlay(string word = "test", int gridSize = 5, float segmentWidth = 0.7f, float xstart = 7f, float ystart = 0f, float outputscale = 1f)
    {

        // Create a new GameObject to hold the word.
        GameObject wordObject = new GameObject("Word");

        // Calculate the starting position of the word based on its initial position.
        Vector3 wordPosition = wordObject.transform.position;

        // Iterate over each character in the word.
        string reversedWord = new string(word.Reverse().ToArray());



        foreach (char letter in reversedWord)
        {
            // Convert the letter to uppercase.
            char upperLetter = char.ToUpper(letter);

            // Get the segments for the current letter.
            List<int> segments = GetSegmentsForLetter(upperLetter);

            // Create a new GameObject to hold the letter.
            GameObject letterObject = new GameObject("Letter " + upperLetter);

            // Set the parent of the letter object to the wordObject.
            letterObject.transform.parent = wordObject.transform;

            // Create a new GameObject to hold the segments.
            GameObject segmentObject = new GameObject("Segments");

            // Set the parent of this GameObject to the letter object.
            segmentObject.transform.parent = letterObject.transform;

            // Iterate over each segment of the letter.
            for (int i = 0; i < segments.Count; i++)
            {
                int segmentCode = segments[i];
                // Calculate the row and column of the segment within the grid.
                int row = i / gridSize;
                int col = i % gridSize;

                // Only create visible segments for 'on' segments.
                if (segmentCode == 1)
                {
                    // Calculate the position for this segment based on its row and column.
                    Vector3 position = new Vector3((col - (gridSize - 1) / 2.0f) * (segmentWidth / 1.5f), -row * (segmentWidth / 2.5f), 0.0f);

                    // Create a new GameObject to hold the segment.
                    GameObject segment = new GameObject("Segment " + i);

                    // Add segment handler script
                    Segment segmentScript = segment.AddComponent<Segment>();

                    // Set the parent of this GameObject to the segment object.
                    segment.transform.parent = segmentObject.transform;

                    // Set the position of this GameObject.
                    segment.transform.localPosition = position;

                    // Create a new SpriteRenderer component for this segment.
                    SpriteRenderer spriteRenderer = segment.AddComponent<SpriteRenderer>();

                    // Set the sprite and other properties based on the specific letter.
                    Sprite selectedSprite = spriteSelection[Random.Range(0, spriteSelection.Length)];

                    float specialRand = Random.Range(dropRandomLower, dropRandomUpper);
                    if (specialRand > dropRandomCutoff)
                    {
                        segmentScript.isSpecial = true;
                        spriteRenderer.sprite = mysterySprite;
                    }
                    else
                    {
                        segmentScript.isSpecial = false;
                        spriteRenderer.sprite = selectedSprite;
                    }

                    segOneMaterial.EnableKeyword("_EMISSION");
                    spriteRenderer.material = segOneMaterial;

                    // Set the width of this sprite based on the segmentWidth property.
                    spriteRenderer.size = new Vector2(segmentWidth, segmentWidth);

                    // Set the rotation of this GameObject to match the angle of this segment (90 degrees clockwise).
                    segment.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);

                    // Set the pill prefabs and initial health for the segment.
                    segment.GetComponent<Segment>().pillPrefabs = pillPrefabs;
                    segment.GetComponent<Segment>().health = 3;
                }
                else
                {
                    // Create an empty game object for 'off' segments.
                    GameObject emptySegment = new GameObject("Segment " + i);
                    emptySegment.transform.parent = segmentObject.transform;
                    emptySegment.transform.localPosition = new Vector3((col - (gridSize - 1) / 2.0f) * (segmentWidth / 1.5f), -row * (segmentWidth / 2.5f), 0.0f);
                    emptySegment.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
                }
            }

            // Update the word position for the next letter.
            wordPosition.x += (gridSize / 1.8f);
            // Update the position of the wo1.2frdObject.
            wordObject.transform.position = wordPosition;
        }

        // Set final position

        wordPosition.x = (xstart);
        wordPosition.y = (ystart);
        wordObject.transform.position = wordPosition;
        Vector3 newScale = new Vector3(outputscale, outputscale, 1f); // Replace with your desired scale values
        wordObject.transform.localScale = newScale;

    }



    List<int> GetSegmentsForLetter(char letter)
    {
        List<int> segments = new List<int>();

        switch (letter)
        {
            case 'A':
                segments.AddRange(new int[] {
                0, 1, 1, 1, 0,
                1, 0, 0, 0, 1,
                1, 1, 1, 1, 1,
                1, 0, 0, 0, 1,
                1, 0, 0, 0, 1 });
                break;
            case 'B':
                segments.AddRange(new int[] {
                1, 1, 1, 1, 0,
                1, 0, 0, 0, 1,
                1, 1, 1, 1, 0,
                1, 0, 0, 0, 1,
                1, 1, 1, 1, 1 });
                break;
            case 'C':
                segments.AddRange(new int[] {
                0, 1, 1, 1, 1,
                1, 0, 0, 0, 0,
                1, 0, 0, 0, 0,
                1, 0, 0, 0, 0,
                0, 1, 1, 1, 1 });
                break;
            case 'D':
                segments.AddRange(new int[] {
                1, 1, 1, 1, 0,
                1, 0, 0, 0, 1,
                1, 0, 0, 0, 1,
                1, 0, 0, 0, 1,
                1, 1, 1, 1, 0 });
                break;
            case 'E':
                segments.AddRange(new int[] {
                1, 1, 1, 1, 1,
                1, 0, 0, 0, 0,
                1, 1, 1, 1, 0,
                1, 0, 0, 0, 0,
                1, 1, 1, 1, 1 });
                break;
            case 'F':
                segments.AddRange(new int[] {
                1, 1, 1, 1, 1,
                1, 0, 0, 0, 0,
                1, 1, 1, 1, 0,
                1, 0, 0, 0, 0,
                1, 0, 0, 0, 0 });
                break;
            case 'G':
                segments.AddRange(new int[] {
                0, 1, 1, 1, 1,
                1, 0, 0, 0, 0,
                1, 0, 0, 1, 1,
                1, 0, 0, 0, 1,
                0, 1, 1, 1, 0 });
                break;
            case 'H':
                segments.AddRange(new int[] {
                1, 0, 0, 0, 1,
                1, 0, 0, 0, 1,
                1, 1, 1, 1, 1,
                1, 0, 0, 0, 1,
                1, 0, 0, 0, 1 });
                break;
            case 'I':
                segments.AddRange(new int[] {
                1, 1, 1, 1, 1,
                0, 0, 1, 0, 0,
                0, 0, 1, 0, 0,
                0, 0, 1, 0, 0,
                1, 1, 1, 1, 1 });
                break;
            case 'J':
                segments.AddRange(new int[] {
                1, 1, 1, 1, 1,
                0, 0, 1, 0, 0,
                0, 0, 1, 0, 0,
                1, 0, 1, 0, 0,
                0, 1, 0, 0, 0 });
                break;
            case 'K':
                segments.AddRange(new int[] {
                1, 0, 0, 1, 0,
                1, 0, 1, 0, 0,
                1, 1, 0, 0, 0,
                1, 0, 1, 0, 0,
                1, 0, 0, 1, 0 });
                break;
            case 'L':
                segments.AddRange(new int[] {
                1, 0, 0, 0, 0,
                1, 0, 0, 0, 0,
                1, 0, 0, 0, 0,
                1, 0, 1, 0, 0,
                1, 1, 1, 1, 1 });
                break;
            case 'M':
                segments.AddRange(new int[] {
                1, 0, 0, 0, 1,
                1, 1, 0, 1, 1,
                1, 0, 1, 0, 1,
                1, 0, 0, 0, 1,
                1, 0, 0, 0, 1 });
                break;
            case 'N':
                segments.AddRange(new int[] {
                1, 0, 0, 0, 1,
                1, 1, 0, 0, 1,
                1, 0, 1, 0, 1,
                1, 0, 0, 1, 1,
                1, 0, 0, 0, 1 });
                break;
            case 'O':
                segments.AddRange(new int[] {
                0, 1, 1, 1, 0,
                1, 0, 0, 0, 1,
                1, 0, 0, 0, 1,
                1, 0, 0, 0, 1,
                0, 1, 1, 1, 0 });
                break;
            case 'p':
                segments.AddRange(new int[] {
                1, 1, 1, 1, 0,
                1, 0, 0, 0, 1,
                1, 1, 1, 1, 0,
                1, 0, 0, 0, 0,
                1, 0, 0, 0, 0 });
                break;
            case 'Q':
                segments.AddRange(new int[] {
                0, 1, 1, 1, 0,
                1, 0, 0, 0, 1,
                1, 0, 1, 0, 1,
                1, 0, 0, 1, 0,
                0, 1, 1, 0, 1 });
                break;
            case 'R':
                segments.AddRange(new int[] {
                1, 1, 1, 1, 0,
                1, 0, 0, 0, 1,
                1, 1, 1, 1, 0,
                1, 0, 0, 0, 1,
                1, 0, 0, 0, 1 });
                break;
            case 'S':
                segments.AddRange(new int[] {
                0, 1, 1, 1, 1,
                1, 0, 0, 0, 0,
                0, 1, 1, 1, 0,
                0, 0, 0, 0, 1,
                1, 1, 1, 1, 0 });
                break;
            case 'T':
                segments.AddRange(new int[] {
                1, 1, 1, 1, 1,
                0, 0, 1, 0, 0,
                0, 0, 1, 0, 0,
                0, 0, 1, 0, 0,
                0, 0, 1, 0, 0 });
                break;
            case 'U':
                segments.AddRange(new int[] {
                1, 0, 0, 0, 1,
                1, 0, 0, 0, 1,
                1, 0, 0, 0, 1,
                1, 0, 0, 0, 1,
                0, 1, 1, 1, 0 });
                break;
            case 'V':
                segments.AddRange(new int[] {
                1, 0, 0, 0, 1,
                1, 0, 0, 0, 1,
                1, 0, 0, 0, 1,
                0, 1, 0, 1, 0,
                0, 0, 1, 0, 0 });
                break;
            case 'W':
                segments.AddRange(new int[] {
                1, 0, 0, 0, 1,
                1, 0, 0, 0, 1,
                1, 0, 0, 0, 1,
                1, 0, 1, 0, 1,
                0, 1, 0, 1, 0 });
                break;
            case 'X':
                segments.AddRange(new int[] {
                1, 0, 0, 0, 1,
                0, 1, 0, 1, 0,
                0, 0, 1, 0, 0,
                0, 1, 0, 1, 0,
                1, 0, 0, 0, 1 });
                break;
            case 'Y':
                segments.AddRange(new int[] {
                1, 0, 0, 0, 1,
                0, 1, 0, 1, 0,
                0, 0, 1, 0, 0,
                0, 0, 1, 0, 0,
                0, 0, 1, 0, 0 });
                break;
            case 'Z':
                segments.AddRange(new int[] {
                1, 1, 1, 1, 1,
                0, 0, 0, 1, 0,
                0, 0, 1, 0, 0,
                0, 1, 0, 0, 0,
                1, 1, 1, 1, 1 });
                break;
            case '0':
                segments.AddRange(new int[] {
                0, 1, 1, 1, 0,
                1, 0, 0, 0, 1,
                1, 0, 0, 0, 1,
                1, 0, 0, 0, 1,
                0, 1, 1, 1, 0 });
                break;
            case '1':
                segments.AddRange(new int[] {
                0, 0, 1, 1, 0,
                0, 1, 1, 1, 0,
                0, 0, 1, 1, 0,
                0, 0, 1, 1, 0,
                0, 1, 1, 1, 0 });
                break;
            case '2':
                segments.AddRange(new int[] {
                0, 1, 1, 1, 0,
                1, 0, 0, 0, 1,
                0, 0, 0, 1, 0,
                0, 0, 1, 0, 0,
                1, 1, 1, 1, 1 });
                break;
            case '3':
                segments.AddRange(new int[] {
                0, 1, 1, 1, 0,
                1, 0, 0, 0, 1,
                0, 0, 1, 1, 0,
                1, 0, 0, 0, 1,
                0, 1, 1, 1, 0 });
                break;
            case '4':
                segments.AddRange(new int[] {
                1, 0, 0, 1, 0,
                1, 0, 0, 1, 0,
                1, 0, 0, 1, 0,
                1, 1, 1, 1, 1,
                0, 0, 0, 1, 0 });
                break;
            case '5':
                segments.AddRange(new int[] {
                1, 1, 1, 1, 1,
                1, 0, 0, 0, 0,
                1, 1, 1, 1, 0,
                0, 0, 0, 0, 1,
                1, 1, 1, 1, 0 });
                break;
            case '6':
                segments.AddRange(new int[] {
                0, 1, 1, 1, 1,
                1, 0, 0, 0, 0,
                1, 1, 1, 1, 0,
                1, 0, 0, 0, 1,
                0, 1, 1, 1, 0 });
                break;
            case '7':
                segments.AddRange(new int[] {
                1, 1, 1, 1, 0,
                0, 0, 0, 1, 0,
                0, 0, 1, 1, 1,
                0, 0, 0, 1, 0,
                0, 0, 0, 1, 0 });
                break;
            case '8':
                segments.AddRange(new int[] {
                0, 1, 1, 1, 0,
                1, 0, 0, 0, 1,
                0, 1, 1, 1, 0,
                1, 0, 0, 0, 1,
                0, 1, 1, 1, 0 });
                break;
            case '9':
                segments.AddRange(new int[] {
                0, 1, 1, 1, 0,
                1, 0, 0, 0, 1,
                0, 1, 1, 1, 1,
                0, 0, 0, 0, 1,
                1, 1, 1, 1, 0 });
                break;
            case '|':
                segments.AddRange(new int[] {
                0, 0, 1, 0, 0,
                0, 0, 1, 0, 0,
                0, 0, 1, 0, 0,
                0, 0, 1, 0, 0,
                0, 0, 1, 0, 0 });
                break;
            case '#':
                segments.AddRange(new int[] {
                0, 1, 0, 1, 0,
                1, 1, 1, 1, 1,
                0, 1, 0, 1, 0,
                1, 1, 1, 1, 1,
                0, 1, 0, 1, 0 });
                break;
            case '*':
                segments.AddRange(new int[] {
                1, 0, 1, 0, 1,
                0, 1, 1, 1, 0,
                1, 1, 1, 1, 1,
                0, 1, 1, 1, 0,
                1, 0, 1, 0, 1 });
                break;
            case '@':
                segments.AddRange(new int[] {
                0, 1, 1, 1, 0,
                1, 0, 1, 0, 1,
                1, 0, 1, 1, 1,
                1, 0, 0, 0, 1,
                0, 1, 1, 1, 0 });
                break;
            case '~':
                segments.AddRange(new int[] {
                0, 0, 1, 0, 0,
                1, 0, 1, 0, 1,
                0, 1, 1, 1, 0,
                0, 0, 1, 0, 0,
                1, 1, 1, 1, 1 });
                break;
            // Add more cases for other letters as needed
            default:
                // If the letter is not defined, return an empty list
                segments.Clear();
                break;
        }

        return segments;
    }


}
