using UnityEngine;

public class Bomb : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player") return;

        StartCoroutine(CameraController.instance.Shake(0.3f, 0.05f));
        AudioManager.instance.PlaySound("Boom");

        if (other.gameObject.tag == "Boss") { other.GetComponent<BossController>().OnHit(20); }
        else if (other.gameObject.tag != "Player") { other.gameObject.SetActive(false); }
    }
}
