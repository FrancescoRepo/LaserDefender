using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float padding = 1f;
    [SerializeField] private float health = 500f;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip laserClip;
    [SerializeField] [Range(0, 1)] private float laserVolume = 0.25f;
    [SerializeField] [Range(0, 1)] private float deathVolume = 0.75f;

    [Header("Laser Settings")]
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float laserSpeed = 10f;
    [SerializeField] private float projectileFiringPeriod = 0.5f;

    private float xMin, yMin;

    private float xMax, yMax;

    private Coroutine firingCoroutine;

    private DamageDealer damageDealer;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);

        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinously());
        }

        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    private IEnumerator FireContinously()
    {
        while (true)
        {
            var laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
            AudioSource.PlayClipAtPoint(laserClip, Camera.main.transform.position, laserVolume);

            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        damageDealer = collision.gameObject.GetComponent<DamageDealer>();

        if (health > 0)
        {
            health -= damageDealer.GetDamage();
            damageDealer.Hit();
        }
        else if (health == 0)
        {
            Die();            
        }
        
    }

    private void Die()
    {
        FindObjectOfType<Level>().LoadGameOverScene();
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathClip, Camera.main.transform.position, deathVolume);
    }

    public float GetHealth()
    {
        return health;
    }
}
