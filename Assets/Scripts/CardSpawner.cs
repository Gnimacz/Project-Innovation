using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    public GameObject cardPrefab;  // The prefab of the card to spawn
    public float cardSpacing = 1f;  // The spacing between cards in the grid
    public Transform cardHolder;  // The empty GameObject to spawn the cards inside

    private Vector3 firstCardPosition;  // The position of the first card in the grid
    private bool isFirstCardSpawned = false;  // Flag to check if the first card is already spawned
    private int cardCount = 0;  // The number of cards that have been spawned

    void Start()
    {
        // Calculate the position of the first card in the grid based on the grid size and spacing
        float xOffset = -(cardSpacing / 2);
        float yOffset = -(cardSpacing / 2);
        firstCardPosition = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z);

    }

    void SpawnCard(Vector3 position)
    {
        GameObject card = Instantiate(cardPrefab, position, Quaternion.identity);
        // Set the card's parent to the cardHolder object
        card.transform.parent = cardHolder;
        cardCount++;
    }

    void Update()
    {

        if (cardCount < 4 && Input.GetKeyDown(KeyCode.A))
        {
            // Spawn a new card in the grid if card count is less than 4
            Vector3 nextCardPosition = firstCardPosition + new Vector3(cardSpacing, 0f, 0f);
            SpawnCard(nextCardPosition);
            firstCardPosition = nextCardPosition;
        }
    }
}
