using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buildingScript : MonoBehaviour {

    GameObject BuyUnitCanvas;
    GameObject GameMain;
    GameObject Sector;

    public void Init(string buildingName, GameObject sector, GameObject buyUnitCanvas, GameObject gameMain)
    {
        this.Sector = sector;

        this.BuyUnitCanvas = buyUnitCanvas;
        this.GameMain = gameMain;

        this.gameObject.transform.SetParent(sector.gameObject.transform); //change buildings owner
        Vector3 currentPosition = new Vector3(); //create new vector to store new unit coordinates before applying them to unit

        currentPosition = sector.transform.position;

        switch (buildingName)
        {
            case "CentralHall":
                this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll("Buildings\\CentralHall")[1] as Sprite;
                this.transform.localScale = new Vector3(2, 2, 1);
                currentPosition.x += -0.19f;
                currentPosition.y += -0.51f;
                currentPosition.z += -0.5f;
                break;

            case "HesHall":
                this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll("Buildings\\HesHall")[1] as Sprite;
                this.transform.localScale = new Vector3(2, 2, 1);
                currentPosition.x += 0.85f;
                currentPosition.y += 0.16f;
                currentPosition.z += -0.5f;
                break;

            case "JBMorrell":
                this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll("Buildings\\JBMorrell")[1] as Sprite;
                this.transform.localScale = new Vector3(2, 2, 1);
                currentPosition.x += -0.09f;
                currentPosition.y += 0.55f;
                currentPosition.z += -0.5f;
                break;

            case "Nisa":
                this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll("Buildings\\Nisa")[1] as Sprite;
                currentPosition.x += 0.29f;
                currentPosition.y += 0.47f;
                currentPosition.z += -0.5f;
                break;

            case "RCH":
                this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll("Buildings\\RCH")[1] as Sprite;
                this.transform.localScale = new Vector3(1, 2, 1);
                currentPosition.x += -0.05f;
                currentPosition.y += 0.55f;
                currentPosition.z += -0.5f;
                break;

            case "SportsVillage":
                this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll("Buildings\\SportsVillage")[1] as Sprite;
                this.transform.localScale = new Vector3(2.5f, 2.5f, 1);
                this.transform.Rotate(new Vector3(0f, 0f, -6.07f));
                currentPosition.x += -0.26f;
                currentPosition.y += 0.86f;
                currentPosition.z += -0.5f;
                break;

            case "track":
                this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll("Buildings\\track")[1] as Sprite;
                this.transform.localScale = new Vector3(0.2f, 0.2f, 1);
                this.transform.Rotate( new Vector3(0f, 0f, -50f) );
                currentPosition.x += 0.05f;
                currentPosition.y += -0.29f;
                currentPosition.z += -0.5f;
                break;
        }

        this.gameObject.GetComponent<Transform>().position = currentPosition;

        this.gameObject.AddComponent<PolygonCollider2D>(); //add polygoncollider component to this sector so that it can be clicked by the user
    }

    void OnMouseDown()
    {
        BuyUnitCanvas.gameObject.SetActive(true);
        GameMain.gameObject.SetActive(false);
        BuyUnitCanvas.GetComponent<unitCanvasScript>().setSpawnPoint(Sector.gameObject);
    }
}
