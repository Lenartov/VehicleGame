using UnityEngine;

public class GroundPart : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;

    public Vector3 Size => Vector3.Scale(meshFilter.mesh.bounds.size, meshFilter.transform.localScale);
}
