using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class sectorScript : MonoBehaviour {

    public GameObject unit; //prefab for units
    public List<GameObject> neighbours = new List<GameObject>();

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}


    //testing ability to generate sector borders on user input
    /*void OnMouseEnter()
    {
        //this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        this.sectorBorder(new Color(0,0,0), 3);
    }



    void OnMouseExit()
    {
        this.sectorBorder(Color.white, 0);
        //this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
    */
    void OnMouseDown()
    {
        GameObject theGameMap = GameObject.Find("gameMap"); //check to see if a unit has been moved to this sector
        GameObject theSelectedUnit = theGameMap.GetComponent<gameMapCode>().selectedUnit;

        if (theSelectedUnit != null && theSelectedUnit.GetComponent<unitScript>().canMoveTo().Contains(this.gameObject)) //if a unit has been selected, and then this sector clicked, move that unit to this sector
        {
            theSelectedUnit.GetComponent<unitScript>().moveUnit(this.gameObject);
        }

        theGameMap.GetComponent<gameMapCode>().selectedUnit = null; //a sector has now been selected, so set the selectedUnit to null

    }

    public void init (int sectorID) //this function is used to pass arguments to each sector, after it is instantiated
    {
        Object[] aay = Resources.LoadAll("mapSectors"); //load all mapSectorSprites 
        this.gameObject.GetComponent<SpriteRenderer>().sprite = (Sprite)aay[sectorID+1]; ////set current sprite to appropriate sector as indicated by sectorID

        this.gameObject.AddComponent<PolygonCollider2D>();
    }
}
