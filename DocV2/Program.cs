using System;
using System.Windows.Forms;

namespace DocV2
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
                args = new string[] { "견적서" }; // 견적서, 거래명세서
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DocForm(args[0]));
        }
    }
}
