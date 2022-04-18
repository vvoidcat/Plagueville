using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PLAGUEV.Cards;
using PLAGUEV.Core;

namespace PLAGUEV.Control {

    public class CardSpawner : MonoBehaviour {

        [SerializeField] GameLocation currentLocation;
        [SerializeField] GameObject cardPrefab = null;
        GameObject card;
        CardData[] cardDatas;
        CardData cardData;
        PlayerSettings playerSettings;

        float timeToWait = 25f;
        float hasWaited = 0f;


        void Awake() {
            cardDatas = Resources.LoadAll<CardData>("Cards");
            playerSettings = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSettings>();
        }

        void Start() {
            ResolveDialogueTrees();
        }

        void FixedUpdate() {
            if (hasWaited == timeToWait) {
                if (LocationExistsInDeck((int)currentLocation)) {
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

        public GameLocation GetCurrentLocation() {
            return currentLocation;
        }

        public void SetCurrentLocation(GameLocation location) {
            currentLocation = location;
            // call location manager and update background
        }

        private void ChooseCard() {
            // if child of the card's current dialogue node is chained to the next one -> choose this card again and make it so that the chained node appears
            // else below
            
            int orderInDeck = (int)Random.Range(0, cardDatas.Length - 1);
            cardData = cardDatas[orderInDeck];

            if (cardData.location != CardLocation.ALL && (int)cardData.location != (int)currentLocation) {
                ChooseAgain();
            }
        }

        private void ChooseAgain() {
            ChooseCard();
        }

        private void CreateNewCard() {
            if (card != null) {     // destruction should happen on dialogue choice
                Destroy(card);
            }

            card = Instantiate(cardPrefab, transform);
            card.GetComponent<SpriteRenderer>().sprite = cardData.sprite;
        }

        private void ResolveDialogueTrees() {
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

            UpdateCardSettings(cardType);
        }

        private void UpdateCardSettings(CardType cardType) {
            foreach (CardData data in cardDatas) {
                if (data.hasClassTrees && data.type == cardType) {
                    data.useMainTree = false;
                }
            }
        }

        private bool LocationExistsInDeck(int location) {
            CardLocation check = (CardLocation)location;
            int count = 0;

            foreach (CardData data in cardDatas) {
                if (data.location == check) {
                    count++;
                }
            }

            if (count > 0) return true;
            else return false;
        }
    }
}