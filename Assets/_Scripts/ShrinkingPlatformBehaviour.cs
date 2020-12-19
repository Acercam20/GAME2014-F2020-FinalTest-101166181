using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkingPlatformBehaviour : MonoBehaviour
{
    public float shrinkingSpeed = 0.1f;
    private float timer = 0;
    private Vector3 startingScale;
    public bool shrinking = false;
    public AudioClip shrinkingSfx;
    public AudioClip growingSfx;
    public AudioSource audioSource;
    private float scaleSize = 1;
    private int bobbing = 0;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Bobbing", 0.5f, 0.15f);
        startingScale = gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

        if (shrinking && (Time.time > timer || Time.time - 0.5f > timer) && scaleSize >= 0)
        {
            timer = Time.time + shrinkingSpeed;
            gameObject.transform.localScale = new Vector3(scaleSize -= 0.1f, startingScale.y, startingScale.z);
        }
        else if (!shrinking && (Time.time > timer || Time.time - 0.5f > timer) && scaleSize <= startingScale.x)
        {
            timer = Time.time + shrinkingSpeed;
            gameObject.transform.localScale = new Vector3(scaleSize += 0.1f, startingScale.y, startingScale.z);
        }

        if (gameObject.transform.localScale.x > startingScale.x)
        {
            scaleSize = startingScale.x;
        }
        else if (gameObject.transform.localScale.x < 0)
        {
            shrinking = false;
            if (audioSource.clip != growingSfx)
            {
                audioSource.clip = growingSfx;
                audioSource.Play();
            }
            scaleSize = 0;
        }
    }

    private void Bobbing()
    {
            //timer = Time.time + shrinkingSpeed;
            if (bobbing == 0)
            {
                bobbing++;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.1f, gameObject.transform.position.z);
            }
            else if (bobbing == 1)
            {
                bobbing++;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.1f, gameObject.transform.position.z);
            }
            else if (bobbing == 2)
            {
                bobbing++;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 0.1f, gameObject.transform.position.z);
            }
            else if (bobbing == 3)
            {
                bobbing = 0;
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.1f, gameObject.transform.position.z);
            }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            shrinking = true;
            audioSource.clip = shrinkingSfx;
            audioSource.Play();
        }
    }

    private void OnCollisionExit2D(Collision2D collision) //Does not work
    {
        if (collision.gameObject.tag == "Player")
        {
            shrinking = false;
            audioSource.clip = growingSfx;
            audioSource.Play();
        }
    }
}
