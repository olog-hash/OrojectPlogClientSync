using UnityEngine;

namespace ProjectOlog.Code.Infrastructure.Logging.Mapping
{
    /*
 * Stores a key binding.
 */

    public enum BindType
    {
        toggle,
        hold
    }

    public class BindRegistration
    {
        public readonly bool locked = false;

        private string c;
        public string Command
        {
            get
            {
                return c;
            }
            set
            {
                if (!locked)
                {
                    c = value;
                }
            }
        }

        private KeyCode k;
        public KeyCode Key
        {
            get
            {
                return k;
            }
            set
            {
                if (!locked)
                {
                    k = value;
                }
            }
        }

        public string Keystring
        {
            get
            {
                return k.ToString();
            }
        }

        private BindType b;
        public BindType BindType
        {
            get
            {
                return b;
            }
            set
            {
                if (!locked)
                {
                    b = value;
                }
            }
        }

        public bool Edit(KeyCode key, string command)
        {
            if (locked)
            {
                return false;
            }

            k = key;
            c = command;
            return true;
        }

        public BindRegistration(KeyCode key, string command, BindType type, bool locked)
        {
            Key = key;
            Command = command;
            BindType = type;
            this.locked = locked;
        }
    }
}