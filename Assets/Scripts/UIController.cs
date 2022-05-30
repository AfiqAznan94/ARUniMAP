using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using UnityEngine.EventSystems;


public class UIController : MonoBehaviour {

    public GameObject itemUIPrefab;
    public GameObject inventoryScreenGO;
    public GameObject inventoryScreen2;
    public Transform inventoryContainer;
    Transform _btne;
    public GameObject nextimagee;
    public Dropdown[] drop;
    XmlDocument itemDataXml;
    string dd;
    Color[] store;
    XmlNode ce;
    int _imagenum;
    GameObject ddo,_ddo;


    public void HandleInputData(int val)
    {
        dd = drop[0].options[drop[0].value].text;

        if (_ddo != null && _btne != null)
        {
            var ddos = _ddo.GetComponent<Renderer>();

            
            int lengthsa2 = ddos.materials.Length;
            int i2 = 0;
            Vector3 scale2 = ddos.transform.localScale;
            scale2 /= 1.2f;
            ddos.transform.localScale = scale2;
            foreach (Material mat in ddos.materials)
            {

                mat.color = store[i2];
                i2++;

            }
            _ddo = null;
            _btne = null;
            

        }




        ddo = GameObject.Find(dd);
        var dds = ddo.name;
        var dds2 = ddo.GetComponent<Renderer>();
        foreach (Transform t in inventoryContainer)
        {
            Destroy(t.gameObject);
        }

        FindItemsWithID(ddo.name);
        int lengthsa = dds2.materials.Length;
        int i = 0;
        Vector3 scale = ddo.transform.localScale;
        store = new Color[lengthsa];

        foreach (Material mat in dds2.materials)
        {
            store[i] = mat.color;
            mat.color = Color.red;
            i++;

        }
        scale *= 1.2f;
        ddo.transform.localScale = scale;
        _ddo = ddo;
        _btne = ddo.transform;



    }


    void Update()
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {

            
        }
        else
        {


            if (/*Input.GetMouseButtonDown(0)*/   Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                

                Ray ray = Camera.main.ScreenPointToRay(/*Input.mousePosition */    Input.GetTouch(0).position);
                RaycastHit Hit;
                if (Physics.Raycast(ray, out Hit))
                {
                    if (Hit.transform.name == "nextimage" || Hit.transform.name == "Dropdown")
                    {

                    }
                    else
                    {

                        if (_btne != null && _ddo != null)
                        {
                            var btnee = _btne.GetComponent<Renderer>();



                            int lengthsa2 = btnee.materials.Length;
                            int i2 = 0;
                            Vector3 scale2 = btnee.transform.localScale;
                            scale2 /= 1.2f;
                            btnee.transform.localScale = scale2;
                            foreach (Material mat in btnee.materials)
                            {

                                mat.color = store[i2];
                                i2++;

                            }
                            _btne = null;


                        }

                        var btnName = Hit.transform;


                        var btne = Hit.transform.GetComponent<Renderer>();
                        foreach (Transform t in inventoryContainer)
                        {
                            Destroy(t.gameObject);
                        }

                        FindItemsWithID(btnName.name);
                        int lengthsa = btne.materials.Length;
                        int i = 0;
                        Vector3 scale = btnName.transform.localScale;
                        store = new Color[lengthsa];

                        foreach (Material mat in btne.materials)
                        {
                            store[i] = mat.color;
                            mat.color = Color.red;
                            i++;

                        }
                        scale *= 1.2f;
                        btnName.transform.localScale = scale;
                        _btne = btnName;
                        _ddo = btnName.gameObject;


                    }
                }
            }



        }


       
    }

    public void nextimage()
    {


            foreach (Transform t in inventoryContainer)
            {
                Destroy(t.gameObject);
            }
            GameObject newItemUI = GameObject.Instantiate(itemUIPrefab, inventoryContainer);

            if (_imagenum < InventoryItem.itemImage.Length - 1)
                _imagenum++;
            else
                _imagenum = 0;

            Debug.LogWarning(InventoryItem.itemImage.Length);
            Debug.LogWarning(_imagenum);
            InventoryItem newInventoryItem = new InventoryItem(ce, nextimagee);
            newInventoryItem.UpdateInventoryUI(newItemUI, _imagenum);



    }

    private void Awake()
    {
        TextAsset xmlTextAsset = Resources.Load<TextAsset>("XML/InventoryItemData");
        itemDataXml = new XmlDocument();
        itemDataXml.LoadXml(xmlTextAsset.text);
        inventoryScreenGO.SetActive(false);
        inventoryScreen2.SetActive(false);
        nextimagee.SetActive(false);
    }



    public void FindItemsWithID(string itemID)
    {
        XmlNode curNode = itemDataXml.SelectSingleNode("/InventoryItems/InventoryItem[@ID='" + itemID + "']");

        if (curNode == null)
        {
            inventoryScreenGO.SetActive(false);
            inventoryScreen2.SetActive(false);
            nextimagee.SetActive(false);
            return;
        }

        SpawnInventoryItem(curNode);
        ce = curNode;
        ShowInventoryItems();
    }

    void ShowInventoryItems()
    {
        inventoryScreenGO.SetActive(true);
        inventoryScreen2.SetActive(true);
    }

    void SpawnInventoryItem(XmlNode item)
    {

        _imagenum = 0;
        GameObject newItemUI = GameObject.Instantiate(itemUIPrefab, inventoryContainer);
        InventoryItem newInventoryItem = new InventoryItem(item, nextimagee);
        newInventoryItem.UpdateInventoryUI(newItemUI, _imagenum);
    }


    public class InventoryItem
    {
        public string itemID { get; private set; }
        public static string itemType { get; private set; }
        public string itemTitle { get; private set; }
        public string itemDescription { get; private set; }
        public Color bgColor { get; private set; }
        public static Texture[] itemImage { get; private set; }


        public InventoryItem(XmlNode curItemNode, GameObject heh)
        {
            itemID = curItemNode.Attributes["ID"].Value;
            itemType = curItemNode.Attributes["Type"].Value;
            itemTitle = curItemNode["ItemTitle"].InnerText;
            itemDescription = curItemNode["ItemDesc"].InnerText;
            string pathToImage;
            XmlNode colorNode = curItemNode.SelectSingleNode("Color");

            if (itemType == "mul")
            {

                heh.SetActive(true);
                pathToImage = "InventoryIcons/" + curItemNode["Image"].InnerText + "/";
            }
            else
            {
                heh.SetActive(false);
                pathToImage = "InventoryIcons/" + curItemNode["Image"].InnerText;
            }
            bgColor = new Color(0, 0, 0, 0);

            Debug.LogWarning(pathToImage);



            itemImage = Resources.LoadAll<Texture2D>(pathToImage);


        }



        public void UpdateInventoryUI(GameObject inventoryUI, int count)
        {
            Transform inventoryUITransform = inventoryUI.transform;
            Image itemBGPanel;
            RawImage itemRawImage;
            Text itemTitleText;
            Text itemDescriptionText;

            itemBGPanel = inventoryUITransform.GetComponent<Image>();
            Transform itemBGPanelTransform = itemBGPanel.GetComponent<Transform>();
            itemRawImage = itemBGPanelTransform.Find("ItemRawImage").GetComponent<RawImage>();
            itemTitleText = itemBGPanelTransform.Find("ItemTitleText").GetComponent<Text>();
            itemDescriptionText = itemBGPanelTransform.Find("ItemDescriptionText").GetComponent<Text>();


            itemBGPanel.color = bgColor;
            itemRawImage.texture = itemImage[count];

            itemTitleText.text = itemTitle;
            itemDescriptionText.text = itemDescription;
        }
    }
}
