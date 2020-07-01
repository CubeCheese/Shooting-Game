using System.Collections;
using UnityEngine;

//Shooter
public class Shooter : MonoBehaviour
{
    private GameManager gameManager;
    public ObjectManager objectManager;
    private AudioManager audioManager;
    [HideInInspector]
    private bool isReloading;

    [Header("Shooter Information")]
    public int projectileNum;
    public float speed;
    public float startNextShotTime;
    public float reloadTime = .3f;

    [Header("Projectile")]
    public Transform[] firePos;
    public float projectileSpeed = 1;

    [HideInInspector]
    public Crosshair crosshair;
    private Vector3 target;
    [HideInInspector]
    public Vector3 direction;
    private Quaternion newRot;

    [Header("Crosshair")]
    public Sprite bagicSp;
    public Sprite shotSp;

    [Header("Follower")]
    private Follower[] follwers;

    //Current Number
    private int projectilesRemainingInShooter;
    private float nextShotTime;

    // [Header("Effects")]
    //   public AudioClip shootAudio;
    //   public AudioClip reloadAudio;

    private void OnEnable()
    {
        isReloading = false;

        projectilesRemainingInShooter = projectileNum;
        nextShotTime = startNextShotTime;
    }

    public void Component(ObjectManager _objectManager, Transform _crosshair)
    {
        audioManager = AudioManager.instance;
        gameManager = GameManager.instance;
        objectManager = _objectManager;
        crosshair = _crosshair.GetComponent<Crosshair>();
    }

    public void SettingFollower(GameObject[] _follwer)
    {
        follwers = new Follower[_follwer.Length];
        for (int i = 0; i < _follwer.Length; i++) follwers[i] = _follwer[i].GetComponent<Follower>();
    }

    private void FixedUpdate()
    {
        Crosshair();
        nextShotTime -= Time.deltaTime;

        if (isReloading) return;

        if (Input.GetMouseButton(0)) { Shoot(); crosshair.SetSprite(shotSp); }
        else { crosshair.SetSprite(bagicSp); }
        if (Input.GetKeyDown(KeyCode.R)) { StartCoroutine(Reload()); }
    }

    //Crosshair Movement & Angle
    private void Crosshair()
    {
        target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));

        crosshair.transform.position = Vector2.Lerp(crosshair.transform.position, target, speed * Time.deltaTime);

        direction = crosshair.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        newRot = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        transform.rotation = newRot;

    }

    //Shoot
    private void Shoot()
    {
        if (nextShotTime <= 0 && projectilesRemainingInShooter > 0)
        {
            for (int index = 0; index < firePos.Length; index++)
            {
                projectilesRemainingInShooter--;
                gameManager.ProjectileTextUpdate(projectileNum, projectilesRemainingInShooter);
                nextShotTime = startNextShotTime;

                GameObject bullet = objectManager.PlaceBullet("PlayerBullet", transform, newRot);
                Rigidbody2D rigid2D = bullet.GetComponent<Rigidbody2D>();
                rigid2D.AddForce(direction * projectileSpeed, ForceMode2D.Impulse);

                for (int num = 0; num < follwers.Length; num++)
                    follwers[num].Shoot(crosshair.transform.position);

                audioManager.PlaySound("Shot");
            }
            // AudioManager.instance.PlaySound(shootAudio, transform.position);
            if (projectilesRemainingInShooter <= 0) { StartCoroutine(Reload()); }
        }
    }

    //Shoot Missile
    public void Missile()
    {
        audioManager.PlaySound("BombShot");
        GameObject bullet = objectManager.PlaceBullet("Missile", transform, newRot);
        Rigidbody2D rigid2D = bullet.GetComponent<Rigidbody2D>();
        rigid2D.AddForce(direction * projectileSpeed, ForceMode2D.Impulse);
    }

    //Reload
    public IEnumerator Reload()
    {
        if (projectilesRemainingInShooter != projectileNum)
        {
            isReloading = true;
            audioManager.PlaySound("Reload");
            yield return new WaitForSeconds(reloadTime);

            isReloading = false;
            projectilesRemainingInShooter = projectileNum;
            gameManager.ProjectileTextUpdate(projectileNum, projectilesRemainingInShooter);
        }
    }
}
