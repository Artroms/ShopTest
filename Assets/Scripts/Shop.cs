using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private ItemConfig itemConfig;
    [SerializeField]
    private int count;

    private Dictionary<string, GameObject> allItems = new Dictionary<string, GameObject>();
    private List<string> itemsKeys;
    private List<GameObject> previewItems;
    private int currentItem;
    private int CurrentItem
    {
        get => currentItem;
        set
        {
            currentItem = value;
            onChangedTarget?.Invoke(previewItems[currentItem].transform.position);
            ChooseLockState();
        }
    }

    [SerializeField]
    private GameObject LockImage;

    public Action<Vector3> onChangedTarget;

    private void Start()
    {
        GetDataFromConfig();
        NeedsToUpdate();
    }

    public void UpdateShop()
    {
        ClearPreview();
        itemsKeys = itemConfig.GetRandomItems(count);
        PlayerData.ClearItems();
        PlayerData.Sample = new HashSet<string>(itemsKeys);
        previewItems = new List<GameObject>();
        for (int i = 0; i < itemsKeys.Count; i++)
        {
            var previewItem = Instantiate(allItems[itemsKeys[i]]);
            previewItem.transform.position = Vector3.zero + Vector3.right * 2 * i;
            previewItems.Add(previewItem);
        }
        SaveTime();
        CurrentItem = 0;
    }

    public void LoadShop()
    {
        ClearPreview();
        itemsKeys = new List<string>(PlayerData.Sample);
        previewItems = new List<GameObject>();
        for (int i = 0; i < itemsKeys.Count; i++)
        {
            var previewItem = Instantiate(allItems[itemsKeys[i]]);
            previewItem.transform.position = Vector3.zero + Vector3.right * 2 * i;
            previewItems.Add(previewItem);
        }
        CurrentItem = 0;
    }

    public void BuyItem()
    {
        var bought = PlayerData.Items;
        if(bought.Contains(itemsKeys[currentItem]))
        {
            return;
        }
        else
        {
            PlayerData.SaveItem(itemsKeys[currentItem]);
        }
        ChooseLockState();
    }

    public void SelectNext()
    {
        CurrentItem = Mathf.Min(CurrentItem + 1, itemsKeys.Count - 1);
    }

    public void SelectPrevious()
    {
        CurrentItem = Mathf.Max(CurrentItem - 1, 0);
    }

    private void GetDataFromConfig()
    {
        allItems = itemConfig.GetAllItems();
    }

    private void ChooseLockState()
    {
        if(PlayerData.Items.Contains(itemsKeys[currentItem]))
        {
            LockImage.SetActive(false);
        }
        else
        {
            LockImage.SetActive(true);
        }
    }

    private void NeedsToUpdate()
    {
        if(PlayerPrefs.HasKey("LastShopUpdate"))
        {
            var now = DateTime.UtcNow;
            var last = DateTime.Parse(PlayerPrefs.GetString("LastShopUpdate"));
            if(now.Subtract(last).Days >= 1)
            {
                UpdateShop();
            }
            else
            {
                LoadShop();
            }
        }
        else
        {
            UpdateShop();
        }
    }

    private void SaveTime()
    {
        PlayerPrefs.SetString("LastShopUpdate", System.DateTime.UtcNow.ToString());
    }

    private void ClearPreview()
    {
        if (previewItems != null)
            for (int i = 0; i < previewItems.Count; i++)
            {
                Destroy(previewItems[i]);
            }
    }
}
