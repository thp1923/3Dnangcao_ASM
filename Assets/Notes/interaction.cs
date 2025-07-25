using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class interaction : MonoBehaviour
{
    //The distance from which the player can interact with an object
    public float interactionDistance;

    //Text or crosshair that shows up to let the player know they can interact with an object they're looking at
    public GameObject interactionText;

    //Layers the raycast can hit/interact with. Any layers unchecked will be ignored by the raycast.
    public LayerMask interactionLayers;

    //Reference to the currently opened letter, if any
    private letter currentOpenLetter = null;

    //The Update() void is used to make stuff happen every frame
    void Update()
    {
        //If a letter is currently open and the player presses T, close the letter
        if (currentOpenLetter != null && Input.GetKeyDown(KeyCode.T))
        {
            currentOpenLetter.openCloseLetter();
            currentOpenLetter = null;
            return; //Skip further processing for this frame
        }

        //RaycastHit variable which will collect information from objects the raycast hits
        RaycastHit hit;

        //If the raycast hits something,
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance, interactionLayers))
        {
            //If the object it hit contains the letter script,
            if (hit.collider.gameObject.GetComponent<letter>())
            {
                //The interaction text will enable
                interactionText.SetActive(true);

                //If the T key is pressed,
                if (Input.GetKeyDown(KeyCode.T))
                {
                    //The letter component is accessed and the letter will open or close
                    letter letterComponent = hit.collider.gameObject.GetComponent<letter>();
                    letterComponent.openCloseLetter();

                    //If the letter is now open, store a reference to it so ESC can close it later
                    if (letterComponent.IsOpen())
                    {
                        currentOpenLetter = letterComponent;
                    }
                    else
                    {
                        currentOpenLetter = null;
                    }
                }
            }
            //else, the interaction text is set false.
            else
            {
                interactionText.SetActive(false);
            }
        }
        //else, the interaction text is set false.
        else
        {
            interactionText.SetActive(false);
        }
    }
}