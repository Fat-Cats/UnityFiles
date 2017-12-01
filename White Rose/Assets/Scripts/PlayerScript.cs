using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public enum Colour { Red, Blue, Green, Yellow, Gray };

    public string playerName;
    public Colour colour;
    public int currency;
    public List<int> ownedSectors;
    public List<int> ownedUnits;

	// Use this for initialization
	void Awake () {
 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
