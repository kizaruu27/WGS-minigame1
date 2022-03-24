using System.Collections;
using UnityEngine;
using Photon.Pun;


public class PlayerRun_Multiplayer : MonoBehaviour
{
    [Header("Player")]
    Rigidbody rb;
    public GameObject Player;
    public Animator TargetAnimator;

    [Header("Animation")]
    public string AnimIdle;
    public string AnimRun;

    [Header("Player Speed")]
    public float PlayerSpeed;
    public float maxSpeed;

    PhotonView view;
    
    

    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();

    }

    // Update is called once per frame
    void Update()
    {

        if (view.IsMine)
        {
            StartCoroutine(Controler());

            if (PlayerSpeed >= 0)
            {
                PlayerSpeed -= 0.01f;
            }
            else if (PlayerSpeed >= 0)
            {
                TargetAnimator.SetBool("isRunning", true);
            }
            else
            {
                TargetAnimator.SetBool("isRunning", false);
            }

            if (PlayerSpeed >= maxSpeed)
            {
                PlayerSpeed = maxSpeed;
            }

            Player.transform.position += new Vector3(0, 0, PlayerSpeed * Time.deltaTime);
        }
    }
    

    IEnumerator Controler(){

        yield return new WaitForSeconds(4);

        if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                
                PlayerSpeed += 1f;
                TargetAnimator.SetBool("isRunning", true);
            }

    }

}
