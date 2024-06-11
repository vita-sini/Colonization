using TMPro;
using UnityEngine;

public class AvailableResources : MonoBehaviour
{
    [SerializeField] private TMP_Text _freeCountResourse;
    [SerializeField] private BaseBot _baseBot;

    private void OnEnable()
    {
        _baseBot.ResourcesChanged += ChangedText;
    }

    private void OnDisable()
    {
        _baseBot.ResourcesChanged -= ChangedText;
    }

    private void ChangedText(int freeCountResourse)
    {
        _freeCountResourse.text = freeCountResourse.ToString();
    }
}
