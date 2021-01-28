using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContainerItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject overlay;
    public RawImage itemImage;
    public Block block;
    void Start()
    {
        overlay.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData _eventData)
    {
        overlay.SetActive(true);
    }

    public void OnPointerExit(PointerEventData _eventData)
    {
        overlay.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Inventory inventory = Inventory.PlayerInventoryPointer;
        inventory.SetHotbarItem(block);
    }

    public void SetBlock(Block _block)
    {
        block = _block;
        Vector2[] blockUvs = block.textures[3].GetUVs();
        itemImage.uvRect = new Rect(blockUvs[0],blockUvs[2]-blockUvs[0]);
    }
}
