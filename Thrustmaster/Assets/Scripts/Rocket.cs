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


    bool enableCollisions = true;

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
           respondToDebugKeys();
           RespondToRotateInput();
           RespondToThrustInput();
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        if(state != State.Alive || !enableCollisions) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
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
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex += 1;
        SceneManager.LoadScene(nextSceneIndex);
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings){
            // nextSceneIndex = 0;
            SceneManager.LoadScene(0);
        }
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
    void respondToDebugKeys(){
            if(Debug.isDebugBuild){
                if(Input.GetKeyDown(KeyCode.L)){
                    LoadNextScene();
                }else if(Input.GetKeyDown(KeyCode.C)){
                    enableCollisions = !enableCollisions;
                }
           }
    }
}
