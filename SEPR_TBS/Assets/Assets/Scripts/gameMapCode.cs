using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameMapCode : MonoBehaviour {

    public Transform sector;
    public Sprite sectorSprite;

	// Use this for initialization
	void Start () {
        
		for (int i = 0; i <= 30; i++) //instantiate 30 sector prefabs, set relevant x and y positions on the map and assign them appropriate sectorID's
        {
            Transform createdSector = Instantiate(sector, getSectorCoordinates(i), Quaternion.identity); //instantiate sector prefab and

            //scale sectors depending on their sector ID (sectors on east and west are scaled differently than originally sized in artwork)
            if (i == 0 || i == 1 || i == 2 || i == 4 || i == 5 || i == 6 || i == 8 || i == 11 || i == 12 || i == 14 || i == 15  || i == 16 || i == 17 || i == 20 || i == 24 || i == 25 || i == 26)
            {
                //scale west sectors
                float scaleFactor = 0.9183525f;
                createdSector.localScale = new Vector3(scaleFactor , scaleFactor, scaleFactor);
            }
            else
            {
                //scale east sectors
                float scaleFactor = 0.8262805f;
                createdSector.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            }

            createdSector.GetComponent<sectorScript>().init(i); //perform additional initilization of sector (pass sectorID, choose sector sprite etc...)
        }
    }
	
	// Update is called once per frame
	void Update () {

    }

    Vector3 getSectorCoordinates(int sectorID) //returns a vector representing the location of a sector specified by sectorID
    {
        Vector3 coordsToReturn = new Vector3(0, 0, 0);
        
        switch (sectorID) //The locations for the sectors were originally hand placed and then their positions were recorded below
        {
            case 1: coordsToReturn = new Vector3(-10.235f, 4.892f, 0); break;
            case 2: coordsToReturn = new Vector3(-6.789f, 5.195f, 0); break;
            case 3: coordsToReturn = new Vector3(7.785001f, -0.362f, 0); break;
            case 4: coordsToReturn = new Vector3(-13.635f, -0.548f, 0); break;
            case 5: coordsToReturn = new Vector3(-7.599f, 1.705f, 0); break;
            case 6: coordsToReturn = new Vector3(-11.214f, 2.909f, 0); break;
            case 7: coordsToReturn = new Vector3(12.364f, -1.072f, 0); break;
            case 8: coordsToReturn = new Vector3(-12.251f, -1.065f, 0); break;
            case 9: coordsToReturn = new Vector3(1.036f, -2.44f, 0); break;
            case 10: coordsToReturn = new Vector3(4.886f, -3.818f, 0); break;
            case 11: coordsToReturn = new Vector3(-9.39f, -4.264f, 0); break;
            case 12: coordsToReturn = new Vector3(-7.858f, -2.541f, 0); break;
            case 13: coordsToReturn = new Vector3(12.841f, 1.158f, 0); break;
            case 14: coordsToReturn = new Vector3(-13.252f, 1.896f, 0); break;
            case 15: coordsToReturn = new Vector3(-9.423f, 2.043f, 0); break;
            case 16: coordsToReturn = new Vector3(-6.203f, 3.529f, 0); break;
            case 17: coordsToReturn = new Vector3(-8.782f, 4.734f, 0); break;
            case 18: coordsToReturn = new Vector3(3.428f, -1.822f, 0); break;
            case 19: coordsToReturn = new Vector3(5.13f, -0.9400001f, 0); break;
            case 20: coordsToReturn = new Vector3(-9.446f, -1.043f, 0); break;
            case 21: coordsToReturn = new Vector3(10.175f, 0.985f, 0); break;
            case 22: coordsToReturn = new Vector3(10.398f, -0.6570001f, 0); break;
            case 23: coordsToReturn = new Vector3(13.61f, -0.6670001f, 0); break;
            case 24: coordsToReturn = new Vector3(-11.541f, -2.473f, 0); break;
            case 25: coordsToReturn = new Vector3(-6.609f, -4.489f, 0); break;
            case 26: coordsToReturn = new Vector3(-11.428f, 0.01499999f, 0); break;
            case 27: coordsToReturn = new Vector3(1.452f, -4.629f, 0); break;
            case 28: coordsToReturn = new Vector3(7.065001f, -3.139f, 0); break;
            case 29: coordsToReturn = new Vector3(2.505f, -3.605f, 0); break;
            case 30: coordsToReturn = new Vector3(10.064f, -2.673f, 0); break;
            case 0: coordsToReturn = new Vector3(-13.613f, 3.799f, 0); break;
        }

        return coordsToReturn;
    }
}
