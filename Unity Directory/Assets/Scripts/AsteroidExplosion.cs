using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidExplosion : MonoBehaviour
{
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gameObject.name = "AsteroidExplosion";
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SelfDestruct());
    }


    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAudio(float volumeLevel, float pitchLevel)
    {
        audioSource.volume = volumeLevel;
        audioSource.pitch = pitchLevel;
    }
}
