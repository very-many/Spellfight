using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCosmeticController : NetworkBehaviour
{
    public SpriteRenderer BodyRenderer;
    public SpriteRenderer CapeRenderer;
    public PlayerCosmeticSet CosmeticSet;
    private PlayerCosmetic[] PlayerCosmetics => CosmeticSet != null ? CosmeticSet.Cosmetics : null;

    [Space]
    [Header("Child Player Object")]
    public GameObject PlayerObject;

    private void Start()
    {
    }

    public void PlayerCosmeticsSetup()
    {
        int idx = GetComponent<PlayerObjectController>().PlayerCosmetic;
        if (PlayerCosmetics != null && PlayerCosmetics.Length > 0 && idx >= 0 && idx < PlayerCosmetics.Length)
        {
            BodyRenderer.color = PlayerCosmetics[idx].BodyColor;
            CapeRenderer.color = PlayerCosmetics[idx].CapeColor;
        }
        else
        {
            BodyRenderer.color = Color.white;
            CapeRenderer.color = Color.white;
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (PlayerObject.activeSelf == false)
            {
                PlayerCosmeticsSetup();
            }
        }
    }
}
