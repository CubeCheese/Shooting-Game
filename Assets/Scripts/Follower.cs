using UnityEngine;
using System.Collections.Generic;

public class Follower : MonoBehaviour
{
    [HideInInspector]
    public ObjectManager objectManager;
    public Shooter shooter;

    [Header("Projectile")]
    public Transform firePos;
    public float projectileSpeed = 1;

    [Header("Follow")]
    public Transform parent;
    public Queue<Vector3> parentPos;
    public Vector3 followPos;
    public int followDelay;

    private Vector3 direction;
    private Quaternion newRot;

    private void Awake() { parentPos = new Queue<Vector3>(); }

    private void OnEnable() { transform.position = followPos; }

    private void FixedUpdate() { Follow(); }

    //Follow
    private void Follow()
    {
        if (!parentPos.Contains(parent.position))
            parentPos.Enqueue(parent.position);

        if (parentPos.Count > followDelay)
            followPos = parentPos.Dequeue();
        else if (parentPos.Count < followDelay)
            followPos = parent.position;

        transform.position = followPos;
    }

    //Shoot
    public void Shoot(Vector3 _target)
    {
        if (gameObject.activeSelf)
        {
            direction = _target - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            newRot = Quaternion.Euler(new Vector3(0, 0, angle - 90));

            GameObject bullet = objectManager.PlaceBullet("PlayerFollowBullet", firePos.transform, newRot);
            Rigidbody2D rigid2D = bullet.GetComponent<Rigidbody2D>();
            rigid2D.AddForce(direction * projectileSpeed, ForceMode2D.Impulse);
        }
    }
}
