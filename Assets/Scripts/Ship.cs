using UnityEngine;

public class Ship : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.name.Contains("EnemyProjectile"))
        {
            collision.gameObject.GetComponent<BulletTime>().DestroyGameObject();

            if (GameMaster.instance.health > 0)
            {
                GameMaster.instance.DecrementHealth(3);
            }
            else
            {
                //Debug.Log("Game Over!");
            }
        }

    }

}
