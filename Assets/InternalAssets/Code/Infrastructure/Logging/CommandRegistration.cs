namespace ProjectOlog.Code.Infrastructure.Logging
{
    public enum CommandType
    {
        normal,
        hidden
    }

    public class CommandRegistration
    {
        public string Command
        {
            get;
            private set;
        }

        public CommandHandler Handler
        {
            get;
            private set;
        }

        public string Help
        {
            get;
            private set;
        }

        public CommandType CommandType
        {
            get;
            private set;
        }

        public CommandRegistration(string command, CommandHandler handler, string help, CommandType type)
        {
            Command = command;
            Handler = handler;
            Help = help;
            CommandType = type;
        }
    }
}