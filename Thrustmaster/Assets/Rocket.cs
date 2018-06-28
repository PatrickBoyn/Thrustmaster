using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    [SerializeField]float rcsThrust = 100f;
    [SerializeField]float mainThrust = 1; 
    Rigidbody rigidbody;
    AudioSource audioSource; 

    enum State {Alive, Dying, Transcending}
    State state = State.Alive;

	// Use this for initializationa
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (state == State.Alive)
        {
           Rotate();
           Thrust();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                    print("Friendly");
                break;
            case "Finish":
                state = State.Transcending;
                print("Change level.");
                Invoke("LoadNextScene", 1f);
                break;
            default:
                state = State.Dying;
                print("Enemy");
                Invoke("ReturnToBeginning", 1f);
                break;
        }
    }

    private void ReturnToBeginning()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    void Rotate()
    {
        rigidbody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A))
        {
            float rotationThisFrame = rcsThrust * Time.deltaTime;
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            float rotationThisFrame = rcsThrust * Time.deltaTime;
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidbody.freezeRotation = false;
    }

    private void Thrust()
    {
        if (state != State.Alive)
        {
            return;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            rigidbody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            if(state == State.Dying)
            {
                audioSource.Stop();
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            audioSource.Stop();
        }
    }
}
