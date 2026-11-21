namespace kpz_lr3
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.cd 
        /// </summary>
        [STAThread]
        static void Main()
        {

            ApplicationConfiguration.Initialize();   // обов'€зково ѕ≈–Ў»ћ р€дком

            bool loggedIn;
            using (var login = new Login())
            {
                if (login.ShowDialog() != DialogResult.OK)
                    return;                           // закрили хрестиком Ч вих≥д
                loggedIn = login.logged_in;
            }
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            Application.Run(new Form1(loggedIn));
        }
    }
}