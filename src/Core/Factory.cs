using Mauve;
using Mauve.Extensibility;
using Mauve.Runtime;

namespace Freya.Core
{
    internal abstract class Factory<T>
    {

        #region Fields

        private readonly ILogger<LogEntry> _logger;

        #endregion

        #region Constructor

        public Factory(ILogger<LogEntry> logger) =>
            _logger = logger;

        #endregion

        #region Public Methods

        public bool TryCreate(string alias, out T? instance)
        {
            T? result = default;
            try
            {
                result = CreateInstance(alias);
            } catch (Exception e)
            {
                string message = $"An unexpected error occurred while creating an instance of {typeof(T).Name} with alias {alias}. {e.FlattenMessages()}";
                _logger.Log(new LogEntry(EventType.Exception, message));
            }

            instance = result;
            return instance is not null;
        }

        #endregion

        #region Protected Methods

        protected abstract T CreateInstance(string alias);

        #endregion

    }
}
