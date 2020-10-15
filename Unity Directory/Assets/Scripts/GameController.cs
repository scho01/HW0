using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public class GameController : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public GameObject spaceshipPrefab;
    public GameObject[] lifeIcons;
    private GameObject spaceship;
    private GameObject gameOverSign;
    private GameObject levelClearedSign;
    public int maxAsteroids = 1;
    public int numAsteroids;
    private int maxLives = 4;
    private int numLivesLeft;
    private float minCollisionRadius = 20;
    private float respawnTime = 3;
    private int myScore = 0;
    private Score scoreText;
    public float timeDied;
    private bool gameFinished = false;
    private bool gameWon = false;
    private float finishTime;
    private void Awake()
    {
        myScore = 0;
        scoreText = FindObjectOfType<Score>();
        scoreText.UpdateScore(myScore);
        numLivesLeft = maxLives;
        gameOverSign = GameObject.Find("GameOver");
        levelClearedSign = GameObject.Find("LevelCleared");
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        numAsteroids = maxAsteroids;
        for (int i = 0; i < numAsteroids; i++)
        {
            spawnAsteroid();
        }
        spawnSpaceship();
        Assert.IsNotNull(gameOverSign);
        gameOverSign.SetActive(false);
        Assert.IsNotNull(levelClearedSign);
        levelClearedSign.SetActive(false);
    }

    private void spawnAsteroid()
    {
        bool valid;
        GameObject newAsteroid;
        do
        {
            newAsteroid = Instantiate(asteroidPrefab);
            newAsteroid.GetComponent<Asteroid>().setGameController(this);
            valid = CheckProximity(newAsteroid);
        } while (valid == false);
    }

    private bool CheckProximity(GameObject testObject)
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        foreach(GameObject asteroid in asteroids)
        {
            if (asteroid != testObject)
            {
                if (Vector3.Distance(testObject.transform.position, asteroid.transform.position) < minCollisionRadius)
                {
                    Destroy(testObject);
                    return false;
                }
            }
        }
        return true;
    }

    private void spawnSpaceship()
    {
        bool valid;
        Assert.IsNull(spaceship);
        do
        {
            spaceship = Instantiate(spaceshipPrefab);
            valid = CheckProximity(spaceship);
        } while (valid == false);
        spaceship.GetComponent<Spaceship>().setGameController(this);
        numLivesLeft -= 1;
    }

    public void IncreaseScore()
    {
        myScore += 10;
        scoreText.UpdateScore(myScore);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (spaceship == null)
        {
            if (Time.time - timeDied > respawnTime)
            {
                if (numLivesLeft > 0)
                {
                    spawnSpaceship();
                    Destroy(lifeIcons[numLivesLeft]);
                }
                else
                {
                    gameOverSign.SetActive(true);
                }
            }
        }
        if ((numAsteroids == 0) && (gameWon == false))
        {
            if (gameFinished)
            {
                if (Time.time - finishTime > respawnTime)
                {
                    levelClearedSign.SetActive(true);
                    gameFinished = false;
                    gameWon = true;
                    StartCoroutine(Pause());
                }
            }
            else
            {
                gameFinished = true;
                finishTime = Time.time;
            }
        }
        Assert.IsTrue(numAsteroids >= 0);
    }

    IEnumerator Pause()
    {
        yield return new WaitForSeconds(3f);
        maxAsteroids = maxAsteroids * 2;
        if (maxAsteroids > 16)
        {
            maxAsteroids = 16;
        }
        Destroy(spaceship);
        spaceship = null;
        numLivesLeft++;
        InitializeLevel();
    }
}
