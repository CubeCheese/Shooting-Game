using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [Header("Enemy Prefab")]
    public GameObject enemyLPrefab;
    public GameObject enemyMPrefab;
    public GameObject enemySPrefab;
    public GameObject bossPrefab;

    [Header("Item Prefab")]
    public GameObject itemCoinPrefab;
    public GameObject itemPowerPrefab;
    public GameObject itemBombPrefab;
    public GameObject itemRecoveryPrefab;
    public GameObject itemFinalPrefab;

    [Header("Bullet Prefab")]
    public GameObject playerBulletPrefab;
    public GameObject missilePrefab;
    public GameObject playerFollowBulletPrefab;
    public GameObject enemyLBulletPrefab;
    public GameObject enemyMBulletPrefab;
    public GameObject enemySBulletPrefab;
    public GameObject bossBulletAPrefab;
    public GameObject bossBulletBPrefab;

    [Header("Bullet Effect Prefab")]
    public GameObject deathEffectPrefab;
    public GameObject playerBulletEffectPrefab;
    public GameObject missileEffectPrefab;
    public GameObject playerFollowBulletEffectPrefab;
    public GameObject enemyLBulletEffectPrefab;
    public GameObject enemyMBulletEffectPrefab;
    public GameObject enemySBulletEffectPrefab;
    public GameObject bossBulletAEffectPrefab;
    public GameObject bossBulletBEffectPrefab;

    private GameObject[] enemyL;
    private GameObject[] enemyM;
    private GameObject[] enemyS;
    private GameObject boss;

    private GameObject[] itemCoin;
    private GameObject[] itemPower;
    private GameObject[] itemBomb;
    private GameObject[] itemRecovery;
    private GameObject itemFinal;

    private GameObject[] playerBullet;
    private GameObject[] missile;
    private GameObject[] playerFollowBullet;
    private GameObject[] enemyLBullet;
    private GameObject[] enemyMBullet;
    private GameObject[] enemySBullet;
    private GameObject[] bossBulletA;
    private GameObject[] bossBulletB;

    private GameObject[] deathEffect;
    private GameObject[] playerBulletEffect;
    private GameObject[] missileEffect;
    private GameObject[] playerFollowBulletEffect;
    private GameObject enemyLBulletEffect;
    private GameObject enemyMBulletEffect;
    private GameObject enemySBulletEffect;
    private GameObject bossBulletAEffect;
    private GameObject bossBulletBEffect;
    private GameObject[] targetPool;
    private GameObject targetEffect;

    private void Awake()
    {
        enemyL = new GameObject[10];
        enemyM = new GameObject[10];
        enemyS = new GameObject[10];

        itemCoin = new GameObject[15];
        itemPower = new GameObject[10];
        itemBomb = new GameObject[10];
        itemRecovery = new GameObject[10];

        playerBullet = new GameObject[30];
        missile = new GameObject[3];
        playerFollowBullet = new GameObject[50];

        enemyLBullet = new GameObject[50];
        enemyMBullet = new GameObject[50];
        enemySBullet = new GameObject[50];

        bossBulletA = new GameObject[500];
        bossBulletB = new GameObject[500];

        deathEffect = new GameObject[5];
        playerBulletEffect = new GameObject[30];
        missileEffect = new GameObject[3];
        playerFollowBulletEffect = new GameObject[10];

        Generate();
    }

    //Generation Object
    private void Generate()
    {
        Transform enemyHolder = new GameObject("Generated Enemies").transform;
        enemyHolder.SetParent(transform);
        //Enemy
        for (int index = 0; index < enemyL.Length; index++)
        {
            enemyL[index] = Instantiate(enemyLPrefab);
            enemyL[index].SetActive(false);
            enemyL[index].transform.parent = enemyHolder;
        }
        for (int index = 0; index < enemyM.Length; index++)
        {
            enemyM[index] = Instantiate(enemyMPrefab);
            enemyM[index].SetActive(false);
            enemyM[index].transform.parent = enemyHolder;
        }
        for (int index = 0; index < enemyS.Length; index++)
        {
            enemyS[index] = Instantiate(enemySPrefab);
            enemyS[index].SetActive(false);
            enemyS[index].transform.parent = enemyHolder;
        }

        Transform itemHolder = new GameObject("Generated Items").transform;
        itemHolder.SetParent(transform);
        //Item
        for (int index = 0; index < itemCoin.Length; index++)
        {
            itemCoin[index] = Instantiate(itemCoinPrefab);
            itemCoin[index].SetActive(false);
            itemCoin[index].transform.parent = itemHolder;
        }
        for (int index = 0; index < itemPower.Length; index++)
        {
            itemPower[index] = Instantiate(itemPowerPrefab);
            itemPower[index].SetActive(false);
            itemPower[index].transform.parent = itemHolder;
        }
        for (int index = 0; index < itemBomb.Length; index++)
        {
            itemBomb[index] = Instantiate(itemBombPrefab);
            itemBomb[index].SetActive(false);
            itemBomb[index].transform.parent = itemHolder;
        }
        for (int index = 0; index < itemRecovery.Length; index++)
        {
            itemRecovery[index] = Instantiate(itemRecoveryPrefab);
            itemRecovery[index].SetActive(false);
            itemRecovery[index].transform.parent = itemHolder;
        }
        itemFinal = Instantiate(itemFinalPrefab);
        itemFinal.SetActive(false);
        itemFinal.transform.parent = itemHolder;

        Transform playerBulletHolder = new GameObject("Generated Player Bullets").transform;
        playerBulletHolder.SetParent(transform);
        //Player Bullet
        for (int index = 0; index < playerBullet.Length; index++)
        {
            playerBullet[index] = Instantiate(playerBulletPrefab);
            playerBullet[index].SetActive(false);
            playerBullet[index].transform.parent = playerBulletHolder;
        }
        for (int index = 0; index < missile.Length; index++)
        {
            missile[index] = Instantiate(missilePrefab);
            missile[index].SetActive(false);
            missile[index].transform.parent = playerBulletHolder;
        }
        for (int index = 0; index < playerFollowBullet.Length; index++)
        {
            playerFollowBullet[index] = Instantiate(playerFollowBulletPrefab);
            playerFollowBullet[index].SetActive(false);
            playerFollowBullet[index].transform.parent = playerBulletHolder;
        }

        Transform enemyBulletHolder = new GameObject("Generated Enemy Bullets").transform;
        enemyBulletHolder.SetParent(transform);
        //Enemy Bullet
        for (int index = 0; index < enemyLBullet.Length; index++)
        {
            enemyLBullet[index] = Instantiate(enemyLBulletPrefab);
            enemyLBullet[index].SetActive(false);
            enemyLBullet[index].transform.parent = enemyBulletHolder;
        }
        for (int index = 0; index < enemyMBullet.Length; index++)
        {
            enemyMBullet[index] = Instantiate(enemyMBulletPrefab);
            enemyMBullet[index].SetActive(false);
            enemyMBullet[index].transform.parent = enemyBulletHolder;
        }
        for (int index = 0; index < enemySBullet.Length; index++)
        {
            enemySBullet[index] = Instantiate(enemySBulletPrefab);
            enemySBullet[index].SetActive(false);
            enemySBullet[index].transform.parent = enemyBulletHolder;
        }

        Transform bossBulletHolder = new GameObject("Generated Boss Bullets").transform;
        bossBulletHolder.SetParent(transform);
        //Boss Bullet
        for (int index = 0; index < bossBulletA.Length; index++)
        {
            bossBulletA[index] = Instantiate(bossBulletAPrefab);
            bossBulletA[index].SetActive(false);
            bossBulletA[index].transform.parent = bossBulletHolder;
        }
        for (int index = 0; index < bossBulletB.Length; index++)
        {
            bossBulletB[index] = Instantiate(bossBulletBPrefab);
            bossBulletB[index].SetActive(false);
            bossBulletB[index].transform.parent = bossBulletHolder;
        }
        boss = Instantiate(bossPrefab);
        boss.SetActive(false);

        Transform EffectHolder = new GameObject("Generated Effect").transform;
        EffectHolder.SetParent(transform);
        for (int index = 0; index < deathEffect.Length; index++)
        {
            deathEffect[index] = Instantiate(deathEffectPrefab);
            deathEffect[index].SetActive(false);
            deathEffect[index].transform.parent = EffectHolder;
        }
        //Player Bullet
        for (int index = 0; index < playerBulletEffect.Length; index++)
        {
            playerBulletEffect[index] = Instantiate(playerBulletEffectPrefab);
            playerBulletEffect[index].SetActive(false);
            playerBulletEffect[index].transform.parent = EffectHolder;
        }
        for (int index = 0; index < missileEffect.Length; index++)
        {
            missileEffect[index] = Instantiate(missileEffectPrefab);
            missileEffect[index].SetActive(false);
            missileEffect[index].transform.parent = EffectHolder;
        }
        for (int index = 0; index < playerFollowBulletEffect.Length; index++)
        {
            playerFollowBulletEffect[index] = Instantiate(playerFollowBulletEffectPrefab);
            playerFollowBulletEffect[index].SetActive(false);
            playerFollowBulletEffect[index].transform.parent = EffectHolder;
        }
        //Enemy Bullet
        enemyLBulletEffect = Instantiate(enemyLBulletEffectPrefab);
        enemyLBulletEffect.SetActive(false);
        enemyLBulletEffect.transform.parent = EffectHolder;
        enemyMBulletEffect = Instantiate(enemyMBulletEffectPrefab);
        enemyMBulletEffect.SetActive(false);
        enemyMBulletEffect.transform.parent = EffectHolder;
        enemySBulletEffect = Instantiate(enemySBulletEffectPrefab);
        enemySBulletEffect.SetActive(false);
        enemySBulletEffect.transform.parent = EffectHolder;
        //Boss Bullet
        bossBulletAEffect = Instantiate(bossBulletAEffectPrefab);
        bossBulletAEffect.SetActive(false);
        bossBulletAEffect.transform.parent = EffectHolder;
        bossBulletBEffect = Instantiate(bossBulletBEffectPrefab);
        bossBulletBEffect.SetActive(false);
        bossBulletBEffect.transform.parent = EffectHolder;
    }

    //Return Object Information
    public GameObject PlaceEnemy(string _objName, Transform _pos, Quaternion _rot)
    {
        switch (_objName)
        {
            case "EnemyL":
                targetPool = enemyL;
                break;
            case "EnemyM":
                targetPool = enemyM;
                break;
            case "EnemyS":
                targetPool = enemyS;
                break;
            case "Boss":
                boss.SetActive(true);
                boss.transform.position = _pos.position;
                boss.transform.localRotation = _rot;
                return boss;
            default:
                Debug.Log("No Enemy Name");
                targetPool = null;
                break;
        }

        for (int index = 0; index < targetPool.Length; index++)
        {
            if (!targetPool[index].activeSelf)
            {
                targetPool[index].SetActive(true);
                targetPool[index].transform.position = _pos.position;
                targetPool[index].transform.localRotation = _rot;
                return targetPool[index];
            }
        }

        return null;
    }

    public GameObject PlaceItem(string _objName, Transform _pos, Quaternion _rot)
    {
        switch (_objName)
        {
            case "Coin":
                targetPool = itemCoin;
                break;
            case "Power":
                targetPool = itemPower;
                break;
            case "Bomb":
                targetPool = itemBomb;
                break;
            case "Recovery":
                targetPool = itemRecovery;
                break;
            case "Final":
                return itemFinal;
            default:
                Debug.Log("No Object Name");
                targetPool = null;
                break;
        }

        for (int index = 0; index < targetPool.Length; index++)
        {
            if (!targetPool[index].activeSelf)
            {
                targetPool[index].SetActive(true);
                targetPool[index].transform.position = _pos.position;
                targetPool[index].transform.localRotation = _rot;
                Rigidbody2D rigid2D = targetPool[index].GetComponent<Rigidbody2D>();
                rigid2D.AddForce(Vector2.down * 3);
                return targetPool[index];
            }
        }

        return null;
    }

    public GameObject PlaceBullet(string _objName, Transform _pos, Quaternion _rot)
    {
        switch (_objName)
        {
            case "PlayerBullet":
                targetPool = playerBullet;
                break;
            case "Missile":
                targetPool = missile;
                break;
            case "PlayerFollowBullet":
                targetPool = playerFollowBullet;
                break;
            case "EnemyLBullet":
                targetPool = enemyLBullet;
                break;
            case "EnemyMBullet":
                targetPool = enemyMBullet;
                break;
            case "EnemySBullet":
                targetPool = enemySBullet;
                break;
            case "BossBulletA":
                targetPool = bossBulletA;
                break;
            case "BossBulletB":
                targetPool = bossBulletB;
                break;
            default:
                Debug.Log("No Bullet Name");
                targetPool = null;
                break;
        }

        if (targetPool == null) return null;

        for (int index = 0; index < targetPool.Length; index++)
        {
            if (!targetPool[index].activeSelf)
            {
                targetPool[index].SetActive(true);
                targetPool[index].transform.position = _pos.position;
                targetPool[index].transform.rotation = _rot;
                return targetPool[index];
            }
        }

        return null;
    }

    public GameObject PlacePlayerBulletEffect(string _objName, Transform _pos, Quaternion _rot)
    {
        switch (_objName)
        {
            case "PlayerBulletEffect":
                targetPool = playerBulletEffect;
                break;
            case "MissileEffect":
                targetPool = missileEffect;
                break;
            case "PlayerFollowerBulletEffect":
                targetPool = playerFollowBulletEffect;
                break;
            case "Death":
                targetPool = deathEffect;
                break;
            default:
                Debug.Log("No Player Bullet Effect Name");
                targetEffect = null;
                break;
        }

        if (targetPool == null) return null;

        for (int index = 0; index < targetPool.Length; index++)
        {
            if (!targetPool[index].activeSelf)
            {
                targetPool[index].SetActive(true);
                targetPool[index].transform.position = _pos.position;
                targetPool[index].transform.rotation = _rot;
                return targetPool[index];
            }
        }

        return null;
    }

    public GameObject PlaceEnemyBulletEffect(string _objName, Transform _pos, Quaternion _rot)
    {
        switch (_objName)
        {
            case "EnemyLBulletEffect":
                targetEffect = enemyLBulletEffectPrefab;
                break;
            case "EnemyMBulletEffect":
                targetEffect = enemyMBulletEffectPrefab;
                break;
            case "EnemySBulletEffect":
                targetEffect = enemySBulletEffectPrefab;
                break;
            case "BossBulletAEffect":
                targetEffect = bossBulletAEffectPrefab;
                break;
            case "BossBulletBEffect":
                targetEffect = bossBulletBEffectPrefab;
                break;
            default:
                Debug.Log("No Enemy Bullet Effect Name");
                targetEffect = null;
                break;
        }

        if (targetPool == null) return null;

        return targetEffect;
    }
}
