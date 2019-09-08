using UnityEngine;

public class Prefab : MonoBehaviour
{
    public string Id { get; private set; }

    public void Apply()
    {
        SymbolManager.Instance.Save(Id);
    }

    public void Delete()
    {
        SymbolManager.Instance.Delete(Id);
    }

    public void SetId(string id)
    {
        Id = id;
    }
}