using UnityEngine;

[System.Serializable]
public struct PlayerCosmetic
{
    public string Name;
    public Color BodyColor;
    public Color CapeColor;
}

[CreateAssetMenu(fileName = "PlayerCosmeticSet", menuName = "Cosmetics/Player Cosmetic Set")]
public class PlayerCosmeticSet : ScriptableObject
{
    public PlayerCosmetic[] Cosmetics;
}