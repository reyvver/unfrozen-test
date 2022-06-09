namespace Game.Scripts.Core.Gameplay
{
    public interface ILiveObject
    {
        public float Hp { get; set; }
        public void ChangeHp(float value);
    }
}