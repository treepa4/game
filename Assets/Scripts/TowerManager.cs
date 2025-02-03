using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerManager : Loader<TowerManager>
{
    public TowersBtn  towerBtnPressed{get; set;}
    SpriteRenderer spriteRenderer;
    private List<TowerControl> TowerList = new List<TowerControl>();
    private List<Collider2D> BuildList = new List<Collider2D>();

    private Collider2D buildTile;


    void Start()
    {
        buildTile = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled=false;
    }

    // Update is called once per frame
    void Update()
    {
        if (spriteRenderer.enabled)
            {
                FollowMouse();
            }

        if (Input.GetMouseButtonDown(0))
    {
        Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        LayerMask mask = LayerMask.GetMask("PlaceForTower"); // Указываем слой

        RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero, Mathf.Infinity, mask);

        if (hit.collider != null && hit.collider.CompareTag("PlaceForTower"))
        {
            if (!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null)
            {
                buildTile = hit.collider;
                buildTile.tag = "TowerPlaceFull";
                RegisterBuildSite(buildTile);
                PlaceTower(hit);

            }
            
        }

        
    } 


    //     if(Input.GetMouseButtonDown(0))
    //     {
    //         Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //         RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero);

            


    //         if (hit.collider.tag == "PlaceForTower")
    //         {
    //             hit.collider.tag = "TowerPlaceFull";
    //             PlaceTower(hit);
    //         }

    //     } 
    }

    public void RegisterBuildSite(Collider2D buildTag)
    {
        BuildList.Add(buildTag);
    }
    
    public void RegisterTower(TowerControl tower)
    {
        TowerList.Add(tower);
    }

    public void RenameTagBuildSite()
    {
        foreach (Collider2D buildTag in BuildList)
        {
            buildTag.tag = "PlaceForTower";
        }
        BuildList.Clear();
    }

    public void DestroyAllTowers()
    {
        foreach (TowerControl tower in TowerList)
        {
            Destroy(tower.gameObject);
        }
        TowerList.Clear();
    }

    public void PlaceTower(RaycastHit2D hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null)
        {
            TowerControl newTower = Instantiate(towerBtnPressed.TowerObject);
            newTower.transform.position = hit.transform.position;
            BuyTower(towerBtnPressed.TowerPrice);
            RegisterTower(newTower);
            DisableDrug();
        }    

    }

    public void BuyTower(int price)
    {
        Manager.Instance.minusMoney(price);
    }

    public void SelectedTower(TowersBtn towerSelected)
    {
        // Debug.Log("Pressed: " + towerBtnPressed.gameObject);
        if (towerSelected.TowerPrice <= Manager.Instance.TotalMoney)
        {
            towerBtnPressed = towerSelected;
            EnableDrug(towerBtnPressed.DragSprite);
        }
    }

    public void FollowMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x,transform.position.y);
    }

    public void EnableDrug(Sprite sprite)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;
    }

    public void DisableDrug()
    {
        spriteRenderer.enabled = false;
    }
}
