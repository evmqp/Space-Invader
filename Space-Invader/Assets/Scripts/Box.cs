using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Sprite[] boxSprites;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = boxSprites[Random.Range(0, boxSprites.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
