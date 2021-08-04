using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject[] cubesMain;
    public GameObject[] cubesBonus;
    public GameObject[] panelForCubesBonus;
    public float timeBtwLevel;
    public List<Cube> listCube;
    public bool isReadLineActive = false;
    public Text textScoreLevel;//при проигрыше показывает сколько заработал.

    [SerializeField] int speed;
    [SerializeField] Transform spawnNewCube;
    [SerializeField] Text textMoney;
    [SerializeField] Text textAllMoney;
    [SerializeField] Text textRecord;
    [SerializeField] GameObject panelGameOver;
    [SerializeField] GameObject shleif;
    [SerializeField] GameObject adsButtonPanel;

    [SerializeField] AudioClip audioClipClick;
    [SerializeField] AudioClip audioClipGameOver;
    [SerializeField] AudioClip audioClipFire;
    [SerializeField] AudioClip audioClipCollision;
    [SerializeField] AudioClip audioClipUseBonus;

    private GameObject cubeMain;
    public bool isNewGame = false;
    private bool isNotMusic = true;
    private bool isActiveController = true;
    private int IDCube = 200;
    private float translatePosition;
    private int countColiisionCube;
    private int speed2 = 400;
    private Cube cube1;
    private int money;
    private int currentRecord = 0;
    private AudioSource audioSource;
    private MobAdsRewarded adsRewarded;
    private int currentMoneyForBonus;
    private GameObject currentCubeMainForBonus;
    private int randIndexSpawnCube;

    public int Money
    {
        get
        {
            return PlayerPrefs.GetInt("money");
        }
        set
        {
            PlayerPrefs.SetInt("money", value);
            textMoney.text = PlayerPrefs.GetInt("money").ToString();
        }
    }

    public int AllMoney
    {
        get
        {
            return PlayerPrefs.GetInt("allMoney");
        }
        set
        {
            PlayerPrefs.SetInt("allMoney", value);
            textAllMoney.text = PlayerPrefs.GetInt("allMoney").ToString();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
#if UNITY_EDITOR
        translatePosition = 10f;//0.2
#else
        translatePosition = 11f;//0.09
#endif
        if (cubeMain != null && isActiveController)
        {
            cubeMain.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            if (eventData.delta.x > 3)
            {
                cubeMain.transform.position -= Vector3.right * translatePosition*Time.deltaTime;
                Vector3 asd = cubeMain.transform.position;
                asd.x = -2.55f;
                if (cubeMain.transform.position.x <= -2.55f) cubeMain.transform.position = asd;
            }
            else if (eventData.delta.x < -3)//движение влево
            {
                cubeMain.transform.position -= Vector3.left * translatePosition * Time.deltaTime;//первый раз было 0,2, на телефоне очень шустро
                Vector3 asd = cubeMain.transform.position;
                asd.x = 2.55f;
                if (cubeMain.transform.position.x >= 2.55f) cubeMain.transform.position = asd;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (cubeMain != null && isActiveController)
        {
            audioSource.clip = audioClipFire;
            audioSource.Play();
            isReadLineActive = false;
            GameObject newShleif = Instantiate(shleif, Vector3.zero, Quaternion.identity);
            newShleif.transform.SetParent(cubeMain.transform);
            newShleif.transform.localScale = new Vector3(0.18f, 0.7f, 1);
            newShleif.transform.localPosition = new Vector3(0, -0.49f, 1.6f);
            newShleif.transform.rotation = Quaternion.Euler(90, 0, 0);
            cubeMain.GetComponent<Rigidbody>().AddForce(-Vector3.forward * speed);
            Destroy(newShleif.gameObject, 1f);
            timeBtwLevel = 2.5f;
            isNewGame = true;
            isActiveController = false;
        }
    }

    void Start()
    {
        LoadNewCube2();
        Money = Money;
        AllMoney = AllMoney;
        textRecord.text = PlayerPrefs.GetInt("recordText").ToString();
        audioSource = GetComponent<AudioSource>();
        adsRewarded = FindObjectOfType<MobAdsRewarded>();
        isNotMusic = true;
    }

    void Update()
    {
        timeBtwLevel -= Time.deltaTime;
        if (timeBtwLevel <= 0 && (isNewGame || cubeMain == null))
        {
            LoadNewCube2();
        }
    }

    private void LoadNewCube2()
    {
        if (cubeMain != null)
        {
            cubeMain.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            cubeMain = null;
        }
        randIndexSpawnCube = Random.Range(0, PlayerPrefs.GetInt("level"));
        cubeMain = Instantiate(cubesMain[randIndexSpawnCube], spawnNewCube.position, Quaternion.identity);
        cubeMain.GetComponent<Cube>().IDCube = UpIDCube();
        isReadLineActive = true;
        isNewGame = false;
        isActiveController = true;
        currentMoneyForBonus = 0;
    }

    public int UpIDCube()//private
    {
        IDCube++;
        return IDCube;
    }

    public void ResetKey()
    {
        PlayerPrefs.DeleteAll();
    }

    public void GameOver()
    {
        if (timeBtwLevel <= 0 && !isNewGame && isReadLineActive && isNotMusic)
        {
            isNotMusic = false;
            audioSource.clip = audioClipGameOver;
            audioSource.Play();
            currentRecord = 0;
            panelGameOver.SetActive(true);
            textScoreLevel.text = PlayerPrefs.GetInt("money").ToString();

            currentRecord = PlayerPrefs.GetInt("money");
            if (currentRecord > PlayerPrefs.GetInt("recordText"))
                PlayerPrefs.SetInt("recordText", currentRecord);
            textRecord.text = PlayerPrefs.GetInt("recordText").ToString();
        }
    }

    public void PlayAgain()
    {
        audioSource.clip = audioClipClick;
        audioSource.Play();
        AllMoney += Money;
        PlayerPrefs.SetInt("level", 1);
        PlayerPrefs.SetInt("money", 0);
        StartCoroutine(LoadNewGame());
    }

    public void Reklama()
    {
        adsButtonPanel.SetActive(true);
        audioSource.clip = audioClipClick;
        audioSource.Play();
        adsRewarded.ShowRewardedAd();
    }

    IEnumerator LoadNewGame()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CollisionCube(Cube cube)
    {
        audioSource.clip = audioClipCollision; ;
        audioSource.Play();

        int numberCube = cube.numberCube;
        Vector3 posCube = cube.transform.position;

        listCube.Remove(cube);
        Destroy(cube);

        timeBtwLevel = 1.3f;
        float randX = Random.Range(-0.2f, 0.2f);
        float randZ = Random.Range(-0.05f, 0.05f);
        Vector3 direction = new Vector3(randX, 0.6f, randZ);

        var newCube = Instantiate(cubesMain[numberCube + 1], posCube, Quaternion.identity);
        newCube.GetComponent<Cube>().IDCube = UpIDCube();

        for (int i = 0; i < listCube.Count; i++)
        {
            if (numberCube + 1 == listCube[i].numberCube)
            {
                Vector3 heading;
                heading = listCube[i].transform.position - newCube.transform.position;
                var distance = heading.magnitude;
                direction = heading / distance;
                direction.y += 1f;
                direction.x /= 2.5f;
                direction.z /= 2.5f;
                break;
            }
        }
        newCube.GetComponent<Rigidbody>().AddForce(direction * speed2);

        for (int i = 0; i < cubesMain.Length; i++)
        {
            if (numberCube == i)
            {
                if (i > PlayerPrefs.GetInt("level"))
                {
                    PlayerPrefs.SetInt("level", i);
                }
                Money += i + 1;
            }
        }
    }

    public void ChangeCubeBonus(int moneyForBonus)
    {
        if (cubeMain != null && isActiveController)
        {
            int number = -5;
            audioSource.clip = audioClipClick;
            audioSource.Play();
            if (moneyForBonus <= AllMoney)
            {
                for (int i = 0; i < 4; i++)
                {
                    panelForCubesBonus[i].GetComponent<Image>().color = new Color(255, 255, 255, 1f);
                }
                if (moneyForBonus == 600)   number = 0;
                else if (moneyForBonus == 800)  number = 1;
                else if (moneyForBonus == 1000) number = 2;
                else    number = 3;
            }
            if(currentMoneyForBonus != 0 && moneyForBonus != currentMoneyForBonus)
            {
                Destroy(cubeMain);
                cubeMain = Instantiate(cubesBonus[number], spawnNewCube.position, Quaternion.identity);
                AllMoney -= moneyForBonus;
                AllMoney += currentMoneyForBonus;
                currentMoneyForBonus = moneyForBonus;
                panelForCubesBonus[number].GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
            }
            else if(moneyForBonus == currentMoneyForBonus)
            {
                Destroy(cubeMain);
                cubeMain = Instantiate(cubesMain[randIndexSpawnCube], spawnNewCube.position, Quaternion.identity);
                AllMoney += moneyForBonus;
                currentMoneyForBonus = 0;
                panelForCubesBonus[number].GetComponent<Image>().color = new Color(255, 255, 255, 1f);
            }
            else if(currentMoneyForBonus == 0)
            {
                currentMoneyForBonus = moneyForBonus;
                listCube.Remove(cubeMain.GetComponent<Cube>());
                Destroy(cubeMain);
                cubeMain = Instantiate(cubesBonus[number], spawnNewCube.position, Quaternion.identity);
                AllMoney -= moneyForBonus;
                panelForCubesBonus[number].GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
            }
            isReadLineActive = true;
            isNewGame = false;
        }
    }
}
