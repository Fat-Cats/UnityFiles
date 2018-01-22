//HEADER
//game executable can be found at: https://drive.google.com/open?id=18i6A5XMkF-5kVlz-RSsyYvMSnSwSpPnT
//HEADER

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class buyMenuScript : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    private Transform buyUnitCanvas; //reference to buyUnitCanvas
    public string unitTypeRep; //this string represents the unit type that is spawned when this character divider is clicked (Set in the unity editor)

    public void OnPointerClick(PointerEventData eventData) //when a character divider is clicked, change it's colour and attempt to spawn a unit (may not spawn due to)
    {                                                      //insufficient funds or not enough space on the spawn sector

        GetComponent<Image>().color = new Color(0, 0, 0, 0.6f); //change character divider colour

        //indicate to the buyUnitCanvas to spawn a unit (the type of which is unitTypeRep)
        //this function must be called with "StartCoroutine" as a time delay will be propogated should a warning arise
        
        switch(unitTypeRep)
        {
            case "basic": StartCoroutine(buyUnitCanvas.GetComponent<unitCanvasScript>().spawnUnit(unitType.basic)); break;
            case "dgirl": StartCoroutine(buyUnitCanvas.GetComponent<unitCanvasScript>().spawnUnit(unitType.dgirl)); break;
            case "jock": StartCoroutine(buyUnitCanvas.GetComponent<unitCanvasScript>().spawnUnit(unitType.jock)); break;
            case "old": StartCoroutine(buyUnitCanvas.GetComponent<unitCanvasScript>().spawnUnit(unitType.old)); break;
            case "sonic": StartCoroutine(buyUnitCanvas.GetComponent<unitCanvasScript>().spawnUnit(unitType.sonic)); break;
            default: Debug.Log("that unitType is not recognised"); break;
        }

    }

    public void OnPointerEnter(PointerEventData eventData) //when this character divider is hovered over, adjust its colour
    {
        GetComponent<Image>().color = new Color(0, 0, 0, 0.1960f);
    }

    public void OnPointerExit(PointerEventData eventData) //when this character divider is not hovered over, adjust its colour
    {
        GetComponent<Image>().color = new Color(0, 0, 0, 0.3098f);
    }

    void Start()
    {
        this.buyUnitCanvas = (this.transform.parent).parent; //set reference to buyUnitCanvas
    }
}
