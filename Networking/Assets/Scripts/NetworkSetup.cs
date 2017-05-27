using UnityEngine;
using UnityEngine.Networking;

public class NetworkSetup : NetworkBehaviour {
    [SerializeField]
    Behaviour[] componentsToDisable = null;

	// Use this for initialization
	void Start () {
        if (!isLocalPlayer)
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }
        }
	}
}
