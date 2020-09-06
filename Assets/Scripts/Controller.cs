using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Controller : MonoBehaviour
{   
    //Set to Public because of accessing from TestSuite 
    public Sprite background;                      

    public Sprite[] cardImages;                          //Loads all cards images to this array
    public List<Sprite> gameCard = new List<Sprite>();   //Hold the representative cards
    public List<Button> cards = new List<Button>();      //Holds buttons on the screen

    public bool firstSelection, secondSelection;

    public int countSelection=0;
    public int countCorrectSelection;                    //Should be 5 for finish the game

    public int firstSelectIndex, secondSelectIndex;
    public string firstSelectName, secondSelectName;


   public void Awake()                                  //Loads images to sprite array
    {
        cardImages = Resources.LoadAll<Sprite>("Sprites/CardInside");
    }
    public void Start()
    {
        GetCards();
        AddListener();
        AddCards(gameCard);
        Shuffle(gameCard);
    }

    public void GetCards()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Card");  //Selects the objects that have Card tag

        for (int i=0; i<objects.Length; i++)
        {
            cards.Add(objects[i].GetComponent<Button>());                  //Adds the buttons to the card list
            cards[i].image.sprite = background;                            //Sets close appearance
        }
    }

    public void AddCards(List<Sprite> gameCard)                           //Ensures there is two identical cards of a card 
    {
        int index = 0;

        for (int i = 0; i < 10; i++)
        {
            if (index == 5)
            {
                index = 0;
            }
            gameCard.Add(cardImages[index]);
            index++;
        }

    }

    void AddListener()                                                  // When click a card it goes to pickACard function
    {
        foreach(Button card in cards)
        {
            card.onClick.AddListener(() => PickACard());
        }
    }

    public void PickACard()
    {
        if (!firstSelection) //true
        {
            firstSelection = true;
            firstSelectIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            firstSelectName = gameCard[firstSelectIndex].name;
            cards[firstSelectIndex].image.sprite = gameCard[firstSelectIndex];  //appear the image

        }else if (!secondSelection) //true
        {
            secondSelection = true;
            secondSelectIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            secondSelectName = gameCard[secondSelectIndex].name;
            cards[secondSelectIndex].image.sprite = gameCard[secondSelectIndex];  //appear the image

            countSelection++;   //each time we guess it increments

            StartCoroutine(CheckTheCards());
        }
    }

    public IEnumerator CheckTheCards()
    {
        yield return new WaitForSeconds(1f);

        if (firstSelectName == secondSelectName)
        {
            
            yield return new WaitForSeconds(.5f);

            cards[firstSelectIndex].interactable = false;
            cards[secondSelectIndex].interactable = false;

            cards[firstSelectIndex].image.color = new Color(0,0,0,0);     // prevents the transparency
            cards[secondSelectIndex].image.color = new Color (0,0,0,0);

            IsTheGameFinished();
        }
        else
        {
            yield return new WaitForSeconds(.5f);

            cards[firstSelectIndex].image.sprite = background;
            cards[secondSelectIndex].image.sprite = background;
        }

        yield return new WaitForSeconds(.5f);
        firstSelection = secondSelection = false;
    }

    public void IsTheGameFinished()
    {
        countCorrectSelection++;
        if (countCorrectSelection == 5) //After 5 guess the game finishes
        {
            Debug.Log("Game Finished");
            Debug.Log("It took you " + countSelection + " guesses to finish the game");

        }
    }

    public void Shuffle(List<Sprite> list)
    {
        for(int i =0; i < list.Count; i++)
        {
            Sprite temp = list[i];  //get a reference
            int randomIndex = Random.Range(i, list.Count);  //create a random index
            list[i] = list[randomIndex];  //assigns the random index to i
            list[randomIndex] = temp;     //assigns temp variable back to random index
        }
    }
}
