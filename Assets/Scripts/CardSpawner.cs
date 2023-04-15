using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardSpawner : MonoBehaviour
{
    public GameObject cardPrefab;  // The prefab of the card to spawn
    public List<GameObject> cardList = new List<GameObject>();
    public float cardSpacing = 1f;  // The spacing between cards in the grid
    public Transform cardHolder;  // The empty GameObject to spawn the cards inside

    //private Vector3 firstCardPosition;  // The position of the first card in the grid
    //private Vector3 lastCardPosition;   // The position of the last card in the grid
    //private bool isFirstCardSpawned = false;  // Flag to check if the first card is already spawned
    //private int cardCount = 0;  // The number of cards that have been spawned

    //PM's Additions
    //private Dictionary<int, GameObject> cards = new Dictionary<int, GameObject>();
    private Dictionary<int, Tuple<int, GameObject>> cards = new Dictionary<int, Tuple<int, GameObject>>();
    [SerializeField] private List<Image> images = new List<Image>();

    void Start()
    {
        // Calculate the position of the first card in the grid based on the grid size and spacing
        //float xOffset = -(cardSpacing / 2);
        //float yOffset = -(cardSpacing / 2);
        //firstCardPosition = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z);

        SimpleServerDemo.OnCharacterSelected += OnCharacterSelected;
        SimpleServerDemo.OnCharacterDeselected += RemoveCard;

    }

    void OnCharacterSelected(int playerId, int characterId)
    {
        characterId--;
        if (cards.ContainsKey(playerId) && cards[playerId].Item1 == characterId) return;
        if(cards.ContainsKey(playerId))
        {
            RemoveCard(playerId);
        }
        SpawnCard(playerId, characterId);
        
    }

    void SpawnCard(int playerId, int characterId)
    {
        GameObject card = Instantiate(cardPrefab, cardHolder);
        //GameObject card = Instantiate(cardList[characterId], cardHolder);
        PlayerCardValueSetter cardValueSetter = card.GetComponent<PlayerCardValueSetter>();
        cardValueSetter.playerText.text = "Player " + (playerId + 1);
        cardValueSetter.yuriImage.SetActive(false);
        cardValueSetter.rubenImage.SetActive(false);
        cardValueSetter.tacoImage.SetActive(false);
        switch (characterId)
        {
            case 0:
                cardValueSetter.yuriImage.SetActive(true);
                break;
            case 1:
                cardValueSetter.rubenImage.SetActive(true);
                break;
            case 2:
                cardValueSetter.tacoImage.SetActive(true);
                break;
        }

        cards.Add(playerId, new Tuple<int, GameObject>(characterId, card));

    }

    void RemoveCard(int playerId)
    {
        Destroy(cards[playerId].Item2);
        cards.Remove(playerId);
        Debug.LogError("Destroyed card");
    }
    void RemoveCard(int playerId, int characterId)
    {
        if (!cards.ContainsKey(playerId)) return;
        Destroy(cards[playerId].Item2);
        cards.Remove(playerId);
        Debug.LogError("Destroyed card");
    }

    //void SpawnCard(Vector3 position)
    //{
    //    GameObject card = Instantiate(cardPrefab, position, Quaternion.identity);
    //    // Set the card's parent to the cardHolder object
    //    card.transform.parent = cardHolder;
    //    lastCardPosition = firstCardPosition;
    //    firstCardPosition = position;
    //    cardCount++;
    //}

    //void Update()
    //{

    //    if (cardCount < 4 && Input.GetKeyDown(KeyCode.A))
    //    {
    //        // Spawn a new card in the grid if card count is less than 4
    //        Vector3 nextCardPosition = firstCardPosition + new Vector3(cardSpacing, 0f, 0f);
    //        SpawnCard(nextCardPosition);
    //        firstCardPosition = nextCardPosition;
    //    }
    //}
}
