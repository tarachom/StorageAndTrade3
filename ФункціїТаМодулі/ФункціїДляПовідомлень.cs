/*
 
Повідомлення про помилки

*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using GeneratedCode;

namespace StorageAndTrade
{
    static class ФункціїДляПовідомлень
    {
        public static async void ДодатиПовідомлення(UuidAndText? basis, string НазваОбєкту, Exception exception)
        {
            await Config.Kernel.MessageErrorAdd("Запис", basis?.Uuid, basis?.Text, НазваОбєкту, exception.Message);
            ПоказатиПовідомлення(basis?.UnigueID());
        }

        public static async void ДодатиІнформаційнеПовідомлення(UuidAndText? basis, string НазваОбєкту, string Повідомлення)
        {
            await Config.Kernel.MessageInfoAdd("Інформація", basis?.Uuid, basis?.Text, НазваОбєкту, Повідомлення);
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