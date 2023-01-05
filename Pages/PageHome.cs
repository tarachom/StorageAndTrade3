#region Info

/*
Copyright (C) 2019-2023 TARAKHOMYN YURIY IVANOVYCH
All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

/*
Автор:    Тарахомин Юрій Іванович
Адреса:   Україна, м. Львів
Сайт:     accounting.org.ua
*/

#endregion

using Gtk;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.РегістриНакопичення;
using Константи = StorageAndTrade_1_0.Константи;

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

            while (!Program.CancellationTokenBackgroundTask!.IsCancellationRequested)
            {
                if (counter > 5)
                {
                    if (!Константи.Системні.ЗупинитиФоновіЗадачі_Const)
                    {
                        Config.Kernel!.DataBase.SpetialTableRegAccumTrigerExecute(VirtualTablesСalculation.Execute);
                    }

                    counter = 0;
                }

                counter++;

                Thread.Sleep(1000);
            }
        }
    }
}