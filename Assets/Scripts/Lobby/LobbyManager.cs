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
    public GameObject myAvatarUI;
    public GameObject avatarSelectUI;
    public GameObject worldButtonUI;
    public GameObject buildingButtonUI;
    public List<GameObject> buildingButtonsList;
    public GameObject officeButtonUI;
    public GameObject lawyerProfileUI;

    public GameObject myOfficeBtn;

    public Transform QnABuildingTrans;

    public Text idText;
    public Text typeText;

    public Avatar myAvatar;

    [SerializeField] private float cameraMoveTime;


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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnAvatarSelectBtnClicked(int idx)
    {
        avatarSelectUI.SetActive(false);

        PlayerPrefs.SetInt("Avatar", idx);

        myAvatarUI.SetActive(true);
        myAvatar.idx = idx;
        myAvatar.gameObject.SetActive(true);

        worldButtonUI.SetActive(true);
    }

    public void OnWorldBtnClicked(World world)
    {
        StartCoroutine(FocusOnWorld(world));
    }

    public void OnOfficeEnterBtnClicked(Office office)
    {
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
        lawyerProfileUI.SetActive(true);
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

        lobbyCamera.Rotate(defaultCameraRot, cameraMoveTime);
        yield return lobbyCamera.Move(defaultCameraPos, cameraMoveTime);

        myAvatarUI.SetActive(true);
        worldButtonUI.SetActive(true);
    }
}
