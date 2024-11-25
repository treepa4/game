using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : Loader<TowerManager>
{
    TowersBtn  towerBtnPressed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero);

            


            if (hit.collider.tag == "PlaceForTower")
            {
                PlaceTower(hit);
            }

        } 
    }

    public void PlaceTower(RaycastHit2D hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null)
        {
            GameObject newTower = Instantiate(towerBtnPressed.TowerObject);
            newTower.transform.position = hit.transform.position;
        }    

    }

    public void SelectedTower(TowersBtn towerSelected)
    {
        towerBtnPressed = towerSelected;
        Debug.Log("Pressed: " + towerBtnPressed.gameObject);
    }
}
