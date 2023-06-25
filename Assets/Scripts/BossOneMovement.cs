using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossOneMovement : MonoBehaviour
{
    public enum Orientation
    {
        Top,
        Bottom,
        Left,
        Right
    }

    public float durationInSeconds = 10f;
    public float timeToWait = 2f;
    public Color[] travelColors;
    public Color[] idleColors;
    public GameObject bossShip;
    public SpriteRenderer shipSpriteRenderer;
    public GameObject[] targetPositions;
    public CanvasGroup enemyHealthbarcanvas1;
    public CanvasGroup enemyHealthbarcanvas2;
    public GameObject Player;

    private Quaternion initialRotation;
    private bool isRunning = false;
    private bool isLerpingForward = true;
    private float colourTime = 0f;
    private float colourDuration = 0.2f;
    private EnemyProjectile enemyProjectileScript;
    private bool inMotion = false;

    private void Start()
    {
        initialRotation = bossShip.transform.rotation;
        Player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(BossBoatRoutine());

        foreach (GameObject targetPosition in targetPositions)
            targetPosition.GetComponent<SpriteRenderer>().color = Color.clear;

        shipSpriteRenderer = bossShip.GetComponent<SpriteRenderer>();
        enemyProjectileScript = GetComponentInChildren<EnemyProjectile>();
    }

    private void Update()
    {
        if (Player != null)
        {
            Vector3 directionToPlayer = Player.transform.position - bossShip.transform.position;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            bossShip.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        if (isLerpingForward)
            colourTime += Time.deltaTime / colourDuration;
        else
            colourTime -= Time.deltaTime / colourDuration;

        if (colourTime >= 1f || colourTime <= 0f)
            isLerpingForward = !isLerpingForward;

        if (inMotion)
        {
            Color targetColor = isLerpingForward ? travelColors[1] : travelColors[0];
            shipSpriteRenderer.color = Color.Lerp(travelColors[0], targetColor, colourTime);
            enemyHealthbarcanvas1.alpha = 0;
            enemyHealthbarcanvas2.alpha = 0;
            enemyProjectileScript.enabled = false;
        }
        else
        {
            Color targetColor = isLerpingForward ? idleColors[1] : idleColors[0];
            shipSpriteRenderer.color = Color.Lerp(idleColors[0], targetColor, colourTime);
            enemyHealthbarcanvas1.alpha = 1;
            enemyHealthbarcanvas2.alpha = 1;
            enemyProjectileScript.enabled = true;
        }
    }

    private IEnumerator BossBoatRoutine()
    {
        while (true)
        {
            if (!isRunning)
            {
                isRunning = true;
                yield return StartCoroutine(ShipFloatAway());
                isRunning = false;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator ShipFloatAway()
    {
        for (int i = 0; i < targetPositions.Length; i++)
        {
            Vector3 startPosition = bossShip.transform.position;
            Vector3 targetPosition = targetPositions[i].transform.position;
            bossShip.transform.position = startPosition;
            inMotion = true;
            yield return StartCoroutine(MoveObject(bossShip, startPosition, targetPosition, durationInSeconds));
            inMotion = false;
            yield return new WaitForSeconds(timeToWait);
        }
    }

    private IEnumerator MoveObject(GameObject objectToMove, Vector3 startPosition, Vector3 endPosition, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            objectToMove.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        objectToMove.transform.position = endPosition;
    }
}
