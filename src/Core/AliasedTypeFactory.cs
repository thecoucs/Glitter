using System.Reflection;

using Mauve;
using Mauve.Extensibility;

namespace Freya.Core
{
    internal abstract class AliasedTypeFactory<T>
    {

        #region Protected Methods

        protected IEnumerable<Type>? GetQualifiedTypes()
        {
            // Get the entry assembly, if we can't find it, there's no work.
            var assembly = Assembly.GetEntryAssembly();
            if (assembly is null)
                return null;

            // Get any types considered valid for the factory.
            return from type
                   in assembly.GetTypes()
                   where !type.IsAbstract &&
                         !type.IsInterface &&
                         type.GetCustomAttribute<AliasAttribute>() is not null &&
                         type.DerivesFrom<T>()
                   select type;
        }

        #endregion

    }
}
