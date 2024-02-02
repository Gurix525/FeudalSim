using Assets.Code;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] _menuClips;
    [SerializeField] private AudioClip[] _defaultClips;
    [SerializeField] private AudioClip[] _combatClips;

    private AudioClip _currentClip;
    private AudioSource _audioSource;

    private static BGMManager _current;

    public static void PlaySoundtrack(BGMType type)
    {
        _current.Play(type);
    }

    private void Awake()
    {
        if (_current == null)
            _current = this;
        _audioSource = GetComponent<AudioSource>();
        PlaySoundtrack(BGMType.Menu);
        if (_current != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Play(BGMType type)
    {
        AudioClip[] clips = GetClips(type);
        Dice dice = new(clips.Length);
        AudioClip selectedClip = clips[dice.Roll() - 1];
        if (_currentClip != selectedClip)
        {
            ChangeClip(selectedClip);
        }
    }

    private void ChangeClip(AudioClip nextClip)
    {
        _currentClip = nextClip;
        _audioSource.clip = _currentClip;
        _audioSource.Play();
    }

    private AudioClip[] GetClips(BGMType type)
    {
        return type switch
        {
            BGMType.Default => _defaultClips,
            BGMType.Menu => _menuClips,
            BGMType.Combat => _combatClips,
            _ => _defaultClips
        };
    }

    public enum BGMType
    {
        Menu,
        Default,
        Combat
    }
}