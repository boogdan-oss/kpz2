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

            using (var login = new Login())
            {
                // Хрестик або закриття без правильного пароля завершує програму
                if (login.ShowDialog() != DialogResult.OK)
                    return;
            }
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}