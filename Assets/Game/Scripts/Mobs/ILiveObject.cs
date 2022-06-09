namespace Game.Scripts.Mobs
{
    public interface ILiveObject
    {
        public float Hp { get; set; }
        public void ChangeHp(float value);
    }
}