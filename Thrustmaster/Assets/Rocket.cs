using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    [SerializeField]float rcsThrust = 100f;
    [SerializeField]float mainThrust = 1;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip win;
    [SerializeField] AudioClip death;
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
           RespondToRotateInput();
           RespondToThrustInput();
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
                startSuccessSequence();
                break;
            default:
                startDeathSequence();
                break;
        }
    }

    private void startDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        Invoke("ReturnToBeginning", 1f);
    }

    private void startSuccessSequence()
    {
        state = State.Transcending;

        audioSource.PlayOneShot(win);
        Invoke("LoadNextScene", 1f);
    }

    private void ReturnToBeginning()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    void RespondToRotateInput()
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

    private void RespondToThrustInput()
    {
        if (state != State.Alive)
        {
            return;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            audioSource.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidbody.AddRelativeForce(Vector3.up * mainThrust);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (state == State.Dying)
        {
            audioSource.Stop();
        }
    }
}
