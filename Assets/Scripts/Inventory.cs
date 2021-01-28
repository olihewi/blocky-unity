using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory PlayerInventoryPointer;
    
    public List<Block> allBlocks = new List<Block>();
    public Block[] hotbar = new Block[NUMBER_OF_HOTBAR_SLOTS];

    public const int NUMBER_OF_HOTBAR_SLOTS = 10;
    public Canvas hotbarCanvas;
    public GameObject hotbarUiPrefab;
    public RectTransform hotbarSelected;

    public Canvas inventoryCanvas;
    public GameObject inventoryUiPrefab;

    private int currentHotbarSelection = 0;

    private float uiItemWidth;
    private float inventoryItemWidth;
    private FreeCamera _freeCamera;
    private List<GameObject> hotbarUIs = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        PlayerInventoryPointer = this;
        _freeCamera = GetComponent<FreeCamera>();
        uiItemWidth = hotbarUiPrefab.GetComponent<RectTransform>().rect.width;
        for (int i = 0; i < NUMBER_OF_HOTBAR_SLOTS; i++)
        {
            GameObject thisUIItem = Instantiate(hotbarUiPrefab, hotbarCanvas.transform, false);
            thisUIItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(i*uiItemWidth-uiItemWidth*NUMBER_OF_HOTBAR_SLOTS/2,0);
            Vector2[] thisItemUvs = hotbar[i].textures[2].GetUVs();
            thisUIItem.GetComponentInChildren<RawImage>().uvRect = new Rect(thisItemUvs[0],thisItemUvs[2]-thisItemUvs[0]);
            hotbarUIs.Add(thisUIItem);
        }
        hotbarSelected.anchoredPosition = new Vector2(-uiItemWidth*NUMBER_OF_HOTBAR_SLOTS/2,0);

        inventoryItemWidth = inventoryUiPrefab.GetComponent<RectTransform>().rect.width;
        int numInventoryColumns = Mathf.FloorToInt(inventoryCanvas.GetComponent<RectTransform>().rect.width/inventoryItemWidth);
        for (int i = 0; i < allBlocks.Count; i++)
        {
            GameObject thisInventoryItem = Instantiate(inventoryUiPrefab, inventoryCanvas.transform, false);
            thisInventoryItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(i % numInventoryColumns * inventoryItemWidth - numInventoryColumns * inventoryItemWidth / 2, i / numInventoryColumns * -inventoryItemWidth);
            thisInventoryItem.GetComponent<ContainerItem>().SetBlock(allBlocks[i]);
        }

        inventoryCanvas.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SetHotbarPosition(9);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetHotbarPosition(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetHotbarPosition(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetHotbarPosition(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetHotbarPosition(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetHotbarPosition(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SetHotbarPosition(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SetHotbarPosition(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SetHotbarPosition(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SetHotbarPosition(8);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            SetHotbarPosition(currentHotbarSelection+1);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            SetHotbarPosition(currentHotbarSelection-1);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            bool enabled1 = inventoryCanvas.enabled;
            enabled1 = !enabled1;
            inventoryCanvas.enabled = enabled1;
            Cursor.lockState = enabled1 ? CursorLockMode.None : CursorLockMode.Locked;
            _freeCamera.isCameraRotating = !enabled1;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && inventoryCanvas.enabled)
        {
            inventoryCanvas.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            _freeCamera.isCameraRotating = true;
        }
        
    }
    public void SetHotbarPosition(int _position)
    {
        currentHotbarSelection = _position % NUMBER_OF_HOTBAR_SLOTS;
        currentHotbarSelection = currentHotbarSelection < 0 ? NUMBER_OF_HOTBAR_SLOTS - 1 : currentHotbarSelection;
        hotbarSelected.anchoredPosition = new Vector2(currentHotbarSelection * uiItemWidth - uiItemWidth * NUMBER_OF_HOTBAR_SLOTS / 2,0);
    }

    public Block GetHotbarItem()
    {
        return hotbar[currentHotbarSelection];
    }

    public int GetHotbarIndex()
    {
        return currentHotbarSelection;
    }

    public void SetHotbarItem(Block _block)
    {
        hotbar[currentHotbarSelection] = _block;
        Vector2[] thisBlockUVs = _block.textures[2].GetUVs();
        hotbarUIs[currentHotbarSelection].GetComponentInChildren<RawImage>().uvRect = new Rect(thisBlockUVs[0],thisBlockUVs[2]-thisBlockUVs[0]);
    }
    
}
