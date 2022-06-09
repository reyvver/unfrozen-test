namespace Game.Scripts.Core.Gameplay
{
    public interface IDamageable
    {
        public float GetDamage();
        public void DoDamage();
        public void WasDamagedBy(float damage);
    }
}