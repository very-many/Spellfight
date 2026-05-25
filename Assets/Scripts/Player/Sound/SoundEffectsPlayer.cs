using UnityEngine;

public class SoundEffectsPlayer : MonoBehaviour
{
    public AudioSource src;
    public AudioClip jump, wallslide, land, spell;
    
    public void PlayJump()
    {
        src.clip = jump;
        src.Play();
    }
    public void PlayWallslide()
    {
        src.clip = wallslide;
        src.Play();
    }
    public void PlayLand()
    {
        src.clip = land;
        src.Play();
    }
    public void PlaySpell()
    {
        src.clip = spell;
        src.Play();
    }
   
}
