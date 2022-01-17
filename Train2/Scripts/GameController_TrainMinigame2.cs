using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController_TrainMinigame2 : MonoBehaviour
{
    public static GameController_TrainMinigame2 instance;

    public TrainObj_TrainMinigame2 trainObj;
    public Canvas canvas;
    public Camera mainCamera;
    public Text txtTime;
    public int time;
    public int tmpTime;
    public bool isLose, isWin;
    private float maxXCamera;
    private float maxYCamera;
    public Vector2 mouseCurrentPos;
    public RaycastHit2D[] hit;
    public bool isHoldCharcoal;
    public Charcoal_TrainMinigame2 charcoal;
    public Charcoal_TrainMinigame2 charcoalPrefab;
    public PowerBar_TrainMinigame2 powerBar;
    public bool isIncreasingPower, isDecreasingPower;
    public BackgroundLoop_TrainMinigame2 background;
    public Enemy_TrainMinigame2 enemyPrefab;
    public Enemy_TrainMinigame2 enemyObj;
    public Transform posEnemy;
    public GameObject VFXPrefab;
    public Slider progressBar;
    public GameObject stationObj;
    public bool isBegin;
    public GameObject tutorial;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(instance);
    }

    private void Start()
    {
        SetSizeCamera();
        tutorial.SetActive(true);
        Tutorial1();
        time = 60;
        tmpTime = 0;
        txtTime.text = time.ToString();
        isHoldCharcoal = false;
        isIncreasingPower = false;
        isBegin = false;
        maxXCamera = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
        maxYCamera = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).y;
    }

    void SetSizeCamera()
    {
        float f1, f2;
        f1 = 16.0f / 9;
        f2 = Screen.width * 1.0f / Screen.height;

        mainCamera.orthographicSize *= f1 / f2;
    }

    void Tutorial1()
    {
        tutorial.transform.DOMoveX(0, 1).SetLoops(-1);
    }

    IEnumerator CountingTime()
    {
        while (time > 0)
        {
            time--;
            tmpTime++;
            if (time == 50 || time == 35 || time == 20)
            {
                SpawnEnemy();
            }
            if (tmpTime == 3)
            {
                tmpTime = 0;
                if (!isIncreasingPower && powerBar.currentPower > 0)
                {
                    isDecreasingPower = true;
                    if (background.speed > 0)
                    {
                        background.speed -= 1.0f / 3;
                    }
                    else
                    {
                        background.speed = 0;
                    }
                }
                powerBar.prePowerDown = powerBar.currentPower;
            }
            if (time < 11)
            {
                txtTime.color = Color.red;
            }
            txtTime.text = time.ToString();

            if (time == 0)
            {
                txtTime.text = "0";
                Lose();
            }
            yield return new WaitForSeconds(1);
        }
    }

    void SpawnEnemy()
    {
        enemyObj = Instantiate(enemyPrefab, posEnemy.position, Quaternion.identity);
    }

    public void Lose()
    {
        isLose = true;
        Debug.Log("Thua");
        StopAllCoroutines();
    }
    public void Win()
    {
        isWin = true;
        Debug.Log("Win");
        StopAllCoroutines();
        if (enemyObj != null)
        {
            Destroy(enemyObj);
        }
        stationObj.transform.parent = background.transform;

        background.transform.DOMoveX(background.transform.position.x - 8, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            trainObj.transform.DOMoveX(stationObj.transform.position.x, 1f).SetEase(Ease.Linear);
        });

    }

    void SpawnCharcoal()
    {
        charcoal = Instantiate(charcoalPrefab, mouseCurrentPos, Quaternion.identity);
    }

    private void Update()
    {
        if (!isWin && !isLose)
        {
            progressBar.value += (Time.deltaTime * background.speed) / 250;
            if (progressBar.value == progressBar.maxValue)
            {
                Win();
            }
        }

        if (Input.GetMouseButtonDown(0) && !isWin && !isLose)
        {
            mouseCurrentPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.RaycastAll(mouseCurrentPos, Vector2.zero);
            if (hit.Length != 0)
            {
                if (hit[0].collider.gameObject.CompareTag("Balloon"))
                {
                    isHoldCharcoal = true;
                    SpawnCharcoal();
                }
                if (hit[0].collider.gameObject.CompareTag("People"))
                {
                    trainObj.PlayAnimBell();
                    if (enemyObj != null)
                    {
                        enemyObj.RunAway();
                    }
                    hit[0].collider.transform.DOShakeRotation(0.5f, 20).OnComplete(() =>
                    {
                        hit[0].collider.transform.localEulerAngles = Vector3.zero;
                    });
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && isHoldCharcoal && !isLose && !isWin)
        {
            isHoldCharcoal = false;
            if (charcoal.isOnFurnace)
            {
                if (!isBegin)
                {
                    isBegin = true;
                    StartCoroutine(CountingTime());
                }
                if (tutorial.activeSelf)
                {
                    tutorial.SetActive(false);
                    tutorial.transform.DOKill();
                }
                Destroy(charcoal.gameObject);
                isIncreasingPower = true;
                var tmpVFX = Instantiate(VFXPrefab, mainCamera.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
                tmpVFX.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                tmpVFX.transform.position = new Vector3(tmpVFX.transform.position.x, tmpVFX.transform.position.y, 0);
                tmpVFX.GetComponent<SpriteRenderer>().DOFade(0, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    Destroy(tmpVFX);
                });
                powerBar.prePowerUp = powerBar.currentPower;
                powerBar.prePowerDown = powerBar.currentPower;
                if (background.speed < 20.0f / 3)
                {
                    background.speed += 4.0f / 3;
                }
                else
                {
                    background.speed = 20.0f / 3;
                }
            }
            else
            {
                Destroy(charcoal.gameObject);
            }
        }

        if (isHoldCharcoal)
        {
            mouseCurrentPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mouseCurrentPos = new Vector2(Mathf.Clamp(mouseCurrentPos.x, -maxXCamera + 1.8f, maxXCamera - 1.8f), Mathf.Clamp(mouseCurrentPos.y, -maxYCamera + 1, maxYCamera - 1));
            charcoal.transform.position = mouseCurrentPos;
        }
        if (isIncreasingPower)
        {
            powerBar.IncreasingPower();
        }
        if (isDecreasingPower && !isIncreasingPower)
        {
            powerBar.DecreasingPower();
        }
    }
}
