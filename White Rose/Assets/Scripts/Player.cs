using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public enum Colour { Red, Blue, Green, Yellow, Gray };

    public string playerName;
    public Colour colour;
    public int currency;
    public List<GameObject> ownedSectors;
    public List<GameObject> ownedUnits;

	// Use this for initialization
	void Awake () {
        colour = Colour.Blue;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
