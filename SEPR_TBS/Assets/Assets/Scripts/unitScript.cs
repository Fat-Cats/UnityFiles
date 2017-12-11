using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitScript : MonoBehaviour {

    public player owner; //class player that owns this unit
    public string unitType; //a string representing this unit's class ("Basic", "Jock"...)
    public int uid; //unique unit identifier
    
    //unit's statistics (these change according to a units class)
    private int maxHP; //units maximum hp
    private int attack; //units attack points, used to calculate damage done in battle
    private int defence; //units defense points, used to calculate hp loss in battle
    private double accuracy; //units accuracy points, used to calculate damage done in battle
    private double critical; //units critical points, used to calculate damage done in battle
    private int speed; //units speed points, indicates how many tiles a unit can move in 1 turn
    private string special; //units special attack
    public int curHP; //units current health points

    void OnMouseDown() //called when the user clicks on this gameObject
    {
        GameObject theGameMap = GameObject.Find("gameMap"); //switch selected unit variable (stored in gameMap) to this unit
        theGameMap.GetComponent<gameMapScript>().selectedUnit = this.gameObject; 
    }

    public int GetCurHP()
    {
        return curHP;
    } 

    public int GetAttack()
    {
        return attack;
    }

    public int GetDefence()
    {
        return defence;
    }

    public double GetAccuracy()
    {
        return accuracy;
    }

    public double GetCritical()
    {
        return critical;
    }

    public int GetSpeed()
    {
        return speed;
    }
    public void Damage(int damage)
    {      
        curHP = curHP - damage;     
    }

    public void Kill()
    {

    }

    public List<GameObject> canMoveTo() //return a list of sectors that a unit can move to. This function uses a recursive function "canMoveToRecursive" to calculate which sectors 
    {                                   //a unit can move to (this is neccessary as some units can move over multiple sectors at once so a flood-fill like algorithm is used)

        List<GameObject> canMoveToSectors = new List<GameObject>(); //create list used to store which sectors a unit can move to

        HashSet<GameObject> sectorsThatCanBeMovedTo = canMoveToRecursive(new HashSet<GameObject>(), this.speed); //use "canMoveToRecursive" to retrieve HashSet of sectors
                                                                                                                 //that can be moved to by this unit. A hashset is used as it
                                                                                                                 //does not allow duplication of elements. This means that I do not
                                                                                                                 //need to micromanage the elements it returns

        foreach (GameObject element in sectorsThatCanBeMovedTo) //convert "sectorsThatCanBeMovedTo" to a list (just because most other functions in this program work with lists) 
        {
            canMoveToSectors.Add(element);
        }

        return canMoveToSectors; //return list of sectors that can be moved to
    }

    private HashSet<GameObject> canMoveToRecursive(HashSet<GameObject> checkedSectors, int unitSpeed) //recursive function used in "canMoveTo" calculates hashSet of sectors that
    {                                                                                                 //this unit can move to from its current position (explores choices for units

        checkedSectors.Add(this.transform.parent.gameObject); //add this unit's starting sector to HashSet, so it can be explored

        if (unitSpeed > 0) //if a unit still has moves, check which sectors it can move to from currently explored sectors, otherwise return all sectors it can reach
        {
            HashSet<GameObject> neighbours = new HashSet<GameObject>(); //used to store neighbours of currently explored sectors

            foreach (GameObject reachableSector in checkedSectors ) //foreach sector that has been explored (I.E: "that can be reached with remaining movement points")...
            {
                foreach (GameObject newNeighbour in reachableSector.GetComponent<sectorScript>().neighbours) //...explore the neighbours of that sector
                {
                    //now that new sectors that can be reached "newNeighbour" have been found, we need to determine if they can be moved to (I.E: if they are already full of units)

                    bool sectorIsAtCapacity = true; //assume that this unit has no spaces left for this unit

                    for (int i = 0; i < 3; i++) //check each value stored in this sector's "unitsContained" array
                    {
                        if (newNeighbour.gameObject.GetComponentInParent<sectorScript>().unitsContained[i] == null ) //if this array contains a null value (I.E: free space) it can be moved to
                        {
                            sectorIsAtCapacity = false; //this sector can be moved to as it has free spaces
                        }
                        /*else if (this.gameObject.GetComponentInParent<sectorScript>().unitsContained[i].GetComponent<unitScript>().owner != this.owner)
                        {
                            sectorIsAtCapacity = true; //this
                            break;
                        }*/ //was used to stop 2 units from opposite teams moving into the same sector but a unit should be able to move into the sector of an enemy, it will then
                        //start a battle
                    }

                    if (!sectorIsAtCapacity) //if this sector can be moved to (if this sector is not full)
                    {
                        neighbours.Add(newNeighbour); //add this sector to list of reachable sectors
                    }
                }
            }

            checkedSectors.UnionWith(neighbours); //add newly reached sectors to list of explored sectors
            checkedSectors.UnionWith( canMoveToRecursive(checkedSectors, unitSpeed - 1) ); //recursively check more neighbours of neighbours
        }

        checkedSectors.Remove(this.transform.parent.gameObject); //remove unit's starting sector as it cannot be moved to, only remained in

        return checkedSectors; //return sectors that a unit can reach
    }

    public void moveUnit(GameObject destinationSector) //function to move a unit from its current position to a new sector
    {
        //GameObject theGameMap = GameObject.Find("gameMap"); //check to see if a unit has been moved to this sector
        //theGameMap.GetComponent<gameMapCode>().selectedUnit = null; //a sector has now been selected, so set the selectedUnit to null
        //was used to set selectedUnit to null but it probably not neccessary as this is handled when a sector is clicked

        for (int i = 0; i < 3; i++) //cyle through current sector's unitsContained array to free up space (as this unit is moving)
        {
            if (this.gameObject.transform.parent != null) //if unit has already been placed (might not have been as this function is used to move freshly instantiated units)
            {
                if (this.GetComponentInParent<sectorScript>().unitsContained[i] == this.gameObject)
                {
                    this.GetComponentInParent<sectorScript>().unitsContained[i] = null; //if this unit has moved out of position i, free up space at position i
                    break;
                }
            }
        }

        this.gameObject.transform.SetParent(destinationSector.gameObject.transform); //change units owner

        Vector3 currentPosition = new Vector3(); //create new vector to store new unti coordinates before applying them to unit

        for (int i = 0; i < 3; i++) //find free standingPoint to move to in destination sector 
        {
            if (destinationSector.GetComponent<sectorScript>().unitsContained[i] == null) //if a free standingPoint is found
            {
                currentPosition = destinationSector.GetComponent<sectorScript>().standingPoints[i]; //place unit at free standingPoint sector 
                destinationSector.GetComponent<sectorScript>().unitsContained[i] = this.gameObject; //list this unit at appropriate place in destination sector's unitsContained array
                break;
            }
        }

        this.gameObject.GetComponent<Transform>().position = currentPosition; //apply new coordinates to unit

    }

    public void Init(string unitType, player owner, GameObject sector) //used to initilize values of units
    {
        this.gameObject.AddComponent<PolygonCollider2D>(); //so that the unit can be clicked
        this.gameObject.AddComponent<SpriteGlow.SpriteGlow>(); //so that a border can be drawn around the unit when selected
        this.gameObject.GetComponent<SpriteGlow.SpriteGlow>().GlowBrightness = 5; //SpriteGlow settings used to assure borders can be drawn correctly
        this.gameObject.GetComponent<SpriteGlow.SpriteGlow>().AlphaThreshold = 0.5f; //SpriteGlow settings used to assure borders can be drawn correctly

        gameMainScript gameManager = GameObject.Find("gameMain").GetComponent<gameMainScript>(); // access to gameManager

        this.moveUnit(sector);

        uid = gameManager.unitList.Count; // PLACEHOLDER
        gameManager.unitList.Add(this.gameObject); // PLACEHOLDER

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

        this.owner = owner; //set units owner
        this.unitType = unitType; //set unitType
        this.curHP = maxHP; //set current health points to maximum health points as this unit was just created
        this.gameObject.transform.SetParent(sector.gameObject.transform); //set this unit's parent to the sector it is placed in

        this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll("scrub")[1] as Sprite ; //CHANGE ME to select appropriate sprite when art is done.
    }
}
