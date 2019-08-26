using UnityEditor.SceneManagement;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending}
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        //todo stop sound on death
        if (state == State.Alive)
        {
            Rotate();
            Thrust();
        }
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextLevel", 1f); //peramiterize time
                break;
            default:
                print("hit something deadly");
                state = State.Dying;
                Invoke("LoadFirstLevel", 1f); //peramiterize time
                break;
        }
        
       
    }

    private void LoadNextLevel()
    {
        EditorSceneManager.LoadScene(1);// todo allow for more than 2 Levels
    }

    private void LoadFirstLevel()
    {
        EditorSceneManager.LoadScene(0);// todo allow for more than 2 Levels
    }

    private void Rotate()
    {
        rigidBody.angularVelocity = Vector3.zero;

        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))// can thrust while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }
}
