namespace Glittertind
{
    /// <summary>
    /// Represents an <see cref="Attribute"/> for marking a <see cref="Command"/> as a system command.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class SystemCommandAttribute : Attribute
    {
    }
}
