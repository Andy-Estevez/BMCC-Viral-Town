using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawnScript : MonoBehaviour
{
    public GameObject HousePrefab;
    public GameObject DuplexPrefab;
    public GameObject CondoPrefab;

    public GameObject OfficeBluePrefab;
    public GameObject OfficeGreyPrefab;

    public GameObject MallPrefab;
    public GameObject SupermarketPrefab;
    public GameObject RestaurantPrefab;
    public GameObject ShopPrefab;
    public GameObject ShopCoffeePrefab;

    public GameObject HospitalPrefab;

    public List<GameObject> residenceList;
    public List<GameObject> officeList;
    public List<GameObject> commercialList;


    void Awake()
    {
        residenceList = new List<GameObject>();
        officeList = new List<GameObject>();
        commercialList = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        residenceList.Add(HousePrefab);
        residenceList.Add(DuplexPrefab);
        residenceList.Add(CondoPrefab);

        officeList.Add(OfficeBluePrefab);
        officeList.Add(OfficeGreyPrefab);

        commercialList.Add(MallPrefab);
        commercialList.Add(SupermarketPrefab);
        commercialList.Add(RestaurantPrefab);
        commercialList.Add(ShopPrefab);
        commercialList.Add(ShopCoffeePrefab);

        //residenceList[Random.Range(0, residenceList.Count)]
        //officeList[Random.Range(0, officeList.Count)]
        //commercialList[Random.Range(0, commercialList.Count)]

    }

    // Update is called once per frame
    void Update()
    {
        // IsEveryoneDead() bool every time a building's occupancy (mortality?) goes to 0
        // end game if everyone is dead
    }
}
