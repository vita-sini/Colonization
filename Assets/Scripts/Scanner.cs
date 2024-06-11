using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    private float _radius = 360f;
    private float _maxDistance = Mathf.Infinity;

    public List<Resource> GetAllResources()
    {
        RaycastHit[] raycastHits = Physics.SphereCastAll(transform.position, _radius, Vector3.one, _maxDistance, _layerMask);

        List<Resource> resources = new List<Resource>();

        foreach (RaycastHit hit in raycastHits)
        {
            if (hit.collider.TryGetComponent(out Resource resource))
            {
                resources.Add(resource);
            }
        }

        return resources;
    }
}
