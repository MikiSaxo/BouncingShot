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
    [SerializeField] Transform[] tpPointsMap;
    [SerializeField] GameObject maps;
    [SerializeField] TextMeshProUGUI indexOfMaps;
    [SerializeField] float transiTimeMap;

    private int currentPoint = 0;
    private int currentMapIndex = 0;
    private bool[] canResetTp = new bool[2];
    private bool cannotTp = false;

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
    }

    public void OnStart()
    {
        EventSystem.current.SetSelectedGameObject(null);
        NextMenus(0);
    }

    public void NextMenus(int index)
    {
        EventSystem.current.SetSelectedGameObject(chooseFirstButtons[index]);

        if (index == 3)
            ChangeIndexOfMap();
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

    public void MoveMapTop()
    {
        if (cannotTp)
            return;

        currentMapIndex++;
        cannotTp = true;

        if (currentMapIndex >= tpPointsMap.Length - 1)
            canResetTp[0] = true;

        maps.transform.DOMoveY(tpPointsMap[currentMapIndex].position.y, transiTimeMap);
        LaunchCanTpTop();
    }

    void LaunchCanTpTop()
    {
        StartCoroutine(CanTpTop());
    }

    IEnumerator CanTpTop()
    {
        yield return new WaitForSeconds(transiTimeMap/2);
        if (canResetTp[0])
        {
            currentMapIndex = 1;
            ChangeIndexOfMap();
            yield return new WaitForSeconds(transiTimeMap/2);

            maps.transform.DOMoveY(tpPointsMap[0].position.y, 0f);
            print("tpTop");
        }
        else
        {
            ChangeIndexOfMap();
            yield return new WaitForSeconds(transiTimeMap/2);
        }

        canResetTp[0] = false;
        cannotTp = false;
    }

    //public void MoveMapDown()
    //{
    //    if (cannotTp)
    //        return;

    //    currentMapIndex--;
    //    cannotTp = true;

    //    if (currentMapIndex <= 0)
    //        canResetTp[1] = true;

    //    maps.transform.DOMoveY(tpPointsMap[currentMapIndex].position.y, 1f);
    //    LaunchCanTpDown();
    //}

    //void LaunchCanTpDown()
    //{
    //    StartCoroutine(CanTpDown());
    //}

    //IEnumerator CanTpDown()
    //{
    //    yield return new WaitForSeconds(.5f);
    //    if (canResetTp[1])
    //    {
    //        currentMapIndex = tpPointsMap.Length - 2;
    //        ChangeIndexOfMap();
    //        yield return new WaitForSeconds(.5f);

    //        maps.transform.DOMoveY(tpPointsMap[tpPointsMap.Length - 1].position.y, 0f);
    //        print("TpDown");
    //    }
    //    else
    //    {
    //        ChangeIndexOfMap();
    //        yield return new WaitForSeconds(.5f);
    //    }

    //    canResetTp[1] = false;
    //    cannotTp = false;
    //}

    void ChangeIndexOfMap()
    {
        indexOfMaps.enabled = true;
        indexOfMaps.text = $"{currentMapIndex}";
        GameParameters.instance.ChooseMap(currentMapIndex);
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(1);
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
