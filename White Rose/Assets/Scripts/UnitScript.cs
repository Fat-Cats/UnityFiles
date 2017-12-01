using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    public GameObject background;
    public GameObject gameManager;
    // public script backgroundScript;
    public GameManagerScript gameManagerScript;
    public bool animating;
    private string unitType;
    private int uid;
    private int maxHP;
    private int curHP;
    private int attack;
    private int defence;
    private string special;
    private double accuracy;
    private double critical;
    private int speed;
    private GameObject sector; //placeholder, should be private Sector position;
    private GameObject owner;    


    // Use this for initialization
    void Awake()
    {
        //background = GameObject.Find("Background");
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManagerScript>();
        //backgroundScript = background.GetComponent<script>();
    }

    public int getUid()
    {
        return uid;
    }

    public int getMaxHP()
    {
        return maxHP;
    }

    public int getCurHP()
    {
        return curHP;
    }

    public int getAttack()
    {
        return this.attack;
    }

    public int getDefence()
    {
        return defence;
    }

    public string getSpecial()
    {
        return special;
    }

    public double getAccuracy()
    {
        return accuracy;
    }

    public double getCritical()
    {
        return critical;
    }

    public int getSpeed()
    {
        return speed;
    }

    // GET POSITION
    // GET PLAYER


    public void Init(string unitType, int uid, GameObject sector, GameObject owner) //placeholder, should be Sector position, Player owner
    {
        this.uid = uid;
        this.sector = sector;
        this.owner = owner;
        this.unitType = unitType;

        if (unitType == "Basic") // Basic but cheap unit.
        {
            maxHP = 10;
            attack = 4;
            defence = 0;
            accuracy = 0.5;
            critical = 1.5;
            speed = 1;
            special = "";
        }
        else if (unitType == "FellowKid") // Wentworth special. Accurate with some defence.
        {
            maxHP = 20;
            attack = 4;
            defence = 1;
            accuracy = 0.75;
            critical = 1.5;
            speed = 1;
            special = "";
        }
        else if (unitType == "Jock") // Strong but stupid.
        {
            maxHP = 25;
            attack = 5;
            accuracy = 0.25;
            defence = 0;
            critical = 1.75;
            speed = 1;
            special = "";
        }
        else if (unitType == "Sonic") // Fast as fuck BOI.
        {
            maxHP = 20;
            attack = 4;
            accuracy = 0.5;
            critical = 1.75;
            defence = 0;
            speed = 2;
            special = "";
        }
        else if (unitType == "Daddy's Girl") // Two huge personalities. Income unit.
        {
            maxHP = 20;
            attack = 4;
            accuracy = 0.5;
            critical = 1.5;
            defence = 0;
            speed = 1;
            special = "income";
        }
        curHP = maxHP;
    }

    public void ReduceHealth(int damage)
    {
        if (damage < curHP)
        {
            this.curHP = this.curHP - damage;
        }
        else
        {
            this.kill();
        }
    }

    public void kill()
    {
        // TBD
    }


    // Update is called once per frame
    void Update()
    {
        //animating = backgroundScript.animate;

    }
}
