namespace Glittertind
{
    /// <summary>
    /// Represents an <see cref="Attribute"/> for marking a <see cref="Command"/> as a system command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SystemCommandAttribute : Attribute
    {
        /// <summary>
        /// The key utilized by consumers to invoke the command.
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Creates a new <see cref="SystemCommandAttribute"/> instance.
        /// </summary>
        /// <param name="key">The key utilized by consumers to invoke the command.</param>
        public SystemCommandAttribute(string key) =>
            Key = key;
    }
}
