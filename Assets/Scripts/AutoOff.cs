using UnityEngine;

public class AutoOff : MonoBehaviour
{
    public float lifetime = 3;

    private void OnEnable() { Invoke("ObjectSetOff", lifetime); }

    private void ObjectSetOff() { gameObject.SetActive(false); }
}
