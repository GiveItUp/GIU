using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public enum eSoundClip
    {
        GUI_Tap,
		
		Game_End_Splat_01,
		
		GUI_Button_01,
		GUI_Button_play,
		GUI_Button_back,
		GUI_Blink_01,
		GUI_Button_popup,
		GUI_PopupShowComponent,
		GUI_PopupHideComponent,
		
		Game_Particle_2_01,
		Game_Particle_2_02,
		Game_Particle_2_03,
		Game_Particle_2_04,

		End_Success,
		End_Success_1,

		Music_Menu,
		Music_Ingame,
		Music_Results,
    };
	
	public static SoundManager Instance = null;
	
	public float Volume
	{
		set
		{
			PlayerPrefs.SetFloat("Volume", value);
		}
		get
		{
			return PlayerPrefs.GetFloat("Volume", 1f);
		}
	}

    Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();
	public List<AudioClip> l_sounds = new List<AudioClip>();
    private List<AudioSource> sources = new List<AudioSource>();

    public AudioClip GetSound(string name) { AudioClip a = null; sounds.TryGetValue(name, out a); return a; }
	
	void Awake()
	{
		Instance = this;

        LoadSounds();
    }

    public void LoadSounds()
    {
        foreach (var sound in l_sounds)
        {
            if (sound != null)
            {
                if (!sounds.ContainsKey(sound.name))
                    sounds.Add(sound.name, sound);
                else
                    Debug.Log("Dictionary - sound alreasy contains : " + sound.name + " key!");
            }
        }
    }

    public AudioSource Play(eSoundClip clip, int loop)
    {
        return Play(clip, loop, 1f);
    }
	
	public AudioSource Play(eSoundClip clip, int loop, bool one)
	{
		if (one)
		{
			List<AudioSource> ns = new List<AudioSource>();
			foreach (var src in sources)
					if (src != null)
						ns.Add(src);
			sources = ns;
			
			foreach (var src in sources)
				if (src.clip == GetSound(clip.ToString()))
					return null;
		}
		return Play (clip, loop);
	}

    public AudioSource Play(eSoundClip clip, int loop, float pitch)
	{
		AudioClip current = null;

        current = GetSound(clip.ToString());

        if (current != null)
        {
            return Play(current, Volume, pitch, loop);
        }
		return null;

	}
	
	public void Stop(eSoundClip clip)
	{
		List<AudioSource> ns = new List<AudioSource>();
		foreach (var src in sources)
				if (src != null)
					ns.Add(src);
		sources = ns;
		
		foreach (var src in sources)
			if (src.clip == GetSound(clip.ToString()))
				src.Stop();
	}

	public AudioSource Play(AudioClip clip, float volume, float pitch, int loop)
	{
		//if(!User.IsSoundEnabled) return null;
		/*
        //Debug.Log("Play - " + clip.name);
		if (clip.name == eSoundClip.Music_Ingame.ToString() ||
		    clip.name == eSoundClip.Music_Menu.ToString())
		{
			if(!User.IsSoundEnabled) return null;
		}
		else
		{
			if(!User.IsVoiceEnabled) return null;
		}*/

        GameObject go = new GameObject ("Audio: " +  clip.name);
        go.transform.position = this.transform.position;
        go.transform.parent = this.transform;
 
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play ();
		if (loop > 1 || loop == 0)
			source.loop = true;
		if (loop > 0)
         Destroy (go, clip.length * loop);
		sources.Add(source);
        return source;
	}
	
	public static void PlayButtonTapSound()
	{
		Instance.Play(SoundManager.eSoundClip.GUI_Button_01, 1);
	}
	
	public static void PlayButtonPlaySound()
	{
		Instance.Play(SoundManager.eSoundClip.GUI_Button_play, 1);
	}
	
	public static void PlayButtonBackSound()
	{
		Instance.Play(SoundManager.eSoundClip.GUI_Button_back, 1);
	}

	public static void PlayRandomParticleSound()
	{
		eSoundClip clip = eSoundClip.Game_Particle_2_01;

		int index = Random.Range (0, 4);
		switch(index)
		{
			case 0: clip = eSoundClip.Game_Particle_2_01; break;
			case 1: clip = eSoundClip.Game_Particle_2_02; break;
			case 2: clip = eSoundClip.Game_Particle_2_03; break;
			case 3: clip = eSoundClip.Game_Particle_2_04; break;
		}
		Instance.Play (clip, 1);
	}

}
