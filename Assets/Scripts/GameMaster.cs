using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameMaster : MonoBehaviour
{
    public Units selectedUnit;
    public int playerTurn = 1;

    public Image playerIndicatior;
    public Sprite player1Indicator;
    public Sprite player2Indicator;

    public GameObject selectedUnitSquare;

    public int player1Gold = 100;
    public int player2Gold = 150;

    public Text player1GoldText;
    public Text player2GoldText;

    public BarrackItem purchasedItem;

    public GameObject statsPanel;
    public Vector2 statsPanelShift;

    Units viewedUnit;

    public Text healthStats;
    public Text armorStats;
    public Text attackDamageStats;
    public Text counterDamageStats;


    void Start()
    {
        Debug.Log(playerTurn);
        GetGoldIncome(1);
    }

    public void UpdateGoldText()
    {
        player1GoldText.text = player1Gold.ToString();
        player2GoldText.text = player2Gold.ToString();
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
        if (selectedUnit != null)
        {
            selectedUnitSquare.SetActive(true);
            selectedUnitSquare.transform.position = selectedUnit.transform.position;
        }
        else
        {
            selectedUnitSquare.SetActive(false);
        }
       
    }
    void GetGoldIncome(int playerTurn)
    {
        foreach (Village village in FindObjectsOfType<Village>())
        {
            if (village.playerNumber == playerTurn)
            {
                if (playerTurn == 1)
                {
                    player1Gold += village.goldPerTurn;
                }
                else
                {
                    player2Gold += village.goldPerTurn;
                }
            }
        }
        UpdateGoldText();
    }
    public void ToggleStatsPanel(Units unit)
    {
        if (unit.Equals(viewedUnit) == false)
        {
            statsPanel.SetActive(true);
            statsPanel.transform.position = (Vector2)unit.transform.position + statsPanelShift;
            viewedUnit = unit;
            UpdateStatsPanel();

        }
        else
        {
            statsPanel.SetActive(false);
            viewedUnit = null;
        }
    }
    public void UpdateStatsPanel()
    {
        if (viewedUnit != null)
        {
            healthStats.text = viewedUnit.health.ToString();
            armorStats.text = viewedUnit.armor.ToString();
            attackDamageStats.text = viewedUnit.attackDamage.ToString();
            counterDamageStats.text = viewedUnit.defenseDamage.ToString();
        }
    }
    public void moveStatsPanel(Units unit)
    {
        if (unit.Equals(viewedUnit))
        {
            statsPanel.transform.position = (Vector2)unit.transform.position + statsPanelShift;

        }
    }
    public void removeStatsPanel(Units unit)
    {
        if (unit.Equals(viewedUnit))
        {
            statsPanel.SetActive(false);
            viewedUnit = null;
        }
    }
    void EndTurn()
    {
        if (playerTurn == 1)
        {
            playerTurn = 2;
            playerIndicatior.sprite = player2Indicator;
        }else if (playerTurn == 2)
        {
            playerTurn = 1;
            playerIndicatior.sprite = player1Indicator;
        }
        if (selectedUnit != null)
        {
            selectedUnit.selected = false;
            selectedUnit = null;
        }
        Debug.Log("Current Turn: " + playerTurn);

        GetGoldIncome(playerTurn);
        ResetTiles();

        Units[] units = FindObjectsOfType<Units>();

        foreach (Units unit in units)
        {
            unit.attackIcon.SetActive(false);
            unit.hasMoved = false;
            unit.hasAttack = false;
        }

        GetComponent<Barrack>().CloseCharacterCreationMenus();
        if (viewedUnit != null)
        {
            removeStatsPanel(viewedUnit);
        }
       
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
