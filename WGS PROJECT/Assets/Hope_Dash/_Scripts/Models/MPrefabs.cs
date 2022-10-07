using UnityEngine;

namespace RunMinigames.Models
{
    public class MPrefabs
    {
        public GameObject gameobject { get; private set; }
        public string name { get; private set; }
        public Vector3 position { get; private set; }
        public Quaternion quartenion { get; private set; }

        public MPrefabs(GameObject _go, string _goName, Vector3 _position, Quaternion _quartenion)
        {
            gameobject = _go;
            name = _goName;
            position = _position;
            quartenion = _quartenion;
        }
    }
}