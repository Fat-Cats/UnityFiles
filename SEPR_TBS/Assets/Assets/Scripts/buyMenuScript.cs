using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class buyMenuScript : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    public Transform attackCanvas; //needed to instantiate unit
    public Transform buyMenu;
    public Transform gameMain;
    public GameObject unit; //prefab for units (neccessary to point unity to the correct prefab)

    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponent<Image>().color = new Color(0, 0, 0, 0.6f);

        //GameObject buyMenu = GameObject.Find("buyUnitCanvas"); //grab a reference to the buyUnitCanvas gameObject
        //GameObject gameMain = GameObject.Find("gameMain"); 
        GameObject sectorToSpawn = buyMenu.GetComponent<unitCanvasScript>().spawnSector;
        Instantiate(unit, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<unitScript>().Init("Basic", gameMain.GetComponent<gameMainScript>().playerList[1], sectorToSpawn, attackCanvas.gameObject);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = new Color(0, 0, 0, 0.1960f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = new Color(0, 0, 0, 0.3098f);
    }

    // Use this for initialization
    void Start () {
        //this.gameObject.AddComponent<PolygonCollider2D>(); //so that the unit can be clicked
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
