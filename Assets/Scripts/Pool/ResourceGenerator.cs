using System.Collections;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private ObjectPool _pool;

    private float _minPositionAxis = -20.0f;
    private float _maxPositionAxis = 20.0f;
    private float _positionAxisY = 0.5f;

    private void Start()
    {
        StartCoroutine(GenerateResource());
    }

    private IEnumerator GenerateResource()
    {
        var wait = new WaitForSeconds(_delay);

        while (enabled)
        {
            Spawn();
            yield return wait;
        }
    }

    private void Spawn()
    {
        Vector3 spawnPoint = new Vector3(Random.Range(_minPositionAxis, _maxPositionAxis), _positionAxisY, Random.Range(_minPositionAxis, _maxPositionAxis));

        var resourse = _pool.GetObject();

        resourse.transform.position = spawnPoint;
       
        resourse.gameObject.SetActive(true);

        resourse.ReleasingResource += ReleaseResource;
    }

    private void ReleaseResource(Resource resource)
    {
        resource.ReleasingResource -= ReleaseResource;
        _pool.PutObject(resource);
    }
}
