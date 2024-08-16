using Sirenix.OdinInspector;
using UnityEngine;

namespace MungFramework.Entity
{
    public abstract class EntityComponent<E> : MonoBehaviour where E: Entity
    {
        private E _entity;
        [ShowInInspector]
        protected E entity
        {
            get
            {
                if (_entity == null)
                {
                    _entity = GetComponentInParent<E>();
                }
                return _entity;
            }
        }

    }
}