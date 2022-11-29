using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject[] switches;
    public Material greenMaterial;
    public GameObject cellDoor;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject levelProgressionTxt;

    public bool isGameOver;

    void Start()
    {
        Time.timeScale = 1;
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

    public void ActivatelevelProgressionTxt()
    {
        levelProgressionTxt.SetActive(true);
        if (!AllEnginesAreShutdown())
        {
            levelProgressionTxt.GetComponent<TMP_Text>().text = $"I Think there are {ActiveEngines()} more...";

        }
        else
        {
            levelProgressionTxt.GetComponent<TMP_Text>().text = "Ok I think it's all, I can escape!"; ;
        }
        StartCoroutine(DeactivatelevelProgressionTxt());
    }
    IEnumerator EnableEnemyVisibility(GameObject creature)
    {
        yield return new WaitForSeconds(3);
        if (creature.GetComponent<EnemyVisibility>())
            creature.GetComponent<EnemyVisibility>().enabled = true;
    } 
    IEnumerator DeactivatelevelProgressionTxt()
    {
        yield return new WaitForSeconds(3);
        levelProgressionTxt.SetActive(false);
    }

    public void Restart()
    {
        isGameOver = false;
        SceneManager.LoadScene("testLevel");
    }    
    
    public void Exit()
    {
        Application.Quit();
    }

    public bool AllEnginesAreShutdown()
    {
        foreach (var en in FindObjectsOfType<Engine>())
        {
            if (en.active) return false;
        };

        return true;
    }

    int ActiveEngines()
    {
        return FindObjectsOfType<Engine>().Where(e => e.active).Count();   
    }
}
