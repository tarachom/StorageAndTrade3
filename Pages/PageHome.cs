using Gtk;

//using Константи = StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0;
using StorageAndTrade_1_0.РегістриНакопичення;

namespace StorageAndTrade
{
    class PageHome : VBox
    {
        public PageHome() : base()
        {

        }

        public void StartBackgroundTask()
        {
            Program.CancellationTokenBackgroundTask = new CancellationTokenSource();

            Thread ThreadBackgroundTask = new Thread(new ThreadStart(CalculationVirtualBalances));
            ThreadBackgroundTask.Start();
        }

        void CalculationVirtualBalances()
        {
            int counter = 0;

            //Service.CalculationBalances.ПідключитиДодаток_UUID_OSSP();

            while (!Program.CancellationTokenBackgroundTask!.IsCancellationRequested)
            {
                if (counter > 5)
                {
                    //if (!Константи.Системні.ЗупинитиФоновіЗадачі_Const)
                    //{
                    //Service.CalculationBalances.ОбчисленняВіртуальнихЗалишківПоДнях();
                    //Service.CalculationBalances.ОбчисленняВіртуальнихЗалишківПоМісяцях();
                    // }

                    Config.Kernel!.DataBase.SpetialTableRegAccumTrigerExecute(VirtualTablesСalculation.Execute);

                    counter = 0;
                }

                counter++;

                Thread.Sleep(1000);
            }
        }
    }
}