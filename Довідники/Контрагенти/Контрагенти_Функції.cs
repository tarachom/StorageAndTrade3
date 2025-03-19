
/*
        Контрагенти_Функції.cs
        Функції
*/

using InterfaceGtk;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    static class Контрагенти_Функції
    {
        public static List<Where> Відбори(string searchText)
        {
            return
            [
                
                //Назва
                new Where(Контрагенти_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //Код
                new Where(Comparison.OR, Контрагенти_Const.Код, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //НазваПовна
                new Where(Comparison.OR, Контрагенти_Const.НазваПовна, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //Опис
                new Where(Comparison.OR, Контрагенти_Const.Опис, Comparison.LIKE, searchText) { FuncToField = "LOWER" },

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
            {
                await page.Елемент.New();

            }
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
