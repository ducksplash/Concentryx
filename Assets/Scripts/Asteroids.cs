using System.Collections;
using UnityEngine;

public class Asteroids : MonoBehaviour
{

    public GameObject[] asteroidTargets;

    public ParticleSystem asteroidParticles;

    private void Start()
    {
        MakeRoids();
    }

    public void MakeRoids()
    {

        int randTarget = Random.Range(0, 8);

        // Set the new position for the object
        transform.position = asteroidTargets[randTarget].transform.position;
        SetRoidRotation(randTarget);

        SetSpeedAndVelocity();
    }



    public void SetSpeedAndVelocity()
    {
        // set the speed and velocity to random values between 0.2f and 1f

        float speed = Random.Range(0.2f, 2f);
        float velocity = Random.Range(0.2f, 2f);

        asteroidParticles.startSpeed = speed;
        asteroidParticles.startLifetime = velocity;



    }



    public void SetRoidRotation(int randTarget = 1)
    {
        switch (randTarget)
        {
            case 0:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 1:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 2:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case 3:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case 4:
                transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
            case 5:
                transform.rotation = Quaternion.Euler(0, 0, 140);
                break;
            case 6:
                transform.rotation = Quaternion.Euler(0, 0, 45);
                break;
            case 7:
                transform.rotation = Quaternion.Euler(0, 0, 120);
                break;
            case 8:
                transform.rotation = Quaternion.Euler(0, 0, 225);
                break;
        }
    }



}
