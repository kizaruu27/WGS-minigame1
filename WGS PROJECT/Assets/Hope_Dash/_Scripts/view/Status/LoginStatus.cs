using UnityEngine;
using UnityEngine.UI;

namespace RunMinigames.View.Loading
{
    public class LoginStatus : MonoBehaviour
    {

        public static LoginStatus instance;
        Text StatusMessage;

        public bool isConnectingToServer { get; set; }

        private void Awake()
        {
            instance = this;
            StatusMessage = GetComponent<Text>();
        }


        public void StepperMessage(string step)
        {
            StatusMessage.text = step;
        }
    }
}