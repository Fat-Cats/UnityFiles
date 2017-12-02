using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sectorScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void init (int sectorID) //this function is used to pass arguments to each sector, after it is instantiated
    {
        Object[] aay = Resources.LoadAll("mapSectors"); //load all mapSectorSprites 
        this.gameObject.GetComponent<SpriteRenderer>().sprite = (Sprite)aay[sectorID+1]; ////set current sprite to appropriate sector as indicated by sectorID
    }

    public void sectorBorder(Color borderColour, int borderWidth) //used to change a sectors border color and width (such as when a sector is selected etc...)
    {
        this.gameObject.GetComponent<SpriteGlow.SpriteGlow>().GlowColor = borderColour;
        this.gameObject.GetComponent<SpriteGlow.SpriteGlow>().OutlineWidth = borderWidth;
    }
}
