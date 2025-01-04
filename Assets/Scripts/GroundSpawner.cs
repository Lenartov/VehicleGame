using System.Collections.Generic;
using UnityEngine;

public partial class Ground
{
    public class GroundSpawner 
    {
        private GroundPart GroundPartPrefab;
        private int partsCount;

        private Vector3 currentSpawnPos;
        private List<GroundPart> parts = new List<GroundPart>();

        public GroundSpawner(GroundPart groundPartPrefab)
        {
            GroundPartPrefab = groundPartPrefab;
        }

        public void SpawnGround(Transform parent, int countToSpawn)
        {
            partsCount = countToSpawn;
            for (int i = 0; i < partsCount; i++)
            {
                SpawnNextPart(parent);
            }
        }

        public void Clear()
        {
            //todo
        }

        private void SpawnNextPart(Transform parent)
        {
            GroundPart part = Instantiate(GroundPartPrefab, parent.position + currentSpawnPos, Quaternion.identity, parent);
            currentSpawnPos.z += part.Size.y;
            parts.Add(part);
        }
    }




}
