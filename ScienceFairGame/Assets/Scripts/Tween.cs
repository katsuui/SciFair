/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tween : MonoBehaviour
{
    public float offscreenX = 10f; // X position off the screen
    public float targetX = 0f; // X position at the middle of the screen
    public float animationDuration = 1f;

    private bool isHidden = true;
    private bool isTweening = false;

    private void Start()
    {
        // Hide the card and its child objects
        HideCard(gameObject);
    }

    public void AnimateCard(Vector3 centerPosition)
    {
        isHidden = false;
        isTweening = true;

        // Set the target position at the center of the screen
        Vector3 targetPosition = new Vector3(targetX, centerPosition.y, centerPosition.z);

        // Activate the card and its child objects
        ShowCard(gameObject);

        // Play the animation
        transform.DOMove(targetPosition, animationDuration).OnComplete(TweenComplete);
    }

    void TweenComplete()
    {
        isTweening = false;
    }

    void HideCard(GameObject card)
    {
        // Deactivate the card and its child objects recursively
        foreach (Transform child in card.transform)
        {
            HideCard(child.gameObject);
        }

        card.SetActive(false);
    }

    void ShowCard(GameObject card)
    {
        // Enable the card and its child objects recursively
        card.SetActive(true);

        foreach (Transform child in card.transform)
        {
            ShowCard(child.gameObject);
        }
    }



}
*/