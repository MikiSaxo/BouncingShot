using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class MenuSelection : MonoBehaviour
{
    [SerializeField] GameObject chooseFirstButton, currentSelected;
    [SerializeField] GameObject[] Buttons;
    [SerializeField] string[] descriptions;
    [SerializeField] TextMeshProUGUI descriptionObject;

    private void Update()
    {
        currentSelected = EventSystem.current.currentSelectedGameObject;
        if (currentSelected != null)
            ChangeText();
    }

    public void OnStart()
    {
        //SceneManager.LoadScene(1);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(chooseFirstButton);
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    void ChangeText()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            if (currentSelected == Buttons[i])
            {
                descriptionObject.text = descriptions[i];
            }
        }
    }
}
