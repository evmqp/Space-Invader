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
        int boxType = Random.Range(0, boxSprites.Length-1);
        spriteRenderer.sprite = boxSprites[boxType];
        if (boxType == 0)
            gameObject.tag = "Box ammo";
        else
            gameObject.tag = "Box shield";
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Box")
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = boxSprites[boxSprites.Length - 1];
        }
            
    }
}
