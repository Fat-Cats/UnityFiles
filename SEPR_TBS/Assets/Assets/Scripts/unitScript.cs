using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitScript : MonoBehaviour {

    public player owner;
    public string unitType;
    public int uid;

    public int maxHP;
    public int attack;
    public int defence;
    public double accuracy;
    public double critical;
    public int speed;
    public string special;

    public int curHP;

    void Start () {

    }

	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        GameObject theGameMap = GameObject.Find("gameMap"); //switch selected unit variable (stored in gameMap) to this unit
        theGameMap.GetComponent<gameMapCode>().selectedUnit = this.gameObject;
    }

    public List<GameObject> canMoveTo() //return a list of sectors that a unit can move to (which depends on its speed and location). This function handles a recursive function "canMoveToRecursive"
    {
        List<GameObject> canMoveToSectors = new List<GameObject>();    

        foreach (GameObject element in canMoveToRecursive(new HashSet<GameObject>(), this.speed))
        {
            canMoveToSectors.Add(element);
        }

        return canMoveToSectors;
    }

    private HashSet<GameObject> canMoveToRecursive(HashSet<GameObject> checkedSectors, int unitSpeed) //recursive function used in "canMoveTo" so units that can move more than 1 tile per turn can explore their movement options
    {
        checkedSectors.Add(this.transform.parent.gameObject); //add unit's starting sector to list, so it can be explored

        if (unitSpeed > 0) //if a unit still has moves check which sectors it can move to, otherwise return all sectors it can reach
        {
            HashSet<GameObject> neighbours = new HashSet<GameObject>(); //used to track neighbours 

            foreach (GameObject neighbour in checkedSectors ) //foreach neighbour that has been explored...
            {
                foreach (GameObject neighbourOfNeighbour in neighbour.GetComponent<sectorScript>().neighbours) //...explore the neighbours of that neighbour
                {
                    neighbours.Add(neighbourOfNeighbour); //store explored neighbours
                }
            }

            checkedSectors.UnionWith(neighbours); //add neighbours of neighbours to explored list
            checkedSectors.UnionWith( canMoveToRecursive(checkedSectors, unitSpeed - 1) ); //recursively check more neighbours of neighbours
        }

        checkedSectors.Remove(this.transform.parent.gameObject); //remove unit's starting sector 

        return checkedSectors; //return sectors that a unit can reach
    }

    public void moveUnit(GameObject destinationSector)
    {
        GameObject theGameMap = GameObject.Find("gameMap"); //check to see if a unit has been moved to this sector
        theGameMap.GetComponent<gameMapCode>().selectedUnit = null; //a sector has now been selected, so set the selectedUnit to null

        this.gameObject.transform.SetParent(destinationSector.gameObject.transform); //change units owner

        Vector3 currentPosition = this.gameObject.GetComponent<Transform>().position;
        currentPosition = destinationSector.gameObject.transform.position; //place unit on sector 
        currentPosition.y += this.gameObject.transform.localScale.y * 0.5f; //properly place unit on sector (feet on center)
        currentPosition.z -= 1; //draw unit infront of sector
        this.gameObject.GetComponent<Transform>().position = currentPosition;
    }

    public void Init(string unitType, /*player owner,*/ GameObject sector) //used to initilize values of units
    {
        this.gameObject.transform.SetParent(sector.gameObject.transform); //set sector as units owner

        this.gameObject.AddComponent<PolygonCollider2D>(); //so that the unit can be clicked
        this.gameObject.AddComponent<SpriteGlow.SpriteGlow>(); //so that a border can be drawn around the unit when selected
        this.gameObject.GetComponent<SpriteGlow.SpriteGlow>().GlowBrightness = 5;
        this.gameObject.GetComponent<SpriteGlow.SpriteGlow>().AlphaThreshold = 0.5f;

        this.unitType = unitType;

        Vector3 currentPosition = this.gameObject.GetComponent<Transform>().position; 
        currentPosition = sector.gameObject.transform.position; //place unit on sector 
        currentPosition.y += this.gameObject.transform.localScale.y * 0.5f; //properly place unit on sector (feet on center)
        currentPosition.z -= 1; //draw unit infront of sector
        this.gameObject.GetComponent<Transform>().position = currentPosition; 

        switch(unitType)
        {
            case "Basic": // Basic but cheap unit.
                this.maxHP = 10;
                this.attack = 4;
                this.defence = 0;
                this.accuracy = 0.5;
                this.critical = 1.5;
                this.speed = 1;
                this.special = "";
                break;
            case "FellowKid": // Wentworth special. Accurate with some defence.
                this.maxHP = 20;
                this.attack = 4;
                this.defence = 1;
                this.accuracy = 0.75;
                this.critical = 1.5;
                this.speed = 1;
                this.special = "";
                break;
            case "Jock": // Strong but stupid.
                this.maxHP = 25;
                this.attack = 5;
                this.accuracy = 0.25;
                this.defence = 0;
                this.critical = 1.75;
                this.speed = 1;
                this.special = "";
                break;
            case "Sonic": // Fast as fuck BOI.
                this.maxHP = 20;
                this.attack = 4;
                this.accuracy = 0.5;
                this.critical = 1.75;
                this.defence = 0;
                this.speed = 2;
                this.special = "";
                break;
            case "Daddy's Girl": // Two huge personalities. Income unit.
                this.maxHP = 20;
                this.attack = 4;
                this.accuracy = 0.5;
                this.critical = 1.5;
                this.defence = 0;
                this.speed = 1;
                this.special = "income";
                break;
        }

        this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll("scrub")[1] as Sprite ; //CHANGE ME to select appropriate sprite when art is done.

        curHP = maxHP;
    }
}
