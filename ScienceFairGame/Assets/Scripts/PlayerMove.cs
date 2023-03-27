using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float jumpHeight = 1f;
    public LayerMask tileLayerMask;

    private bool isJumping = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Example setup for the tile layer mask
        tileLayerMask = LayerMask.GetMask("Tiles");
    }

    void Update()
    {
        if (!isJumping && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, tileLayerMask))
            {
                Vector3 jumpTarget = hit.collider.transform.position + Vector3.up * (hit.collider.transform.localScale.y + jumpHeight);

                StartCoroutine(JumpToTile(jumpTarget));
            }
        }
    }

    IEnumerator JumpToTile(Vector3 targetPosition)
    {
        isJumping = true;

        Vector3 startPosition = transform.position;
        float jumpDuration = Mathf.Sqrt(Vector3.Distance(startPosition, targetPosition) / Physics.gravity.magnitude);

        float time = 0f;

        while (time < jumpDuration)
        {
            float height = Mathf.Sin(Mathf.PI * (time / jumpDuration)) * jumpHeight;

            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, time / jumpDuration);
            currentPosition.y += height;

            rb.MovePosition(currentPosition);

            time += Time.deltaTime;

            yield return null;
        }

        rb.MovePosition(targetPosition);

        isJumping = false;
    }
}
