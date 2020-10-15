using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;

    private float maxX = 92;
    private float maxY = 52;
    private float turnSpeed = 150;
    private float thrust = 10;
    private Vector3 shipDirection = new Vector3(0, 1, 0);
    private Rigidbody2D rb;
    private Collider2D cd;
    private float maxSpeed = 25;
    private float bulletSpeed = 200;
    private GameController gameController;
    private AudioSource audioSource;
    public AudioClip shootingSoundFX;
    public AudioClip thrustersFX;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gameObject.name = "Spaceship";
        gameObject.tag = "Spaceship";
        rb = GetComponent<Rigidbody2D>();
        transform.position = new Vector3(Random.Range(-maxX + 10, maxX - 10), Random.Range(-maxY + 10, maxY - 10), 0);
        cd = GetComponent<Collider2D>();
    }

    public void setGameController(GameController _gameController)
    {
        gameController = _gameController;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float turnAngle;
        if (Input.GetKey("j"))
        {
            turnAngle = turnSpeed * Time.deltaTime;
            transform.Rotate(0, 0, turnAngle);
            shipDirection = Quaternion.Euler(0, 0, turnAngle) * shipDirection;
        }
        if (Input.GetKey("l"))
        {
            turnAngle = -turnSpeed * Time.deltaTime;
            transform.Rotate(0, 0, turnAngle);
            shipDirection = Quaternion.Euler(0, 0, turnAngle) * shipDirection;
        }
        if (Input.GetKey("k"))
        {
            rb.AddForce(shipDirection * thrust);
        }
        if (Input.GetKeyDown("k"))
        {
            audioSource.clip = thrustersFX;
            audioSource.Play();
        }
        if (Input.GetKeyUp("k"))
        {
            audioSource.clip = thrustersFX;
            audioSource.Stop();
        }
        if (Input.GetKeyDown("space"))
        {
            audioSource.PlayOneShot(shootingSoundFX);
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation * Quaternion.Euler(0, 0, 90);
            bullet.GetComponent<Rigidbody2D>().velocity = shipDirection * bulletSpeed;
        }

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            gameController.timeDied = Time.time;
            GameObject explosion = Instantiate(explosionPrefab);
            explosion.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
