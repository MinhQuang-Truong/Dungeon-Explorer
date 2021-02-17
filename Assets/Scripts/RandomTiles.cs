using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTiles : MonoBehaviour
{
    // Start is called before the first frame update
    private SpriteRenderer rend;
    public Sprite[] sprites;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        int rand = Random.Range(0, sprites.Length);
        rend.sprite = sprites[rand];
    }
}
