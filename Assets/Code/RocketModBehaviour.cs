using UnityEngine;

public abstract class RocketModBehaviour : MonoBehaviour
{
    protected ResourceManager resourceManager;

    public virtual void Initialize(ResourceManager resource)
    {
        resourceManager = resource;
        Debug.Log($"{this.name}: Initialized with ResourceManager");
    }

    public abstract void OnModUpdate(); // Called every frame
}
