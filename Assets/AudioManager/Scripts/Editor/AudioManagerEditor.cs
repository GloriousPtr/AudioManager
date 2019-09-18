using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor 
{
    private AudioManager instance;
    private AudioManager.Sound soundInstance;

    public override void OnInspectorGUI()
    {
        instance = (AudioManager) target;
        Undo.RecordObject(instance, "AudioManager");
        
        DrawInspector();

        if (instance.tracks == null)
            instance.tracks = new List<AudioManager.MusicTrack>();

        if (instance.sounds == null)
            instance.sounds = new List<AudioManager.Sound>();

        DrawMusic();

        DrawSounds();

        EditorUtility.SetDirty(target);
    }

    private void DrawInspector()
    {
        instance.musicOn = (Sprite) EditorGUILayout.ObjectField("Music On", instance.musicOn, typeof(Sprite), true);
        instance.musicOff = (Sprite) EditorGUILayout.ObjectField("Music Off", instance.musicOff, typeof(Sprite), true);
        instance.musicButton = (Image) EditorGUILayout.ObjectField("Music Button", instance.musicButton, typeof(Image), true);
        instance.musicVolSlider = (Slider) EditorGUILayout.ObjectField("Music Volume Slider", instance.musicVolSlider, typeof(Slider), true);

        instance.sfxOn = (Sprite) EditorGUILayout.ObjectField("Sfx On", instance.sfxOn, typeof(Sprite), true);
        instance.sfxOff = (Sprite) EditorGUILayout.ObjectField("Sfx Off", instance.sfxOff, typeof(Sprite), true);
        instance.sfxButton = (Image) EditorGUILayout.ObjectField("Sfx Button", instance.sfxButton, typeof(Image), true);
        instance.sfxVolSlider =
            (Slider) EditorGUILayout.ObjectField("Sfx Volume Slider", instance.sfxVolSlider, typeof(Slider), true);
    }

    private void DrawMusic()
    {
        GUILayout.Label("Music Tracks");

        EditorGUILayout.BeginVertical(EditorStyles.textArea);

        EditorGUILayout.BeginHorizontal();

        GUILayout.Space(20);
        GUILayout.Label("Name", EditorStyles.centeredGreyMiniLabel, GUILayout.Width(100));
        GUILayout.Label("Audio Clip", EditorStyles.centeredGreyMiniLabel, GUILayout.ExpandWidth(true));

        EditorGUILayout.EndHorizontal();

        foreach (AudioManager.MusicTrack track in instance.tracks)
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                instance.tracks.Remove(track);
                break;
            }

            track.name = EditorGUILayout.TextField(track.name, GUILayout.Width(100));
            track.track = (AudioClip) EditorGUILayout.ObjectField(track.track, typeof(AudioClip), false,
                GUILayout.ExpandWidth(true));

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add", GUILayout.Width(60)))
            instance.tracks.Add(new AudioManager.MusicTrack());
        if (GUILayout.Button("Sort", GUILayout.Width(60)))
        {
            instance.tracks.Sort((a, b) => string.Compare(a.name, b.name));
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }

    private void DrawSounds()
    {
        GUILayout.Label("Sounds");


        EditorGUILayout.BeginVertical(EditorStyles.textArea);

        EditorGUILayout.BeginHorizontal();

        GUILayout.Space(20);
        GUILayout.Label("Edit", EditorStyles.centeredGreyMiniLabel, GUILayout.Width(40));
        GUILayout.Label("Name", EditorStyles.centeredGreyMiniLabel, GUILayout.Width(120));
        GUILayout.Label("Audio Clips", EditorStyles.centeredGreyMiniLabel, GUILayout.ExpandWidth(true));

        EditorGUILayout.EndHorizontal();

        foreach (AudioManager.Sound sound in instance.sounds)
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                instance.sounds.Remove(sound);
                break;
            }

            if (GUILayout.Button("Edit", GUILayout.Width(40)))
            {
                soundInstance = soundInstance == sound ? null : sound;
            }

            sound.name = EditorGUILayout.TextField(sound.name, GUILayout.Width(120));

            if (soundInstance == sound || sound.clips.Count == 0)
            {
                EditorGUILayout.BeginVertical();
                for (int i = 0; i < sound.clips.Count; i++)
                {
                    sound.clips[i] = (AudioClip) EditorGUILayout.ObjectField(sound.clips[i], typeof(AudioClip), false,
                        GUILayout.ExpandWidth(true));
                    if (sound.clips[i] != null) continue;
                    sound.clips.RemoveAt(i);
                    break;
                }

                AudioClip new_clip =
                    (AudioClip) EditorGUILayout.ObjectField(null, typeof(AudioClip), false, GUILayout.Width(150));
                if (new_clip)
                    sound.clips.Add(new_clip);
                EditorGUILayout.EndVertical();
            }
            else
            {
                GUILayout.Label(sound.clips.Count.ToString() + " audio clip(s)", EditorStyles.miniBoldLabel);
            }


            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add", GUILayout.Width(60)))
        {
            instance.sounds.Add(new AudioManager.Sound());
            soundInstance = instance.sounds[instance.sounds.Count - 1];
        }

        if (GUILayout.Button("Sort", GUILayout.Width(60)))
        {
            instance.sounds.Sort((a, b) => string.Compare(a.name, b.name));
            foreach (AudioManager.Sound sound in instance.sounds)
                sound.clips.Sort((a, b) => string.Compare(a.ToString(), b.ToString()));
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }
}
