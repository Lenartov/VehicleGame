using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private GroundPart GroundPartPrefab;
    [Space]
    [SerializeField] private int partsCount;

    private Vector3 currentSpawnPos;
    private List<GroundPart> parts = new List<GroundPart>();

    private void Start()
    {
        SpawnGround();
    }

    public void SpawnGround()
    {
        for (int i = 0; i < partsCount; i++)
        {
            SpawnNextPart();
        }
    }

    public void SpawnNextPart()
    {
        GroundPart part = Instantiate(GroundPartPrefab, transform.position + currentSpawnPos, Quaternion.identity, transform);
        currentSpawnPos.z += part.Size.y;
        parts.Add(part);
    }
}
