using UnityEngine;

//Background Offset -> Matertial Main Texture Offset Movement
public class Background : MonoBehaviour
{
    public float scrollSpeed = 1.2f;
    private Material material;

    void Awake() { material = GetComponent<Renderer>().material; }

    void FixedUpdate()
    {
        Vector2 nextOffset = material.mainTextureOffset;
        nextOffset.Set(0, nextOffset.y + (-scrollSpeed * Time.deltaTime));

        material.mainTextureOffset = nextOffset;
    }
}
