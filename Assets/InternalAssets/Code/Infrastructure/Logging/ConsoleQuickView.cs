using System.Collections.Generic;
using ProjectOlog.Code.Infrastructure.Logging.Configuration;
using TMPro;
using UnityEngine;

/*
 * A script for managing an additional console output preview
 * at the top of the game screen.
 */

namespace ProjectOlog.Code.Infrastructure.Logging
{
    public class ConsoleQuickView : MonoBehaviour
    {
        public TMP_Text field;

        private Queue<string> lines = new Queue<string>();

        private float currentLineTimer = 0f;

        ConfVar quickviewSize;
        ConfVar quickviewTime;

        private void Awake()
        {
            quickviewSize = Cvar.Get("quickview_size", "5");
            quickviewTime = Cvar.Get("quickview_time", "4");
        }

        private void Update()
        {
            currentLineTimer += Time.deltaTime;

            if(currentLineTimer > quickviewTime.Value)
            {
                DequeueOldestLine();
                currentLineTimer = 0f;
            }
        }

        public void ToggleField(bool active)
        {
            gameObject.SetActive(active);
            if (!active)
            {
                lines.Clear();
            }
            RefreshFieldText();
        }

        public void AddNewLine(string line)
        {
            lines.Enqueue(line);
            if(lines.Count > quickviewSize.Integer)
            {
                lines.Dequeue();
            }
            RefreshFieldText();
            if(lines.Count  == 1)
            {
                currentLineTimer = 0f;
            }
        }

        private void DequeueOldestLine()
        {
            if (lines.Count > 0)
            {
                lines.Dequeue();
                RefreshFieldText();
            }
        }

        private void RefreshFieldText()
        {
            string[] text = lines.ToArray();
            field.text = string.Join("\n", text);
        }
    }
}
