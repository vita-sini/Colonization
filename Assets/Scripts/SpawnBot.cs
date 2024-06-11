using UnityEngine;

public class SpawnBot : MonoBehaviour
{
    [SerializeField] private Unit _botPrefab;
    [SerializeField] private BaseBot _baseBot;

    public Unit Spawn(Vector3 randomPosition)
    {
        return Instantiate(_botPrefab, randomPosition, transform.rotation);
    }
}
