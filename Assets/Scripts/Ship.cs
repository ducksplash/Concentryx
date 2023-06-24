using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using RDG; // vibrate plugin
using UnityEngine.UI;
using TMPro;

public class Ship : MonoBehaviour
{
    public static Ship instance;


    public SpriteRenderer shipSprite;
    private Rigidbody2D rb;
    private AndroidJavaObject vibrator;

    public Animator splodanim;

    public CanvasGroup healthCanvas;

    public Sprite[] shipSpriteList;


    public Vector3 startPosition;

    public Coroutine gameOverCoroutine = null;

    public Button shipSelectButton1;
    public Button shipSelectButton2;
    public Button shipSelectButton3;

    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        shipSprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        startPosition = transform.position;

        SetSprite(PlayerPrefs.GetInt("ShipSprite", 0));

    }



    void Update()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            if (!GameMaster.instance.onMobile)
            {
                if (Time.timeScale > 0f)
                {
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 direction = mousePos - transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
                }
            }
        }
    }
    public void MoveShip(Vector2 direction)
    {
        if (Time.timeScale > 0f)
        {
            // Calculate the angle in degrees based on the direction vector
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Add an offset of 90 degrees to adjust for the rotation difference
            angle -= 90f;

            // Rotate the game object to the calculated angle
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

    }


    public void SetSprite(int selectedShipSprite = 0)
    {
        shipSprite.sprite = shipSpriteList[selectedShipSprite];
        PlayerPrefs.SetInt("ShipSprite", selectedShipSprite);
        PlayerPrefs.Save();

        int shipSpriteCode = PlayerPrefs.GetInt("ShipSprite", 0);
        shipSelectButton1.interactable = (shipSpriteCode == 0) ? false : true;
        shipSelectButton2.interactable = (shipSpriteCode == 1) ? false : true;
        shipSelectButton3.interactable = (shipSpriteCode == 2) ? false : true;


    }



    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (!GameMaster.instance.invulnerable)
        {

            if (collision.gameObject.name.Contains("EnemyProjectile") || collision.gameObject.name.Contains("BossProjectile"))
            {
                collision.gameObject.GetComponent<EnemyBulletTime>().DestroyGameObject();

                if (GameMaster.instance.health > 0)
                {

                    //
                    Vibration.Vibrate(50, 255);

                    StartCoroutine(FlashPlayer());
                    GameMaster.instance.DecrementHealth(1);
                }
                else
                {
                    DestroyShip();
                }
            }



            if (collision.gameObject.name.Contains("EnemyKamikaziDrone"))
            {

                if (GameMaster.instance.health > 0)
                {

                    //
                    Vibration.Vibrate(50, 255);

                    StartCoroutine(FlashPlayer());
                    GameMaster.instance.DecrementHealth(10);
                }
                else
                {
                    DestroyShip();
                }
            }



        }


        if (collision.gameObject.name.Contains("Segment"))
        {

            Destroy(collision.gameObject);
        }

    }



    public void DestroyShip()
    {

        GameMaster.instance.playerReady = false;

        Material material = shipSprite.material;

        // Get the current color
        Color color = material.color;

        // Set the new alpha value
        color.a = 0f;

        // Assign the modified color back to the material
        material.color = color;

        healthCanvas.alpha = 0f;


        splodanim.SetTrigger("shipsplode");

        if (gameOverCoroutine == null)
        {
            gameOverCoroutine = StartCoroutine(GameOver());
        }



    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSecondsRealtime(1f);
        StartCoroutine(GameMaster.instance.EndLevel(2));
        StartCoroutine(reposition());
        gameOverCoroutine = null;

    }

    public IEnumerator reposition()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Material material = shipSprite.material;

        // Get the current color
        Color color = material.color;

        // Set the new alpha value
        color.a = 1f;

        // Assign the modified color back to the material
        material.color = color;

        healthCanvas.alpha = 1f;
        GameMaster.instance.playerReady = true;
    }

    public IEnumerator FlashPlayer()
    {
        shipSprite.color = Color.red;

        yield return new WaitForSecondsRealtime(0.1f);
        shipSprite.color = Color.white;
    }

}
