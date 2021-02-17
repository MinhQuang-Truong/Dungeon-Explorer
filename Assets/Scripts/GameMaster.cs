using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public Units selectedUnit;
    public int playerTurn = 1;
    void Start()
    {
        
    }
    public void ResetTiles()
    {
        TileSetting[] tiles = FindObjectsOfType<TileSetting>();
        foreach (TileSetting tile in tiles)
        {
            tile.Reset();
        }
    }
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndTurn();
        }
    }
    
    void EndTurn()
    {
        if (playerTurn == 1)
        {
            playerTurn = 2;
        }else if (playerTurn == 2)
        {
            playerTurn = 1;
        }
        if (selectedUnit != null)
        {
            selectedUnit.selected = false;
            selectedUnit = null;
        }

        ResetTiles();

        Units[] units = FindObjectsOfType<Units>();

        foreach (Units unit in units)
        {
            unit.attackIcon.SetActive(false);
            unit.hasMoved = false;
            unit.hasAttack = false;
        }


    }
}
