using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class closeButtonScript : MonoBehaviour, IPointerClickHandler {

    public GameObject buyMenuCanvas;

    public void OnPointerClick(PointerEventData eventData)
    {
        buyMenuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
