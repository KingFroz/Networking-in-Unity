using UnityEngine;
using System.Collections.Generic;

public class SoundLibrary : MonoBehaviour {

    public SoundGroup[] soundGroups;

    Dictionary<string, AudioClip[]> groupDitionary = new Dictionary<string, AudioClip[]>();

    void Awake()
    {
        foreach(SoundGroup sound in soundGroups) {
            groupDitionary.Add(sound.groupID, sound.group);
        }
    }
    public AudioClip GetClipFromName(string name) {
        if (groupDitionary.ContainsKey(name)) {
            AudioClip[] getSounds = groupDitionary[name];
            return getSounds[Random.Range(0, getSounds.Length)];
        }

        return null;
    }

    [System.Serializable]
    public class SoundGroup {
        public string groupID;
        public AudioClip[] group;
    }
}
