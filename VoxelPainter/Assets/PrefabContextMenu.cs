using UnityEngine;

public class PrefabContextMenu : MonoBehaviour
{
    public string Id { get; private set; }

    public void SetContext(string id)
    {
        Id = id;
    }

    public void RemoveContext()
    {
        Id = null;
    }

    public void Apply()
    {
        SymbolManager.Instance.Save(Id);

        // Apply changes to all prefabs?
    }

    public void Delete()
    {
        SymbolManager.Instance.Delete(Id);
    }
}
