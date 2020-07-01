using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameManager gameManager;
    public ObjectManager objectManager;
    private AudioManager audioManager;
    private SpriteRenderer spRender;

    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public PlayerController playerLogic;

    [Header("Information")]
    public string name;
    public string projectileName;
    private int score;
    public int health;
    [HideInInspector]
    public float speed;

    [Header("Shooter Information")]
    public Transform[] firePos;
    public float shotDelay;
    private float currentShotDelay;

    [Header("Projectile")]
    public float projectileSpeed = 1;

    [Header("Flash")]
    public Color flashColor;
    private Color originalColor = Color.white;

    private void Awake()
    {
        //1. Get Components
        gameManager = GameManager.instance;
        audioManager = AudioManager.instance;
        spRender = GetComponent<SpriteRenderer>();

        currentShotDelay = shotDelay;

        switch (name)
        {
            case "EnemyL":
                projectileName = "EnemyLBullet";
                score = 1000;
                health = 5;
                speed = 1f;
                break;
            case "EnemyM":
                projectileName = "EnemyMBullet";
                score = 700;
                health = 3;
                speed = 1.2f;
                break;
            case "EnemyS":
                projectileName = "EnemySBullet";
                score = 500;
                health = 2;
                speed = 0.7f;
                break;
        }
    }

    private void FixedUpdate()
    {
        if (gameManager.gameEnd) return;

        Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        if (targetScreenPos.x > Screen.width + 300f || targetScreenPos.x < 0 - 300f || targetScreenPos.y > Screen.height || targetScreenPos.y < 0 + 200f) gameObject.SetActive(false);

        //Shot
        currentShotDelay -= Time.deltaTime;

        Shoot();
    }

    //Shoot
    private void Shoot()
    {
        if (player != null && currentShotDelay <= 0)
        {
            for (int i = 0; i < firePos.Length; i++)
            {
                Vector3 dir = (player.transform.position - firePos[i].transform.position).normalized;
                float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) + 90;
                Quaternion bulletRot = Quaternion.Euler(new Vector3(0, 0, angle));

                GameObject bullet = objectManager.PlaceBullet(projectileName, transform, bulletRot);
                Rigidbody2D rigid2D = bullet.GetComponent<Rigidbody2D>();
                rigid2D.AddForce(dir * projectileSpeed, ForceMode2D.Impulse);
            }
            audioManager.PlaySound("EnemyShot");
            currentShotDelay = shotDelay;
        }
    }

    //Hit
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            OnHit(1);
            other.gameObject.SetActive(false);
            objectManager.PlacePlayerBulletEffect(other.GetComponent<Projectile>().fxName, other.transform, other.transform.rotation);
        }
    }

    //Damage
    public void OnHit(int _damage)
    {
        health -= _damage;
        if (health <= 0) { OnDeath(); }
        else
        {
            spRender.color = flashColor; Invoke("OriginalColor", 0.05f);
            audioManager.PlaySound("EnemyDamage");
        }
    }

    private void OriginalColor() { spRender.color = originalColor; }

    //Death
    private void OnDeath()
    {
        if (health > 0) return;

        objectManager.PlacePlayerBulletEffect("Death", transform, Quaternion.identity);
        audioManager.PlaySound("EnemyDeath");
        playerLogic.ScoreUpdate(score);
        ItemDrop();
        this.gameObject.SetActive(false);
    }

    //Item Drop
    private void ItemDrop()
    {
        int randomItemNum = Random.Range(0, 11);

        if (randomItemNum < 4) { return; }

        GameObject item = null;

        if (randomItemNum < 7) { item = objectManager.PlaceItem("Coin", transform, Quaternion.identity); }
        else if (randomItemNum < 8) { item = objectManager.PlaceItem("Power", transform, Quaternion.identity); }
        else if (randomItemNum < 9) { item = objectManager.PlaceItem("Bomb", transform, Quaternion.identity); }
        else if (randomItemNum < 10) { item = objectManager.PlaceItem("Recovery", transform, Quaternion.identity); }

        if (item != null)
        {
            Rigidbody2D rigid2D = item.GetComponent<Rigidbody2D>();
            rigid2D.AddForce(Vector2.down * 1.5f, ForceMode2D.Impulse);
        }
    }
}
