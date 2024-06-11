using UnityEngine;

public class User : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Flag _flag;

    private BaseBot _selectedBaseBot;
    private Flag _currentFlag;
    private bool _isBaseBotSelected;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (_isBaseBotSelected)
                {
                    if (hit.transform.TryGetComponent(out Ground ground))
                    {
                        if (_currentFlag == null)
                        {
                            _currentFlag = Instantiate(_flag, new Vector3(hit.point.x, hit.point.y + 1, hit.point.z), Quaternion.identity);
                            _currentFlag.transform.SetParent(_selectedBaseBot.transform);
                            _selectedBaseBot.SetFlag(_currentFlag);
                        }
                        else
                        {
                            _currentFlag.transform.position = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
                        }

                        _selectedBaseBot = null;
                        _isBaseBotSelected = false;
                    }
                }
                else
                {
                    if (hit.transform.TryGetComponent(out BaseBot baseBot))
                    {
                        _selectedBaseBot = baseBot;
                        _currentFlag = baseBot.GetComponentInChildren<Flag>();
                        _isBaseBotSelected = true;
                    }
                }
            }
        }
    }
}
