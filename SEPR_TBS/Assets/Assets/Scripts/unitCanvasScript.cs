using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitCanvasScript : MonoBehaviour {

    public GameObject gameMain; //this is a reference to the gameMain gameObject. it allows us to access players so that owners can be assigned to newly created players.
                                //it also allows us to access the "createUnit" function in its child gameobject "gameMapScript" (set in the unity editor)

    public GameObject spawnSector; //this is set when a building is clicked. The building sets "spawnSector" to the sector that it is placed on.
                                   //this is so that when the user selects a unit to spawn, we know where to create that unit (at the building).

    public Transform warningMessage; //this is a reference to the warningMessage gameObject. This is used to display error messages when purchasing units (set in the unity editor)

    public void setSpawnPoint (GameObject spawnSector) //this is called from a building (when clicked) to indicate which sector to spawn any purchased units on
    {
        this.spawnSector = spawnSector;
    }

    public IEnumerator spawnUnit(string unitType) //this function spawns a unit, of type unitType, in sector spawningSector if there is available space
    {
        bool isSpace = false; //this bool is used to keep track of when an empty space in the sector is found

        for (int positionIndex = 0; positionIndex < 3; positionIndex++) //search all possible standing positions on spawning sector too see if the sector is full
        {
            if (spawnSector.GetComponent<sectorScript>().unitsContained[positionIndex] == null && !isSpace) //if an empty space is found, and has not been found before
            {                                                                                               //as if one has already been found then the unit has already been
                                                                                                            //placed and duplicates need to be avoided 

                gameMainScript GameMainScript = gameMain.GetComponent<gameMainScript>(); //set reference to gameMain's gameMainScript
                gameMapScript GameMapScript = gameMain.transform.GetChild(0).GetComponent<gameMapScript>(); //set reference to gameMap's gameMapScript

                GameMapScript.createUnit(unitType, GameMainScript.currentPlayer, spawnSector); //use the "createUnit" funtion in the gameMapScript to spawn a new unit

                isSpace = true; //stop searching for spaces
            }
        }

        if (!isSpace) //if no space was found for the unit to stand on the sector show a warning message to the user
        {
            warningMessage.gameObject.SetActive(true); //show warning message

            yield return new WaitForSeconds(2); //show message for 2 seconds

            warningMessage.gameObject.SetActive(false); //stop showing warning message
        }
    }
}
