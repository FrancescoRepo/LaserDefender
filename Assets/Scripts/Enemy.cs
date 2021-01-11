using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int Health = 500;
    [SerializeField] private float minTimeWaitShoot = 0.2f;
    [SerializeField] private float maxTimeWaitShoot = 3f;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float laserSpeed = -10f;
    [SerializeField] private GameObject explosionParticles;
    [SerializeField] private AudioClip dieClip;
    [SerializeField] private AudioClip laserClip;
    [SerializeField] [Range(0, 1)] private float deathVolume = 0.75f;
    [SerializeField] [Range(0, 1)] private float laserVolume = 0.14f;
    [SerializeField] private int scoreToAdd = 150;

    private DamageDealer damageDealer;

    private float shotCounter;

    private GameSession currentSession;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeWaitShoot, maxTimeWaitShoot);
        currentSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        CountAndShoot();
    }

    private void CountAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeWaitShoot, maxTimeWaitShoot);
        }
    }

    private void Fire()
    {
        Vector3 laserPosition = transform.position;
        laserPosition.y += 3f;

        var laser = Instantiate(laserPrefab, laserPosition, Quaternion.identity);
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
        AudioSource.PlayClipAtPoint(laserClip, Camera.main.transform.position, laserVolume);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        damageDealer.Hit();

        if (Health > 0)
            Health = Health - damageDealer.GetDamage();
        else if (Health == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        currentSession.AddScore(scoreToAdd);
        Destroy(gameObject);
        var explosion = Instantiate(explosionParticles, transform.position, Quaternion.identity);
        Destroy(explosion, 1f);
        AudioSource.PlayClipAtPoint(dieClip, Camera.main.transform.position, deathVolume);
    }
}
