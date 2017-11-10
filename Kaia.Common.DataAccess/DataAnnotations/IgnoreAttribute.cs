using System;

namespace Kaia.Common.DataAccess.DataAnnotations
{
    /// <summary>
    /// Indicates that the property is to be ignored when generating SQL from 
    /// an entity class's member
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreAttribute : Attribute
    {
    }
}
