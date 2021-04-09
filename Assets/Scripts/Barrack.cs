using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barrack : MonoBehaviour
{
    GameMaster gm;

    public Button player1openButton;
    public Button player2openButton;

    public GameObject player1Menu;
    public GameObject player2Menu;

    
    private void Start()
    {
        gm = FindObjectOfType<GameMaster>();
    }

    private void Update()
    {
        if (gm.playerTurn == 1)
        {
            player1openButton.interactable = true;
            player2openButton.interactable = false;
        }
        else
        {
            player2openButton.interactable = true;
            player1openButton.interactable = false;
        }
    }

    public void ToggleMenu(GameObject menu)
    {
        menu.SetActive(!menu.activeSelf);
    }

    public void CloseCharacterCreationMenus()
    {
        player1Menu.SetActive(false);
        player2Menu.SetActive(false);
    }
    
    public void BuyUnit(BarrackItem item)
    {

        if (gm.playerTurn == 1 && item.cost <= gm.player1Gold)
        {
            gm.player1Gold -= item.cost;
            player1Menu.SetActive(false);
        }
        else if (gm.playerTurn == 2 && item.cost <= gm.player2Gold)
        {
            player2Menu.SetActive(false);
            gm.player2Gold -= item.cost;
        }
        else
        {
            print("NOT ENOUGH GOLD, SORRY!");
            return;
        }

        gm.UpdateGoldText();

        gm.purchasedItem = item;

        if(gm.selectedUnit != null)
        {
            gm.selectedUnit.selected = false;
            gm.selectedUnit = null; 
        }
        // DeselectUnit();
        GetCreatableTiles();
    }
    void GetCreatableTiles()
    {
        gm.ResetTiles();

        TileSetting[] tiles = FindObjectsOfType<TileSetting>();
        foreach (TileSetting tile in tiles)
        {
            if (tile.isClear())
            {
                tile.SetCreatable();
            }
        }
    }
    /*
    public void BuyVillage(Village village)
    {
        if (village.playerNumber == 1 && village.cost <= gm.player1Gold)
        {
            player1Menu.SetActive(false);
            gm.player1Gold -= village.cost;
        }
        else if (village.playerNumber == 2 && village.cost <= gm.player2Gold)
        {
            player2Menu.SetActive(false);
            gm.player2Gold -= village.cost;
        }
        else
        {
            print("NOT ENOUGH GOLD, SORRY!");
            return;
        }
        gm.UpdateGoldText();
        gm.createdVillage = village;

        DeselectUnit();

        SetCreatableTiles();

    }

    

    void DeselectUnit()
    {
        if (gm.selectedUnit != null)
        {
            gm.selectedUnit.isSelected = false;
            gm.selectedUnit = null;
        }
    }*/
}
