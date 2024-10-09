/*
Copyright (C) 2019-2024 TARAKHOMYN YURIY IVANOVYCH
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

/*
 
Повідомлення про помилки

*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0;

namespace StorageAndTrade
{
    static class ФункціїДляПовідомлень
    {
        public static async void ДодатиПовідомлення(UuidAndText basis, string НазваОбєкту, Exception exception)
        {
            await Config.Kernel.MessageErrorAdd("Запис", basis.Uuid, basis.Text, НазваОбєкту, exception.Message);
            ПоказатиПовідомлення(basis.UnigueID());
        }

        public static async void ДодатиІнформаційнеПовідомлення(UuidAndText basis, string НазваОбєкту, string Повідомлення)
        {
            await Config.Kernel.MessageInfoAdd("Інформація", basis.Uuid, basis.Text, НазваОбєкту, Повідомлення);
            ПоказатиПовідомлення();
        }

        public static async void ВідкритиПовідомлення()
        {
            СпільніФорми_ВивідПовідомленняПроПомилки page = new();
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Повідомлення", () => page);
            await page.LoadRecords();
        }

        public static async void ПоказатиПовідомлення(UnigueID? ВідбірПоОбєкту = null, int? limit = null)
        {
            СпільніФорми_ВивідПовідомленняПроПомилки page = new();

            Popover popover = new Popover(Program.GeneralForm?.ButtonMessage) { Position = PositionType.Bottom, BorderWidth = 5, WidthRequest = 600, HeightRequest = 600 };
            popover.Add(page);
            popover.Show();

            await page.LoadRecords(ВідбірПоОбєкту, limit);
        }
    }
}