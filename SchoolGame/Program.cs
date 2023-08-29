using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SchoolGame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1 && args[1] == "fullversionofthegame")
            {
                Application.Run(new frmSettings());                
            }
            else
            {
                Application.Run(new frmGame(new Team("free", new string[10])));
            }
        }
    }
}
