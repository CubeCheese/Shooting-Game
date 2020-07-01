using UnityEngine;

//Boss Movement
public class BossController : MonoBehaviour
{
    private GameManager gameManager;
    [HideInInspector]
    public ObjectManager objectManager;
    private AudioManager audioManager;
    private SpriteRenderer spRender;
    private Rigidbody2D rigid2D;

    [HideInInspector]
    public Transform player;
    [HideInInspector]
    public PlayerController playerLogic;

    [Header("Information")]
    public string name;
    private string projectileAName;
    private string projectileBName;
    public int score;
    public int health;
    public int maxHealth;
    public float speed;

    [Header("Shooter Information")]
    public Transform[] firePos;
    public float shotDelay;
    private float currentShotDelay;

    [Header("Projectile")]
    public float projectileSpeed = 1;

    public int patternIndex;
    public int[] patternMaxCount;

    [Header("Flash")]
    public Color flashColor;
    private Color originalColor;

    private void Awake()
    {
        //1. Get Components
        gameManager = GameManager.instance;
        audioManager = AudioManager.instance;
        spRender = GetComponent<SpriteRenderer>();
        rigid2D = GetComponent<Rigidbody2D>();

        //2. Initialation Value
        originalColor = Color.white;
    }

    //Bullet Name Setting
    private void OnEnable()
    {
        currentShotDelay = shotDelay;

        projectileAName = "BossBulletA";
        projectileBName = "BossBulletB";

        health = maxHealth;

        Invoke("Stop", 0.5f);
    }

    //Speed Stop
    private void Stop()
    {
        if (!gameObject.activeSelf)
            return;

        rigid2D.velocity = Vector3.zero;
        Invoke("Think", 2f);
    }

    //AI
    void Think()
    {
        if (health <= 0 || gameManager.gameEnd) return;

        int random;
        patternIndex = 0;

        if (health > maxHealth * 0.5f)
        {
            if (player.position.x >= firePos[1].position.x || player.position.x >= firePos[4].position.x) { random = Random.Range(0, 2); }
            else { random = Random.Range(1, 3); }

        }
        else
        {
            if (player.position.x >= firePos[1].position.x || player.position.x >= firePos[4].position.x) { random = Random.Range(2, 5); }
            else { random = Random.Range(2, 6); }
        }

        switch (random)
        {
            case 0:
                Invoke("FireFoward", 0.5f);
                break;
            case 1:
                Invoke("FireTarget", 0.5f);
                break;
            case 2:
                Invoke("FireArc", 0.5f);
                break;
            case 3:
                Invoke("FireRotate", 0.5f);
                break;
            case 4:
                Invoke("FireArround", 0.5f);
                break;
        }
    }

    //Foward Shot(4)
    void FireFoward()
    {
        for (int index = 1; index < 5; index++)
        {
            if (health <= 0) break;
            GameObject bullet = objectManager.PlaceBullet(projectileAName, firePos[index].transform, firePos[index].rotation);
            Rigidbody2D rigid2D = bullet.GetComponent<Rigidbody2D>();
            rigid2D.AddForce(Vector2.down * projectileSpeed * 2, ForceMode2D.Impulse);
        }
        audioManager.PlaySound("BossShot");
        patternIndex++;

        if (patternIndex < patternMaxCount[0]) { Invoke("FireFoward", 0.5f); }
        else { Invoke("Think", 1f); }
    }

    //Player Position Shot(4)
    void FireTarget()
    {
        for (int index = 0; index < 4; index++)
        {
            if (health <= 0) break;
            GameObject bullet = objectManager.PlaceBullet(projectileAName, firePos[0].transform, Quaternion.identity);
            Rigidbody2D rigid2D = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = player.position - transform.position;
            Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 0.5f));
            dirVec += ranVec;
            rigid2D.AddForce(dirVec.normalized * projectileSpeed, ForceMode2D.Impulse);
        }
        audioManager.PlaySound("BossShot");
        patternIndex++;

        if (patternIndex < patternMaxCount[1]) { Invoke("FireTarget", 0.5f); }
        else { Invoke("Think", 1f); }
    }

    //Left -> Right  -> Left
    void FireArc()
    {
        if (health <= 0) return;

        GameObject bullet = objectManager.PlaceBullet(projectileAName, firePos[0].transform, Quaternion.identity);
        Rigidbody2D rigid2D = bullet.GetComponent<Rigidbody2D>();
        Vector2 dirVec = new Vector2(Mathf.Sin(Mathf.PI * 20 * patternIndex / patternMaxCount[2]), -1f);
        rigid2D.AddForce(dirVec.normalized * projectileSpeed, ForceMode2D.Impulse);
        Vector3 rotVec = new Vector2(Mathf.Sin(Mathf.PI * 20 * patternIndex / patternMaxCount[2]), -1f);
        bullet.transform.Rotate(rotVec);

        audioManager.PlaySound("BossShot");
        patternIndex++;
        if (patternIndex < patternMaxCount[2]) { Invoke("FireArc", 0.2f); }
        else { Invoke("Think", 1f); }
    }

    //Entire Place
    void FireRotate()
    {
        int roundNum = patternIndex % 2 == 0 ? 12 : 17;

        for (int index = 0; index < roundNum; index++)
        {
            if (health <= 0) break;

            GameObject bullet = objectManager.PlaceBullet(projectileBName, firePos[0].transform, Quaternion.identity);
            Rigidbody2D rigid2D = bullet.GetComponent<Rigidbody2D>();

            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * index / roundNum), Mathf.Sin(Mathf.PI * 2 * index / roundNum));
            rigid2D.AddForce(dirVec.normalized * projectileSpeed, ForceMode2D.Impulse);

            Vector3 rotVec = Vector3.forward * 360 * index / roundNum + Vector3.forward * 90;
            bullet.transform.Rotate(rotVec);
        }
        audioManager.PlaySound("BossShot");
        patternIndex++;

        if (patternIndex < patternMaxCount[3]) { Invoke("FireRotate", 0.3f); }
        else { Invoke("Think", 1f); }
    }

    //Entire Place
    void FireArround()
    {
        int roundNumA = 40;
        int roundNumB = 35;
        int roundNum = patternIndex % 2 == 0 ? roundNumA : roundNumB;

        for (int index = 0; index < roundNum; index++)
        {
            if (health <= 0) break;

            GameObject bullet = objectManager.PlaceBullet(projectileBName, firePos[0].transform, Quaternion.identity);
            Rigidbody2D rigid2D = bullet.GetComponent<Rigidbody2D>();
            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * index / roundNum), Mathf.Sin(Mathf.PI * 2 * index / roundNum));
            rigid2D.AddForce(dirVec.normalized * projectileSpeed, ForceMode2D.Impulse);

            Vector3 rotVec = Vector3.forward * 360 * index / roundNum + Vector3.forward * 90;
            bullet.transform.Rotate(rotVec);
        }
        audioManager.PlaySound("BossShot");
        patternIndex++;

        if (patternIndex < patternMaxCount[4]) { Invoke("FireArround", 0.5f); }
        else { Invoke("FireArc", 1f); }
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
        gameManager.BossLifeIconUpdate(); 

        if (health <= 0) { OnDeath(); }
        else
        {
            gameManager.BossSp(1);
            spRender.color = flashColor;
            Invoke("OriginalColor", 0.05f);
            audioManager.PlaySound("BossDamage"); }
    }

    private void OriginalColor() { gameManager.BossSp(0); spRender.color = originalColor; }

    //Death
    private void OnDeath()
    {
        if (health > 0) return;

        objectManager.PlacePlayerBulletEffect("Death", transform, Quaternion.identity);
        audioManager.PlaySound("BossDeath");
        playerLogic.ScoreUpdate(score);
        ItemDrop();
        gameManager.GameClear();
    }

    private void ItemDrop()
    {
        for (int num = 0; num < 10; num++)
        {
            GameObject item = objectManager.PlaceItem("Final", firePos[0].transform , Quaternion.identity);
        }

        this.gameObject.SetActive(false);
    }
}
