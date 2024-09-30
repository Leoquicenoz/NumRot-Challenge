using Gtk;
using System;

namespace User_Interface_App
{
    static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();

            // Crear la ventana principal
            var win = new MainWindow();
            win.Show();

            Application.Run();
        }
    }
}
