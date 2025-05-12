using UnityEngine;

public class MusicToggleButton : MonoBehaviour
{
    public void ToggleMusic()
    {
        if (MusicPlayer.instance != null)
            MusicPlayer.instance.ToggleMusic();
    }
}
