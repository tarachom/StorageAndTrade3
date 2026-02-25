

/*
        ВведенняЗалишків_Функції.cs
        Функції
*/

using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Документи;

namespace StorageAndTrade
{
    static class ВведенняЗалишків_Функції
    {
        public static List<Where> Відбори(string searchText)
        {
            return
            [
                
                //Назва
                new Where(ВведенняЗалишків_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //Коментар
                new Where(Comparison.OR, ВведенняЗалишків_Const.Коментар, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //КлючовіСловаДляПошуку
                new Where(Comparison.OR, ВведенняЗалишків_Const.КлючовіСловаДляПошуку, Comparison.LIKE, searchText) { FuncToField = "LOWER" },

            ];
        }

        public static async ValueTask OpenPageElement(bool IsNew, UniqueID? uniqueID = null,
            Action<UniqueID?>? сallBack_LoadRecords = null)
        {
            ВведенняЗалишків_Елемент page = new ВведенняЗалишків_Елемент
            {
                CallBack_LoadRecords = сallBack_LoadRecords
            };

            if (IsNew)
                await page.Елемент.New();
            else if (uniqueID == null || !await page.Елемент.Read(uniqueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return;
            }

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);
            await NotebookFunction.AddLockObjectFunc(Program.GeneralNotebook, page.Name, page.Елемент);
            await page.LockInfo(page.Елемент);
            page.SetValue();
        }

        public static async ValueTask SetDeletionLabel(UniqueID uniqueID)
        {
            ВведенняЗалишків_Pointer Вказівник = new(uniqueID);
            bool? label = await Вказівник.GetDeletionLabel();
            if (label.HasValue) await Вказівник.SetDeletionLabel(!label.Value);
        }

        public static async ValueTask<UniqueID?> Copy(UniqueID uniqueID)
        {
            ВведенняЗалишків_Objest Обєкт = new ВведенняЗалишків_Objest();
            if (await Обєкт.Read(uniqueID))
            {
                ВведенняЗалишків_Objest Новий = await Обєкт.Copy(true);
                await Новий.Save();

                await Новий.Товари_TablePart.Save(false); // Таблична частина "Товари"

                await Новий.Каси_TablePart.Save(false); // Таблична частина "Каси"

                await Новий.БанківськіРахунки_TablePart.Save(false); // Таблична частина "БанківськіРахунки"

                await Новий.РозрахункиЗКонтрагентами_TablePart.Save(false); // Таблична частина "РозрахункиЗКонтрагентами"

                return Новий.UniqueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }
    }
}
