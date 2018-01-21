using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class gameMapScript : MonoBehaviour
{
    public Transform fightCanvas; //references the "fightCanvas" canvas, which is used to display battles between units (set in the unity editor)
    public Transform buyUnitCanvas; //references the "buyUnitCanvas" canvas, which is used to purchase units (set in the unity editor)

    public Transform sector; //prefab for sector used to instantiate sectors (set in the unity editor)
    public GameObject unit; //prefab for unit used to instantiate units (set in the unity editor)
    public GameObject building; //prefab for unit used to instantiate units (set in the unity editor)

    public List<GameObject> sectors = new List<GameObject>(); //lists all sectors in the game 
    private GameObject _selectedUnit; //needed to store intermediate values during getter and setter use
    public GameObject selectedUnit //this variable stores the last unit that has been clicked on (selected) by a player
    {
        get { return this._selectedUnit; } //return selectedUnit value when requested

        set //when a new unit is selected, this code highlights that unit and un-highlights the previously selected unit
        {   //this setter also detects when 2 units have been selected at the same time and, if both units are not on the same team, start a battle

            unitScript _selectedUnitScript; //this is used to store the _selectedUnit's unitScript (so that ".getComponent..." does not need to be used at every reference)
            unitScript valueUnitScript; //this is used to store the value's unitScript 
            SpriteGlow.SpriteGlow _selectedGlowScript; //this is used to store the _selectedUnit's SpriteGlow script 

            if (value != null && _selectedUnit != null) //check if a new unit has been selected whilst another unit is selected
            {
                _selectedUnitScript = _selectedUnit.GetComponent<unitScript>(); //if the old selected unit (_selectedUnit) is not null, set its unitScript
                valueUnitScript = value.GetComponent<unitScript>(); //if the newly selected unit (value) is not null, set its unitScript

                //if a unit has been selected, and an opponents unit is then selected then they will engage in battle (if they are in range of eachother)
                if (_selectedUnitScript.owner != valueUnitScript.owner)
                {
                    //given that the selected unit can move to a newly selected enemy unit, battle commences 
                    if (_selectedUnitScript.canAttack().Contains(value))
                    {
                        _selectedUnitScript.attackUnit(value); //previously selected unit attacks newly selected unit
                    }
                    else //if a unit has chosen to attack an out of range enemy unit, unselect that unit
                    {
                        selectedUnit = null; //unselect current unit
                        Debug.Log("that unit is out of range");
                    }

                    value = null; //new value is set to null, so that whether or not a battle takes place both units are unselected
                }
            }

            if (_selectedUnit != null) //if a unit has previously been selected, remove its borders before changing selectedUnit's value
            {
                _selectedUnitScript = _selectedUnit.GetComponent<unitScript>(); //if the old selected unit (_selectedUnit) is not null, set its unitScript
                _selectedGlowScript = _selectedUnit.GetComponent<SpriteGlow.SpriteGlow>(); //if the old selected unit (_selectedUnit) is not null, set its SpriteGlow script

                _selectedGlowScript.OutlineWidth = 0; //remove old selected unit border

                foreach (GameObject sect in _selectedUnitScript.canMoveTo()) //remove old selected unit's "can move to" indicators
                {
                    sect.gameObject.GetComponent<SpriteGlow.SpriteGlow>().OutlineWidth = 0;
                }

                foreach (GameObject unitToAttack in selectedUnit.GetComponent<unitScript>().canAttack()) //add new "can attack" indicators
                {
                    unitToAttack.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                }
            }

            _selectedUnit = value; //set new selectedUnit value

            if (_selectedUnit != null) //if the new unit is not null, highlight it and the sectors to which it can move
            {
                _selectedUnitScript = _selectedUnit.GetComponent<unitScript>(); //if the newly selected unit (_selectedUnit) is not null, set its unitScript
                _selectedGlowScript = _selectedUnit.GetComponent<SpriteGlow.SpriteGlow>(); //if the newly selected unit (_selectedUnit) is not null, set its SpriteGlow script

                _selectedGlowScript.OutlineWidth = 3; //draw border around unit
                _selectedGlowScript.GlowColor = _selectedUnitScript.owner.teamColour; //color unit's border using it's team's colour

                foreach (GameObject sect in selectedUnit.GetComponent<unitScript>().canMoveTo()) //add new selected unit's "can move to" indicators
                {
                    sect.gameObject.GetComponent<SpriteGlow.SpriteGlow>().OutlineWidth = 3;
                    sect.gameObject.GetComponent<SpriteGlow.SpriteGlow>().GlowColor = _selectedUnitScript.owner.teamColour;
                }

                foreach (GameObject unitToAttack in selectedUnit.GetComponent<unitScript>().canAttack()) //add new "can attack" indicators
                {
                    unitToAttack.GetComponent<SpriteRenderer>().color = Color.red;
                }

            }
        }
    }

    public void gameMapStart()
    {
        fightCanvas.GetComponent<battleAnimationScript>().start();

        for (int i = 0; i <= 30; i++) //instantiate 30 sector prefabs, set relevant x and y positions on the map and assign appropriate sectorID's
        {
            Transform createdSector = Instantiate(sector, new Vector3(0, 0, 0), Quaternion.identity); //instantiate new sector and set its position according to
                                                                                                      //"getSectorCoordinates(i)" which stores sector positions
                                                                                                      //according to sectorID's

            createdSector.GetComponent<sectorScript>().init(i, this.gameObject); //perform additional initilization of sector (choose sector sprite, specify/set standing points etc...)
            //"this.gameObject" is passed so that each sector can make a reference of the gameMap script, so that it can do things like access the "sectors" list

            sectors.Add(createdSector.gameObject); //add this sector to list of sectors stored in gameMapCode

        }

        foreach (GameObject sector in sectors) //now all sectors have been created, loop through them again and add a list of neighbours to each (this has to be done after
        {                                      //all sectors have been created or references to sectors that do not yet exist may be used as neighbours)

            sector.GetComponent<sectorScript>().setSectorNeighbours(); //set sector neighbours according to "setSectorNeighbours()"
                                                                       //which sets a sectors neighbour list according to it's sectorID
        }

        //instantiate and place buildings
        createBuilding("CentralHall", sectors[15]);
        createBuilding("HesHall", sectors[20]);
        createBuilding("JBMorrell", sectors[17]);
        createBuilding("Nisa", sectors[6]);
        createBuilding("RCH", sectors[10]);
        createBuilding("SportsVillage", sectors[13]);
        createBuilding("track", sectors[24]);
        createBuilding("Nisa", sectors[25]);
        //need to place vice chancelor, as a building, randomly at the start of each game

        //create units for testing
        createUnit(unitType.basic, GetComponentInParent<gameMainScript>().playerList[0], sectors[0]);
        createUnit(unitType.basic, GetComponentInParent<gameMainScript>().playerList[2], sectors[2]);
        createUnit(unitType.basic, GetComponentInParent<gameMainScript>().playerList[3], sectors[3]);
        createUnit(unitType.basic, GetComponentInParent<gameMainScript>().playerList[4], sectors[4]);
        createUnit(unitType.basic, GetComponentInParent<gameMainScript>().playerList[1], sectors[1]);
        createUnit(unitType.basic, GetComponentInParent<gameMainScript>().playerList[5], sectors[5]);
        createUnit(unitType.basic, GetComponentInParent<gameMainScript>().playerList[6], sectors[6]);
        createUnit(unitType.basic, GetComponentInParent<gameMainScript>().playerList[7], sectors[7]);
        createUnit(unitType.basic, GetComponentInParent<gameMainScript>().playerList[8], sectors[8]);
        
        this.selectedUnit = null; //at the start of the game, no units are selected

        fightCanvas.gameObject.SetActive(false); //make fight canvas not visible at start of game (will be made visible during battles)
        buyUnitCanvas.gameObject.SetActive(false); //make unit buying canvas not visible at start of game (will be made visible when a building is clicked)
        buyUnitCanvas.GetComponent<unitCanvasScript>().warningMessage.gameObject.SetActive(false);
    }

    public void createUnit(unitType unitType, player owner, GameObject sectorToSpawn) //this function creates a unit of type "unitType" at "sectorToSpawn" and sets it's owner to "owner"
    {
        GameObject newUnit =  Instantiate(unit, new Vector3(0, 0, 0), Quaternion.identity);  //instantate unit prefab
        newUnit.GetComponent<unitScript>().Init(unitType, owner, sectorToSpawn, fightCanvas, this.gameObject); //perform unit initialization
        //fight canvas is passed so that any unit can start playing the battle animation
    }

    private void createBuilding(string buildingType, GameObject sector) //this function creates a building of type "buildingType" at "sector"
    {
        GameObject newBuilding = Instantiate(building, new Vector3(0, 0, 0), Quaternion.identity); //instantiate building prefab
        newBuilding.GetComponent<buildingScript>().Init(buildingType, sector, this.buyUnitCanvas, this.gameObject); //perform building initialization
    }
}
