

/*
        ЗакриттяЗамовленняПостачальнику_Функції.cs
        Функції
*/

using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Документи;

namespace StorageAndTrade
{
    static class ЗакриттяЗамовленняПостачальнику_Функції
    {
        public static List<Where> Відбори(string searchText)
        {
            return
            [
                
                //Назва
                new Where(ЗакриттяЗамовленняПостачальнику_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //НомерДок
                new Where(Comparison.OR, ЗакриттяЗамовленняПостачальнику_Const.НомерДок, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //ДатаДок
                new Where(Comparison.OR, ЗакриттяЗамовленняПостачальнику_Const.ДатаДок, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //Коментар
                new Where(Comparison.OR, ЗакриттяЗамовленняПостачальнику_Const.Коментар, Comparison.LIKE, searchText) { FuncToField = "LOWER" },

            ];
        }

        public static async ValueTask OpenPageElement(bool IsNew, UniqueID? uniqueID = null,
            Action<UniqueID?>? сallBack_LoadRecords = null)
        {
            ЗакриттяЗамовленняПостачальнику_Елемент page = new ЗакриттяЗамовленняПостачальнику_Елемент
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
            ЗакриттяЗамовленняПостачальнику_Pointer Вказівник = new(uniqueID);
            bool? label = await Вказівник.GetDeletionLabel();
            if (label.HasValue) await Вказівник.SetDeletionLabel(!label.Value);
        }

        public static async ValueTask<UniqueID?> Copy(UniqueID uniqueID)
        {
            ЗакриттяЗамовленняПостачальнику_Objest Обєкт = new ЗакриттяЗамовленняПостачальнику_Objest();
            if (await Обєкт.Read(uniqueID))
            {
                ЗакриттяЗамовленняПостачальнику_Objest Новий = await Обєкт.Copy(true);
                await Новий.Save();

                await Новий.Товари_TablePart.Save(false); // Таблична частина "Товари"

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
