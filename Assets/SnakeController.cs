using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SnakeController : MonoBehaviour
{
    public float speed;
    public float rotateSpeed;
    public GameObject bodyPartPrefab;
    public GameObject foodPrefab;
    public GameObject enemySnakePrefab;
    public CircleCollider2D collider2D;
    public Rigidbody2D rigidbody2D;
    public TextMeshProUGUI scoreText;
    [HideInInspector]
    public GameController gameController;

    float leftConstraint;
    float rightConstraint;
    float topConstraint;
    float bottomConstraint;
    bool canMove = false;
    bool collisionsEnabled = false;
    int score = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        leftConstraint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.0f, 0.0f, 0.0f)).x;
        rightConstraint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 1.0f, 0.0f, 0.0f)).x;
        bottomConstraint = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, Screen.height * 0.0f, 0.0f)).y;
        topConstraint = Camera.main.ScreenToWorldPoint(new Vector3(0.0f, Screen.height * 1.0f, 0.0f)).y;
        InvokeRepeating("AddBodyPart", 0f, 0.005f);
        Invoke("EnableMovement", 0.5f);
        Invoke("EnableCollision", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
    }

    void FixedUpdate() {
        if (canMove) {
            var horizontal = Input.GetAxis("Horizontal");
            if (horizontal > 0)
            {
                transform.Rotate(0, 0, -1 * rotateSpeed * Time.deltaTime, Space.Self);
            }
            else if (horizontal < 0)
            {
                transform.Rotate(0, 0, 1 * rotateSpeed * Time.deltaTime, Space.Self);
            }
        }
    }

    void EnableMovement() {
        canMove = true;
    }

    void EnableCollision() {
        collider2D.enabled = true;
        rigidbody2D.simulated = true;
        collisionsEnabled = true;
    }

    void AddBodyPart() {
        var bodyPart = Instantiate(bodyPartPrefab, transform.position, transform.rotation).GetComponent<BodyPart>();
        if (collisionsEnabled) {
            bodyPart.EnableCollision();
        }
    }

    void OnBecameInvisible() {
        if (transform.position.x >= rightConstraint) {
            transform.position = new Vector2(leftConstraint, transform.position.y);
        }
        else if (transform.position.x <= leftConstraint) {
            transform.position = new Vector2(rightConstraint, transform.position.y);
        }
        if (transform.position.y <= bottomConstraint) {
            transform.position = new Vector2(transform.position.x, topConstraint);
        }
        else if (transform.position.y >= topConstraint) {
            transform.position = new Vector2(transform.position.x, bottomConstraint);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (enemySnakePrefab != null)
        {
            if (col.tag == "Food")
            {
                score++;
                scoreText.text = $"SCORE: {score}";
                AddEnemySnake();
                Destroy(col.gameObject);
                var newX = Random.Range(leftConstraint + 1, rightConstraint - 1);
                var newY = Random.Range(bottomConstraint + 1, topConstraint - 1);
                var pos = new Vector3(newX, newY, 1f);
                Instantiate(foodPrefab, pos, Quaternion.identity);
            }
        }
    }

    void AddEnemySnake() {
        var enemy = Instantiate(enemySnakePrefab, transform.position, transform.rotation);
        enemy.transform.RotateAround(enemy.transform.position, enemy.transform.up, 180f);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (gameController != null) {
            gameController.GameOver();
        }
        Destroy(gameObject);
    }
}
