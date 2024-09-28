

/*
        ЗамовленняПостачальнику_Функції.cs
        Функції
*/

using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    static class ЗамовленняПостачальнику_Функції
    {
        public static List<Where> Відбори(string searchText)
        {
            return
            [
                
                //Назва
                new Where(ЗамовленняПостачальнику_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //Коментар
                new Where(Comparison.OR, ЗамовленняПостачальнику_Const.Коментар, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //КлючовіСловаДляПошуку
                new Where(Comparison.OR, ЗамовленняПостачальнику_Const.КлючовіСловаДляПошуку, Comparison.LIKE, searchText) { FuncToField = "LOWER" },

            ];
        }

        public static async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null,
            Action<UnigueID?>? сallBack_LoadRecords = null)
        {
            ЗамовленняПостачальнику_Елемент page = new ЗамовленняПостачальнику_Елемент
            {
                IsNew = IsNew,
                CallBack_LoadRecords = сallBack_LoadRecords
            };

            if (IsNew)
                await page.Елемент.New();
            else if (unigueID == null || !await page.Елемент.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return;
            }

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);
            page.SetValue();
        }

        public static async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            ЗамовленняПостачальнику_Objest Обєкт = new ЗамовленняПостачальнику_Objest();
            if (await Обєкт.Read(unigueID))
                await Обєкт.SetDeletionLabel(!Обєкт.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        public static async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ЗамовленняПостачальнику_Objest Обєкт = new ЗамовленняПостачальнику_Objest();
            if (await Обєкт.Read(unigueID))
            {
                ЗамовленняПостачальнику_Objest Новий = await Обєкт.Copy(true);
                await Новий.Save();

                await Новий.Товари_TablePart.Save(false); // Таблична частина "Товари"

                return Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }
    }
}
