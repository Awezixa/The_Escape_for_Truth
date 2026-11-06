using UnityEngine;

public enum EState { Start, Working, End }

public class DocumentManager : MonoBehaviour
{
    public static DocumentManager instance = null;

    public EState state { get; private set; } = EState.Start;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public void SetState(EState s) => state = s;
}