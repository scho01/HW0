using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float maxX = 120;
    private float maxY = 70;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position.x < -maxX) ||
            (transform.position.x > maxX) ||
            (transform.position.y < -maxY) ||
            (transform.position.y > maxY))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Asteroid")
        {
            Asteroid asteroid = collision.GetComponent<Asteroid>();
            asteroid.takeDamage();
            Destroy(gameObject);
        }
    }
}
