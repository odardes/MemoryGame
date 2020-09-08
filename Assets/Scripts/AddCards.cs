using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCards : MonoBehaviour
{
    [SerializeField] private Transform gameField;
    public GameObject card;

    public void Awake()
    {
        for (int i = 0; i< 10; i++)
        {
            GameObject button = Instantiate(card);         //Creates 10 cards
            button.name = "" + i;                          //Set names 1 to 10
            button.transform.SetParent(gameField, false);  //Puts cards to game field
        }
    }
}
