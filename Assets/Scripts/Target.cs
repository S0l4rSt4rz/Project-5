using Unity.Hierarchy;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Target : MonoBehaviour
{
    private Rigidbody targetRb;
    private float minSpeed = 10;
    private float maxSpeed = 16;
    private float maxTorque = 6;
    private float xRange = 4;
    private float ySpawnPos = -2;
    private GameManager gameManager;
    public int pointValue;
    public ParticleSystem explosionParticle;
    public AudioClip fredSound;
    private AudioSource fredSpeaker;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
        transform.position = RandomSpawnPos();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        fredSpeaker = GameObject.Find("Game Manager").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Mouse was clicked");   
            Ray ray =
Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // If ray hits this enemy, destroy it
                if (hit.transform == transform)
                if (gameManager.isGameActive)
                {
                    fredSpeaker.PlayOneShot(fredSound, 0.2f);
                    Destroy(gameObject);
                    Instantiate(explosionParticle, transform.position,
                    explosionParticle.transform.rotation);
                    gameManager.UpdateScore(pointValue);
                }
            }
            }    }
    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }
    float RandomTorque()       
    {
        return Random.Range(-maxTorque, maxTorque);
    }
    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("DestroyZone"))
        {   
            Destroy(gameObject);
            if (!gameObject.CompareTag("Bad")) 
            { 
                gameManager.GameOver(); 
            }
        }
    }
}
