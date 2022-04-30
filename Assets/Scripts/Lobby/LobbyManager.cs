using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoSingleton<LobbyManager>
{
    public WorldManager worldManager;

    public Camera lobbyCamera;
    private Vector3 defaultCameraPos;
    private Vector3 defaultCameraRot;

    public Canvas canvas;
    public GameObject UI;
    public GameObject myAvatarUI;
    public GameObject avatarSelectUI;
    public GameObject serviceChoiceUI;
    public GameObject commissionUI;
    public GameObject estimationUI;
    public GameObject searchUI;
    public GameObject worldButtonUI;
    public GameObject buildingButtonUI;
    public List<GameObject> buildingButtonsList;
    public GameObject officeButtonUI;
    public GameObject lawyerProfileUI;
    public GameObject lendingUI;

    public GameObject reservationTabUI;
    public GameObject commissionTabUI;

    public GameObject myOfficeBtn;

    public Button lawWorldBtn;
    public Button detectiveBuildingBtn;
    public List<GameObject> officeLightList;

    public Transform QnABuildingTrans;

    public Text idText;
    public Text typeText;

    public Avatar myAvatar;

    [SerializeField] private float cameraMoveTime;

    private UIParent[] UIParents;
    private bool bVisible = true;


    // Start is called before the first frame update
    void Start()
    {
        defaultCameraPos = lobbyCamera.transform.position;
        defaultCameraRot = lobbyCamera.transform.localEulerAngles;

        idText.text = PlayerPrefs.GetString("username");
        typeText.text = PlayerPrefs.GetString("typename");

        Loading.Instance.Off();

        if (PlayerPrefs.GetInt("type") == 1)
            myOfficeBtn.SetActive(true);

        avatarSelectUI.SetActive(true);

        UIParents = GetComponentsInChildren<UIParent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            bVisible = !bVisible;

            myAvatarUI.SetActive(bVisible);

            foreach (var UIParent in UIParents)
                UIParent.SetVisible(bVisible);
        }
    }

    public void OnAvatarSelectBtnClicked(int idx)
    {
        avatarSelectUI.SetActive(false);

        PlayerPrefs.SetInt("Avatar", idx);

        //myAvatarUI.SetActive(true);
        myAvatar.idx = idx;
        myAvatar.gameObject.SetActive(true);

        serviceChoiceUI.SetActive(true);
    }

    public void OnServiceChoiceClicked(int side)
    {
        serviceChoiceUI.SetActive(false);

        if (side == 0)
        {
            myAvatarUI.SetActive(true);
            worldButtonUI.SetActive(true);

        }
        else
        {
            commissionUI.SetActive(true);
        }

    }

    public void OnCommissionWritingCompleteBtnClicked()
    {
        commissionUI.SetActive(false);

        myAvatarUI.SetActive(bVisible);
        worldButtonUI.SetActive(true);
    }

    public void OnReservationCompleteBtnClicked()
    {
        officeButtonUI.SetActive(false);

        OnBackBtnClicked();

        reservationTabUI.SetActive(true);
    }

    public void OnReservationClicked()
    {
        reservationTabUI.SetActive(!reservationTabUI.activeSelf);
        commissionTabUI.SetActive(false);
    }

    public void OnCommissionClicked()
    {
        reservationTabUI.SetActive(false);
        commissionTabUI.SetActive(!commissionTabUI.activeSelf);
    }

    public void OnSearchBtnClicked()
    {
        searchUI.SetActive(true);
    }

    public void OnSearchCompleteBtnClicked()
    {
        searchUI.SetActive(false);

        StartCoroutine(ShowSearchedLawyersRoutine());
    }

    private IEnumerator ShowSearchedLawyersRoutine()
    {
        if (worldButtonUI.activeSelf)
        {
            lawWorldBtn.onClick.Invoke();

            yield return new WaitForSeconds(cameraMoveTime);
        }
        if (buildingButtonUI.activeSelf)
        {
            detectiveBuildingBtn.onClick.Invoke();

            yield return new WaitForSeconds(cameraMoveTime);
        }

        for (int i = 0; i < 3; i++)
        {
            foreach (var light in officeLightList)
                light.SetActive(true);

            yield return new WaitForSeconds(0.2f);

            foreach (var light in officeLightList)
                light.SetActive(false);

            yield return new WaitForSeconds(0.2f);
        }
    }

    public void OnWorldBtnClicked(World world)
    {
        StartCoroutine(FocusOnWorld(world));
    }

    public void OnLendingBtnClicked(World world)
    {
        StartCoroutine(FocusOnWorld(world));

        StartCoroutine(LendingUIRoutine());
    }
    private IEnumerator LendingUIRoutine()
    {
        yield return new WaitForSeconds(cameraMoveTime);

        lendingUI.SetActive(true);
    }

    public void OnOfficeEnterBtnClicked(Office office)
    {
        foreach (var light in office.lightList)
            light.gameObject.SetActive(false);

        TeleportAfterWorldEnter.Instance.toPos = office.toTrans.position;
        TeleportAfterWorldEnter.Instance.toRot = office.toTrans.eulerAngles;

        //PhotonManager.Instance.EnterWorld();

        gameObject.SetActive(false);

        worldManager.EnableChild(true);
    }

    public void OnQnAEnterBtnClicked()
    {
        TeleportAfterWorldEnter.Instance.toPos = QnABuildingTrans.position;
        TeleportAfterWorldEnter.Instance.toRot = QnABuildingTrans.eulerAngles;

        gameObject.SetActive(false);

        worldManager.EnableChild(true);
    }

    public void OnBuildingBtnClicked(Building building)
    {
        StartCoroutine(FocusOnBuilding(building));
    }

    public void OnOfficeBtnClicked()
    {
        //lawyerProfileUI.SetActive(true);
    }

    public void OnBackBtnClicked()
    {
        lawyerProfileUI.SetActive(false);
        StartCoroutine(ResetCamera());
    }

    public IEnumerator FocusOnWorld(World world)
    {
        myAvatarUI.SetActive(false);
        worldButtonUI.SetActive(false);

        lobbyCamera.Rotate(world.cameraRot, cameraMoveTime);
        yield return lobbyCamera.Move(world.cameraPos, cameraMoveTime);

        buildingButtonUI.SetActive(true);
        buildingButtonsList[world.transform.GetSiblingIndex()].SetActive(true);;
        //world.SetBuildingBtns();
    }

    public IEnumerator FocusOnBuilding(Building building)
    {
        buildingButtonUI.SetActive(false);
        foreach (var btn in buildingButtonsList)
            btn.SetActive(false);

        lobbyCamera.Rotate(building.cameraRot, cameraMoveTime);
        yield return lobbyCamera.Move(building.cameraPos, cameraMoveTime);

        officeButtonUI.SetActive(true);
    }

    public IEnumerator ResetCamera()
    {
        buildingButtonUI.SetActive(false);
        foreach (var btn in buildingButtonsList)
            btn.SetActive(false);
            
        officeButtonUI.SetActive(false);

        lendingUI.SetActive(false);

        lobbyCamera.Rotate(defaultCameraRot, cameraMoveTime);
        yield return lobbyCamera.Move(defaultCameraPos, cameraMoveTime);

        myAvatarUI.SetActive(bVisible);
        worldButtonUI.SetActive(true);
    }
}
