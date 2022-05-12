namespace RunMinigames.Interface
{
    public interface ICharacterItem
    {
        public bool IsItemSpeedActive { get; set; }
        public bool CanMove { get; set; }
        public float CharSpeed { get; set; }
        public float MaxSpeed { get; set; }

        public void Movement();
        public void Running(float runSpeed);
        public void Jump();
    }
}