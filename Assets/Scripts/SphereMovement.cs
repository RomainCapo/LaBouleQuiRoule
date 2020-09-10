using BayatGames.SaveGameFree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SphereMovement : MonoBehaviour
{
    Rigidbody spherePhysic = null;
    private Vector3 jump;
    public Material sphereColor;
    private bool isGrounded;
    public static bool gameOver = false;
    private bool gameIsStart = false;

    public AudioClip jumpSound;
    public AudioClip acceleratorSound;
    public AudioClip deceleratorSound;
    public AudioClip shrinkerSound;
    public AudioClip expanderSound;
    public AudioClip startSound;

    private AudioSource audioSource;

    public GameObject gameManager;
    private GameManager gm;
    private const int NB_LEVEL = 4;
    private const float MAX_POSITION_GAMEOVER = -20;

    // Start is called before the first frame update
    void Start()
    {
        jump = new Vector3(0.0f, 5.0f, 1.0f);
        gm = gameManager.GetComponent<GameManager>();

        //Add a random color to the sphere at the begin of the game
        gameObject.GetComponent<MeshRenderer>().material = sphereColor;
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionStay(Collision col)
    {
        if(col.gameObject.tag != "wall")
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit()
    {
        isGrounded = false;
    }

    void OnCollisionEnter(Collision col)
    {
        //Detect the collision with the final plane
        //If collision is detected restart scene
        if(col.gameObject.name == "Plane")
        {
            gameOver = true;

            int level = SaveGame.Load<int>("level");
            if (level >= NB_LEVEL)
            {
                //fin du jeu faire qqch ici
                gm.GameWin();
            }
            else
            {
                gm.EndLevel();
                SaveGame.Save<int>("level", ++level);
            }
        }
        else if(col.gameObject.tag == "accelerator")
        {
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0,0, 200f));
            audioSource.PlayOneShot(acceleratorSound);
        }
        else if (col.gameObject.tag == "decelerator")
        {
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 400f, -200f));
            audioSource.PlayOneShot(deceleratorSound);
        }
        else if(col.gameObject.tag == "shrinker")
        {
            Vector3 scale = gameObject.GetComponent<Transform>().localScale;
            scale *= 0.95f;
            gameObject.GetComponent<Transform>().localScale = scale;
            audioSource.PlayOneShot(shrinkerSound);
        }
        else if(col.gameObject.tag == "expander")
        {
            Vector3 scale = gameObject.GetComponent<Transform>().localScale;
            scale *= 1.05f;
            gameObject.GetComponent<Transform>().localScale = scale;
            audioSource.PlayOneShot(expanderSound);
        }
        else if(col.gameObject.tag == "brick")
        {
            StartCoroutine(DropFloor(col.gameObject));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Start the game when the key e is press
        if (!gameIsStart && Input.GetKeyDown(KeyCode.E))
        {
            spherePhysic = gameObject.AddComponent<Rigidbody>();
            gameOver = false;
            gameIsStart = true;
            audioSource.PlayOneShot(startSound);
        }

        //Add jump force to the sphere if space is press
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            if(spherePhysic != null)
            {
                spherePhysic.AddForce(jump, ForceMode.Impulse);
                isGrounded = false;
                audioSource.PlayOneShot(jumpSound);
            }
        }

        //Restart the game if the ball fall from the board
        if(this.transform.position.y < MAX_POSITION_GAMEOVER)
        {          
            if(!gameOver)
            {
                gm.GameOver();
                gameOver = true;
            }
        }
    }

    IEnumerator DropFloor(GameObject go)
    {
        yield return new WaitForSeconds(0.5f);

        Destroy(go);
    }
}
