using UnityEngine;

[CreateAssetMenu(fileName = "DialogueActor", menuName = "ScriptableObject/DialogueSystem/DialogueActor", order = 1)]
public sealed class DialogueActor : ScriptableObject
{
    [SerializeField] private string actorName;
    public string ActorName => actorName;

    [SerializeField] private Sprite actorSprite;
    public Sprite ActorSprite => actorSprite;
}