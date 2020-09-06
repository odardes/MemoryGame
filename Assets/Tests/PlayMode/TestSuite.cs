using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests
{
    public class TestSuite
    {
        [UnityTest]
        public IEnumerator CardCreateTestsWithEnumeratorPasses() //For control testing
        {
            GameObject controller = GameObject.Instantiate(Resources.Load("Prefabs/Controller")) as GameObject;
            yield return new WaitForSeconds(5);
            int expectedCardCount = 10;
            GameObject[] cardObjects = GameObject.FindGameObjectsWithTag("Card");
            int result = cardObjects.Length;
            Assert.AreEqual(expectedCardCount, result);
        }

        [UnityTest]
        public IEnumerator CardPrefabIsNotNull() //Important for the rest
        {
            yield return new WaitForSeconds(1);
            var cardPrefab = Resources.Load("Prefabs/Card");
            Assert.IsNotNull(cardPrefab);
        }

        [UnityTest]
        public IEnumerator LoadImagesToArray()  //Awake Function - Be sure cardImages array is not empty
        {
            GameObject controller = GameObject.Instantiate(Resources.Load("Prefabs/Controller")) as GameObject;
            Controller control = controller.GetComponent<Controller>();
            Assert.IsNotNull(control.cardImages);
            yield return new WaitForSeconds(.5f);
        }

        [UnityTest]
        public IEnumerator LoadObjectsToList()  //GetCards Function - Be sure cards list is not empty
        {
            GameObject controller = GameObject.Instantiate(Resources.Load("Prefabs/Controller")) as GameObject;
            Controller control = controller.GetComponent<Controller>();
            control.GetCards();
            Assert.IsNotNull(control.cards);
            yield return new WaitForSeconds(.5f);
        }

        [UnityTest]
        public IEnumerator LoadObjectBackgroundToList()  //GetCards Function - Be sure card list background is not empty
        {
            GameObject controller = GameObject.Instantiate(Resources.Load("Prefabs/Controller")) as GameObject;
            Controller control = controller.GetComponent<Controller>();
            control.GetCards();
            Sprite background = control.background;
            Assert.AreEqual(background, control.cards[2].image.sprite);
            yield return new WaitForSeconds(.5f);
        }

        [UnityTest]
        public IEnumerator LoadImagesToList()  //AddCards Function - Be sure gameCards list is filling
        {
            GameObject controller = GameObject.Instantiate(Resources.Load("Prefabs/Controller")) as GameObject;
            Controller control = controller.GetComponent<Controller>();
            control.AddCards(control.gameCard);
            Assert.IsNotNull(control.gameCard);
            yield return new WaitForSeconds(.5f);
        }

        [UnityTest]
        public IEnumerator ControlLists()  //AddCards Function - Be sure gameCards and CardImages lists equal
        {
            GameObject controller = GameObject.Instantiate(Resources.Load("Prefabs/Controller")) as GameObject;
            Controller control = controller.GetComponent<Controller>();
            control.AddCards(control.gameCard);
            Assert.AreEqual(control.gameCard[2], control.cardImages[2]);
            yield return new WaitForSeconds(.5f);
        }

        [UnityTest]
        public IEnumerator SelectIndexesControl()  //PickACard Function control indexes are not null
        {
            GameObject controller = GameObject.Instantiate(Resources.Load("Prefabs/Controller")) as GameObject;
            Controller control = controller.GetComponent<Controller>();
            //Act
            control.GetCards();
            control.AddCards(control.gameCard);
            control.cards[3].onClick.AddListener(control.PickACard);
            control.cards[4].onClick.AddListener(control.PickACard);
            //Assert
            yield return new WaitForSeconds(.5f);
            Assert.IsNotNull(control.firstSelectIndex);
            Assert.IsNotNull(control.secondSelectIndex);
        }

        [UnityTest]
        public IEnumerator SelectNamesControl()  //PickACard Function control names are not null
        {
            //Arrange
            GameObject controller = GameObject.Instantiate(Resources.Load("Prefabs/Controller")) as GameObject;
            Controller control = controller.GetComponent<Controller>();
            //Act
            control.GetCards();
            control.AddCards(control.gameCard);
            control.cards[5].onClick.AddListener(control.PickACard);
            control.cards[6].onClick.AddListener(control.PickACard);
            //Assert
            Assert.IsNotNull(control.firstSelectName);
            Assert.IsNotNull(control.secondSelectName);
            yield return new WaitForSeconds(.5f);
        }

        [UnityTest]
        public IEnumerator SetImageToCards()  //PickACard Function control images is not null
        {
            //Arrange
            GameObject controller = GameObject.Instantiate(Resources.Load("Prefabs/Controller")) as GameObject;
            Controller control = controller.GetComponent<Controller>();
            //Act
            control.GetCards();
            control.AddCards(control.gameCard);
            int random1 = Random.Range(0, 6);
            int random2 = Random.Range(6, 10);
            control.cards[random1].onClick.AddListener(control.PickACard);
            control.cards[random2].onClick.AddListener(control.PickACard);
            //Assert
            Assert.IsNotNull(control.cards[random1].image.sprite);
            Assert.IsNotNull(control.cards[random2].image.sprite);
            yield return new WaitForSeconds(2);
        }

        [UnityTest]
        public IEnumerator IncreaseCountSelectionWhenPickACArd() //PickACard Function
        {
            //Arrange
            GameObject controller = GameObject.Instantiate(Resources.Load("Prefabs/Controller")) as GameObject;
            Controller control = controller.GetComponent<Controller>();
            int firstCountSelection = control.countSelection; //Because we will compare them
            //Act          
            control.GetCards();
            control.AddCards(control.gameCard);
            int random1 = Random.Range(0, 6);
            int random2 = Random.Range(6, 10);
            control.cards[random1].onClick.AddListener(control.PickACard);
            control.cards[random2].onClick.AddListener(control.PickACard);
            yield return new WaitForSeconds(3);
            Debug.Log(control.countSelection);
            Debug.Log(firstCountSelection);
            //Assert
            Assert.AreNotEqual(firstCountSelection, control.countSelection);
        }

        [UnityTest]
        public IEnumerator DisappearWhenMatches() //PickACard Function =>  IEnumerator CheckTheCards()
        {
            //Arrange
            GameObject controller = GameObject.Instantiate(Resources.Load("Prefabs/Controller")) as GameObject;
            Controller control = controller.GetComponent<Controller>();
            control.firstSelectName = "Equal";
            control.secondSelectName= "Equal";
            //Act
            control.GetCards();
            control.AddCards(control.gameCard);
            control.CheckTheCards();
            yield return new WaitForSeconds(3);
            Debug.Log(control.firstSelectName);
            Debug.Log(control.secondSelectName);
            //Assert
            Assert.IsFalse(control.cards[control.firstSelectIndex].interactable);
            Assert.IsFalse(control.cards[control.secondSelectIndex].interactable);           
        }
      

        [UnityTest]
        public IEnumerator SetBackgroundWhenNotMatches() //PickACard Function controls set the background again when not matched
        {
            //Arrange
            GameObject controller = GameObject.Instantiate(Resources.Load("Prefabs/Controller")) as GameObject;
            Controller control = controller.GetComponent<Controller>();
            Sprite background = control.background;
            control.firstSelectName = "First Select Name";
            control.secondSelectName = "Second Select Name"; //Sets different strings
            //Act           
            control.GetCards();
            control.AddCards(control.gameCard);
            control.cards[5].onClick.AddListener(control.PickACard);
            control.cards[6].onClick.AddListener(control.PickACard);
            //Assert
            Assert.AreEqual(background, control.cards[control.firstSelectIndex].image.sprite);
            Assert.AreEqual(background, control.cards[control.secondSelectIndex].image.sprite);
            yield return new WaitForSeconds(.5f);
        }

        [UnityTest]
        public IEnumerator CountCorrectSelectionIncreases() //IsTheGameFinished() function controls countcorrectselection increases
        {
            //Arrange
            GameObject controller = GameObject.Instantiate(Resources.Load("Prefabs/Controller")) as GameObject;
            Controller control = controller.GetComponent<Controller>();
            int correct = control.countCorrectSelection;
            //Act
            control.IsTheGameFinished();
            //Assert
            Assert.AreNotEqual(correct, control.countCorrectSelection);
            yield return new WaitForSeconds(.5f);
        }

        [UnityTest]
        public IEnumerator ShuffleTheList()  //Shuffle function controls the list is mixed
        {
            GameObject controller = GameObject.Instantiate(Resources.Load("Prefabs/Controller")) as GameObject;
            Controller control = controller.GetComponent<Controller>();
            //Arrange
            yield return new WaitForSeconds(5);
            control.GetCards();
            control.AddCards(control.gameCard);
            List<Sprite> gameCardCopy = new List<Sprite>();
            foreach(Sprite sprite in control.gameCard)  //Copy gameCard elements to gameCardCopy List. Because we will compare them.
            {
                gameCardCopy.Add(sprite);
            }
            control.Shuffle(gameCardCopy);
            //Assert
            Assert.AreNotEqual(gameCardCopy, control.gameCard);
        }
    }
}
 