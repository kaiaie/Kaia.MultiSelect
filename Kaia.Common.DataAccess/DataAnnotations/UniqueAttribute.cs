using System;

namespace Kaia.Common.DataAccess.DataAnnotations
{
    /// <summary>
    /// Indicates an entity property has a UNIQUE constraint on it in the 
    /// underlying database
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class UniqueAttribute : Attribute
    {
    }
}
