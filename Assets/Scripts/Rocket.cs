using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rotSpeed = 100f;
    [SerializeField] float flySpeed = 100f;
    [SerializeField] AudioClip flySound;
    [SerializeField] AudioClip finishSound;
    [SerializeField] AudioClip deathSound;

    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem flyParticles;
    [SerializeField] ParticleSystem finishParticles;


    enum State {
        Playing,
        Dead,
        NextLevel
    };

    State state = State.Playing;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Playing;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        if (state == State.Playing)
        {
            Launch();
            Rotation();
        }
        
    }

    void OnCollisionEnter(Collision collision)
    {

        if (state != State.Playing)
        {
            return;
        }

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                print("ok");
                break;
            case "Battery":
                print("Charge");
                break;
            case "Finish":
                Finish();
                break;
            default:
                Lose();
                break;
        }
    }


    void Finish()
    {
        state = State.NextLevel;
        audioSource.Stop();
        audioSource.PlayOneShot(finishSound);
        finishParticles.Play();
        Invoke(nameof(NextLevel), 2f);
    }

    void Lose()
    {
        state = State.Dead;
        audioSource.Stop();
        flyParticles.Stop();
        deathParticles.Play();
        audioSource.PlayOneShot(deathSound);
        Invoke(nameof(LoadFirstLevel), 2f);
    }

    void NextLevel()
    {
        SceneManager.LoadScene(1);

    }

    void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    void Rotation()
    {
        float rotationSpeed = rotSpeed * Time.deltaTime;

        rigidBody.freezeRotation = true;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);

        } else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }

        rigidBody.freezeRotation = false;
    }


    void Launch()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * flySpeed * Time.deltaTime);
            flyParticles.Play();

            if (!audioSource.isPlaying) audioSource.PlayOneShot(flySound);

        }
        else
        {
            flyParticles.Stop();
            audioSource.Pause();
        }
    }

}
