namespace RunMinigames.Interface
{
    public interface M1_ICharacterItem
    {
        public bool IsItemSpeedActive { get; set; }
        public bool CanMove { get; set; }
        public float CharSpeed { get; set; }
        public float MaxSpeed { get; set; }

        public bool Active { get; set; }

        public void Movement();
        public void Running(float runSpeed);
        public void Jump();
    }
}