using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{

    public List<GameObject> unitList;
    public List<GameObject> sectorList;
    public int counter;
    public int positionX;
    public int positionY;
    public GameObject unit;
    public GameObject temp;



    // Use this for initialization
    void Start()
    {
        positionX = 0;
        positionY = 0;
        counter = 0;
    }

    void CreateUnit(string unitType, GameObject sector, GameObject player)                                                              // should turn unitType to enum. Or could just catch errors.
     {
         temp = Instantiate(unit, sector.GetComponent<SectorScript>().getVector3(), Quaternion.identity);
         temp.GetComponent<UnitScript>().Init(unitType, counter, sector, player);                                                             // Placeholder (unitType, uid, sector, player)
         unitList.Add(temp);
     }
    
    public void Battle(GameObject unit1, GameObject unit2) // Take two units as input, unit1 aggressor, unit2 defender. Damage unit2 by the attack stat of unit1, more detailed mechanics to be implemented later. 
    {
        UnitScript unit1Script = unit1.GetComponent<UnitScript>();
        UnitScript unit2Script = unit2.GetComponent<UnitScript>();
        unit2Script.ReduceHealth(unit1Script.getAttack());
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            temp = Instantiate(unit, new Vector3(positionX, positionY, 1), Quaternion.identity);
            temp.GetComponent<UnitScript>().Init("Basic", counter, GameObject.Find("Sector1"), GameObject.Find("Player1"));
            unitList.Add(temp);
            positionX = positionX + 1;
            counter = counter + 1;

        }
        else if (Input.GetKeyDown("q"))
        {
            temp = Instantiate(unit, new Vector3(positionX, positionY, 1), Quaternion.identity);
            temp.GetComponent<UnitScript>().Init("FellowKid", counter, GameObject.Find("Sector2"), GameObject.Find("Player2"));
            unitList.Add(temp);
            positionX = positionX + 1;
            counter = counter + 1;
        }
    }
}
