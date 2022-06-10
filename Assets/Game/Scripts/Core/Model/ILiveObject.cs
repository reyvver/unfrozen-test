namespace Game.Scripts.Core.Model
{
    public interface ILiveObject
    {
        public float Hp { get;}
        public void ChangeHp(float value);
    }
}