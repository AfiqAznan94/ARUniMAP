using UnityEngine;
using UnityEngine.UI;
using System.Xml;



public class Bilikscript : MonoBehaviour
{
    public GameObject itemUIPrefab;
    public GameObject inventoryScreenGO;
    public GameObject inventoryScreen2;
    public Transform inventoryContainer;
    private Transform _btne;
    XmlDocument itemDataXml;
     Color[] store;





    void Update()
    {




        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit Hit;
            if (Physics.Raycast(ray, out Hit))
            {
                if (_btne != null)
                {


                    var btnee = _btne.GetComponentInParent<Renderer>();
                    int lengthsa2 = btnee.materials.Length;
                    int i2 = 0;
                    foreach (Material mat in btnee.materials)
                    {

                        mat.color = store[i2];
                        i2++;

                    }
                    _btne = null;

                }

                var btnName = Hit.transform;
                var btne = Hit.transform.GetComponentInParent<Renderer>();
                foreach (Transform t in inventoryContainer)
                {
                    Destroy(t.gameObject);
                }

                int lengthsa = btne.materials.Length;
                int i = 0;

                FindItemsWithID(btnName.name);
                store = new Color[lengthsa];
                foreach (Material mat in btne.materials)
                {

                    store[i] = mat.color;
                    mat.color = Color.yellow;
                    i++;

                }
                _btne = btnName;



            }
        }
    }


    private void Awake()
    {
        TextAsset xmlTextAsset = Resources.Load<TextAsset>("XML/InventoryItemData");
        itemDataXml = new XmlDocument();
        itemDataXml.LoadXml(xmlTextAsset.text);
        inventoryScreenGO.SetActive(false);
        inventoryScreen2.SetActive(false);
 
    }



    public void FindItemsWithID(string itemID)
    {
        XmlNode curNode = itemDataXml.SelectSingleNode("/InventoryItems/InventoryItem[@ID='" + itemID + "']");
        if (curNode == null)
        {
            inventoryScreenGO.SetActive(false);
            inventoryScreen2.SetActive(false);
            return;
        }

        SpawnInventoryItem(curNode);
        ShowInventoryItems();
    }

    void ShowInventoryItems()
    {
        inventoryScreenGO.SetActive(true);
        inventoryScreen2.SetActive(true);
    }

    void SpawnInventoryItem(XmlNode item)
    {
        Debug.Log("Spawning Inventory Item");

        GameObject newItemUI = GameObject.Instantiate(itemUIPrefab, inventoryContainer);

        InventoryItem newInventoryItem = new InventoryItem(item);
        newInventoryItem.UpdateInventoryUI(newItemUI);
    }

    public void OnButtonBack()
    {
        foreach (Transform t in inventoryContainer)
        {
            Destroy(t.gameObject);
        }

        inventoryScreenGO.SetActive(false);
    }

    class InventoryItem
    {
        public string itemID { get; private set; }
        public string itemType { get; private set; }
        public string itemTitle { get; private set; }
        public string itemDescription { get; private set; }
        public Color bgColor { get; private set; }
        public Texture itemImage { get; private set; }

        public InventoryItem(XmlNode curItemNode)
        {
            itemID = curItemNode.Attributes["ID"].Value;
            itemType = curItemNode.Attributes["Type"].Value;
            itemTitle = curItemNode["ItemTitle"].InnerText;
            itemDescription = curItemNode["ItemDesc"].InnerText;


            bgColor = new Color(0, 0, 0, 0);

            
            string pathToImage = "InventoryIcons/" + curItemNode["Image"].InnerText;
            itemImage = Resources.Load<Texture2D>(pathToImage);
        }


        public void UpdateInventoryUI(GameObject inventoryUI)
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

            if (itemBGPanel == null)
            {
                Debug.LogError("Error could not find itemBGPanel on inventoryUITransform: " + inventoryUITransform.gameObject.name);
            }
            if (itemRawImage == null)
            {
                Debug.LogError("Error could not find itemRawImage on inventoryUITransform: " + inventoryUITransform.gameObject.name);
            }
            if (itemTitleText == null)
            {
                Debug.LogError("Error could not find itemTitleText on inventoryUITransform: " + inventoryUITransform.gameObject.name);
            }
            if (itemDescriptionText == null)
            {
                Debug.LogError("Error could not find itemDescriptionText on inventoryUITransform: " + inventoryUITransform.gameObject.name);
            }

            itemBGPanel.color = bgColor;
            itemRawImage.texture = itemImage;
            itemTitleText.text = itemTitle;
            itemDescriptionText.text = itemDescription;
        }
    }
}
