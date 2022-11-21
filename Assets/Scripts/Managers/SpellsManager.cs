using ScriptableObjects;
using UnityEngine;

namespace Managers
{
    public class SpellsManager : MonoBehaviour
    {
        [SerializeField] private SpellData currentSpell;
        [SerializeField] private Transform projectileSpawnPoint;


        public void Cast()
        {
            var spellObj = ObjectPoolManager.Instance.GetPooledObject();
            spellObj.transform.position = projectileSpawnPoint.position;
            spellObj.GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x), GetCurrentSpell);
        }

        public void ChangeSpell(SpellData spell)
        {
            currentSpell = spell;
            ObjectPoolManager.Instance.UpdatePooledObjects(currentSpell.SpellPrefab);
        }
    
        public SpellData GetCurrentSpell => currentSpell;
    }
}