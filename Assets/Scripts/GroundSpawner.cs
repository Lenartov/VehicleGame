using System.Collections.Generic;
using UnityEngine;

public partial class Ground
{
    public class GroundSpawner 
    {
        private GroundPart GroundPartPrefab;
        private GameObject FinishLinePrefab;
        private int partsCount;

        private Vector3 currentSpawnPos;
        private List<GroundPart> parts = new List<GroundPart>();
        private GameObject finishLine;

        public GroundSpawner(GroundPart groundPartPrefab, GameObject finishLine)
        {
            GroundPartPrefab = groundPartPrefab;
            FinishLinePrefab = finishLine;
        }

        public void SpawnGround(Transform parent, int countToSpawn, float finishDistance)
        {
            SpawnDecorativePartBehind(parent);

            partsCount = countToSpawn;
            for (int i = 0; i < partsCount; i++)
            {
                SpawnNextPart(parent);
            }

            SpawnFinishLine(parent, finishDistance);
        }

        private void SpawnFinishLine(Transform parent, float finishDistance)
        {
            finishLine = Instantiate(FinishLinePrefab, parent);
            finishLine.transform.localPosition = new Vector3(0f, 0f, finishDistance);

            if (parts.Count > 0) 
            {
                finishLine.transform.localScale = new Vector3(parts[0].Size.x, finishLine.transform.localScale.y, finishLine.transform.localScale.z);
                return;
            }

            //inst temp part to setup finish
            GroundPart tempPart = Instantiate(GroundPartPrefab);
            finishLine.transform.localScale = new Vector3(tempPart.Size.x, finishLine.transform.localScale.y, finishLine.transform.localScale.z);
            Destroy(tempPart);
        }

        private void SpawnNextPart(Transform parent)
        {
            GroundPart part = Instantiate(GroundPartPrefab, parent.position + currentSpawnPos, Quaternion.identity, parent);
            currentSpawnPos.z += part.Size.y;
            parts.Add(part);
        }

        private void SpawnDecorativePartBehind(Transform parent)
        {
            GroundPart part = Instantiate(GroundPartPrefab, parent);
            part.transform.localPosition = new Vector3(parent.position.x, parent.position.y, parent.position.z - part.Size.y);
            parts.Add(part);
        }
    }
}
