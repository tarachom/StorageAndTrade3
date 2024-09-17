
/*
        Організації_Функції.cs
        Функції
*/

using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    static class Організації_Функції
    {
        public static List<Where> Відбори(string searchText)
        {
            return
            [
                //Код
                new Where(Організації_Const.Код, Comparison.LIKE, searchText) { FuncToField = "LOWER" },

                //Назва
                new Where(Comparison.OR, Організації_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
            ];
        }

        public static async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null,
            Action<UnigueID?>? сallBack_LoadRecords = null,
            Action<UnigueID>? сallBack_OnSelectPointer = null)
        {
            Організації_Елемент page = new Організації_Елемент
            {
                IsNew = IsNew,
                CallBack_LoadRecords = сallBack_LoadRecords,
                CallBack_OnSelectPointer = сallBack_OnSelectPointer
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
            Організації_Objest Обєкт = new Організації_Objest();
            if (await Обєкт.Read(unigueID))
                await Обєкт.SetDeletionLabel(!Обєкт.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        public static async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            Організації_Objest Обєкт = new Організації_Objest();
            if (await Обєкт.Read(unigueID))
            {
                Організації_Objest Новий = await Обєкт.Copy(true);
                await Новий.Save();

                await Новий.Контакти_TablePart.Save(false); // Таблична частина "Контакти"

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
