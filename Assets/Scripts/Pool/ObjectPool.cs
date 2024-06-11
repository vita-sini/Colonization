using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private Resource _prefab;

    private Queue<Resource> _pool;

    public IEnumerable<Resource> PooledObjects => _pool;

    private void Awake()
    {
        _pool = new Queue<Resource>();
    }

    public Resource GetObject()
    {
        if (_pool.Count == 0)
        {
            var resource = Instantiate(_prefab);
            resource.transform.parent = _container;

            return resource;
        }

        return _pool.Dequeue();
    }

    public void PutObject(Resource resource)
    {
        _pool.Enqueue(resource);
        resource.gameObject.SetActive(false);
    }
}
