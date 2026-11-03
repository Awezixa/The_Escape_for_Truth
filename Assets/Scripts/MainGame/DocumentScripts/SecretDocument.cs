// SecretDocument.cs
using UnityEngine;

public class SecretDocument : DocumentCollection
{
    void Reset() { suspicionWhenSeen = 0.40f; } // when add more doc types, just copy and change amount want to add depending on type
}