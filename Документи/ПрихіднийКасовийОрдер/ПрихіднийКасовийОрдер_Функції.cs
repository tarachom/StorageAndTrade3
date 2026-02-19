

/*
        ПрихіднийКасовийОрдер_Функції.cs
        Функції
*/

using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Документи;

namespace StorageAndTrade
{
    static class ПрихіднийКасовийОрдер_Функції
    {
        public static List<Where> Відбори(string searchText)
        {
            return
            [
                
                //Назва
                new Where(ПрихіднийКасовийОрдер_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //Коментар
                new Where(Comparison.OR, ПрихіднийКасовийОрдер_Const.Коментар, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //КлючовіСловаДляПошуку
                new Where(Comparison.OR, ПрихіднийКасовийОрдер_Const.КлючовіСловаДляПошуку, Comparison.LIKE, searchText) { FuncToField = "LOWER" },

            ];
        }

        public static async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null,
            Action<UnigueID?>? сallBack_LoadRecords = null)
        {
            ПрихіднийКасовийОрдер_Елемент page = new ПрихіднийКасовийОрдер_Елемент
            {
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
            await NotebookFunction.AddLockObjectFunc(Program.GeneralNotebook, page.Name, page.Елемент);
            await page.LockInfo(page.Елемент);
            page.SetValue();
        }

        public static async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            ПрихіднийКасовийОрдер_Pointer Вказівник = new(unigueID);
            bool? label = await Вказівник.GetDeletionLabel();
            if (label.HasValue) await Вказівник.SetDeletionLabel(!label.Value);
        }

        public static async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ПрихіднийКасовийОрдер_Objest Обєкт = new ПрихіднийКасовийОрдер_Objest();
            if (await Обєкт.Read(unigueID))
            {
                ПрихіднийКасовийОрдер_Objest Новий = await Обєкт.Copy(true);
                await Новий.Save();

                await Новий.РозшифруванняПлатежу_TablePart.Save(false); // Таблична частина "РозшифруванняПлатежу"

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
