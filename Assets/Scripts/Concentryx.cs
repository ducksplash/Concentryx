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
    public int dropRandomUpper = 10;

    public int dropRandomCutoff = 90;

    public Sprite mysterySprite;
    public Sprite[] spriteSelection;

    public float rotationSpeed = 10f;
    public string currentLevelName = "none";
    public Material segOneMaterial;

    public GridLayouts gridLayouts;
    public GameObject[] pillPrefabs;

    public GameObject[] enemyPrefabs;

    public GameObject[] bossPrefabs;

    public GameObject[] planetPrefabs;
    public GameObject[] nearbyStarPrefabs;
    public GameObject[] farStarPrefabs;



    public GameObject defaultParent;
    public GameObject gridParent;
    public GameObject ringParent;
    public GameObject bossParent;

    public TextMeshProUGUI levelText1;
    public TextMeshProUGUI levelText2;
    public TextMeshProUGUI levelText3;

    private Coroutine GameTimer;

    public string[] thoseLevels;


    public AudioSource audioSource;

    public bool AudioPlaying;

    public string[] WordList = { "mutant", "carrot", " foot " };

    public AudioClip[] gameMusic;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }


    public void BuildLevel(int SelectedLevel)
    {

        CleanLevel();

        if (SelectedLevel == 999)
        {
            GameMaster.instance.ActiveEnemies = 0;
        }


        // first 10 levels should be named.
        // subsequent X levels should be selected randomly from a list of pre defined levels.
        // all levels beyond shall be randomly generated using the individual parts:
        // ImagePlay - Creates a grid based image out of segments, and is fed with 'ASCII Art' patterns made of 1s and 0s
        // LevelPatternPhrase - Creates a word out of segments, and is fed with up to 3 strings
        // ConcentricRings - Creates concentric rings of segments that rotate around the player, rotating faster the further away from the player.
        // CreateEnemyShip - Creates specified number of rotating enemies that fire projectiles at the player.
        // CreateEnemyWaller - Creates enemies that crawl the walls and sweeps the screen with a laser.
        // CreatePlanet - Creates a planet; these do not attack until their shield is brought down, then fire projectiles at the player.


        // decide on stars outside of other level decisions.

        // nearby
        int numNearbyStars = UnityEngine.Random.Range(1, 2);
        // faraway
        int numFarStars = UnityEngine.Random.Range(3, 8);



        // do levels under 12
        if (SelectedLevel < 13)
        {
            switch (SelectedLevel)
            {
                case 0:
                    // void level - never gets loaded
                    GameTimer = StartCoroutine(GameMaster.instance.Countdown(5));
                    currentLevelName = "Concentryx";
                    ConcentricRings();
                    CreateEnemyShip(1);
                    CreateNearbyStar(numNearbyStars);
                    CreateFarStar(numFarStars);
                    audioSource.clip = gameMusic[0];
                    break;

                case 1:
                    GameTimer = StartCoroutine(GameMaster.instance.Countdown(160));
                    currentLevelName = "Concentryx";
                    ConcentricRings();
                    CreateEnemyShip(1);
                    CreateNearbyStar(numNearbyStars);
                    CreateFarStar(numFarStars);
                    audioSource.clip = gameMusic[0];
                    break;

                case 2:
                    GameTimer = StartCoroutine(GameMaster.instance.Countdown(80));
                    currentLevelName = "New Friends";
                    ConcentricRings(6);
                    CreateEnemyWaller(1);
                    CreateNearbyStar(numNearbyStars);
                    CreateFarStar(numFarStars);
                    audioSource.clip = gameMusic[1];
                    break;

                case 3:
                    GameTimer = StartCoroutine(GameMaster.instance.Countdown(90));
                    currentLevelName = "Old Enemies";
                    LevelPatternPhrase(" ", "Ready?", " ");
                    CreateEnemyWaller(1);
                    CreateEnemyShip(2);
                    CreatePlanet(1);
                    CreateNearbyStar(numNearbyStars);
                    CreateFarStar(numFarStars);
                    audioSource.clip = gameMusic[2];
                    break;


                case 4:
                    GameTimer = StartCoroutine(GameMaster.instance.Countdown(120));
                    currentLevelName = "Inevitable";
                    ImagePlay(gridLayouts.GetGridPattern("Sunrise"));
                    CreateEnemyWaller(2);
                    CreateNearbyStar(numNearbyStars);
                    CreateFarStar(numFarStars);
                    audioSource.clip = gameMusic[3];
                    break;


                case 5:
                    GameTimer = StartCoroutine(GameMaster.instance.Countdown(200));
                    currentLevelName = "Meow";
                    ImagePlay(gridLayouts.GetGridPattern("Meow"));
                    CreateEnemyWaller(0);
                    CreatePlanet(1);
                    CreateNearbyStar(numNearbyStars);
                    CreateFarStar(numFarStars);
                    audioSource.clip = gameMusic[4];
                    break;


                case 6:
                    // boss 1
                    GameTimer = StartCoroutine(GameMaster.instance.Countdown(300));
                    currentLevelName = "uh oh";
                    // create enemy boss
                    CreateBossOne();
                    //CreatePlanet(1);
                    CreateNearbyStar(numNearbyStars);
                    CreateFarStar(numFarStars);
                    audioSource.clip = gameMusic[5];
                    break;

                case 7:
                    GameTimer = StartCoroutine(GameMaster.instance.Countdown(200));
                    currentLevelName = "Concentryx";
                    ImagePlay(gridLayouts.GetGridPattern("Concentryx"));
                    CreateEnemyWaller(2);
                    CreateEnemyShip(2);
                    CreatePlanet(1);
                    CreateNearbyStar(numNearbyStars);
                    CreateFarStar(numFarStars);
                    audioSource.clip = gameMusic[6];
                    break;

                case 8:
                    GameTimer = StartCoroutine(GameMaster.instance.Countdown(240));
                    currentLevelName = "Quack";
                    ImagePlay(gridLayouts.GetGridPattern("Quack"));
                    CreateEnemyCaterpillar(1);
                    CreateEnemyShip(4);
                    CreatePlanet(2);
                    CreateNearbyStar(numNearbyStars);
                    CreateFarStar(numFarStars);
                    audioSource.clip = gameMusic[7];
                    break;

                case 9:
                    GameTimer = StartCoroutine(GameMaster.instance.Countdown(300));
                    currentLevelName = "Arena";
                    ImagePlay(gridLayouts.GetGridPattern("Arena"));
                    CreateEnemyWaller(4);
                    CreateEnemyShip(0);
                    CreatePlanet(0);
                    CreateNearbyStar(numNearbyStars);
                    CreateFarStar(numFarStars);
                    audioSource.clip = gameMusic[8];
                    break;

                case 10:
                    GameTimer = StartCoroutine(GameMaster.instance.Countdown(240));
                    currentLevelName = "Vote, Dummy!";
                    ImagePlay(gridLayouts.GetGridPattern("Vote, Dummy!"));
                    CreateEnemyWaller(1);
                    CreateEnemyShip(4);
                    CreateNearbyStar(numNearbyStars);
                    CreateFarStar(numFarStars);
                    audioSource.clip = gameMusic[9];
                    break;

                case 11:
                    GameTimer = StartCoroutine(GameMaster.instance.Countdown(220));
                    currentLevelName = "Resonance Cascade";
                    ImagePlay(gridLayouts.GetGridPattern("Resonance Cascade"));
                    CreateEnemyWaller(1);
                    CreateEnemyShip(4);
                    CreatePlanet(0);
                    CreateNearbyStar(numNearbyStars);
                    CreateFarStar(numFarStars);
                    audioSource.clip = gameMusic[UnityEngine.Random.Range(0, gameMusic.Length - 1)];
                    break;

                case 12:
                    // boss 2
                    GameTimer = StartCoroutine(GameMaster.instance.Countdown(300));
                    currentLevelName = "Controversy";
                    ImagePlay(gridLayouts.GetGridPattern("Fleg"));
                    CreateBossTwo();
                    CreateEnemyCaterpillar(1);
                    CreateNearbyStar(numNearbyStars);
                    CreateFarStar(numFarStars);
                    audioSource.clip = gameMusic[UnityEngine.Random.Range(0, gameMusic.Length - 1)];
                    break;
            }

            audioSource.Play();
        }
        else
        {
            // set up the level, randomly generate some baddies and screen furniture.
            GameTimer = StartCoroutine(GameMaster.instance.Countdown(UnityEngine.Random.Range(140, 300)));
            currentLevelName = " ";
            audioSource.clip = gameMusic[UnityEngine.Random.Range(0, gameMusic.Length - 1)];
            audioSource.Play();
            CreateNearbyStar(numNearbyStars);
            CreateFarStar(numFarStars);
            ImagePlay(gridLayouts.GetRandomPattern());


            // create a boss every 10th SelectedLevel
            if (SelectedLevel % 5 == 0)
            {
                // random boss man
                BossPotLuck();

                // a reduced number of additional beasties, we're not hercules.
                CreateEnemyWaller(UnityEngine.Random.Range(0, 1));
                CreateEnemyCaterpillar(UnityEngine.Random.Range(0, 1));
                CreateEnemyShip(UnityEngine.Random.Range(0, 2));
            }
            else
            {
                CreateEnemyWaller(UnityEngine.Random.Range(0, 4));
                CreateEnemyCaterpillar(UnityEngine.Random.Range(0, 3));
                CreateEnemyShip(UnityEngine.Random.Range(0, 8));
                CreatePlanet(UnityEngine.Random.Range(0, 3));
            }
        }
    }



    public void ImagePlay(string[] InstanceParameters)
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

                        case "Resonance Cascade":
                            selectedSprite = GetSprite(x, y, 8);
                            break;

                        default:
                            selectedSprite = GetSprite(x, y, UnityEngine.Random.Range(0, 8));
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

        switch (pattern)
        {
            case 0:
                selectedSprite = spriteSelection[Random.Range(0, spriteSelection.Length)];
                break;

            case 1:
                selectedSprite = y % 2 == 0 ? spriteSelection[3] : (x % 2 == 0 ? spriteSelection[0] : spriteSelection[1]);
                break;

            case 2:
                selectedSprite = y % 2 == 0 ? spriteSelection[3] : (x % 2 == 0 ? spriteSelection[1] : spriteSelection[1]);
                break;

            case 3:
                selectedSprite = x % 2 == 0 ? spriteSelection[3] : (y % 2 == 0 ? spriteSelection[1] : spriteSelection[1]);
                break;

            case 4:
                selectedSprite = (x < 20 || x > 40) ? spriteSelection[5] : (x < 20 || x > 40) ? spriteSelection[2] : spriteSelection[1];
                break;

            case 5:
                int totalSections = 3;
                int sectionWidth = 64 / totalSections;
                selectedSprite = x >= sectionWidth && x < sectionWidth * 2 ? spriteSelection[5] : (x >= sectionWidth * 2 ? spriteSelection[6] : spriteSelection[4]);
                break;

            case 6:
                selectedSprite = spriteSelection[3];
                break;

            case 7:
                selectedSprite = spriteSelection[2];
                break;

            case 8:
                selectedSprite = spriteSelection[6];
                break;
        }

        return selectedSprite;
    }






    public void LevelPatternGUI()
    {

        string word1 = levelText1.text ?? " ";
        string word2 = levelText2.text ?? " ";
        string word3 = levelText3.text ?? " ";


        float yLevel = -1.0f;
        foreach (string word in new string[] { word3, word2, word1 })
        {
            WordPlay(word, 5, 0.7f, 15f, yLevel, 1f); // word, gridsize, segmentwidth, xstart, ystart, outputscale
            yLevel -= 2.5f;
        }


    }




    public void LevelPatternPhrase(string dword1 = " ", string dword2 = " ", string dword3 = " ")
    {

        string word1 = dword1 ?? " ";
        string word2 = dword2 ?? " ";
        string word3 = dword3 ?? " ";


        float yLevel = -1.0f;
        foreach (string word in new string[] { word3, word2, word1 })
        {
            WordPlay(word, 5, 0.7f, 15f, yLevel, 1f); // word, gridsize, segmentwidth, xstart, ystart, outputscale
            yLevel -= 2.5f;
        }


    }


    // enemy ship type 3

    public void CreateEnemyCaterpillar(int numships = 1)
    {

        for (int i = 0; i < numships; i++)
        {

            GameObject enemyShip = Instantiate(enemyPrefabs[2]);
            enemyShip.transform.parent = ringParent.transform;

        }
        // we will NOT increment AE here because the caterpillar is made up of multiple segments
        // so AE is incremented in the Start() method of CaterpillarMovement.cs on a per-segment basis.
        // GameMaster.instance.ActiveEnemies += numships;

    }

    // enemy ship type 2 

    public void CreateEnemyWaller(int numships = 1)
    {

        for (int i = 0; i < numships; i++)
        {

            GameObject enemyShip = Instantiate(enemyPrefabs[1]);
            enemyShip.transform.parent = ringParent.transform;

        }

        GameMaster.instance.ActiveEnemies += numships;

    }



    // enemy ship type 1

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
            enemyShip.transform.parent = gridParent.transform;

            // Calculate rotation towards the player and flip it
            Vector3 directionToPlayer = playerPosition - enemyPosition;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            angle += 180f; // Add 180 degrees to flip the direction
            enemyShip.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        ChainLightning.instance.InitialiseLightning();
        Debug.Log("enemy created");

        GameMaster.instance.ActiveEnemies += numships;
    }




    public void CreateBossOne()
    {


        GameObject bossBoat = Instantiate(bossPrefabs[0], bossParent.transform.position, Quaternion.identity);
        bossBoat.transform.parent = bossParent.transform;


        Debug.Log("boss created");

        GameMaster.instance.ActiveEnemies += 1;
    }


    public void CreateBossTwo()
    {


        GameObject bossBoat = Instantiate(bossPrefabs[1], bossParent.transform.position, Quaternion.identity);
        bossBoat.transform.parent = bossParent.transform;


        Debug.Log("boss created");

        GameMaster.instance.ActiveEnemies += 1;
    }


    public void CreateBossThree()
    {


        GameObject bossBoat = Instantiate(bossPrefabs[2], bossParent.transform.position, Quaternion.identity);
        bossBoat.transform.parent = bossParent.transform;


        Debug.Log("boss created");

        GameMaster.instance.ActiveEnemies += 1;
    }


    public void BossPotLuck()
    {

        int bossType = Random.Range(0, 3);

        switch (bossType)
        {
            case 0:
                CreateBossOne();
                break;

            case 1:
                CreateBossTwo();
                break;

            case 2:
                CreateBossThree();
                break;
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





    // planets

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


            planetPrefab.transform.parent = gridParent.transform;

        }
        Debug.Log("planet created");
        GameMaster.instance.ActiveEnemies += numplanets;

    }



    // nearby star field
    public void CreateNearbyStar(int numstars = 1)
    {
        for (int i = 0; i < numstars; i++)
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

            Vector3 starPosition = Vector3.zero;
            bool isValidPosition = false;
            int maxAttempts = 100;
            int attempts = 0;

            while (!isValidPosition && attempts < maxAttempts)
            {
                float randomPositionOffsetX = Random.Range(-maxPositionOffsetX, maxPositionOffsetX);
                float randomPositionOffsetY = Random.Range(-maxPositionOffsetY, maxPositionOffsetY);

                starPosition = parentPosition + new Vector3(randomPositionOffsetX, randomPositionOffsetY, 0f);

                if (!CheckOverlapWithPlayer(starPosition, playerPosition, playerSize))
                {
                    isValidPosition = true;
                }

                attempts++;
            }

            if (!isValidPosition)
            {
                return;
            }


            GameObject nearbyStarPrefab = Instantiate(nearbyStarPrefabs[Random.Range(0, nearbyStarPrefabs.Length)], starPosition, Quaternion.identity);


            nearbyStarPrefab.transform.parent = gridParent.transform;

        }
        Debug.Log("close star created");
    }




    // far away star field

    public void CreateFarStar(int numstars = 1)
    {
        for (int i = 0; i < numstars; i++)
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

            Vector3 starPosition = Vector3.zero;
            bool isValidPosition = false;
            int maxAttempts = 100;
            int attempts = 0;

            while (!isValidPosition && attempts < maxAttempts)
            {
                float randomPositionOffsetX = Random.Range(-maxPositionOffsetX, maxPositionOffsetX);
                float randomPositionOffsetY = Random.Range(-maxPositionOffsetY, maxPositionOffsetY);

                starPosition = parentPosition + new Vector3(randomPositionOffsetX, randomPositionOffsetY, 0f);

                if (!CheckOverlapWithPlayer(starPosition, playerPosition, playerSize))
                {
                    isValidPosition = true;
                }

                attempts++;
            }

            if (!isValidPosition)
            {
                return;
            }


            GameObject farStarPrefab = Instantiate(farStarPrefabs[Random.Range(0, farStarPrefabs.Length)], starPosition, Quaternion.identity);


            farStarPrefab.transform.parent = gridParent.transform;

        }
        Debug.Log("close star created");
    }











    public void ConcentricRings(int numRings = 3)
    {
        float innerRadius = 1.9f; // The inner radius of the ring.
        float outerRadius = 1.8f; // The outer radius of the ring.

        int numSegments = 28; // The number of segments in the ring.
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
                BoxCollider2D collider = segmentObject.AddComponent<BoxCollider2D>();

                // Set the size of the BoxCollider2D to match the size of the SpriteRenderer.
                collider.size = spriteRenderer.bounds.size;

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

            ringObject.transform.parent = ringParent.transform;
            ringObject.transform.position = ringParent.transform.position;
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

            float gapFacta = 0.8f;

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
                    // Vector3 position = new Vector3((col - (gridSize - 1) / 2.0f) * (segmentWidth / 2f), -row * (segmentWidth / 1.5f), 0.0f);
                    Vector3 position = new Vector3((col - (gridSize - 1) / 2.0f) * (segmentWidth / 2f) * gapFacta, -row * (segmentWidth / 1.5f), 0.0f);

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
                    emptySegment.transform.localPosition = new Vector3((col - (gridSize - 1) / 2.0f) * (segmentWidth / 2f) * gapFacta, -row * (segmentWidth / 1.5f), 0.0f);
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
        wordObject.transform.parent = gridParent.transform;


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








    public void CleanLevel()
    {
        GameMaster.instance.ChangeBG();

        // Loop through all child objects of the parent
        for (int i = gridParent.transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = gridParent.transform.GetChild(i).gameObject;

            // Destroy the child object
            Object.Destroy(child);
        }


        // Loop through all child objects of the parent
        for (int i = ringParent.transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = ringParent.transform.GetChild(i).gameObject;

            // Destroy the child object
            Object.Destroy(child);
        }

        // Loop through all child objects of the parent
        for (int i = bossParent.transform.childCount - 1; i >= 0; i--)
        {
            GameObject child = bossParent.transform.GetChild(i).gameObject;

            // Destroy the child object
            Object.Destroy(child);
        }

        if (GameTimer != null)
        {
            StopCoroutine(GameTimer);
            GameTimer = null;
        }
    }


}
