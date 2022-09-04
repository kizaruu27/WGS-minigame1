using UnityEngine;
using System.Collections;
using RunMinigames.Interface;
using Photon.Pun;

namespace RunMinigames.Mechanics.Items
{
    public abstract class InteractableItem : MoveItem
    {
        [Header("Item Info")]

        [SerializeField] protected float SpeedCharacter = 3f;
        [SerializeField] protected float LongTimeBehaviour = 3f;

        [SerializeField] protected GameObject itemMesh;
        protected SphereCollider sphereCollider;
        protected bool isObstacles;

        [Header("Item Effect")]
        public ParticleSystem itemEffect;

        MoveItem moveItem;

        protected PhotonView pv;

        protected new void Awake()
        {
            base.Awake();
            sphereCollider = GetComponent<SphereCollider>();

            pv = GetComponent<PhotonView>();
        }

        protected void Update()
        {
            if (CanMove) Destroy(gameObject, 60f);
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICharacterItem character))
                StartCoroutine(OnCollideBehaviour(character));
        }

        public abstract IEnumerator OnCollideBehaviour(ICharacterItem character);
    }
}


