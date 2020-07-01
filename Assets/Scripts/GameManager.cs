using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector]
    public bool gameEnd;
    private bool isStop;

    private int currentLevel;
    private int currentScore;

    public GameObject player;
    public ObjectManager objectManager;
    private AudioManager audioManager;
    private PlayerController playerLogic;

    //Spawn Data
    public List<SpawnData> spawnList;
    private bool spawnEnd;
    public int spawnIndex;
    public Transform[] spawnPoint;
    public Transform playerSpawnPoint;
    public Transform bossSpawnPoint;

    [Header("UI")]
    public GameObject stopPanel;
    public GameObject gameOverPanel;
    public Text gameOverScoreText;
    public GameObject gameClearPanel;
    public Text gameClearScoreText;
    public Image[] lifeIcon;
    public Image[] bombIcon;
    public Image bossLife;
    public Text scoreText;
    public Text projectileText;
    public Text levelText;

    public Image playerImage;
    public Image bossImage;
    public Sprite[] playerSp;
    public Sprite[] bossSp;

    private float nextSpawnTime;
    private float currentSpawnTime;

    private void Awake()
    {
        instance = this;
        audioManager = AudioManager.instance;

    }

    private void OnEnable()
    {
        gameEnd = false;
        isStop = false;
        gameOverPanel.SetActive(false);
        LifeIconUpdate(5);
        ScoreTextUpdate(0);
        BombIconUpdate(0);
        currentLevel = PlayerPrefs.GetInt("Level");
        levelText.text = "Level " + (currentLevel + 1).ToString();
        ReadSpawnFile();
        SpawnPlayer();
    }

    //Read Spawn File 
    private void ReadSpawnFile()
    {
        spawnList = new List<SpawnData>();

        //1. Spawn List Clear
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        //2. Read Respawn File
        TextAsset textFile = Resources.Load("Stage " + currentLevel.ToString()) as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);

        //3. Add Spawn Data 
        while (stringReader != null)
        {
            string line = stringReader.ReadLine();
            if (line == null) break;

            SpawnData spawnData = new SpawnData();

            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.name = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnData.xDir = float.Parse(line.Split(',')[3]);
            spawnData.yDir = float.Parse(line.Split(',')[4]);
            spawnList.Add(spawnData);
        }

        nextSpawnTime = spawnList[0].delay;
        stringReader.Close();
    }

    //Spawn Player
    private void SpawnPlayer()
    {
        player.transform.position = playerSpawnPoint.position;
        playerLogic = player.GetComponent<PlayerController>();
        Rigidbody2D rigid2d = player.GetComponent<Rigidbody2D>();

        rigid2d.velocity = Vector2.up * 1.5f;
        playerLogic.objectManager = objectManager;
    }

    private void FixedUpdate()
    {
        if (spawnEnd) return;
        
            currentSpawnTime += Time.deltaTime;

            if (currentSpawnTime > nextSpawnTime)
            {
                if (spawnIndex >= spawnList.Count - 1)
                {
                    SpawnBoss();
                }
                else
                {
                    SpawnEnemy();
                    nextSpawnTime = spawnList[spawnIndex].delay;
                    currentSpawnTime = 0;
                }
            }  
        if (Input.GetKeyDown(KeyCode.Escape)) { isStop = !isStop; Stop(isStop); }
    }

    //Spawn Enemy
    private void SpawnEnemy()
    {
        int enemyPoint = spawnList[spawnIndex].point;
        string name = spawnList[spawnIndex].name;

        if (spawnIndex>= spawnList.Count) { SpawnBoss(); return; }

        GameObject enemy = objectManager.PlaceEnemy(name, spawnPoint[enemyPoint], Quaternion.identity);
        EnemyController enemyLogic = enemy.GetComponent<EnemyController>();
        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();

        rigid.velocity = new Vector2(spawnList[spawnIndex].xDir,-spawnList[spawnIndex].yDir) * enemyLogic.speed;
        enemyLogic.player = player;
        enemyLogic.playerLogic = playerLogic;
        enemyLogic.objectManager = objectManager;
        //Spawn Index Plus
        spawnIndex++;
    }

    private void SpawnBoss()
    {
        audioManager.PlaySound("BossAppearance");
        GameObject boss = objectManager.PlaceEnemy("Boss", bossSpawnPoint, Quaternion.identity);
        Rigidbody2D rigid2d = boss.GetComponent<Rigidbody2D>();
        BossController bossLogic = boss.GetComponent<BossController>();
        bossLogic.player = player.transform;
        bossLogic.playerLogic = playerLogic;
        bossLogic.objectManager = objectManager;

        rigid2d.velocity = new Vector2(spawnList[spawnIndex].xDir, -spawnList[spawnIndex].yDir) * bossLogic.speed;

        spawnEnd = true;
    }

    //Score
    public void ScoreTextUpdate(int _score) { currentScore = _score; scoreText.text = currentScore.ToString("D9"); }

    //Life
    public void LifeIconUpdate(int _health)
    {
        for (int index = 0; index < lifeIcon.Length; index++) { lifeIcon[index].color = Color.clear; }
        for (int index = 0; index < _health; index++) { lifeIcon[index].color = Color.white; }
    }

    public void BossLifeIconUpdate() { bossLife.fillAmount -= 0.01f; }

    //Projectile
    public void ProjectileTextUpdate(int _maxNum, int _currentNum) { projectileText.text = _currentNum.ToString(); }

    //Bomb
    public void BombIconUpdate(int _count)
    {
        for (int index = 0; index < bombIcon.Length; index++) { bombIcon[index].color = Color.clear; }
        for (int index = 0; index < _count; index++) { bombIcon[index].color = Color.white; }
    }

    public void PlayerSp(int _num) { playerImage.sprite = playerSp[_num]; }

    public void BossSp(int _num) { bossImage.sprite = bossSp[_num]; }

    //GameOver
    public void GameOver()
    {
        //1. GameOver Declaration
        Cursor.visible = true;
        audioManager.StopMusic();
        audioManager.PlaySound("GameOver");
        gameEnd = true;
        gameOverPanel.SetActive(true);
        gameOverScoreText.text = currentScore.ToString("D9");

        //2. Destory GameObject
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] projectile = GameObject.FindGameObjectsWithTag("EnemyBullet");
        GameObject[] item = GameObject.FindGameObjectsWithTag("Item");

        for (int index = 0; index < enemies.Length; index++) { Destroy(enemies[index]); }
        for (int index = 0; index < projectile.Length; index++) { Destroy(projectile[index]); }
        for (int index = 0; index < item.Length; index++) { Destroy(item[index]); }
    }

    //GameClear
    public void GameClear()
    {
        PlayerPrefs.SetInt("Level", currentLevel + 1);
        gameEnd = true;
        playerLogic.Invincibility();
        Invoke("GameClearTiming", 3.0f);
    }

    private void GameClearTiming()
    {
        Cursor.visible = true;
        gameClearPanel.SetActive(true);
        gameClearScoreText.text = currentScore.ToString("D9");

        audioManager.StopMusic();
        audioManager.PlaySound("GameClear");
        playerLogic.GameClear();
    }

    //NextStage
    public void NextStage()
    {
        if (currentLevel + 1 < 3) { SceneManager.LoadScene(1); }
        else { SceneManager.LoadScene(0); }
    }

    public void Stop(bool _active)
    {
        Cursor.visible = _active;
        stopPanel.SetActive(_active);

        if (_active) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    //Main Menu
    public void MainMenu()
    {
        currentLevel = 0;
        SceneManager.LoadScene(0);
    }

    //Retry
    public void Retry() { Application.LoadLevel(Application.loadedLevel); }
}
