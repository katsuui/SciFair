using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum CardCategory
{
    Exercise,
    Food,
    Self
}

public class TileMoves : MonoBehaviour
{
    public route currentRoute;
    private int routePosition;
    private int steps;
    private bool isMoving;

    public GameObject negativeExercisePrefab; // Reference to the negative exercise card prefab
    public GameObject positiveExercisePrefab; // Reference to the positive exercise card prefab

    public GameObject negativeFoodPrefab; // Reference to the negative food card prefab
    public GameObject positiveFoodPrefab; // Reference to the positive food card prefab

    public GameObject negativeSelfPrefab; // Reference to the negative self card prefab
    public GameObject positiveSelfPrefab; // Reference to the positive self card prefab
    public GameObject CardContainer; // Reference to the card container GameObject
    private Vector3 targetPosition; // Target position for the cloned card


    public Vector3 centerPosition; // Center position for card animation
    private bool canRollDice = true;
    private CardCategory currentCategory; // Current category of the card
    private GameObject chosenCard;
    private bool shouldMoveWithoutDice = false;
    private GameObject clonedCard; // Declare a class-level variable to store the cloned card

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            if (!shouldMoveWithoutDice)
            {
                steps = Random.Range(1, 7);
                Debug.Log("Dice rolled: " + steps);
            }

            if (routePosition + steps < currentRoute.childNodeList.Count)
            {
                StartCoroutine(Move());
            }
            else
            {
                Debug.Log("Number too high: " + steps);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (clonedCard != null)
            {
                Destroy(clonedCard);
                clonedCard = null; // Reset the reference to null if needed
            }
        }
    }

    IEnumerator Move()
    {
        if (isMoving)
        {
            yield break;
        }

        isMoving = true;

        int direction = Mathf.RoundToInt(Mathf.Sign(steps)); // Determine the direction of movement (forward or backward)
        int remainingSteps = Mathf.Abs(Mathf.RoundToInt(steps)); // Get the absolute value of steps to represent the remaining steps to move

        while (remainingSteps > 0)
        {
            int nextStep = direction > 0 ? 1 : -1; // Determine the next step based on the direction (forward or backward)
            Vector3 nextPos = currentRoute.childNodeList[routePosition + nextStep].position;

            while (MoveToNextNode(nextPos))
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.1f);
            remainingSteps--;
            routePosition += nextStep;
        }

        // Check if the last step lands on a special tile
        if (remainingSteps == 0 && IsSpecialTile(transform.position))
        {
            ShowRandomCard();
        }

        isMoving = false;
    }


    bool MoveToNextNode(Vector3 goal)
    {
        goal.y += 0.7f; // Adjust y-coordinate to move player above tile

        if (shouldMoveWithoutDice)
        {
            shouldMoveWithoutDice = false; // Reset the flag
            return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, 2f * Time.deltaTime));
        }
        else
        {
            return goal != (transform.position = Vector3.MoveTowards(transform.position, goal, 2f * Time.deltaTime));
        }
    }

    bool IsSpecialTile(Vector3 position)
    {
        float detectionDistance = 0.7f; // Adjust this value based on your game's needs

        foreach (Transform tile in currentRoute.childNodeList)
        {
            float distance = Vector3.Distance(position, tile.position);
            if (distance <= detectionDistance)
            {
                if (tile.CompareTag("Exercise"))
                {
                    currentCategory = CardCategory.Exercise;
                    return true;
                }
                else if (tile.CompareTag("Food"))
                {
                    currentCategory = CardCategory.Food;
                    return true;
                }
                else if (tile.CompareTag("Self"))
                {
                    currentCategory = CardCategory.Self;
                    return true;
                }

            }
        }

        return false;
    }

    void Start()
    {
        DOTween.Init();
        // Disable the card container and its children at the start of the game
        CardContainer.SetActive(false);
    }


    void ShowRandomCard()
    {
        GameObject cardPrefab = null;

        switch (currentCategory)
        {
            case CardCategory.Exercise:
                cardPrefab = Random.Range(0, 2) == 0 ? positiveExercisePrefab : negativeExercisePrefab;
                break;

            case CardCategory.Food:
                cardPrefab = Random.Range(0, 2) == 0 ? positiveFoodPrefab : negativeFoodPrefab;
                break;

            case CardCategory.Self:
                cardPrefab = Random.Range(0, 2) == 0 ? positiveSelfPrefab : negativeSelfPrefab;
                break;

            default:
                Debug.LogWarning("Invalid card category!");
                return;
        }

        // Get the chosen card prefab
        GameObject chosenCard = cardPrefab.transform.GetChild(0).gameObject;

        if (chosenCard != null)
        {
            Debug.Log("Chosen card: " + chosenCard.name);

            // Get the count of child card objects
            int childCount = chosenCard.transform.childCount;

            if (childCount > 0)
            {
                // Generate a random index within the child count range
                int randomIndex = Random.Range(0, childCount);

                // Get the randomly selected child card object
                GameObject childWithTag = chosenCard.transform.GetChild(randomIndex).gameObject;

                if (childWithTag != null)
                {
                    Debug.Log("Chosen card tag: " + childWithTag.tag);

                    // Modify the steps based on the child's tag
                    switch (childWithTag.tag)
                    {
                        case "+2 Card":
                            steps = 2;
                            break;

                        case "-2 Card":
                            steps = -2;
                            break;

                        case "+3 Card":
                            steps = 3;
                            break;

                        case "-3 Card":
                            steps = -3;
                            break;

                        default:
                            Debug.LogWarning("Invalid card tag!");
                            return;
                    }

                    // Clone the chosen card
                    clonedCard = Instantiate(chosenCard, centerPosition, Quaternion.identity);
                    clonedCard.SetActive(true);
                    shouldMoveWithoutDice = true;
                    StartCoroutine(Move());
                }
                else
                {
                    Debug.LogWarning("Randomly selected child card object is null!");
                }
            }
            else
            {
                Debug.LogWarning("No child card objects found under the chosen card!");
            }
        }
        else
        {
            Debug.LogWarning("Card not found in the chosen prefab!");
        }
    }




}