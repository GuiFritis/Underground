using System.Collections;
using System.Collections.Generic;
using Floors;
using UnityEngine;
using Padrao.Core.Utils;

public class MenuBackgroundManager : MonoBehaviour
{
    public List<Floor> floors = new();
    private Floor _floor = null;
    private int _lastIndex = -1;

    void Awake()
    {
        ChangeBackground();
    }

    public void ChangeBackground()
    {
        if(_floor != null)
        {
            Destroy(_floor.gameObject);
        }
        if(_lastIndex >= 0)
        {
            _floor = floors.GetRandom(floors[_lastIndex]);
        }
        else
        {
            _floor = floors.GetRandom();
        }
        _lastIndex = floors.FindIndex(i => i.Equals(_floor));
        _floor = Instantiate(_floor, transform.position, transform.rotation);
    }
}
