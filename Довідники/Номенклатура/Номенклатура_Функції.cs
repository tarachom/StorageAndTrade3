
/*
        Номенклатура_Функції.cs
        Функції
*/

using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;
using GeneratedCode;

namespace StorageAndTrade
{
    static class Номенклатура_Функції
    {
        public static List<Where> Відбори(string searchText)
        {
            return
            [
                
                //Назва
                new Where(Номенклатура_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //Код
                new Where(Comparison.OR, Номенклатура_Const.Код, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //НазваПовна
                new Where(Comparison.OR, Номенклатура_Const.НазваПовна, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //Опис
                new Where(Comparison.OR, Номенклатура_Const.Опис, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //Артикул
                new Where(Comparison.OR, Номенклатура_Const.Артикул, Comparison.LIKE, searchText) { FuncToField = "LOWER" },

            ];
        }

        public static async ValueTask OpenPageElement(bool IsNew, UniqueID? uniqueID = null,
            Action<UniqueID?>? сallBack_LoadRecords = null,
            Action<UniqueID>? сallBack_OnSelectPointer = null)
        {
            Номенклатура_Елемент page = new Номенклатура_Елемент
            {
                CallBack_LoadRecords = сallBack_LoadRecords,
                CallBack_OnSelectPointer = сallBack_OnSelectPointer
            };

            if (IsNew)
            {
                await page.Елемент.New();

            }
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
            Номенклатура_Pointer Вказівник = new(uniqueID);
            bool? label = await Вказівник.GetDeletionLabel();
            if (label.HasValue) await Вказівник.SetDeletionLabel(!label.Value);
        }

        public static async ValueTask<UniqueID?> Copy(UniqueID uniqueID)
        {
            Номенклатура_Objest Обєкт = new Номенклатура_Objest();
            if (await Обєкт.Read(uniqueID))
            {
                Номенклатура_Objest Новий = await Обєкт.Copy(true);
                await Новий.Save();

                await Новий.Файли_TablePart.Save(false); // Таблична частина "Файли"

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
