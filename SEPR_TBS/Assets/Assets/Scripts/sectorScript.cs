using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class sectorScript : MonoBehaviour {

    public GameObject unit; //prefab for units
    public List<GameObject> neighbours = new List<GameObject>(); //stores a list of neighbouring sectors
    public GameObject[] unitsContained = new GameObject[3]; //stores all units currently placed in this sector [maximum: 3]
    public Vector3[] standingPoints = new Vector3[3]; //stores vectors representing locations for 3 units to stand in this sector

    public player owner; //::::::::::::::::add getters and setters to change sectors as units move in and out of territories 

    public void init(int sectorID) //this function is used to pass arguments to each sector, after it is instantiated
    {
        Object[] sectorSprites = Resources.LoadAll("mapSectors"); //load all mapSectorSprites 
        this.gameObject.GetComponent<SpriteRenderer>().sprite = (Sprite)sectorSprites[sectorID + 1]; //set current sprite to appropriate sector as indicated by sectorID 
                                                                                           //"mapSectors_" + sectorID.toString()

        this.gameObject.AddComponent<PolygonCollider2D>(); //add polygoncollider component to this sector so that it can be clicked by the user

        this.gameObject.name = "mapSector" + sectorID.ToString(); //set this sectors name to "mapSector" followed by it's sectorID

        //set standing points for this sector
        switch (sectorID)
        {
            case 4: //fine tune standing points for sector 4
                standingPoints[0] = this.gameObject.transform.position;
                standingPoints[0].y += 0.67f;
                standingPoints[0].z = -1;

                standingPoints[1] = this.gameObject.transform.position;
                standingPoints[1].y += 0.17f;
                standingPoints[1].x += -0.2999993f;
                standingPoints[1].z = -2;

                standingPoints[2] = this.gameObject.transform.position;
                standingPoints[2].x += -0.69f;
                standingPoints[2].y += 1.32f;
                standingPoints[2].z = -2;
                break;
            case 26: //fine tune standing points for sector 26
                standingPoints[0] = this.gameObject.transform.position;
                standingPoints[0].y += 0.67f;
                standingPoints[0].z = -1;

                standingPoints[1] = this.gameObject.transform.position;
                standingPoints[1].x += -0.6f;
                standingPoints[1].y += 1.06f;
                standingPoints[1].z = -2;

                standingPoints[2] = this.gameObject.transform.position;
                standingPoints[2].y += 0.16f;
                standingPoints[2].x += 0.34f;
                standingPoints[2].z = -2;
                break;
            case 7: //fine tune standing points for sector 7
                standingPoints[0] = this.gameObject.transform.position;
                standingPoints[0].y += 0.67f;
                standingPoints[0].z = -1;

                standingPoints[1] = this.gameObject.transform.position;
                standingPoints[1].y += 0.17f;
                standingPoints[1].x += -0.2999993f;
                standingPoints[1].z = -2;

                standingPoints[2] = this.gameObject.transform.position;
                standingPoints[2].x += 0.14f;
                standingPoints[2].y += -0.27f;
                standingPoints[2].z = -2;
                break;
            default: //standing points for all other sectors
                standingPoints[0] = this.gameObject.transform.position;
                standingPoints[0].y += 0.67f;
                standingPoints[0].z = -1;

                standingPoints[1] = this.gameObject.transform.position;
                standingPoints[1].y += 0.17f;
                standingPoints[1].x += -0.2999993f;
                standingPoints[1].z = -2;

                standingPoints[2] = this.gameObject.transform.position;
                standingPoints[2].y += 0.16f;
                standingPoints[2].x += 0.34f;
                standingPoints[2].z = -2;
                break;
        }

        for (int i = 0; i < 3; i++) //set all unitsContained values to null because no units should be placed until after a sector is created
        {
            unitsContained[i] = null;
        }
    }

    void OnMouseDown() //when the mouse clicks on this sector
    {
        GameObject theGameMap = GameObject.Find("gameMap"); //grab a reference to the gameMap gameObject
        GameObject theSelectedUnit = theGameMap.GetComponent<gameMapScript>().selectedUnit; //see if a unit was selected before this sector was clicked on

        if (theSelectedUnit != null && theSelectedUnit.GetComponent<unitScript>().canMoveTo().Contains(this.gameObject)) //if a unit has been selected, and this sector 
        {                                                                                                                //has been clicked, move that unit to this sector

            theGameMap.GetComponent<gameMapScript>().selectedUnit = null; //a sector has now been selected, so set the selectedUnit to null
            theSelectedUnit.GetComponent<unitScript>().moveUnit(this.gameObject); //move unit to this sector
        }

    }
}
