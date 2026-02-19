

/*
        ЧекККМ_Функції.cs
        Функції
*/

using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Документи;

namespace StorageAndTrade
{
    static class ЧекККМ_Функції
    {
        public static List<Where> Відбори(string searchText)
        {
            return
            [
                
                //Назва
                new Where(ЧекККМ_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "TO_CHAR", FuncToField_Param1 = "''" },
                        
            ];
        }

        public static async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null, 
            Action<UnigueID?>? сallBack_LoadRecords = null)
        {
            ЧекККМ_Елемент page = new ЧекККМ_Елемент
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
            ЧекККМ_Pointer Вказівник = new(unigueID);
            bool? label = await Вказівник.GetDeletionLabel();
            if (label.HasValue) await Вказівник.SetDeletionLabel(!label.Value);
        }

        public static async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ЧекККМ_Objest Обєкт = new ЧекККМ_Objest();
            if (await Обєкт.Read(unigueID))
            {
                ЧекККМ_Objest Новий = await Обєкт.Copy(true);
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
    