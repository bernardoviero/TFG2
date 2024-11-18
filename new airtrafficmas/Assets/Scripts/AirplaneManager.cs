using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneManager : MonoBehaviour
{
    public GameObject airplanePrefab;
    private List<GameObject> airplanesInAir = new List<GameObject>();
    private GameObject airplaneOnRunway = null;
    private int timeStep = 0;
    private float animationSpeed = 0.3f;
    private int airplaneCounter = 1;
    private int totalAirplanes = 30;
    
    private Vector2 runwayStartPosition;
    private Vector2 runwayEndPosition;
    private Vector2 skyExitPosition;
    private float safeDistance = 1.5f;

    void Start()
    {
        if (airplanePrefab == null)
        {
            Debug.LogError("Prefab de avião não atribuído no AirplaneManager.");
            return;
        }

        runwayStartPosition = new Vector2(-4.6f, -3f);
        runwayEndPosition = new Vector2(4.21f, -3f);
        skyExitPosition = new Vector2(6f, Random.Range(4f, 5f));

    }

    private IEnumerator AddAirplanes(int count)
    {
        float skyYMin = 1.0f;
        float skyYMax = 5.0f;
        float skyXMin = -8.0f;
        float skyXMax = 6.0f;

        for (int i = 0; i < count && airplaneCounter <= totalAirplanes; i++)
        {
            float randomY = Random.Range(skyYMin, skyYMax);
            float randomX = Random.Range(skyXMin, skyXMax);
            Vector2 entryPosition = new Vector2(randomX, randomY);
            
            if (IsAnyPlaneTooClose(entryPosition)) 
            {
                i--;
                continue;
            }
            
            GameObject newAirplane = Instantiate(airplanePrefab, entryPosition, Quaternion.identity);
            AirplaneMovement movementComponent = newAirplane.GetComponent<AirplaneMovement>();
            if (movementComponent == null)
            {
                Debug.LogError("AirplaneMovement component missing on the airplane prefab.");
                Destroy(newAirplane);
                yield break;
            }

            movementComponent.SetSpeed(animationSpeed);
            movementComponent.SetName($"Airplane {airplaneCounter++}");
            
            Vector2 skyTargetPosition = new Vector2(Random.Range(skyXMin, skyXMax), Random.Range(skyYMin, skyYMax));
            movementComponent.MoveToPosition(entryPosition, skyTargetPosition);
            airplanesInAir.Add(newAirplane);
            yield return new WaitForSeconds(0.5f);
        }
    }


    private IEnumerator GradualLandAirplane()
    {
        if (airplanesInAir.Count > 0 && airplaneOnRunway == null)
        {
            GameObject airplane = airplanesInAir[0];
            airplanesInAir.RemoveAt(0);
            AirplaneMovement movementComponent = airplane.GetComponent<AirplaneMovement>();
            if (movementComponent != null)
            {
                Vector2 currentAirplanePosition = airplane.transform.position;
                float randomLandingY = Random.Range(-4f, -1f);
                Vector2 landingPosition = new Vector2(runwayEndPosition.x, randomLandingY);

                movementComponent.MoveToPosition(currentAirplanePosition, landingPosition, false, true);
                airplaneOnRunway = airplane;
                yield return new WaitForSeconds(2f);
            }
            else
            {
                Debug.LogError("AirplaneMovement component missing on airplane attempting to land.");
            }
        }
    }

    private IEnumerator TakeOffAirplane()
    {
        if (airplaneOnRunway != null)
        {
            AirplaneMovement movementComponent = airplaneOnRunway.GetComponent<AirplaneMovement>();
            if (movementComponent != null)
            {
                Vector2 currentRunwayPosition = airplaneOnRunway.transform.position;
                Vector2 adjustedSkyExit = new Vector2(skyExitPosition.x, Random.Range(4f, 6f));
                movementComponent.MoveToPosition(currentRunwayPosition, adjustedSkyExit, true, false);
                airplaneOnRunway = null;
                yield return new WaitForSeconds(2f);
            }
            else
            {
                Debug.LogError("AirplaneMovement component missing on airplane attempting to take off.");
            }
        }
    }

    private bool IsAnyPlaneTooClose(Vector2 position)
    {
        foreach (GameObject airplane in airplanesInAir)
        {
            if (Vector2.Distance(airplane.transform.position, position) < safeDistance)
            {
                return true;
            }
        }

        if (airplaneOnRunway != null && Vector2.Distance(airplaneOnRunway.transform.position, position) < safeDistance)
        {
            return true;
        }

        return false;
    }
}