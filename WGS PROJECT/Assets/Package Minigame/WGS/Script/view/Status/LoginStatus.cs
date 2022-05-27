using UnityEngine;
using TMPro;

namespace RunMinigames.View.Loading
{
    public class LoginStatus : MonoBehaviour
    {

        public static LoginStatus instance;
        TextMeshProUGUI StatusMessage;

        public bool isConnectingToServer { get; set; }

        private void Awake()
        {
            instance = this;
            StatusMessage = GetComponent<TextMeshProUGUI>();
        }


        public void StepperMessage(string step)
        {
            StatusMessage.text = step;
        }
    }
}