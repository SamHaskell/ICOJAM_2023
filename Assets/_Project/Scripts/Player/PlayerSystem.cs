using UnityEngine;

public abstract class PlayerSystem : MonoBehaviour
{
    protected Player Player { get; private set; }
    protected virtual void Awake()
    {
        Player = transform.root.GetComponent<Player>();
        if (Player == null) {
            Debug.Log("Player is Null!");
        }
    }
}
