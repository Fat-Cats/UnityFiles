using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class unitScript : MonoBehaviour {

    public Transform fightCanvas; //references the "fightCanvas" canvas, which is used to display battles between units
    public GameObject gameMap; //references the "gameMap", which is used to display the map 
    public player owner; //instance of the player class that owns this unit
    public unitType unitType; //an enum representing this unit's class ("Basic", "Jock"...)
    public GameObject sectorStandingOn //stores the sector gameobject on which this unit is currently placed
    {
        get //return the sector that this unit is standing on when requested
        {
            if (this.transform.parent != null) //if this unit is standing on a sector
            {
                return this.transform.parent.gameObject; //return the GameObject of the sector that this unit is standing on
            }
            else //if this unit is not standing on any sector (it could be that this unit has just been instantiated)
            {
                return null;
            }
        }
    }

    //unit's statistics (these change according to a units class)
    public int maxHP; //units maximum hp
    public int attack; //units attack points, used to calculate damage done in battle
    public int defence; //units defense points, used to calculate hp loss in battle
    public double accuracy; //units accuracy points, used to calculate damage done in battle
    public double critical; //units critical points, used to calculate damage done in battle
    public int speed; //units speed points, indicates how many tiles a unit can move in 1 turn

    public int curHP; //unit current health points
    public int currentSpeed; //units remaining moves in a turn

    public Sprite mapImage
    {
        get
        {
            string path = this.unitType.ToString() + "-" + this.owner.collegeRep;
            return Resources.LoadAll(path)[1] as Sprite; //select appropriate sprite.
        }
    }

    public Sprite battleImage
    {
        get
        {
            string path;

            if (this.unitType == unitType.jock)
            {
                path = this.unitType.ToString() + "-" + this.owner.collegeRep;
                return Resources.LoadAll(path)[1] as Sprite; //select appropriate sprite.
            }
            else
            {
                path = this.unitType.ToString() + "A-" + this.owner.collegeRep;
                return Resources.LoadAll(path)[1] as Sprite; //select appropriate sprite.
            }
        }
    }

    public void Init(unitType unitType, player owner, GameObject sector, Transform fightCanvas, GameObject gameMap) //used to initilize values of units
    {
        this.unitType = unitType; //set unit's unitType

        this.fightCanvas = fightCanvas; //set a reference to the "fightCanvas" canvas Transform
        this.gameMap = gameMap; //set a reference to the "gameMap" GameObject

        this.moveUnit(sector); //move this unit into the specified starting sector

        switch (unitType.ToString()) //set unit statistics depending on the unitType
        {
            case "basic": // Basic but cheap unit.
                this.maxHP = 10;
                this.attack = 4;
                this.defence = 0;
                this.accuracy = 0.5;
                this.critical = 1.5;
                this.speed = 1;
                break;
            case "dgirl": // Accurate with some defence.
                this.maxHP = 20;
                this.attack = 4;
                this.defence = 1;
                this.accuracy = 0.75;
                this.critical = 1.5;
                this.speed = 1;
                break;
            case "jock": // Strong but stupid.
                this.maxHP = 25;
                this.attack = 5;
                this.accuracy = 0.25;
                this.defence = 0;
                this.critical = 1.75;
                this.speed = 1;
                break;
            case "sonic": // Fast unit.
                this.maxHP = 20;
                this.attack = 4;
                this.accuracy = 0.5;
                this.critical = 1.75;
                this.defence = 0;
                this.speed = 2;
                break;
            case "old":
                this.maxHP = 20;
                this.attack = 4;
                this.accuracy = 0.5;
                this.critical = 1.5;
                this.defence = 0;
                this.speed = 1;
                break;
        }

        this.currentSpeed = this.speed; //at instantiation currentSpeed is set to speed

        this.owner = owner; //set unit's owner
        this.curHP = maxHP; //at instantiation set current health points to maximum health points
        this.gameObject.transform.SetParent(sector.gameObject.transform); //set this unit's parent to the sector it is placed in

        this.gameObject.GetComponent<SpriteRenderer>().sprite = this.mapImage; //select appropriate sprite.

        this.gameObject.AddComponent<PolygonCollider2D>(); //add a PolygonCollider2D to the unit, so that the unit can be clicked

        this.gameObject.AddComponent<SpriteGlow.SpriteGlow>(); //add a SpriteGlow script to the unit, so that a border can be drawn around the unit when selected
        this.gameObject.GetComponent<SpriteGlow.SpriteGlow>().OutlineWidth = 0; //do not draw border around unit when it is created
        this.gameObject.GetComponent<SpriteGlow.SpriteGlow>().GlowBrightness = 5; //SpriteGlow settings used to ensure borders can be drawn correctly
        this.gameObject.GetComponent<SpriteGlow.SpriteGlow>().AlphaThreshold = 0.5f; //SpriteGlow settings used to assure borders can be drawn correctly
    }

    void OnMouseDown() //called when the user clicks on this gameObject
    {
        gameMap.GetComponent<gameMapScript>().selectedUnit = this.gameObject; //set this unit as the selected unit in the gameMap's gameMapScript 
    }

    public List<GameObject> canAttack() //return a list of units that can be attacked by this unit
    {
        List<GameObject> canAttackUnits = new List<GameObject>();

        List<GameObject> immediateSectors = this.sectorStandingOn.GetComponent<sectorScript>().neighbours;

        List<GameObject> copyList = new List<GameObject>();

        if (this.sectorStandingOn.GetComponent<sectorScript>().sectorID == 28)
        {
            foreach (GameObject sector in immediateSectors)
            {
                if (!sector.GetComponent<sectorScript>().isBusStop || sector.GetComponent<sectorScript>().sectorID == 10)
                {
                    copyList.Add(sector);
                }
            }
        }
        else if (this.sectorStandingOn.GetComponent<sectorScript>().sectorID == 10)
        {
            foreach(GameObject sector in immediateSectors)
            {
                if (!sector.GetComponent<sectorScript>().isBusStop || sector.GetComponent<sectorScript>().sectorID == 28)
                {
                    copyList.Add(sector);
                }
            }
        }
        else if (this.sectorStandingOn.GetComponent<sectorScript>().isBusStop)
        {
            foreach (GameObject sector in immediateSectors)
            {
                if (!sector.GetComponent<sectorScript>().isBusStop)
                {
                    copyList.Add(sector);
                }
            }
        }
        else
        {
            copyList = immediateSectors;
        }

        foreach (GameObject reachableSector in copyList)
        {
            foreach (GameObject containedUnit in reachableSector.GetComponent<sectorScript>().unitsContained)
            {
                if (containedUnit == null)
                {

                }
                else if (containedUnit.GetComponent<unitScript>().owner != this.owner) //if unit in reachable sector is an enemy
                {
                    canAttackUnits.Add(containedUnit);
                }
            }
        }

        return canAttackUnits;
    }

    public List<GameObject> canMoveTo() //return a list of sectors that a unit can move to. This function uses a recursive function "canMoveToRecursive" to calculate which sectors 
    {                                   //a unit can move to (this is neccessary as some units can move over multiple sectors at once so a flood-fill like algorithm is used)

        List<GameObject> canMoveToSectors = new List<GameObject>(); //create list used to store which sectors a unit can move to

        HashSet<GameObject> sectorsThatCanBeMovedTo = canMoveToRecursive(new HashSet<GameObject>(), this.currentSpeed); //use "canMoveToRecursive" to retrieve HashSet of sectors
                                                                                                                        //that can be moved to by this unit. A hashset is used as it
                                                                                                                        //does not allow duplication of elements. This means that I do not
                                                                                                                        //need to micromanage the elements it returns
        //check for full sectors before adding to list 
        foreach (GameObject reachableSector in sectorsThatCanBeMovedTo) //convert "sectorsThatCanBeMovedTo" to a list (just because most other
        {                                                       //functions in this program work with lists rather than HashSets) 

            if (reachableSector.GetComponent<sectorScript>().unitsContained.Contains(null))
            {
                canMoveToSectors.Add(reachableSector);
            }
        }

        return canMoveToSectors; //return list of sectors that can be moved to
    }

    //EDIT THIS FUNCTION TO INCLUDE HOW MANY MOVING POINTS (currentSpeed) MOVING TO ANY TILE REQUIRES
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

                    neighbours.Add(newNeighbour); //add this sector to list of reachable sectors
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
        sectorScript thisSectorScript;

        if (this.sectorStandingOn != null) //if unit has already been placed (might not have been as this function is used to move freshly instantiated units)
        {
            thisSectorScript = this.sectorStandingOn.GetComponent<sectorScript>(); //set a reference to the sectorScript of the sector this unit is standing on

            for (int i = 0; i < 3; i++) //cyle through current sector's unitsContained array to free up space (as this unit is moving)
            {
                if (thisSectorScript.unitsContained[i] == this.gameObject) //if the unit being inspected in unitsContained, remove it from the unitsContained list
                {
                    thisSectorScript.unitsContained[i] = null; //if this unit has moved out of position i, free up space at position i
                    break; //if the moving unit has been found, stop searching the unitsContained array for that unit
                }
            }
        }

        this.gameObject.transform.SetParent(destinationSector.gameObject.transform); //change the sector that this unit is standing on

        thisSectorScript = this.sectorStandingOn.GetComponent<sectorScript>(); //set a reference to the sectorScript of the sector this unit is newly standing on

        Vector3 currentPosition = new Vector3(); //create new vector to store new unit coordinates before applying them to unit

        for (int i = 0; i < 3; i++) //find free standingPoint to move to in destination sector 
        {
            if (thisSectorScript.unitsContained[i] == null) //if a free standingPoint is found
            {
                currentPosition = thisSectorScript.standingPoints[i]; //place unit at the empty standingPoint of the newly moved to sector 
                thisSectorScript.unitsContained[i] = this.gameObject; //list this unit at appropriate place in the newly moved to sector's unitsContained array
                break; //once the unit has been correctly assigned to a standing point in the newly moved to sector, stop searching
            }
        }

        this.gameObject.GetComponent<Transform>().position = currentPosition; //apply new coordinates to unit

    }

    public void attackUnit(GameObject unitToAttack) //This function is called when a player clicks on an enemy unit, to attack it
    {
        fightCanvas.GetComponent<battleAnimationScript>().fightAnimation(this.gameObject, unitToAttack); //start battle animation
    }

    public void killIfDead()
    {
        if (this.curHP <= 0) //if this unit's current health points are at 0 or lower, delete the unit
        {
            GameObject[] containedUnits = this.sectorStandingOn.GetComponent<sectorScript>().unitsContained; //fetch list of units standing on
                                                                                                             //the sector that this unit is
                                                                                                             //standing on
            
            for (int positionIndex = 0; positionIndex < 3; positionIndex++) //cycle through units contained
            {
                if (containedUnits[positionIndex] == this.gameObject) //remove this unit from units contained when found
                {
                    this.transform.parent.gameObject.GetComponent<sectorScript>().unitsContained[positionIndex] = null;
                    break;
                }
            }

            Destroy(gameObject); //destroy this unit
        }
    }
}
