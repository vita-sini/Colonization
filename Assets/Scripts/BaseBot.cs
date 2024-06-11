using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Scanner), typeof(SpawnBot))]
public class BaseBot : MonoBehaviour
{
    private float _resourceCollectionDelay = 0.1f;
    private float _spawnRadius = 3f;
    private int _resourcesForNewBase = 5;
    private int _resourceCount = 0;
    private int _startCountBots = 3;

    private bool _isFlagPlaced = false;
    private bool _isCreatedUnit = false;

    public List<Unit> _bots = new List<Unit>();
    private Dictionary<Resource, bool> _resourceStates = new Dictionary<Resource, bool>();

    private SpawnBot _createBot;
    private Scanner _scanner;
    private Flag _flag;

    public event UnityAction<int> ResourcesChanged;

    private void Awake()
    {
        _scanner = GetComponent<Scanner>();
        _createBot = GetComponent<SpawnBot>();
    }

    private void Start()
    {
        if (!_isCreatedUnit)
        {
            CreateBot(_startCountBots);
        }

        StartCoroutine(CollectResourcesRoutine());
    }

    public void SetUnitCreated()
    {
        _isCreatedUnit = true;
    }

    public void SetFlag(Flag flag)
    {
        _flag = flag;
        _isFlagPlaced = true;
    }

    public void TakeResource(Resource resource)
    {
        _resourceStates[resource] = false;
        _resourceCount++;
        ResourcesChanged?.Invoke(_resourceCount);

        if (_isFlagPlaced)
        {
            if (_resourceCount >= _resourcesForNewBase)
            {
                SpawnNewBase();
                _resourceCount -= _resourcesForNewBase;
            }
        }
        else
        {
            CeateNewBot();
        }
    }

    public void RemoveFlag()
    {
        _isFlagPlaced = false;
        Destroy(_flag.gameObject);
        _flag = null;
    }

    private void SpawnNewBase()
    {
        foreach (Unit bot in _bots)
        {
            if (!bot._isBusy)
            {
                bot.SetDestination(_flag);
                bot.DetachUnit(); 
                break;
            }
        }
    }

    private void CeateNewBot()
    {
        if (_resourceCount >= 3)
        {
            _resourceCount -= 3;
            CreateBot(1);
        }
    }

    private void CreateBot(int startCount)
    {
        for (int i = 0; i < startCount; i++)
        {
            float randomX = Random.Range(-_spawnRadius, _spawnRadius);
            float randomZ = Random.Range(-_spawnRadius, _spawnRadius);
            Vector3 randomPosition = transform.position + new Vector3(randomX, 0, randomZ);
            Unit bot = _createBot.Spawn(randomPosition);
            bot.SetBaseBot(this);
            _bots.Add(bot);;
        }
    }

    private IEnumerator CollectResourcesRoutine()
    {
        var waitSeconds = new WaitForSeconds(_resourceCollectionDelay);

        while (true)
        {
            yield return waitSeconds;
            CollectResource();
        }
    }

    private void CollectResource()
    {
        Resource resource = _scanner.GetAllResources().FirstOrDefault(resource => !_resourceStates.ContainsKey(resource) || !_resourceStates[resource]);

        if (resource != null)
        {
            foreach (Unit bot in _bots)
            {
                if (!bot._isBusy)
                {
                    bot.SetDestination(resource);
                    _resourceStates[resource] = true;
                    break;
                }
            }
        }
    }
}
