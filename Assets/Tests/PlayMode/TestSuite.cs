using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Tests
{
    public class TestSuite
    {
        private GameObject controller;

        [SetUp]                                     //Setup method will run before a unit test
        public void Setup()
        {
            controller = GameObject.Instantiate(Resources.Load("Prefabs/Controller")) as GameObject;
        }

        [TearDown]                                 //Tear Down method will run after a unit test 
        public void Teardown()
        {
            GameObject.Destroy(controller);
            GameObject[] cardObjects = GameObject.FindGameObjectsWithTag("Card");
            foreach(GameObject card in cardObjects)
            {
                GameObject.Destroy(card);
            }
        }

        [UnityTest]
        public IEnumerator ControllerPrefabIsNotNull() //Important for the rest
        {
            yield return null;
            Assert.IsNotNull(controller);
        }

        [UnityTest]
        public IEnumerator CardPrefabIsNotNull() //Important for the rest
        {
            yield return null;
            GameObject cardPrefab = controller.GetComponent<AddCards>().card;
            Assert.IsNotNull(cardPrefab);
        }


        [UnityTest]
        public IEnumerator CardCreateTestsWithEnumeratorPasses() //For control testing
        {
            yield return null;
            int expectedCardCount = 10;
            GameObject[] cardObjects = GameObject.FindGameObjectsWithTag("Card");
            int result = cardObjects.Length;
            Assert.AreEqual(expectedCardCount, result);
        }
       

        [UnityTest]
        public IEnumerator LoadImagesToArray()  //Awake Function - Be sure cardImages array is not empty
        {
            yield return null;
            Controller control = controller.GetComponent<Controller>();
            Assert.IsNotNull(control.cardImages);
        }

        [UnityTest]
        public IEnumerator LoadObjectsToList()  //GetCards Function - Be sure cards list is not empty
        {
            yield return null;
            Controller control = controller.GetComponent<Controller>();
            Assert.IsNotNull(control.cards);
        }

        [UnityTest]
        public IEnumerator LoadObjectBackgroundToList()  //GetCards Function - Be sure card list background is not empty
        {
            yield return null;
            Controller control = controller.GetComponent<Controller>();
            Sprite background = control.background;
            foreach(Button button in control.cards)
            {
                Assert.AreEqual(background, button.image.sprite);
            }
        }

        [UnityTest]
        public IEnumerator LoadImagesToList()  //AddCards Function - Be sure gameCards list is filling
        {
            yield return null;
            Controller control = controller.GetComponent<Controller>();
            Assert.IsNotNull(control.gameCard);
        }

        [UnityTest]
        public IEnumerator SelectIndexesControl()  //PickACard Function control indexes are not null
        {
            Controller control = controller.GetComponent<Controller>();
            //Act
            foreach (Button button in control.cards)
            {
                button.onClick.AddListener(control.PickACard);
                Assert.AreEqual(control.firstSelectIndex, int.Parse(control.cards[control.firstSelectIndex].name));
                Assert.AreEqual(control.secondSelectIndex, int.Parse(control.cards[control.secondSelectIndex].name));
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator SelectNamesControl()  //PickACard Function control names are not null
        {
            //Arrange
            Controller control = controller.GetComponent<Controller>();
            //Act
            foreach (Button button in control.cards)
            {
                button.onClick.AddListener(control.PickACard);
                yield return null;
                Assert.AreEqual(control.firstSelectName, control.gameCard[control.firstSelectIndex].name);
                Assert.AreEqual(control.secondSelectName, control.gameCard[control.secondSelectIndex].name);
            }
        }

        [UnityTest]
        public IEnumerator SetImageToCards()  //PickACard Function control images is not null
        {
            //Arrange
            Controller control = controller.GetComponent<Controller>();
            //Act
            foreach (Button button in control.cards)
            {
                button.onClick.AddListener(control.PickACard);
                Assert.AreEqual(control.cards[control.firstSelectIndex].image.sprite, control.gameCard[control.firstSelectIndex]);
                Assert.AreEqual(control.cards[control.secondSelectIndex].image.sprite, control.gameCard[control.secondSelectIndex]);
                yield return new WaitForSeconds(3);
            }
        }

        [UnityTest]
        public IEnumerator IncreaseCountSelectionWhenPickACArd() //PickACard Function
        {
            //Arrange
            Controller control = controller.GetComponent<Controller>();
            int firstCountSelection = control.countSelection; //Because we will compare them
            //Act   
            foreach (Button button in control.cards)
            {
                control.firstSelection = true;
                control.secondSelection = false;
                button.onClick.AddListener(control.PickACard);
                yield return null;
                Assert.AreNotEqual(firstCountSelection, control.countSelection);
            }
        }

        [UnityTest]
        public IEnumerator DisappearWhenMatches() //PickACard Function =>  IEnumerator CheckTheCards()
        {
            //Arrange
            Controller control = controller.GetComponent<Controller>();            
            //Act
            foreach (Button button in control.cards)
            {
                control.firstSelectName = "Equal";
                control.secondSelectName = "Equal";
                button.onClick.AddListener(control.PickACard);
                yield return null;
                Assert.IsFalse(control.cards[control.firstSelectIndex].interactable);
                Assert.IsFalse(control.cards[control.secondSelectIndex].interactable);
            }
        }
      

        [UnityTest]
        public IEnumerator SetBackgroundWhenNotMatches() //PickACard Function controls set the background again when not matched
        {
            yield return null;
            //Arrange
            Controller control = controller.GetComponent<Controller>();
            Sprite background = control.background;
            //Act          
            foreach (Button button in control.cards)
            {
                control.firstSelectName = "First Select Name";
                control.secondSelectName = "Second Select Name"; //Sets different strings
                button.onClick.AddListener(control.PickACard);
                yield return null;
                Assert.AreEqual(background, control.cards[control.firstSelectIndex].image.sprite);
                Assert.AreEqual(background, control.cards[control.secondSelectIndex].image.sprite);
            }
        }

        [UnityTest]
        public IEnumerator CountCorrectSelectionIncreases() //IsTheGameFinished() function controls countcorrectselection increases
        {
            yield return null;
            //Arrange
            GameObject controller = GameObject.Instantiate(Resources.Load("Prefabs/Controller")) as GameObject;
            Controller control = controller.GetComponent<Controller>();
            int correct = control.countCorrectSelection;
            //Act
            foreach (Button button in control.cards)
            {
                control.firstSelectName = "Equal";
                control.secondSelectName = "Equal";
                button.onClick.AddListener(control.PickACard);
                yield return null; 
                Assert.AreNotEqual(correct, control.countCorrectSelection);
            }                       
        }

        [UnityTest]
        public IEnumerator ShuffleTheList()  //Shuffle function controls the list is mixed
        {
            yield return null;
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
 