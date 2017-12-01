using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//attach this script as a component to all sectors

//automatically adds SpriteRender component to object if script is added as component
[RequireComponent(typeof(PolygonCollider2D))]

public class SectorScript : MonoBehaviour
{
    private enum Colour { Red, Blue, Green, Yellow, Gray };
    private SpriteRenderer spriteRenderer;

    public Sprite spriteNetural;
    public Sprite spriteRed;
    public Sprite spriteBlue;
    public Sprite spriteGreen;
    public Sprite spriteYellow;
    public Vector3 location1;
    public Vector3 location2;
    public Vector3 location3;

    //public int sectorID;
    public Player owner;
    public bool containsVice;
    public List<GameObject> containedUnits;
    public List<GameObject> neighbouringSectors;
    public List<GameObject> containedBuidlings;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); //binding the actual SpriteRenderer to our own var in code
        spriteRenderer.sprite = spriteNetural; //if statement based on player to choose between sprites to use. ie P1,P2,Netural ect
    }

    public void UpdateSprite()
    {
        Colour colour = (Colour)owner.colour;
        if (colour == Colour.Blue)
        {
            spriteRenderer.sprite = spriteBlue;
        }
        else if (colour == Colour.Red)
        {
            spriteRenderer.sprite = spriteRed;
        }
        else if (colour == Colour.Green)
        {
            spriteRenderer.sprite = spriteGreen;
        }
        else if (colour == Colour.Yellow)
        {
            spriteRenderer.sprite = spriteYellow;
        }
        else if (colour == Colour.Gray)
        {
            spriteRenderer.sprite = spriteNetural;
        }
        else { throw new System.ArgumentException("Player's colour was not of a correct type!"); }
    }

    public Vector3 getVector3()
    {
        return new Vector3(1, 1, 1);
    }
    /*
    private void OnMouseDown()
    {
        if (spriteRenderer.sprite == sprite)
        {
            spriteRenderer.sprite = sprite2;
        }
        else if (spriteRenderer.sprite == sprite2)
        {
            spriteRenderer.sprite = sprite;
        }
    }

    //activates when mouse hovers over the 2D polygon collider
    private void OnMouseEnter()
    {
        spriteRenderer.sprite = sprite2;
    }

    //activates when mouse no longer hovers over the 2D polygon collider
    private void OnMouseExit()
    {
        spriteRenderer.sprite = sprite;
    }
    */
}
