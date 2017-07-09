
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour {
    
    public Image fade;

    public GameObject gameOverUI;

    public static GUIManager instance;

    void Awake() {
        instance = this;
    }

    void Start() {
        FindObjectOfType<Player>().OnDeath += OnGameOver;
    }

    void OnGameOver() {
        StartCoroutine(Fade(Color.clear, Color.black, 1));
        gameOverUI.SetActive(true);
    }

    public void Restart() {
        SceneManager.LoadScene("TileMapGeneration");
    }

    IEnumerator Fade(Color from, Color to, float time)
    {
        float percent = 0;
        float speed = 1 / time;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            fade.color = Color.Lerp(from, to, percent);
            yield return null;
        }
    }
}
