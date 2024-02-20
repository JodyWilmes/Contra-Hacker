using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPossessions : MonoBehaviour
{
    public static PlayerPossessions Instance;
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    [SerializeField] private int money;
    [SerializeField] public List<BuyableTrinkets> boughtTrinkets;
    // Start is called before the first frame update
    public void AddMoney(int addedmoney) //can be negative so
    {
        money += addedmoney;
    }

    public int GetMoney()
    {
        return money;
    }
    public void SetMoney(int addedmoney)
    {
        money = addedmoney;
    }
    public void ObtainItem(BuyableTrinkets item)
    {
        boughtTrinkets.Add(item);
    }
    public List<BuyableTrinkets> GetTrinkets()
    {
        return boughtTrinkets;
    }
}
