using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class MenuSelection : MonoBehaviour
{
    [SerializeField] GameObject currentSelected;
    [SerializeField] GameObject[] chooseFirstButtons;
    [SerializeField] GameObject[] Buttons;
    [SerializeField] Transform[] tpPoints;
    [SerializeField] private GameObject menus = null;
    [SerializeField] string[] descriptions;
    [SerializeField] TextMeshProUGUI descriptionObject;
    [SerializeField] GameObject[] blueColors;
    [SerializeField] GameObject[] redColors;
    [SerializeField] GameObject[] validateIconsColor;
    [SerializeField] GameObject followIconColor;

    private int currentPoint = 0;

    public static MenuSelection Instance;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        menus.transform.DOMoveX(tpPoints[0].position.x, 1f);
    }

    public void ChangeBlueColor(int index, Color color)
    {
        blueColors[index].GetComponent<Image>().color = color;
    }
    public void ChangeRedColor(int index, Color color)
    {
        redColors[index].GetComponent<Image>().color = color;
    }

    private void Update()
    {
        currentSelected = EventSystem.current.currentSelectedGameObject;

        if (currentPoint == 3)
            followIconColor.transform.position = currentSelected.transform.position;
        else
            followIconColor.transform.position = new Vector3(3000, 3000, 0);

        if (currentSelected != null)
            ChangeText();
    }

    public void OnStart()
    {
        //SceneManager.LoadScene(1);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(chooseFirstButtons[0]);
    }

    public void SelectNbPlayers()
    {
        EventSystem.current.SetSelectedGameObject(chooseFirstButtons[1]);
    }

    public void ChooseNbPlayers()
    {
        EventSystem.current.SetSelectedGameObject(chooseFirstButtons[2]);
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void MoveToLeftMenus()
    {
        currentPoint++;
        menus.transform.DOMoveX(tpPoints[currentPoint].position.x, 1f);
    }

    public void MoveToRightMenus()
    {
        currentPoint--;
        menus.transform.DOMoveX(tpPoints[currentPoint].position.x, 1f);
    }

    public void ValidateBlueSide()
    {
        validateIconsColor[0].transform.position = followIconColor.transform.position;
    }

    public void ValidateRedSide()
    {
        validateIconsColor[1].transform.position = followIconColor.transform.position;
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
