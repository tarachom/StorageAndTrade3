
/*
        Контрагенти_Функції.cs
        Функції
*/

using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    static class Контрагенти_Функції
    {
        public static List<Where> Відбори(string searchText)
        {
            return
            [
                //Код
                new Where(Контрагенти_Const.Код, Comparison.LIKE, searchText) { FuncToField = "LOWER" },

                //Назва
                new Where(Comparison.OR, Контрагенти_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
            ];
        }

        public static async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null, 
            Action<UnigueID?>? сallBack_LoadRecords = null, 
            Action<UnigueID>? сallBack_OnSelectPointer = null)
        {
            Контрагенти_Елемент page = new Контрагенти_Елемент
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
            Контрагенти_Objest Обєкт = new Контрагенти_Objest();
            if (await Обєкт.Read(unigueID))
                await Обєкт.SetDeletionLabel(!Обєкт.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        public static async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            Контрагенти_Objest Обєкт = new Контрагенти_Objest();
            if (await Обєкт.Read(unigueID))
            {
                Контрагенти_Objest Новий = await Обєкт.Copy(true);
                await Новий.Save();
                
                    await Новий.Контакти_TablePart.Save(false); // Таблична частина "Контакти"
                
                    await Новий.Файли_TablePart.Save(false); // Таблична частина "Файли"
                
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
    