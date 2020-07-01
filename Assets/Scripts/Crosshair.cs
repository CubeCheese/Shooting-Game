using UnityEngine;

//Crosshair -> Aiming Target
public class Crosshair : MonoBehaviour
{
    private SpriteRenderer spRender;

    [Header("Crosshair")]
    private Color originalColor;
    public Color setColor;
    public LayerMask whatIsTarget;
    private Transform player;
    private Camera cam;

    [Header("Dot")]
    public GameObject dotPrefab;
    public int dotAmount;
    private GameObject[] dotArray;
    private float dotGap;

    [Space]
    [Header("Line Variables")]
    public AnimationCurve followCurve;
    public float followSpeed;

    private void Awake()
    {
        //1. Get Components
        spRender = GetComponent<SpriteRenderer>();
        player = GetComponentInParent<PlayerController>().transform;
        cam = Camera.main;

        originalColor = Color.white;
        SpawnDots();

        Cursor.visible = false;
    }

    //Spawn Dots
    private void SpawnDots()
    {
        dotGap = 1f / dotAmount;

        dotArray = new GameObject[dotAmount];
        for (int i = 0; i < dotAmount; i++) { dotArray[i] = Instantiate(dotPrefab); }
    }

    //Rotation Crosshair
    private void FixedUpdate() {  SetDotPos(); SetTarget(); }

    //Setting Dot Position
    private void SetDotPos()
    {
        for (int i = 0; i < dotAmount; i++)
        {
            Vector3 _dotPos = dotArray[i].transform.position;
            Vector3 _targetPos = Vector2.Lerp(player.position, transform.position, i * dotGap);

            float _smoothSpeed = (1f - followCurve.Evaluate(i - dotGap)) * followSpeed;

            dotArray[i].transform.position = Vector2.Lerp(_dotPos, _targetPos, _smoothSpeed * Time.deltaTime);
        }
    }

    //Setting Target Position
    private void SetTarget()
    {
        Collider2D detection = Physics2D.OverlapCircle(transform.position, 0.05f, whatIsTarget);

        if (detection) { spRender.color = setColor; transform.position = detection.transform.position; }
        else { spRender.color = originalColor; }
    }

    public void SetSprite(Sprite _sprite) { spRender.sprite = _sprite; }
}
