using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    [SerializeField]float rcsThrust = 100f;
    [SerializeField]float mainThrust = 1;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip win;
    [SerializeField] AudioClip death;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem winParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] float levelLoadDelay = 2f;

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

        if(state != State.Alive) { return; }

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
        deathParticles.Play();
        Invoke("ReturnToBeginning", levelLoadDelay);
    }

    private void startSuccessSequence()
    {
        state = State.Transcending;

        audioSource.PlayOneShot(win);
        winParticles.Play();
        Invoke("LoadNextScene", levelLoadDelay);
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
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
        if (state == State.Dying)
        {
            audioSource.Stop();
        }
    }
}
