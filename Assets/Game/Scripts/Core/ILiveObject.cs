namespace Game.Scripts.Core
{
    public interface ILiveObject
    {
        public float Hp { get; set; }
        public void ChangeHp(float value);
    }
}