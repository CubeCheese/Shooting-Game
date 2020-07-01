using System.Collections;
using UnityEngine;

//카메라 흔들림
public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    //위치값
    private Vector3 originPos;

    //Instance Setting
    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        originPos = transform.position;
    }

    //Camera Shake
    public IEnumerator Shake(float _amount, float _duration)
    {
        float timer = 0;
        while (timer <= _duration)
        {
            transform.localPosition = (Vector3)Random.insideUnitCircle * _amount + originPos;

            timer += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originPos;
    }
}
