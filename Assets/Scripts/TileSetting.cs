﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSetting : MonoBehaviour
{
    private SpriteRenderer rend;
    public Color highlightedColor;
    public Color creatableColor;

    public LayerMask obstacles;

    public bool isWalkable;
    public bool isCreatable;


    GameMaster gm;
    public float amount;
    private bool sizeIncrease;

    private AudioSource source;

    private void Start()
    {
       
        rend = GetComponent<SpriteRenderer>();
        gm = FindObjectOfType < GameMaster>();
    }

    public bool isClear() // does this tile have an obstacle on it. Yes or No?
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, 0.2f, obstacles);
        if (col == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Highlight()
    {
        rend.color = highlightedColor;
        isWalkable = true;
    }

    public void Reset()
    {
        rend.color = Color.white;
        isWalkable = false;
        isCreatable = false;
    }

    public void SetCreatable()
    {
        rend.color = creatableColor;
        isCreatable = true;
    }

    private void OnMouseDown()
    {
        if (isWalkable == true && gm.selectedUnit != null)
        {
            gm.selectedUnit.Move(this.transform);
        }
        else if (isCreatable)
        {
            BarrackItem item = Instantiate(gm.purchasedItem, new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y)), Quaternion.identity);
            gm.ResetTiles();
            Units unit = item.GetComponent<Units>();
            if (unit != null)
            {
                unit.hasMoved = true;
                unit.hasAttack = true;
            }
        }
    }


    private void OnMouseEnter()
    {
        if (isClear() == true)
        {
            sizeIncrease = true;
            transform.localScale += new Vector3(amount, amount, amount);
        }

    }

    private void OnMouseExit()
    {
        if (isClear() == true)
        {
            sizeIncrease = false;
            transform.localScale -= new Vector3(amount, amount, amount);
        }

        if (isClear() == false && sizeIncrease == true)
        {
            sizeIncrease = false;
            transform.localScale -= new Vector3(amount, amount, amount);
        }
    }
}
