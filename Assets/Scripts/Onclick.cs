
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class Onclick : MonoBehaviour
{

    string btnName;

    public GameObject itemUIPrefab;
    public GameObject inventoryScreenGO;
    public GameObject inventoryScreen2;
    public Transform inventoryContainer;
    public GameObject nextimagee;
    private Texture2D _changer;
    XmlDocument itemDataXml;
    XmlNode ce;
    int _imagenum;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame



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
            Debug.LogError("Error could not find Inventory Item with ID: " + itemID + " in IeventoryItemData.xml");
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
        InventoryItem newInventoryItem = new InventoryItem(item , nextimagee);
        newInventoryItem.UpdateInventoryUI(newItemUI , _imagenum);
    }

    public void OnButtonBack()
    {
        foreach (Transform t in inventoryContainer)
        {
            Destroy(t.gameObject);
        }

        inventoryScreenGO.SetActive(false);
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
        InventoryItem newInventoryItem = new InventoryItem(ce , nextimagee);
        newInventoryItem.UpdateInventoryUI(newItemUI , _imagenum);



    }
    public class InventoryItem
    {
        public string itemID { get; private set; }
        public static string itemType { get; private set; }
        public string itemTitle { get; private set; }
        public string itemDescription { get; private set; }
        public Color bgColor { get; private set; }
        public static Texture[] itemImage { get; private set; }
        
        
        public InventoryItem(XmlNode curItemNode , GameObject heh)
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



        public void UpdateInventoryUI(GameObject inventoryUI , int count)
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



