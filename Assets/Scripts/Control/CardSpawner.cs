using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PLAGUEV.Dialogue;
using PLAGUEV.Cards;
using PLAGUEV.Core;

namespace PLAGUEV.Control {

    public class CardSpawner : MonoBehaviour {

        [SerializeField] LocationType currentLocation;
        [SerializeField] GameObject cardPrefab = null;
        GameObject card;
        CardData[] cardDatas;
        CardData cardData;
        PlayerSettings playerSettings;

        CardData savedCard = null;
        DialogueNode savedNode = null;

        [SerializeField] int defaultMaxCounter = 5;
        [SerializeField] int uniqueMaxCounter = 20;

        float timeToWait = 25f;
        float hasWaited = 0f;


        void Awake() {
            cardDatas = Resources.LoadAll<CardData>("Cards");
            playerSettings = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSettings>();
        }

        void Start() {
            InitializeCards();
        }

        void FixedUpdate() {
            if (hasWaited == timeToWait) {
                if (LocationExistsInDeck()) {
                    ChooseCard();
                    CreateNewCard();
                } else {
                    print("error: the deck has no cards for this location");        // remove later when there're enough cards for every location
                }

                hasWaited = 0f;
            }
            hasWaited++;
        }


        public CardData GetCardData() {
            return cardData;
        }

        public LocationType GetCurrentLocation() {
            return currentLocation;
        }

        public void SetCurrentLocation(LocationType location) {
            currentLocation = location;
            // call location manager and update background
        }

        private void InitializeCards() {
            CardType cardType = new CardType();

            switch (playerSettings.GetPlayerClass()) {
                case PlayerClass.THIEF:
                    cardType = CardType.THIEF;
                    break;
                case PlayerClass.WIZARD:
                    cardType = CardType.WIZARD;
                    break;
                case PlayerClass.GUARD:
                    cardType = CardType.GUARD;
                    break;
                case PlayerClass.NUN:
                    cardType = CardType.NUN;
                    break;
            }

            foreach (CardData data in cardDatas) {
                data.SetMaxCounter(defaultMaxCounter, uniqueMaxCounter);
                data.SetDialogueTree(cardType);
            }
        }

        private void ChooseCard() {
            if (savedCard == null) {
                int orderInDeck = Random.Range(0, cardDatas.Length);
                cardData = cardDatas[orderInDeck];

                if (!cardData.CanBeChosen(currentLocation)) {
                    cardData.UpdateCounter();
                    ChooseCard();
                } else {
                    // select DialogueNode
                    // check if there's a child of the player response node that is chained
                    // if there is, remember the card && check if there's any player node next && get the next node in the chain if there isn't &&
                    // && wait for the player's response if there is and remember its non-player child
                    // savedCard = cardData;
                    // ChooseCard();

                    // if there isn't -> reset savings
                }
            } else {
                // check for chained nodes again
            }
        }

        private void ResetSavings() {
            savedCard = null;
            savedNode = null;
        }

        private void CreateNewCard() {
            if (card != null) {     // destruction should happen on dialogue choice
                Destroy(card);
            }

            card = Instantiate(cardPrefab, transform);
            card.GetComponent<SpriteRenderer>().sprite = cardData.sprite;
            card.transform.GetChild(0).GetComponent<TextMeshPro>().SetText(cardData.characterName);         // should this be configurable in the dialogue system instead?..

            // card.transform.GetChild(1).GetComponent<TextMeshPro>().SetText(cardData.characterName);      // dialogue node text
        }

        private bool LocationExistsInDeck() {       // remove later
            bool isInDeck = false;

            foreach (CardData data in cardDatas) {
                if (!data.canAppearEverywhere) {
                    isInDeck = true;
                    break;
                }

                foreach (LocationType cardLocation in data.locations) {
                    if (cardLocation == currentLocation) {
                        isInDeck = true;
                        break;
                    }
                }
            }

            return isInDeck;
        }
    }
}
