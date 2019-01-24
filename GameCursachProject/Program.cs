using System;
using System.Windows.Forms;

namespace GameCursachProject
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary> 
        [STAThread]
        static void Main()
        {
            //TODO: РАСКОММЕНТИТЬ НА РЕЛИЗ
            //try
            //{
                using (var game = new Game1())
                {
                    game.Run();
                }
            //}
            //catch(Exception e)
            //{
            //    Log.SendError("[Game]" + e.ToString());
            //    MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }
    }
}
