using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowersBtn : MonoBehaviour
{   
    [SerializeField]
    Sprite dragSprite;
    [SerializeField]
    int towerPrice;
    [SerializeField]
    TowerControl towerObject;
    public TowerControl TowerObject
    {
        get
        {

            return towerObject; 
        }
    }

    public Sprite DragSprite
    {
        get
        {
            return dragSprite; 
        }
    }

    public int TowerPrice
    {
        get
        {
            return towerPrice;
        }
    }
}
