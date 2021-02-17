using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Units : MonoBehaviour
{
    public bool selected;
    GameMaster gm;
    public int tileSpeed;
    public float moveSpeed;
    public bool hasMoved;


    public int playerNumber;

    public int attackRange;
    List<Units> enemiesInRange = new List<Units>();
    public bool hasAttack;
    public GameObject attackIcon;

    //Unit stats
    public int health;
    public int attackDamage;
    public int defenseDamage;
    public int armor;


    // Start is called before the first frame update
    void Start()
    {
        ResetAttackIcon();
        gm = FindObjectOfType<GameMaster>();
    }

    private void OnMouseDown()
    {
        ResetAttackIcon();

        if(selected == true)
        {
            selected = false;
            gm.selectedUnit = null;
            gm.ResetTiles();
        }
        else
        {
            if (playerNumber == gm.playerTurn) {
                if (gm.selectedUnit != null)
                {
                    gm.selectedUnit.selected = false;
                }
                selected = true;
                gm.selectedUnit = this;
                gm.ResetTiles();
                GetEnemies();
                GetWalkableTiles();
            }
            
        }

        Collider2D col = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition),0.15f);
        Units enemy = col.GetComponent<Units>();
        if (gm.selectedUnit != null){
            if (gm.selectedUnit.enemiesInRange.Contains(enemy)&& !gm.selectedUnit.hasAttack)
            {
                gm.selectedUnit.Attack(enemy);
            }
        }

    }

    void Attack(Units enemy)
    {
        hasAttack = true;

        int enemyDamage = attackDamage - enemy.armor;
        int myDamage = enemy.defenseDamage - armor;

        if (enemyDamage >= 1)
        {
            enemy.health -= enemyDamage;
            Debug.Log("Player: "+enemy.playerNumber+" health: " + enemy.health);
        }

        if(myDamage >= 1)
        {
            health -= myDamage;
            Debug.Log("Player: " + playerNumber + " health: " + health);

        }

        if (enemy.health <= 0)
        {
            Destroy(enemy.gameObject);
            GetWalkableTiles();
        }

        if(health <= 0)
        {
            gm.ResetTiles();
            Destroy(this.gameObject);
        }
    }
    void GetWalkableTiles()
    {
        if (hasMoved == true)
        {
            return;
        }

        TileSetting[] tiles = FindObjectsOfType<TileSetting>();
        foreach (TileSetting tile in tiles)
        {
            
            if (Math.Round(Math.Abs(transform.position.x - tile.transform.position.x)) + Math.Round(Math.Abs(transform.position.y - tile.transform.position.y)) <= tileSpeed)
            { // how far he can move
                if (tile.isClear() == true)
                { // is the tile clear from any obstacles
                    tile.Highlight();
                }

            }
        }
    }

    void GetEnemies()
    {
        enemiesInRange.Clear();
        Units[] units = FindObjectsOfType<Units>();
        foreach (Units unit in units)
        {
            if (Math.Round(Math.Abs(transform.position.x - unit.transform.position.x)) + Math.Round(Math.Abs(transform.position.y - unit.transform.position.y)) <= attackRange)
            { 
                //how far unit can attack
                if(unit.playerNumber != gm.playerTurn && !hasAttack)
                {
                    enemiesInRange.Add(unit);
                    unit.attackIcon.SetActive(true);
                }

            }
        }
    }

    public void ResetAttackIcon()
    {
        Units[] units = FindObjectsOfType<Units>();
        foreach (Units unit in units)
        {
            unit.attackIcon.SetActive(false);
        }
    }
    public void Move(Vector2 movePos)
    {
        gm.ResetTiles();
        StartCoroutine(StartMovement(movePos));
    }
    IEnumerator StartMovement(Vector2 movePos)
    { // Moves the character to his new position.


        while (transform.position.x != movePos.x)
        { // first aligns him with the new tile's x pos
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(movePos.x, transform.position.y), moveSpeed * Time.deltaTime);
            yield return null;
        }
        while (transform.position.y != movePos.y) // then y
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, movePos.y), moveSpeed * Time.deltaTime);
            yield return null;
        }

        hasMoved = true;
        ResetAttackIcon();
        GetEnemies();

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
