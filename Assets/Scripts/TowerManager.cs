using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : Loader<TowerManager>
{
    TowersBtn  towerBtnPressed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectedTower(TowersBtn towerSelected)
    {
        towerBtnPressed = towerSelected;
        Debug.Log("Pressed: " + towerBtnPressed.gameObject);
    }
}
