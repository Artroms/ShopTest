using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemConfig", menuName = "ScriptableObjects/ItemConfig", order = 1)]
public class ItemConfig : ScriptableObject
{
    [SerializeField]
    private List<Item> items = new List<Item>();
    private Dictionary<string, GameObject> dictOfItems = new Dictionary<string, GameObject>();

    public GameObject go;

    private void OnValidate()
    {
        HashSet<string> keyValues = new HashSet<string>();
        List<Item> checkedItems = new List<Item>();
        for (int i = 0; i < items.Count; i++)
        {
            if(!keyValues.Contains(items[i].key))
            {
                keyValues.Add(items[i].key);
                checkedItems.Add(items[i]);
            }
        }
        items = checkedItems;
    }

    public List<string> GetRandomItems(int count)
    {

        count = Mathf.Min(count, items.Count);
        List<Item> randomFrom = new List<Item>(items);
        List<string> randomItems = new List<string>();
        for (int i = 0; i < count; i++)
        {
            var rndIndex = Random.Range(0, randomFrom.Count);
            var rndItem = randomFrom[rndIndex];
            randomItems.Add(rndItem.key);
            randomFrom.RemoveAt(rndIndex);
        }
        return randomItems;
    }

    public Dictionary<string, GameObject> GetAllItems()
    {
        if (dictOfItems.Count == 0)
            for (int i = 0; i < items.Count; i++)
            {
                dictOfItems.Add(items[i].key, items[i].value);
            }
        return new Dictionary<string, GameObject>(dictOfItems);
    }

    [System.Serializable]
    public struct Item
    {
        public string key;
        public GameObject value;

        public Item(string key, GameObject value)
        {
            this.key = key;
            this.value = value;
        }
    }

#if UNITY_EDITOR
    [ContextMenu("CreateRandomCubes")]
    public void CreateRandomCubes()
    {
        var gos = new List<GameObject>();
        for (int i = 0; i < 50; i++)
        {
            var newGo = Instantiate(go);
            var renderer = newGo.GetComponent<MeshRenderer>();
            var mat = renderer.material;
            mat.color = new Color(Random.value, Random.value, Random.value);
            UnityEditor.AssetDatabase.CreateAsset(mat, @"Assets\Generated\" + mat.GetHashCode() + ".mat");
            UnityEditor.AssetDatabase.SaveAssets();
            var saved = UnityEditor.PrefabUtility.SaveAsPrefabAsset(newGo, @"Assets\Generated\" + newGo.GetHashCode() + ".prefab");
            DestroyImmediate(newGo);
            gos.Add(saved);
        }
        for (int i = 0; i < gos.Count; i++)
        {
            items.Add(new Item(gos[i].GetHashCode().ToString(), gos[i]));
        }
    }
#endif
}
