using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject[] switches;
    public Material greenMaterial;
    public GameObject cellDoor;
    [SerializeField] GameObject gameOverPanel;

    public bool isGameOver;

    void Start()
    {
        
    }

    public void GameOver()
    {

        if (cellDoor && cellDoor.GetComponent<Door>().unlocked)
        {
            isGameOver = true;
            Time.timeScale = 0;
            gameOverPanel?.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void SetMaterial(int index)
    {
        switches[index].GetComponent<Renderer>().material = greenMaterial;
    }

    void Update()
    {
        
    }

    public void EnableEnemyVisibilityAfter3Seconds(GameObject creature)
    {
        StartCoroutine(EnableEnemyVisibility(creature));
    }
    IEnumerator EnableEnemyVisibility(GameObject creature)
    {
        yield return new WaitForSeconds(3);
        if (creature.GetComponent<EnemyVisibility>())
            creature.GetComponent<EnemyVisibility>().enabled = true;
    }

    public void Restart()
    {
        isGameOver = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("testLevel");
    }    
    
    public void Exit()
    {
        Application.Quit();
    }
}
