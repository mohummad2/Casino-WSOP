using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class escapebutton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // аналогично для float или string, но пишите SetFloat или SetString
                PlayerPrefs.SetInt("Тут ключ, на который сохраняются значения", 42); // 42 - тут любое int, какое хотите сохранить
                // все, что нужно сохранить
                SceneManager.LoadScene(0); // загружаете нужную сцену
            }
        }
    }
}
