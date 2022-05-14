using UnityEngine;
using System.Collections;
using RunMinigames.Interface;

namespace RunMinigames.Mechanics.Items
{
    public abstract class InteractableItem : MoveItem
    {
        [Header("Item Info")]

        [SerializeField] protected float SpeedCharacter = 3f;
        [SerializeField] protected float LongTimeBehaviour = 3f;

        protected MeshRenderer mesh;
        protected SphereCollider sphereCollider;
        protected bool isObstacles;

        MoveItem moveItem;

        protected new void Awake()
        {
            base.Awake();

            mesh = GetComponent<MeshRenderer>();
            sphereCollider = GetComponent<SphereCollider>();
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


