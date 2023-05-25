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
        LevelPatternMutantCarrot();
        //Pot();





    }


    public void LevelPatternMutantCarrot()
    {
        WordPlay("mutant", 5, 0.7f, 7f, 3.5f, 0.8f); // word, gridsize, segmentwidth, xstart, ystart, outputscale
        WordPlay("carrot", 5, 0.7f, 7f, -3f, 0.8f); // word, gridsize, segmentwidth, xstart, ystart, outputscale


        // 2 enemies, 1 planet

        // CreateEnemyShip(2);

    }

    public void CreateEnemyShip(int numships = 1)
    {
        for (int i = 0; i < numships; i++)
        {
            Vector3 parentPosition = transform.position;
            Vector3 parentScale = transform.localScale;
            Vector3 parentSize = new Vector3(parentScale.x * 2, parentScale.y * 2, parentScale.z * 2);

            Vector3 playerPosition = Player.transform.position;
            Vector3 playerScale = Player.transform.localScale;
            Vector3 playerSize = new Vector3(playerScale.x * 2, playerScale.y * 2, playerScale.z * 2);

            Camera mainCamera = Camera.main;
            float cameraSize = mainCamera.orthographicSize;
            float aspectRatio = mainCamera.aspect;

            float boundingAreaReduction = 0.2f; // 20%
            float reducedParentSizeX = parentSize.x * (1f - boundingAreaReduction);
            float reducedParentSizeY = parentSize.y * (1f - boundingAreaReduction);

            float maxPositionOffsetX = (cameraSize * aspectRatio) - reducedParentSizeX / 2f;
            float maxPositionOffsetY = cameraSize - reducedParentSizeY / 2f;

            Vector3 enemyPosition = Vector3.zero;
            bool isValidPosition = false;
            int maxAttempts = 100;
            int attempts = 0;

            while (!isValidPosition && attempts < maxAttempts)
            {
                float randomPositionOffsetX = Random.Range(-maxPositionOffsetX, maxPositionOffsetX);
                float randomPositionOffsetY = Random.Range(-maxPositionOffsetY, maxPositionOffsetY);

                enemyPosition = parentPosition + new Vector3(randomPositionOffsetX, randomPositionOffsetY, 0f);

                if (!CheckOverlapWithPlayer(enemyPosition, playerPosition, playerSize))
                {
                    isValidPosition = true;
                }

                attempts++;
            }

            if (!isValidPosition)
            {
                return;
            }

            GameObject enemyShip = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], enemyPosition, Quaternion.identity);
            enemyShip.transform.parent = transform;

            // Calculate rotation towards the player and flip it
            Vector3 directionToPlayer = playerPosition - enemyPosition;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            angle += 180f; // Add 180 degrees to flip the direction
            enemyShip.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        }
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

                    spriteRenderer.sortingOrder = 1;


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
