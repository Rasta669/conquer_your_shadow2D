using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer; // Drag & Drop your Unity Audio Mixer

    public UIDocument uiDocument;
    private Slider masterVolumeSlider;
    private Slider musicVolumeSlider;
    private Slider effectsVolumeSlider;

    private void OnEnable()
    {
        uiDocument = uiDocument.GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;
        root = root.Q<VisualElement>("AudioPage");

        // Get the Sliders from Audio Page
        masterVolumeSlider = root.Q<Slider>("MasterVolume");
        musicVolumeSlider = root.Q<Slider>("MusicVolume");
        effectsVolumeSlider = root.Q<Slider>("EffectsrVolume");

        // Add listeners
        masterVolumeSlider.RegisterValueChangedCallback(evt => SetMasterVolume(evt.newValue));
        musicVolumeSlider.RegisterValueChangedCallback(evt => SetMusicVolume(evt.newValue));
        effectsVolumeSlider.RegisterValueChangedCallback(evt => SetEffectsVolume(evt.newValue));
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    public void SetEffectsVolume(float volume)
    {
        audioMixer.SetFloat("Music/Effects", Mathf.Log10(volume) * 20);
    }
}
