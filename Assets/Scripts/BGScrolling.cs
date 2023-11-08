using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScrolling : MonoBehaviour
{
    public float scrollSpeed = 0.1f;
    private Renderer renderer;
    //private Vector2 offset = Vector2.zero;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float y = Mathf.Repeat(Time.time * scrollSpeed, 1);
        Vector2 offset = new Vector2(0, y);
        renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
    }

    private void OnApplicationQuit()
    {
        Vector2 offset = new Vector2(0, 0);
        renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
    }
}
