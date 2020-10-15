using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Asteroid : MonoBehaviour
{
    public GameObject asteroidExplosionPrefab;

    private Rigidbody2D rb;
    public GameObject asteroidPrefab;
    private GameController gameController;
    private float maxSpeed = 15;
    private float maxX = 102;
    private float maxY = 62;
    private int health = 1;

    public int scale;
    private int maxScale = 3;

    private float childAsteroidOffset = 10;
    public void setGameController(GameController _gameController)
    {
        gameController = _gameController;
    }

    private void Awake()
    {
        scale = maxScale;
        rb = GetComponent<Rigidbody2D>();
        gameObject.tag = "Asteroid";
        gameObject.name = "Asteroid";
        transform.position = new Vector3(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY), 0);
        rb.velocity = Quaternion.Euler(0, 0, Random.Range(0, 360)) * new Vector3(Random.Range(2, maxSpeed), 0, 0);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -maxX)
        {
            transform.position = new Vector2(maxX, transform.position.y);
        }
        if (transform.position.x > maxX)
        {
            transform.position = new Vector2(-maxX, transform.position.y);
        }
        if (transform.position.y < -maxY)
        {
            transform.position = new Vector2(transform.position.x, maxY);
        }
        if (transform.position.y > maxY)
        {
            transform.position = new Vector2(transform.position.x, -maxY);
        }
    }

    private void Die()
    {
        GameObject asteroidExplosion = Instantiate(asteroidExplosionPrefab);
        int scaleFactor = maxScale - scale;
        asteroidExplosion.GetComponent<AsteroidExplosion>().SetAudio(0.8f - scaleFactor * 0.25f, 1f + scaleFactor * 0.5f);
        asteroidExplosion.transform.position = transform.position;
        ParticleSystem partSys = asteroidExplosion.GetComponent<ParticleSystem>();
        partSys.Stop();
        var main = partSys.main;
        if ((scale < 3) && (scale > 0))
        {
            main.startSize = scale * 10;
        }
        else if (scale == 0)
        {
            main.startSize = 5;
        }

        main.simulationSpeed = 1 * (maxScale - scale + 1);
        partSys.Play();
        if (scale > 0)
        {
            spawnChildAsteroids();
            gameController.numAsteroids += 4;
        }
        Destroy(gameObject);
        gameController.numAsteroids -= 1;
    }

    private void spawnChildAsteroids()
    {
        Vector2[] newDirection = new Vector2[4];
        newDirection[0] = new Vector2(1, 0);
        newDirection[1] = new Vector2(0, 1);
        newDirection[2] = new Vector2(-1, 0);
        newDirection[3] = new Vector2(0, -1);

        float randAngle = Random.Range(0, 360);
        for (int i = 0; i < 4; i++)
        {
            GameObject newAsteroid = Instantiate(asteroidPrefab);
            newAsteroid.GetComponent<Asteroid>().setGameController(gameController);
            Asteroid asteroidHandle = newAsteroid.GetComponent<Asteroid>();
            newDirection[i] = Quaternion.Euler(0, 0, randAngle + Random.Range(-30, 30)) * newDirection[i];
            newAsteroid.transform.position = transform.position + (Vector3)(newDirection[i] * childAsteroidOffset);
            newAsteroid.transform.localScale = transform.localScale / 2;
            asteroidHandle.scale = scale - 1;
            asteroidHandle.childAsteroidOffset = childAsteroidOffset / 2;

            Rigidbody2D childRb = newAsteroid.GetComponent<Rigidbody2D>();
            childRb.mass = rb.mass / 8;
            childRb.AddForce((Vector3)newDirection[i] * childAsteroidOffset * childAsteroidOffset / 4);
        }
    }

    public void takeDamage()
    {
        health -= 1;
        if (health == 0)
        {
            Die();
            gameController.IncreaseScore();
        }
    }
}
