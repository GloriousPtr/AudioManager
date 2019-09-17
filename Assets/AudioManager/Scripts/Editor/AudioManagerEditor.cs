using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor 
{
    private AudioManager Instance;
    private AudioManager.Sound soundInstance;

	public Sprite MusicOn;
	public Sprite MusicOff;
	public Transform MusicButton;
	public Transform mVolSlider;
	public Sprite SfxOn;
	public Sprite SfxOff;
	public Transform SfxButton;
	public Transform sVolSlider;

    public override void OnInspectorGUI()
    {
        Instance = (AudioManager) target;
        Undo.RecordObject(Instance, "AudioManager");

        if (Instance.MusicOn)
            MusicOn = Instance.MusicOn;
        MusicOn = (Sprite) EditorGUILayout.ObjectField("Music On", MusicOn, typeof(Sprite), true);
        Instance.MusicOn = MusicOn;

        if (Instance.MusicOff)
            MusicOff = Instance.MusicOff;
        MusicOff = (Sprite) EditorGUILayout.ObjectField("Music Off", MusicOff, typeof(Sprite), true);
        Instance.MusicOff = MusicOff;

        if (Instance.MusicButton)
            MusicButton = Instance.MusicButton;
        MusicButton = (Transform) EditorGUILayout.ObjectField("Music Button", MusicButton, typeof(Transform), true);
        Instance.MusicButton = MusicButton;

        if (Instance.MusicVolSlider)
            mVolSlider = Instance.MusicVolSlider;
        mVolSlider =
            (Transform) EditorGUILayout.ObjectField("Music Volume Slider", mVolSlider, typeof(Transform), true);
        Instance.MusicVolSlider = mVolSlider;



        if (Instance.SfxOn)
            SfxOn = Instance.SfxOn;
        SfxOn = (Sprite) EditorGUILayout.ObjectField("Sfx On", SfxOn, typeof(Sprite), true);
        Instance.SfxOn = SfxOn;

        if (Instance.SfxOff)
            SfxOff = Instance.SfxOff;
        SfxOff = (Sprite) EditorGUILayout.ObjectField("Sfx Off", SfxOff, typeof(Sprite), true);
        Instance.SfxOff = SfxOff;

        if (Instance.SfxButton)
            SfxButton = Instance.SfxButton;
        SfxButton = (Transform) EditorGUILayout.ObjectField("Sfx Button", SfxButton, typeof(Transform), true);
        Instance.SfxButton = SfxButton;

        if (Instance.SfxVolSlider)
            sVolSlider = Instance.SfxVolSlider;
        sVolSlider = (Transform) EditorGUILayout.ObjectField("Sfx Volume Slider", sVolSlider, typeof(Transform), true);
        Instance.SfxVolSlider = sVolSlider;

        if (Instance.tracks == null)
            Instance.tracks = new List<AudioManager.MusicTrack>();

        if (Instance.sounds == null)
            Instance.sounds = new List<AudioManager.Sound>();

        #region Music Tracks

        GUILayout.Label("Music Tracks");

        EditorGUILayout.BeginVertical(EditorStyles.textArea);

        EditorGUILayout.BeginHorizontal();

        GUILayout.Space(20);
        GUILayout.Label("Name", EditorStyles.centeredGreyMiniLabel, GUILayout.Width(100));
        GUILayout.Label("Audio Clip", EditorStyles.centeredGreyMiniLabel, GUILayout.ExpandWidth(true));

        EditorGUILayout.EndHorizontal();

        foreach (AudioManager.MusicTrack track in Instance.tracks)
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                Instance.tracks.Remove(track);
                break;
            }

            track.name = EditorGUILayout.TextField(track.name, GUILayout.Width(100));
            track.track =
                (AudioClip) EditorGUILayout.ObjectField(track.track, typeof(AudioClip), false,
                    GUILayout.ExpandWidth(true));

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add", GUILayout.Width(60)))
            Instance.tracks.Add(new AudioManager.MusicTrack());
        if (GUILayout.Button("Sort", GUILayout.Width(60)))
        {
            Instance.tracks.Sort((a, b) => string.Compare(a.name, b.name));
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        #endregion

        #region Sounds

        GUILayout.Label("Sounds");


        EditorGUILayout.BeginVertical(EditorStyles.textArea);

        EditorGUILayout.BeginHorizontal();

        GUILayout.Space(20);
        GUILayout.Label("Edit", EditorStyles.centeredGreyMiniLabel, GUILayout.Width(40));
        GUILayout.Label("Name", EditorStyles.centeredGreyMiniLabel, GUILayout.Width(120));
        GUILayout.Label("Audio Clips", EditorStyles.centeredGreyMiniLabel, GUILayout.ExpandWidth(true));

        EditorGUILayout.EndHorizontal();

        foreach (AudioManager.Sound sound in Instance.sounds)
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                Instance.sounds.Remove(sound);
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
            Instance.sounds.Add(new AudioManager.Sound());
            soundInstance = Instance.sounds[Instance.sounds.Count - 1];
        }

        if (GUILayout.Button("Sort", GUILayout.Width(60)))
        {
            Instance.sounds.Sort((a, b) => string.Compare(a.name, b.name));
            foreach (AudioManager.Sound sound in Instance.sounds)
                sound.clips.Sort((a, b) => string.Compare(a.ToString(), b.ToString()));
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        #endregion

        EditorUtility.SetDirty(target);
    }
}
