using UnityEngine;
using System.Collections;
using Photon.Pun;
using TMPro;


public class CountdownUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _CountdownTXT;
    public float InitialCountdown = 4f;

    PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }


    private void Update()
    {
        Debug.Log(_CountdownTXT.text);
        StartCoroutine(WaitAllPlayerReadyUI());

    }

    public IEnumerator WaitAllPlayerReadyUI()
    {
        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Player").Length == (int)PhotonNetwork.PlayerList.Length);

        StartCoroutine(StartCountdown());
    }

    public IEnumerator StartCountdown()
    {
        yield return new WaitForSeconds(1f);

        InitialCountdown -= Time.deltaTime;

        if (PhotonNetwork.IsMasterClient)
        {
            view.RPC("SetCountdown", RpcTarget.Others, InitialCountdown);
        }

        _CountdownTXT.fontSize = InitialCountdown > 2 ? 200f : 80f;
        _CountdownTXT.text = InitialCountdown > 2 ? $"{(int)(InitialCountdown - 1)}" : "Start";

        gameObject.SetActive(!(InitialCountdown <= 1f));
    }

    [PunRPC]
    void SetCountdown(float newCountdown) => InitialCountdown = newCountdown;
}