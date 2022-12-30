using System.Reflection;

using Freya.Commands;
using Freya.Core;

using Mauve;

namespace Freya.Runtime
{
    /// <summary>
    /// Represents a factory for creating <see cref="Command"/> instances.
    /// </summary>
    internal class CommandFactory : AliasedTypeFactory<Command>
    {
        private Dictionary<string, Type>? _types;
        /// <summary>
        /// Attempts to create a new instance of <see cref="Command"/> from the specified <see cref="CommandRequest"/>.
        /// </summary>
        /// <param name="request">The request for the command.</param>
        /// <param name="command">The command created by the factory.</param>
        /// <returns><see langword="true"/> if creation was successful, otherwise <see langword="false"/>.</returns>
        public bool TryCreate(CommandRequest request, out Command? command)
        {
            // Attempt to identify a type that matches the specified key.
            command = null;
            Type? type = GetTypeForAlias(request.Key);
            if (type is null)
                return false;

            try
            {
                // Create an instance of the command.
                object? createdInstance = Activator.CreateInstance(type, request.Parameters);
                if (createdInstance is Command commandInstance)
                    command = commandInstance;
            } catch
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Gets the <see cref="Type"/> for the specified alias.
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        protected Type? GetTypeForAlias(string alias)
        {
            // If we already have the requested alias, return the associated type.
            if (_types?.ContainsKey(alias) == true)
                return _types[alias];

            // If the alias wasn't found but we've already recorded all eligible types, then return null.
            else if (_types is not null)
                return null;

            // Get any types considered valid for the factory.
            IEnumerable<Type>? types = GetQualifiedTypes();
            if (types is null)
                return null;

            // Store the types with their associated aliases.
            _types = new Dictionary<string, Type>();
            foreach (Type type in types)
                HandleType(type);

            // If we already have the requested alias, return the associated type.
            if (_types?.ContainsKey(alias) == true)
                return _types[alias];

            // If we haven't found the requested alias by now, we're not going to.
            return null;
        }
        /// <summary>
        /// Attempts to add the specified type to the underlying cache.
        /// </summary>
        /// <param name="type">The type to handle.</param>
        /// <exception cref="InvalidOperationException">Thrown if the specied type is associated with an alias that's already cached.</exception>
        private void HandleType(Type type)
        {
            // Get the type alias and validate it.
            AliasAttribute? typeAlias = type?.GetCustomAttribute<AliasAttribute>();
            if (typeAlias is null)
                return;

            // Iterate over the identified aliases, validating, and then recording them.
            foreach (string value in typeAlias.Values)
            {
                // Validate the value.
                if (string.IsNullOrWhiteSpace(value))
                    continue;

                // Prevent duplicate associations.
                if (_types?.ContainsKey(value) == true)
                    throw new InvalidOperationException("Unable to register more than one type with a single alias.");

                // Record the association.
                if (_types is not null && type is not null)
                    _types.Add(value, type);
            }
        }
    }
}
