namespace Freya.Commands
{
    internal interface CommandRequest
    {
        public string Key { get; set; }
        public IEnumerable<object> Parameters { get; set; }
    }
}
