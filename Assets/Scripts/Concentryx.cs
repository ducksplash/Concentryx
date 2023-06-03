using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

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

    public GridLayouts gridLayouts;
    public GameObject[] pillPrefabs;

    public GameObject[] enemyPrefabs;

    public GameObject[] planetPrefabs;

    public GameObject defaultParent;
    public GameObject gridParent;

    public TextMeshProUGUI levelText1;
    public TextMeshProUGUI levelText2;
    public TextMeshProUGUI levelText3;
    public string[] WordList = { "mutant", "carrot", " foot " };


    private void Start()
    {

        //ConcentricRings();
        //LevelPatternPhrase(WordList);
        //Pot();




        ArrangeInstances(gridLayouts.GetGridPattern("Boxed In"));


    }



    public void ArrangeInstances(string[] InstanceParameters)
    {

        int width = 64; // Width of the grid
        int height = Mathf.CeilToInt((float)InstanceParameters[1].Length / width); // Height of the grid
        float spacing = 0.28f; // Spacing between each instantiated sprite

        GameObject gridObject = new GameObject("GridObject");

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = y * width + x;
                if (index < InstanceParameters[1].Length && InstanceParameters[1][index] == '1')
                {
                    Vector3 position = gridParent.transform.position + new Vector3(x * spacing, -y * (spacing * 1.7f), 0f);
                    GameObject segmentObject = new GameObject("Segment " + y + x);
                    segmentObject.transform.position = position;

                    SpriteRenderer spriteRenderer = segmentObject.AddComponent<SpriteRenderer>();
                    Segment segmentScript = segmentObject.AddComponent<Segment>();


                    // get sprite colour pattern
                    Sprite selectedSprite = GetSprite(x, y, 0);


                    switch (InstanceParameters[0])
                    {
                        case "Meow":
                            selectedSprite = GetSprite(x, y, 6);
                            break;

                        case "Fleg":
                            selectedSprite = GetSprite(x, y, 5);
                            break;

                        case "Quack":
                            selectedSprite = GetSprite(x, y, 7);
                            break;

                        case "Sunrise":
                            selectedSprite = GetSprite(x, y, 7);
                            break;

                        case "Boxed In":
                            selectedSprite = GetSprite(x, y, 3);
                            break;

                        default:
                            selectedSprite = GetSprite(x, y, 0);
                            break;
                    }


                    // insert mystery/bonus drops

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
                    spriteRenderer.sortingLayerName = "Default";
                    spriteRenderer.sortingOrder = -10;


                    // Create a new BoxCollider2D component for this segment.
                    BoxCollider2D collider = segmentObject.AddComponent<BoxCollider2D>();

                    // Set the size of the BoxCollider2D to match the size of the SpriteRenderer.
                    collider.size = spriteRenderer.bounds.size;


                    // Set the pill prefabs and initial health for the segment.
                    segmentObject.GetComponent<Segment>().pillPrefabs = pillPrefabs;
                    segmentObject.GetComponent<Segment>().health = 3;

                    segmentObject.transform.parent = gridObject.transform;
                }
            }
        }
        gridObject.transform.parent = gridParent.transform;


    }



    public Sprite GetSprite(int x, int y, int pattern = 0)
    {

        Sprite selectedSprite = spriteSelection[0];

        // random
        if (pattern == 0)
        {
            selectedSprite = spriteSelection[Random.Range(0, spriteSelection.Length)];
        }


        // old TV close up
        if (pattern == 1)
        {


            if (y % 2 == 0)
            {
                selectedSprite = spriteSelection[3];
            }
            else
            {

                if (x % 2 == 0)
                {
                    selectedSprite = spriteSelection[0];
                }
                else
                {
                    selectedSprite = spriteSelection[1];
                }
            }

        }

        // blue, black, horizontal
        if (pattern == 2)
        {

            if (y % 2 == 0)
            {
                selectedSprite = spriteSelection[3];
            }
            else
            {

                if (x % 2 == 0)
                {
                    selectedSprite = spriteSelection[1];
                }
                else
                {
                    selectedSprite = spriteSelection[1];
                }
            }

        }


        // blue, black, vertical
        if (pattern == 3)
        {

            if (x % 2 == 0)
            {
                selectedSprite = spriteSelection[3];
            }
            else
            {

                if (y % 2 == 0)
                {
                    selectedSprite = spriteSelection[1];
                }
                else
                {
                    selectedSprite = spriteSelection[1];
                }
            }

        }




        // blue stripe on white vertical
        if (pattern == 4)
        {

            if (x < 20 || x > 40)
            {
                selectedSprite = spriteSelection[5];
            }
            else
            {

                if (x < 20 || x > 40)
                {
                    selectedSprite = spriteSelection[2];
                }
                else
                {
                    selectedSprite = spriteSelection[1];
                }
            }

        }



        if (pattern == 5)
        {
            int totalSections = 3;
            int sectionWidth = 64 / totalSections;

            if (x >= sectionWidth && x < sectionWidth * 2)
            {
                selectedSprite = spriteSelection[5];
            }
            else if (x >= sectionWidth * 2)
            {
                selectedSprite = spriteSelection[6];
            }
            else
            {
                selectedSprite = spriteSelection[4];
            }
        }


        // black
        if (pattern == 6)
        {

            selectedSprite = spriteSelection[3];

        }


        // yellow
        if (pattern == 7)
        {

            selectedSprite = spriteSelection[2];

        }


        return selectedSprite;


    }








    public void LevelPatternPhrase()
    {

        string word1 = levelText1.text;
        string word2 = levelText2.text;
        string word3 = levelText3.text;


        float yLevel = -1.0f;
        foreach (string word in new string[] { word3, word2, word1 })
        {
            WordPlay(word, 5, 0.7f, 15f, yLevel, 1f); // word, gridsize, segmentwidth, xstart, ystart, outputscale
            yLevel -= 2.5f;
        }


    }




    public void CreateEnemyWaller(int numships = 1)
    {

        for (int i = 0; i < numships; i++)
        {

            GameObject enemyShip = Instantiate(enemyPrefabs[1]);
            //enemyShip.transform.parent = laserEnemyParent.transform;

        }

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


            float maxPositionOffsetX = (cameraSize * aspectRatio) / 2;
            float maxPositionOffsetY = cameraSize / 2;

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

            GameObject enemyShip = Instantiate(enemyPrefabs[0], enemyPosition, Quaternion.identity);
            enemyShip.transform.parent = transform;

            // Calculate rotation towards the player and flip it
            Vector3 directionToPlayer = playerPosition - enemyPosition;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            angle += 180f; // Add 180 degrees to flip the direction
            enemyShip.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        ChainLightning.instance.InitialiseLightning();
        Debug.Log("enemy created");
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







    public void CreatePlanet(int numplanets = 1)
    {
        for (int i = 0; i < numplanets; i++)
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


            float maxPositionOffsetX = (cameraSize * aspectRatio) / 2;
            float maxPositionOffsetY = cameraSize / 2;

            Vector3 planetPosition = Vector3.zero;
            bool isValidPosition = false;
            int maxAttempts = 100;
            int attempts = 0;

            while (!isValidPosition && attempts < maxAttempts)
            {
                float randomPositionOffsetX = Random.Range(-maxPositionOffsetX, maxPositionOffsetX);
                float randomPositionOffsetY = Random.Range(-maxPositionOffsetY, maxPositionOffsetY);

                planetPosition = parentPosition + new Vector3(randomPositionOffsetX, randomPositionOffsetY, 0f);

                if (!CheckOverlapWithPlayer(planetPosition, playerPosition, playerSize))
                {
                    isValidPosition = true;
                }

                attempts++;
            }

            if (!isValidPosition)
            {
                return;
            }


            GameObject planetPrefab = Instantiate(planetPrefabs[Random.Range(0, planetPrefabs.Length)], planetPosition, Quaternion.identity);


            planetPrefab.transform.parent = transform;

        }
        Debug.Log("planet created");
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
                    Vector3 position = new Vector3((col - (gridSize - 1) / 2.0f) * (segmentWidth / 2f), -row * (segmentWidth / 1.5f), 0.0f);

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


                    spriteRenderer.sortingOrder = -11;


                    // Create a new BoxCollider2D component for this segment.
                    BoxCollider2D collider = segment.AddComponent<BoxCollider2D>();

                    // Set the size of the BoxCollider2D to match the size of the SpriteRenderer.
                    collider.size = spriteRenderer.bounds.size;

                    // Set the rotation of this GameObject to match the angle of this segment (90 degrees clockwise).
                    //segment.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);

                    // Set the pill prefabs and initial health for the segment.
                    segment.GetComponent<Segment>().pillPrefabs = pillPrefabs;
                    segment.GetComponent<Segment>().health = 3;
                }
                else
                {
                    // Create an empty game object for 'off' segments.
                    GameObject emptySegment = new GameObject("Segment " + i);
                    emptySegment.transform.parent = segmentObject.transform;
                    emptySegment.transform.localPosition = new Vector3((col - (gridSize - 1) / 2.0f) * (segmentWidth / 2f), -row * (segmentWidth / 1.5f), 0.0f);
                }
            }

            // Update the word position for the next letter.
            wordPosition.x += (gridSize / 2f);
            // Update the position of the word object.
            wordObject.transform.position = wordPosition;
        }

        // Set final position

        wordPosition.y = (ystart);
        wordObject.transform.position = gridParent.transform.position + wordPosition;


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
                1, 0, 0, 0, 0,
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
            case 'P':
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
            case '!':
                segments.AddRange(new int[] {
                1, 1, 0, 1, 1,
                1, 1, 0, 1, 1,
                1, 1, 0, 1, 1,
                0, 0, 0, 0, 0,
                1, 1, 0, 1, 1 });
                break;
            case '?':
                segments.AddRange(new int[] {
                0, 1, 1, 1, 0,
                1, 0, 0, 0, 1,
                0, 0, 1, 1, 0,
                0, 0, 0, 0, 0,
                0, 0, 1, 0, 0 });
                break;
            case '>':
                segments.AddRange(new int[] {
                1, 1, 1, 0, 0,
                0, 0, 1, 1, 0,
                0, 0, 0, 1, 1,
                0, 0, 1, 1, 0,
                1, 1, 1, 0, 0 });
                break;
            case '<':
                segments.AddRange(new int[] {
                0, 0, 1, 1, 1,
                0, 1, 1, 0, 0,
                1, 1, 0, 0, 0,
                0, 1, 1, 0, 0,
                0, 0, 1, 1, 1 });
                break;
            case '/':
                segments.AddRange(new int[] {
                0, 0, 0, 1, 1,
                0, 0, 1, 1, 0,
                0, 1, 1, 0, 0,
                1, 1, 0, 0, 0,
                1, 0, 0, 0, 0 });
                break;
            case '\\':
                segments.AddRange(new int[] {
                1, 1, 0, 0, 0,
                0, 1, 1, 0, 0,
                0, 0, 1, 1, 0,
                0, 0, 0, 1, 1,
                0, 0, 0, 0, 1, });
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
