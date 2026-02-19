
/*
        ЗбереженіЗвіти_Функції.cs
        Функції
*/

using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    static class ЗбереженіЗвіти_Функції
    {
        public static List<Where> Відбори(string searchText)
        {
            return
            [
                
                //Назва
                new Where(ЗбереженіЗвіти_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //Опис
                new Where(Comparison.OR, ЗбереженіЗвіти_Const.Опис, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
            ];
        }

        public static async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null, 
            Action<UnigueID?>? сallBack_LoadRecords = null, 
            Action<UnigueID>? сallBack_OnSelectPointer = null)
        {
            ЗбереженіЗвіти_Елемент page = new ЗбереженіЗвіти_Елемент
            {
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
            ЗбереженіЗвіти_Pointer Вказівник = new(unigueID);
            bool? label = await Вказівник.GetDeletionLabel();
            if (label.HasValue) await Вказівник.SetDeletionLabel(!label.Value);
        }

        public static async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ЗбереженіЗвіти_Objest Обєкт = new ЗбереженіЗвіти_Objest();
            if (await Обєкт.Read(unigueID))
            {
                ЗбереженіЗвіти_Objest Новий = await Обєкт.Copy(true);
                await Новий.Save();
                
                    await Новий.ЗвітСторінка_TablePart.Save(false); // Таблична частина "ЗвітСторінка"
                
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
    