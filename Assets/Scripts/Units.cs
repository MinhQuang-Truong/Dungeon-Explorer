using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    private Animator camAnim;
    public GameObject deathEffect;
    public DamageIcon damageIcon;

    public Text generalHealth;
    public bool isGeneral;


    public GameObject victoryPanel;
    // Start is called before the first frame update
    void Start()
    {
        ResetAttackIcon();
        gm = FindObjectOfType<GameMaster>();
        camAnim = Camera.main.GetComponent<Animator>();
        UpdateGeneralHealth();
    }
   
    public void UpdateGeneralHealth()
    {
        if (isGeneral)
        {
            generalHealth.text = health.ToString();
        }
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
        camAnim.SetTrigger("shake");
        hasAttack = true;

        int enemyDamage = attackDamage - enemy.armor;
        int myDamage = enemy.defenseDamage - armor;

        if (enemyDamage >= 1)
        {
            enemy.health -= enemyDamage;
            DamageIcon d = Instantiate(damageIcon, enemy.transform.position, Quaternion.identity);
            d.Setup(enemyDamage);
            enemy.UpdateGeneralHealth();
        }

        if (transform.tag == "Range" && enemy.tag != "Range")
        {
            if (Math.Round(Mathf.Abs(transform.position.x - enemy.transform.position.x))+Math.Round( Mathf.Abs(transform.position.y - enemy.transform.position.y) )<= 1) // check is the enemy is near enough to attack
            {
                if (myDamage >= 1)
                {
                    health -= myDamage;
                    DamageIcon d = Instantiate(damageIcon, transform.position, Quaternion.identity);
                    d.Setup(myDamage);
                    UpdateGeneralHealth();
                }
            }
        }
        else
        {
            if (myDamage >= 1)
            {
                health -= myDamage;
                DamageIcon d = Instantiate(damageIcon, transform.position, Quaternion.identity);
                d.Setup(myDamage);
                UpdateGeneralHealth();

            }
        }

        if (enemy.health <= 0)
        {
            if (enemy.isGeneral)
            {
                enemy.victoryPanel.SetActive(true);
            }
            Instantiate(deathEffect, enemy.transform.position, Quaternion.identity);
            gm.removeStatsPanel(enemy);
            Destroy(enemy.gameObject);
            GetWalkableTiles();
        }

        if(health <= 0)
        {
            if (isGeneral)
            {
                victoryPanel.SetActive(true);
            }
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            gm.removeStatsPanel(this);
            gm.ResetTiles();
            GetWalkableTiles();
            Destroy(this.gameObject);
        }
        gm.UpdateStatsPanel();
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
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            gm.ToggleStatsPanel(this);
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
    public void Move(Transform movePos)
    {
        gm.ResetTiles();
        StartCoroutine(StartMovement(movePos));
     
    }
    IEnumerator StartMovement(Transform movePos)
    { // Moves the character to his new position.

        Debug.Log(movePos.position.x + "   " + movePos.position.y);

        while (transform.position.x != Mathf.Round(movePos.position.x))
        { // first aligns him with the new tile's x pos
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(Mathf.Round(movePos.position.x), transform.position.y), moveSpeed * Time.deltaTime);
            yield return null;
        }
        while (transform.position.y != Mathf.Round(movePos.position.y)) // then y
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, Mathf.Round(movePos.position.y)), moveSpeed * Time.deltaTime);
            yield return null;
        }

        hasMoved = true;
        ResetAttackIcon();
        GetEnemies();
        gm.moveStatsPanel(this);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
