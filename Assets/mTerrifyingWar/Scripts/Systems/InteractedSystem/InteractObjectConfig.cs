using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractObject", menuName = "InteractObject")]
public class InteractObjectConfig : ScriptableObject
{
    [field: SerializeField, BoxGroup("Global interact config"), HorizontalLine] public string InteractName { get; private set;}
}