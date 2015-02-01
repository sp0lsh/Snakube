using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public struct AudioPlayerItem
{
    public string name;
    public AudioClip clip;
}


[SingletonPrefab( "AudioPlayer" )]
public class AudioPlayer : Singleton<AudioPlayer>
{

    [SerializeField]
    public List<AudioPlayerItem> namedClips;
    public List<AudioClip> sourceNamedClips;


    Dictionary<string, AudioClip> _clips;
    GameObject _mainCamera;


    void Awake() {

        if ( namedClips == null ) {
            namedClips = new List<AudioPlayerItem>();
        }

        if ( sourceNamedClips == null ) {
            sourceNamedClips = new List<AudioClip>();
        }

        _clips = new Dictionary<string, AudioClip>();
        namedClips.ForEach( i => {
            if ( string.IsNullOrEmpty( i.name ) ) {
                Debug.LogError( "AudioPlayer: Empty name for " + i.name );
            } else {
                _clips.Add( i.name, i.clip );
            }
        } );

        sourceNamedClips.ForEach( c => {
            _clips.Add( c.name, c );
        } );

        _mainCamera = GameObject.FindGameObjectWithTag( "MainCamera" );
    }


    public AudioSource PlayAtMainCamera( string name,
        bool autoDestroy = true, float volume = 1f, float pitch = 1f ) {

            return PlayAtMainCamera( _clips[name], autoDestroy, volume, pitch );
    }

    /// <summary>
    /// Plays a sound by creating an empty game object with an AudioSource
    /// and attaching it to the Main Camera transform. Destroys it after it finished playing.
    /// </summary>
    public AudioSource PlayAtMainCamera( AudioClip clip,
        bool autoDestroy = true, float volume = 1f, float pitch = 1f ) {

        if ( !_mainCamera ) {
            _mainCamera = GameObject.FindGameObjectWithTag( "MainCamera" );
        }

        return Play( clip, _mainCamera.transform, autoDestroy, volume, pitch );
    }

    public AudioSource Play( string name, Transform emitter,
        bool autoDestroy = true, float volume = 1f, float pitch = 1f ) {

            return Play( _clips[name], transform, autoDestroy, volume, pitch );
    }

    /// <summary>
    /// Plays a sound by creating an empty game object with an AudioSource
    /// and attaching it to the given transform (so it moves with the transform). Destroys it after it finished playing.
    /// </summary>
    public AudioSource Play( AudioClip clip, Transform emitter,
        bool autoDestroy = true, float volume = 1f, float pitch = 1f ) {

        //Create an empty game object
        GameObject go = new GameObject( "Audio: " + clip.name );
        go.transform.position = emitter.position;
        go.transform.parent = emitter;

        //Create the source
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();

        if ( autoDestroy ) {
            Destroy( go, clip.length );
        }

        return source;
    }

    public AudioSource Play( string name, Vector3 point,
        bool autoDestroy = true, float volume = 1f, float pitch = 1f ) {

            return Play( _clips[name], point, autoDestroy, volume, pitch );
    }

    /// <summary>
    /// Plays a sound at the given point in space by creating an empty game object with an AudioSource
    /// in that place and destroys it after it finished playing.
    /// </summary>
    public AudioSource Play( AudioClip clip, Vector3 point,
        bool autoDestroy = true, float volume = 1f, float pitch = 1f ) {

        //Create an empty game object
        GameObject go = new GameObject( "Audio: " + clip.name );
        go.transform.position = point;

        //Create the source
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();

        if ( autoDestroy ) {
            Destroy( go, clip.length );
        }

        return source;
    }
}
