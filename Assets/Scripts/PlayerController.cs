using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private float elapsedTime = 0f;
    public UIDocument uiDocument;
    private Label scoreText;

    private float score = 0f;
    public float scoreMultiplier = 10f;

    public float maxSpeed = 5f;
    public float thrustForce = 1f;

    public GameObject boosterFlame;
    public GameObject explosionEffect; // Assign your explosion prefab here in the Inspector
    Rigidbody2D rb;

    void Start()
    {
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        scoreText.text = "Score: " + score;

        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            // Ensure Z-axis doesn't mess up 2D calculations
            mousePos.z = 0;
            Vector2 direction = (mousePos - transform.position).normalized;

            transform.up = direction;
            rb.AddForce(direction * thrustForce);

            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            boosterFlame.SetActive(true);
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            boosterFlame.SetActive(false);
        }
    }

    // This triggers when the player hits another collider
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. Check if the prefab is assigned
        if (explosionEffect != null)
        {
            // 2. Spawn the explosion at the player's current position and rotation
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        // 3. Destroy the player object
        Destroy(gameObject);
    }
}