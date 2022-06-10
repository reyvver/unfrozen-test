namespace Game.Scripts.Core.Model
{
    public interface IDamageable
    {
        public float GetDamage();
        public void DoAttack();
        public void WasAttacked(float damage);
    }
}