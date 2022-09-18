using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using UnityEngine.InputSystem;

public class MenuSelection : MonoBehaviour
{
    private GameObject currentSelected;
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

    [SerializeField] Transform[] tpPointsMap;
    [SerializeField] GameObject maps;
    [SerializeField] Transform[] tpPointsMapSoccer;
    [SerializeField] GameObject mapsSoccer;

    [SerializeField] TextMeshProUGUI indexOfMaps;
    [SerializeField] TextMeshProUGUI textOfMaps;
    [SerializeField] float transiTimeMap;


    private int currentPoint = 0;
    private int currentMapIndex = 0;
    private int currentButtonsIndex = 0;
    private bool[] canResetTp = new bool[2];
    private bool cannotTp = false;
    private bool hasChooseSoccer = false;

    private bool IsBack;
    private bool canBack;
    private float nextAttack;
    [SerializeField] float attackRate;

    public static MenuSelection Instance;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        menus.transform.DOMoveX(tpPoints[0].position.x, 1f);
        currentMapIndex = 1;
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

        if (!canBack)
        {
            nextAttack -= Time.deltaTime;
            if (nextAttack <= 0)
            {
                canBack = true;
                nextAttack = attackRate;
            }
        }
        if (IsBack && canBack && currentButtonsIndex > 0)
        {
            GoBackMenus();
            canBack = false;

            if (currentButtonsIndex < 4)
            {
                indexOfMaps.enabled = false;
                textOfMaps.enabled = false;
            }
        }
    }

    public void OnStart()
    {
        EventSystem.current.SetSelectedGameObject(null);
        NextMenus(0);
    }

    public void NextMenus(int index)
    {
        currentButtonsIndex = index;
        EventSystem.current.SetSelectedGameObject(chooseFirstButtons[index]);

        if (index == 3)
            ChangeIndexOfMap();
    }

    public void GoBackMenus()
    {
        canBack = false;
        MoveToRightMenus();
        currentButtonsIndex--;
        NextMenus(currentButtonsIndex);

        print("alllzgjz^ghiz");
    }

    public void OnBack(InputAction.CallbackContext context)
    {
        IsBack = context.action.triggered;
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
        currentPoint -= 1;
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

    public void HasChooseSoccerMode(bool which)
    {
        hasChooseSoccer = which;
        if (which)
        {
            maps.SetActive(false);
            mapsSoccer.SetActive(true);
        }
        else
        {
            maps.SetActive(true);
            mapsSoccer.SetActive(false);
        }
        currentMapIndex = 1;
    }

    public void MoveMapTop()
    {
        if (cannotTp)
            return;

        currentMapIndex++;
        cannotTp = true;

        if (!hasChooseSoccer)
        {
            if (currentMapIndex >= tpPointsMap.Length - 1)
                canResetTp[0] = true;

            maps.transform.DOMoveY(tpPointsMap[currentMapIndex].position.y, transiTimeMap);
        }
        else
        {
            if (currentMapIndex >= tpPointsMapSoccer.Length - 1)
                canResetTp[0] = true;

            mapsSoccer.transform.DOMoveY(tpPointsMapSoccer[currentMapIndex].position.y, transiTimeMap);
        }
        LaunchCanTpTop();
    }

    void LaunchCanTpTop()
    {
        StartCoroutine(CanTpTop());
    }

    IEnumerator CanTpTop()
    {
        yield return new WaitForSeconds(transiTimeMap / 2);
        if (canResetTp[0])
        {
            currentMapIndex = 1;
            ChangeIndexOfMap();
            yield return new WaitForSeconds(transiTimeMap / 2);

            if (!hasChooseSoccer)
               maps.transform.DOMoveY(tpPointsMap[0].position.y, 0f);
            else
               mapsSoccer.transform.DOMoveY(tpPointsMapSoccer[0].position.y, 0f);
            //print("tpTop");
        }
        else
        {
            ChangeIndexOfMap();
            yield return new WaitForSeconds(transiTimeMap / 2);
        }

        canResetTp[0] = false;
        cannotTp = false;
    }


    void ChangeIndexOfMap()
    {
        indexOfMaps.enabled = true;
        textOfMaps.enabled = true;
        indexOfMaps.text = $"{currentMapIndex}";
        GameParameters.instance.ChooseMap(currentMapIndex);
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
