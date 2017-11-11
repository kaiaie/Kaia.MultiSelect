using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Kaia.Common
{
    public class EntityBuilder
    {
        public static TEntity Build<TEntity>(IDataReader reader)
            where TEntity : class
        {
            // Because entity objects are immutable, we need to call 
            // the constructor, but arguments to the constructor and 
            // fields returned from the database may not be
            // in the same order
            var argValues = new List<Tuple<int, object>>();
            // XXX Assume the entity only has a single constructor
            var constructor = typeof(TEntity).GetConstructors().First();
            var constructorArgs = constructor.GetParameters();
            for (var i = 0; i < reader.FieldCount; ++i)
            {
                var fieldName = reader.GetName(i).ToCamelCase();
                var arg = constructorArgs.Single(a => a.Name == fieldName);
                argValues.Add(new Tuple<int, object>(arg.Position, reader[i]));
            }
            var @params = argValues
                .OrderBy(v => v.Item1).Select(v => v.Item2).ToArray();
            return constructor.Invoke(@params) as TEntity;
        }
    }
}
