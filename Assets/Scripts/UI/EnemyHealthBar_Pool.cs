using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar_Pool : PoolBase<UI_HealthBarEnemy, EnemyHealthBar_Pool>
{
    public void ShowHealthBar(HealthBase hp, float offsetY)
    {
        var item = GetPoolItem();
        item.SwitchHealthBase(hp, offsetY);
    }

    protected override bool CheckItem(UI_HealthBarEnemy item)
    {
        return item.uiHealth.sizeDelta.x == 0;
    }
}
