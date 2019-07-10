using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region privates
    private Animator anim;
    private Rigidbody2D rb;
    private float touchTime = 0.0f;
    private float velocityScale;
    private bool isCharging;
    private bool isJumping;
    private int numLives;
    private float cameraNewY;
    private Vector3 spikeNewPos;
    private bool cameraMoving;
    private Camera mainCamera;
    private bool energyIncreasing;
    private const float maxEnergy = 0.5f;
    private float minSpikeHeight;
    private float maxSpikeHeight;
    private float minTargetHeight;
    private float maxTargetHeight;
    private float characterHeight;
    private GameObject extraLifeObject = null;
    private float playerHeight;
    #endregion


    #region publics
    public GameObject basePlatform;
    public GameObject targetPlatform;
    public GameObject extraPlatform;
    public GameObject spike;
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public GameObject extraLife;
    public Slider energySlider;
    public Sprite heart;
    public Sprite deadHeart;
    public int maxLives;
    public float minimumTargetHeight;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        isCharging = false;
        isJumping = false;
        numLives = maxLives;
        mainCamera = Camera.main;
        characterHeight = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
        ScoreScript.singleton.ResetScore();
        playerHeight = gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraMoving)
        {
            spike.transform.position = Vector3.MoveTowards(spike.transform.position, spikeNewPos, 0.175f);
        }

        // Transition the camera upward.
        if (mainCamera.transform.position.y < cameraNewY)
        {
            Vector3 newpos = mainCamera.transform.position;
            newpos.y = cameraNewY;
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, newpos, 0.08f);
        }
        else
        {
            cameraMoving = false;
        }

        // Introduce the new target platform after transitioning the camera.
        if (!cameraMoving && targetPlatform.transform.position.x > 0)
        {
            Vector3 newtargetPosition = targetPlatform.transform.position;
            newtargetPosition.x = 0;
            targetPlatform.transform.position = Vector3.MoveTowards(targetPlatform.transform.position, newtargetPosition, .35f);
        }

        if (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Space))
        {
            if (!isCharging && !isJumping && !cameraMoving)
            {
                isCharging = true;
                anim.SetBool("charging", true);
                Debug.Log("Charging now!");
                energyIncreasing = true;
            }
            if (isCharging)
            {
                float increment = energyIncreasing ? Time.deltaTime : -Time.deltaTime;
                touchTime += increment/2;
                if (touchTime >= maxEnergy)
                {
                    energyIncreasing = false;
                    touchTime = maxEnergy;
                }
                else if (touchTime <= 0.0f)
                {
                    energyIncreasing = true;
                    touchTime = 0.0f;
                }
                EnergyScript.energyAmount = touchTime;
            }
        }
        else
        {
            if (isCharging)
            {
                isCharging = false;
                isJumping = true;
                anim.SetBool("charging", false);
                anim.SetBool("jumping", true);
                velocityScale = 36.0f * (touchTime);
                float energyAmount = Mathf.Min(Mathf.Max(velocityScale, 8.0f), 18.0f);
                rb.AddForce(Vector2.up * energyAmount, ForceMode2D.Impulse);
                touchTime = 0.0f;
                EnergyScript.energyAmount = touchTime;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision!");

        if (!isJumping)
        {
            return;
        }

        Collider2D col = collision.collider;
        if (collision.gameObject.transform.CompareTag("Platform"))
        {
            bool top = rb.velocity.y <= 0;
            if (top)
            {
                // Landed on original platform
                if (Mathf.Abs(gameObject.transform.position.y - basePlatform.transform.position.y) < 1.0)
                {
                    LoseLife();
                }
                else
                {
                    // Get position increments to use for updating platform positions.
                    float x = targetPlatform.transform.position.y - basePlatform.transform.position.y;
                    float y = spike.transform.position.y - targetPlatform.transform.position.y;

                    cameraNewY = mainCamera.transform.position.y + x;

                    // Move spike to new position
                    minSpikeHeight = cameraNewY + 1.0f;
                    maxSpikeHeight = minSpikeHeight + 3.2f;
                    spikeNewPos = spike.transform.position;
                    spikeNewPos.y = Random.Range(minSpikeHeight, maxSpikeHeight);

                    // Move extra platform to new target position.
                    minTargetHeight = targetPlatform.transform.position.y + minimumTargetHeight;
                    maxTargetHeight = Mathf.Min(spikeNewPos.y - (characterHeight * 1.25f));
                    Vector3 newTargetPosition = extraPlatform.transform.position;
                    newTargetPosition.y = Random.Range(minTargetHeight, maxTargetHeight);
                    extraPlatform.transform.position = newTargetPosition;

                    // Swap platform roles.
                    GameObject temp = basePlatform;
                    basePlatform = targetPlatform;
                    targetPlatform = extraPlatform;
                    extraPlatform = temp;
                    targetPlatform.transform.position = MoveRight(targetPlatform.transform.position, 5);

                    cameraMoving = true;

                    // Update score.
                    ScoreScript.singleton.IncrementScore();

                    // Reset energy meter.
                    energySlider.value = 0;

                    if (extraLifeObject != null)
                    {
                        Destroy(extraLifeObject);
                        extraLifeObject = null;
                    }

                    if (Random.Range(0f, 1.0f) <= 0.075f)
                    {
                        // Create extra life powerup
                        extraLifeObject = Instantiate(extraLife, spikeNewPos - new Vector3(0f, 1.0f, 0f), Quaternion.identity);
                    }

                    // Update player height.
                    playerHeight = gameObject.transform.position.y;

                    //;
                    //Vector3 extraLifePosition = spikeNewPos;
                    //extraLifePosition.y -= 2.0f;
                    //extraLife.gameObject.transform.position = extraLifePosition;
                }

                isJumping = false;
                anim.SetBool("jumping", false);
            }
        }
        else if (collision.gameObject.transform.CompareTag("Ceiling"))
        {
            LoseLife();
            isJumping = false;
            anim.SetBool("jumping", false);

            // Reset energy meter.
            energySlider.value = 0;
        }

    }


    #region health
    void LoseLife()
    {
        numLives--;
        Debug.Log("Number of lives: " + numLives);
        if (numLives <= 0)
        {
            Destroy(this.gameObject);
            heart1.GetComponent<Image>().sprite = deadHeart;

            //// Update leaderboard.
            LeaderboardScript.NewScore(ScoreScript.singleton.scoreValue);
            //LeaderboardScript.playerDead = true;

            ScoreScript.singleton.FinalScore();

            // Trigger anything we need to end the game, find game manager and lose game
            GameObject gm = GameObject.FindWithTag("GameController");
            gm.GetComponent<GameManagerScript>().EndGame();
        }
        else
        {
            //gameObject.transform.position = new Vector3(0, basePlatform.transform.position.y);
            gameObject.transform.position = new Vector3(0, playerHeight);
            if (numLives == 2)
            {
                heart3.GetComponent<Image>().sprite = deadHeart;
            }
            else if (numLives == 1)
            {
                heart2.GetComponent<Image>().sprite = deadHeart;
            }
        }
    }

    public void GainLife()
    {
        if (numLives <= 2)
        {
            if (numLives == 1)
            {
                heart2.GetComponent<Image>().sprite = heart;
            }
            else if (numLives == 2)
            {
                heart3.GetComponent<Image>().sprite = heart;
            }
            numLives++;
        }
    }
    #endregion


    #region movement
    Vector3 MoveUp(Vector3 position, float yVal)
    {
        Vector3 newPosition = position;
        newPosition.y += yVal;
        return newPosition;
    }

    Vector3 MoveRight(Vector3 position, float xVal)
    {
        Vector3 newPosition = position;
        newPosition.x += xVal;
        return newPosition;
    }
    #endregion



    //private IEnumerator Wait()
    //{
    //    Debug.Log("IN WAIT");
    //    while (true)
    //    {
    //        yield return new WaitForSecondsRealtime(2);
    //    }
    //}
}