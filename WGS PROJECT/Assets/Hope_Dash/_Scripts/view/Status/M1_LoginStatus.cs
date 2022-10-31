using UnityEngine;
using UnityEngine.UI;

namespace RunMinigames.View.Loading
{
    public class M1_LoginStatus : MonoBehaviour
    {

        public static M1_LoginStatus instance;
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