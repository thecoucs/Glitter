namespace Freya.Core
{
    internal class BotCommand
    {

        #region Properties

        public string Key { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        #endregion

        #region Constructor

        public BotCommand(string key, string displayName, string description)
        {
            Key = key;
            DisplayName = displayName;
            Description = description;
        }

        #endregion

    }
}