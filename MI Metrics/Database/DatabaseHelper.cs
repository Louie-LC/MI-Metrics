using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Input;

namespace MI_Metrics
{
    static class DatabaseHelper
    {
        public static string CnnVal(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public static bool ValidateCharacters(string name)
        {
            foreach (char c in Database.InvalidCharacters)
            {
                if (name.Contains("" + c))
                {
                    return false;
                }
            }
            return true;
        }

        public static void ValidateCharacters(TextCompositionEventArgs e)
        {
            if (e.Text.Length == 0) return;
            char inputCharacter = e.Text[0];
            foreach(char c in Database.InvalidCharacters)
            {
                if (inputCharacter == c) e.Handled = true;
            }
        }
    }
}
