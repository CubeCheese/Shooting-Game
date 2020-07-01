using UnityEngine;
using System.Collections;

//Player Controller
public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;
    public ObjectManager objectManager;
    private AudioManager audioManager;

    [HideInInspector]
    public bool canMove;
    [HideInInspector]
    public bool isInvincibility;

    private Animator anim;
    private SpriteRenderer spRender;
    private Rigidbody2D rigid2d;

    private Vector3 movement;

    [Header("Information")]
    public int health;
    private int currentHealth;
    public float speed;

    [Header("Flash")]
    public Color flashColor;
    private Color originalColor = Color.white;

    [Header("Item")]
    public int maxPower;
    private int power;
    public int maxBomb;
    private int bomb;
    private int score;

    [Header("Follower")]
    public GameObject[] followers;
    private Transform crosshair;

    [Header("Weapon")]
    public Transform weaponHold;

    public Shooter[] allShooters;
    private Shooter equippedShooter;

    private void Awake()
    {
        //1. Get Components
        gameManager = GameManager.instance;
        audioManager = AudioManager.instance;
        anim = GetComponent<Animator>();
        spRender = GetComponent<SpriteRenderer>();
        rigid2d = GetComponent<Rigidbody2D>();
        crosshair = GetComponentInChildren<Crosshair>().transform;
    }

    private void OnEnable()
    {
        Invoke("Stop", 1.5f);
        canMove = false;
        isInvincibility = false;

        currentHealth = health;
        power = 0;
        bomb = 0;
        score = 0;

        //Follower
        for (int index = 0; index < followers.Length; index++)
        {
            followers[index].GetComponent<Follower>().objectManager = objectManager;
            followers[index].SetActive(false);
        }

        //Shooter
        EquipShooter(0);
        equippedShooter.SettingFollower(followers);
        equippedShooter.Component(objectManager, GetComponentInChildren<Crosshair>().transform);
    }

    //Stop Auto Movement
    private void Stop()
    {
        if (!gameObject.activeSelf)
            return;

        rigid2d.velocity = Vector3.zero;
        canMove = true;
    }

    private void FixedUpdate()
    {
        if (!canMove || gameManager.gameEnd) return;

        Movement();
        Cheat();
    }

    private void Cheat()
    {
        if (Input.GetKeyDown(KeyCode.Q)) { isInvincibility = true; }
    }

    //Movements
    private void Movement()
    {
        //1. Check Pressing Keyboard 
        float xPos = Input.GetAxisRaw("Horizontal");
        float yPos = Input.GetAxisRaw("Vertical");

        movement.Set(xPos, yPos, 0);

        if (movement == Vector3.zero) return;

        //2. Movement
        transform.position += movement * speed * Time.deltaTime;

        //3. Limit Movement & Animation 
        LimitMovement();

        if (Mathf.Abs(xPos) <= 0) { anim.SetBool("side", false); }
        else
        {
            anim.SetBool("side", true);
            if (xPos > 0) { spRender.flipX = true; }
            else { spRender.flipX = false; }
        }

        Boom();
    }

    //Limit Movement
    private void LimitMovement()
    {
        //1. World To Viewport Point
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

        //2. Limit
        viewPos.x = Mathf.Clamp01(viewPos.x);
        viewPos.y = Mathf.Clamp01(viewPos.y);

        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos);
        transform.position = worldPos;
    }

    //Boom!
    private void Boom()
    {
        if (!Input.GetMouseButtonDown(1) || bomb <= 0) { return; }

        //1. Minus Bomb Count
        bomb--;
        equippedShooter.Missile();
        gameManager.BombIconUpdate(bomb);

        //2. Effect OnOff
        Invoke("BoomEffectOff", 2.5f);
    }

    //Hit
    private void OnTriggerEnter2D(Collider2D other)
    {
        //1. Hit Enemy Bullet
        if (other.CompareTag("EnemyBullet"))
        {
            OnHit(1);

            other.gameObject.SetActive(false);
            objectManager.PlaceEnemyBulletEffect(other.GetComponent<Projectile>().fxName, other.transform, other.transform.rotation);
        }
        else if (other.CompareTag("Item"))
        {
            Item item = other.gameObject.GetComponent<Item>();

            switch (item.type)
            {
                case "Coin":
                    ScoreUpdate(1000);
                    audioManager.PlaySound("Coin");
                    break;

                case "Power":
                    if (power >= maxPower)
                    {
                        ScoreUpdate(500);
                        audioManager.PlaySound("Coin");
                    }
                    else
                    {
                        power++;
                        AddFollower();
                        audioManager.PlaySound("PowerUpItem");
                    }
                    break;

                case "Bomb":
                    if (bomb >= maxBomb)
                    {
                        ScoreUpdate(500);
                        audioManager.PlaySound("Coin");
                    }
                    else
                    {
                        bomb++;
                        gameManager.BombIconUpdate(bomb);
                        audioManager.PlaySound("BombItem");
                    }
                    break;
                case "Recovery":
                    if (currentHealth >= health)
                    {
                        ScoreUpdate(500);
                        audioManager.PlaySound("RecoveryHPsItem");
                    }
                    else
                    {
                        RecoveryHP(1);
                        audioManager.PlaySound("RecoveryHP");
                    }
                    break;
                case "Final":
                    ScoreUpdate(5000);
                    break;
            }
            other.gameObject.SetActive(false);
        }
    }

    //Add Follower
    private void AddFollower() { followers[power - 1].SetActive(true); }

    //Score
    public void ScoreUpdate(int _point)
    {
        score += _point;
        gameManager.ScoreTextUpdate(score);
    }

    //Damage
    private void OnHit(int _damage)
    {
        if (!isInvincibility)
        {
            currentHealth -= _damage;
            gameManager.LifeIconUpdate(currentHealth);

            if (currentHealth > 0)
            {
                StartCoroutine("Flashing");
                StartCoroutine(CameraController.instance.Shake(0.1f, 0.05f));
                audioManager.PlaySound("Damage");
            }
            else OnDeath();
        }
    }

    //Recovery HP
    public void RecoveryHP(int _point)
    {
        if (currentHealth + _point >= health) { currentHealth = health; }
        else
        {
            currentHealth += _point;
            gameManager.LifeIconUpdate(currentHealth);
            audioManager.PlaySound("RecoveryHP");
        }
    }

    //Death
    private void OnDeath()
    {
        objectManager.PlacePlayerBulletEffect("Death", transform, Quaternion.identity);
        gameManager.GameOver();
        gameObject.SetActive(false);
        audioManager.PlaySound("Death");
    }

    //Flashing
    private IEnumerator Flashing()
    {
        int count = 0;
        isInvincibility = true;
        gameManager.PlayerSp(1);
        while (count < 3)
        {
            spRender.color = flashColor;
            yield return new WaitForSeconds(0.2f);
            spRender.color = originalColor;
            yield return new WaitForSeconds(0.2f);
            count++;
        }

        gameManager.PlayerSp(0);
        isInvincibility = false;
    }

    public void Invincibility() { isInvincibility = true; }

    //Equipment Shooter
    public void EquipShooter(int _weaponIndex) { EquipShooter(allShooters[_weaponIndex]); }

    public void EquipShooter(Shooter _shooterToEquip)
    {
        if (equippedShooter != null) { Destroy(equippedShooter.gameObject); }

        equippedShooter = Instantiate(_shooterToEquip, weaponHold.position, weaponHold.rotation) as Shooter;
        equippedShooter.transform.parent = weaponHold;
    }

    public void GameClear() { Invoke("GameComplete", 3.0f); }

    public void GameComplete()
    {
        canMove = false;
        rigid2d.velocity = Vector2.up * 1.5f;
    }
}
