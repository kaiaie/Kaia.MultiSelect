using System;

namespace Kaia.Common.DataAccess.DataAnnotations
{
    /// <summary>
    /// Used to match a modifier or creator type with its corresponding entity
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EntityAttribute : Attribute
    {
        private readonly Type _entityType;

        public Type EntityType { get { return _entityType; } }

        public EntityAttribute(Type entityType)
        {
            _entityType = entityType;
        }
    }
}
