using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Audio manager.音乐音效的简单管理器
/// </summary>
public class AudioManager : IManager
{
    /// <summary>
    /// 将声音放入字典中，方便管理
    /// </summary>
    private Dictionary<string, AudioClip> _soundDictionary;
    //背景音乐和音效的音频源
    private AudioSource[] audioSources;
    public AudioSource bgAudioSource;
    public AudioSource audioSourceEffect;

    /// <summary>
    /// 设置音效开闭的
    /// </summary>
    /// <param name="boo"></param>
    public void SetSound(bool boo) {
        float value = boo == true ? 1f : 0f;
        audioSourceEffect.volume = value;
    }
    /// <summary>
    /// 设置背景音乐开闭的
    /// </summary>
    /// <param name="boo"></param>
    public void SetBGM(bool boo) {
        float value = boo == true ? 1f : 0f;
        bgAudioSource.volume = value;
    }
    public override void OnInit() {
        Initialize();
    }
    public override void OnUpdate() {
        base.OnUpdate();
    }
    public override void OnRelease() {
        base.OnRelease();
    }


    /// <summary>
    /// Start this instance.
    /// </summary>
    void Initialize() {
        //加载资源存的所有音频资源
        LoadAudio();
        //获取音频源
        audioSources = GameObject.Find("Main Camera").GetComponents<AudioSource>();
        if (audioSources == null) return;
        bgAudioSource = audioSources[0];
        bgAudioSource.playOnAwake = true;
        bgAudioSource.loop = true;
        audioSourceEffect = audioSources[1];
        audioSourceEffect.playOnAwake = false;
        audioSourceEffect.loop = false;
    }
    /// <summary>
    /// Loads the audio.
    /// </summary>
    private void LoadAudio() {
        //初始化字典
        _soundDictionary = new Dictionary<string, AudioClip>();
        //本地加载 
        AudioClip[] audioArray = Resources.LoadAll<AudioClip>("Audio");
        //存放到字典
        foreach (AudioClip item in audioArray) {
            _soundDictionary.Add(item.name, item);
        }
    }
    /// <summary>
    /// Plaies the background audio.播放背景音乐
    /// </summary>
    /// <param name="audioName">Audio name.</param>
    public void PlayBGAudio(MusicType musicType, float volume = 1) {

        if (_soundDictionary.ContainsKey(musicType.ToString())) {
            bgAudioSource.clip = _soundDictionary[musicType.ToString()];
            if (MediaTool.Instance.bgmBool == false) {
                bgAudioSource.volume = 0;
                bgAudioSource.Stop();
            }
            else {
                bgAudioSource.volume = volume;
                bgAudioSource.Play();
            }

        }
        else {
            Debug.LogError("没有这个背景音乐");
        }
    }
    /// <summary>
    /// Plaies the audio effect.播放音效
    /// </summary>
    /// <param name="audioEffectName">Audio effect name.</param>
    public float PlayAudioEffect(MusicType musicType) {
        if (_soundDictionary.ContainsKey(musicType.ToString())) {
            AudioClip clip = _soundDictionary[musicType.ToString()];
            audioSourceEffect.PlayOneShot(clip);
            return clip.length;
        }
        else {
            Debug.LogError("没有这个音效");
        }
        return 0;
    }

    public void BtnClick() {
        PlayAudioEffect(MusicType.BtnClick);
    }
}