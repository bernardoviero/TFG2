using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class AirplaneMovement : MonoBehaviour
{
    private float moveSpeed = 1f;
    private Vector2 targetPosition;
    private bool isExiting = false;
    private bool landingComplete = false;
    private GameObject nameTagObject;
    private string airplaneName; // Armazena o nome do avião
    private float safeDistance = 1.5f; // Distância mínima para evitar colisões
    public static List<AirplaneMovement> allAirplanes = new List<AirplaneMovement>();

    private void Awake()
    {
        allAirplanes.Add(this);
    }

    private void OnDestroy()
    {
        allAirplanes.Remove(this);
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void SetName(string name)
    {
        airplaneName = name; // Define o nome do avião
        if (nameTagObject == null)
        {
            nameTagObject = new GameObject("NameTag");
            TextMeshPro tmp = nameTagObject.AddComponent<TextMeshPro>();

            tmp.text = name;
            tmp.fontSize = 3;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.color = Color.white;

            nameTagObject.transform.SetParent(transform);
            nameTagObject.transform.localPosition = new Vector3(0, 1f, 0);
        }
    }

    public string GetName()
    {
        return airplaneName;
    }

    public void MoveToPosition(Vector2 startPosition, Vector2 endPosition, bool exiting = false, bool landing = false)
    {
        targetPosition = endPosition;
        transform.position = startPosition;
        isExiting = exiting;
        landingComplete = landing;
        StartCoroutine(SmoothMovement());
    }

    private IEnumerator SmoothMovement()
    {
        while (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            foreach (AirplaneMovement otherAirplane in allAirplanes)
            {
                if (otherAirplane != this && Vector2.Distance(transform.position, otherAirplane.transform.position) < safeDistance)
                {
                    Vector2 avoidanceDirection = (transform.position - otherAirplane.transform.position).normalized;
                    transform.position += (Vector3)(avoidanceDirection * moveSpeed * Time.deltaTime);
                    break;
                }
            }

            transform.position = Vector2.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        if (isExiting || landingComplete)
        {
            yield return new WaitForSeconds(1f);
            Destroy(nameTagObject);
            Destroy(gameObject);
        }
    }
}