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
        int randTarget = Random.Range(0, asteroidTargets.Length);
        transform.position = asteroidTargets[randTarget].transform.position;
        SetRoidRotation(randTarget);
        SetSpeedAndVelocity();
    }

    public void SetSpeedAndVelocity()
    {
        float speed = Random.Range(0.2f, 2f);
        float velocity = Random.Range(0.2f, 2f);

        var mainModule = asteroidParticles.main;
        mainModule.startSpeed = speed;
        mainModule.startLifetime = velocity;
    }

    public void SetRoidRotation(int randTarget)
    {
        float randomAngle = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0, 0, randomAngle);
    }
}
