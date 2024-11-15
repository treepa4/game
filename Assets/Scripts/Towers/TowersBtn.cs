using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowersBtn : MonoBehaviour
{
    [SerializeField]
    GameObject towerObject;
    public GameObject TowerObject
    {
        get
        {

            return towerObject; 
        }
    }
}
