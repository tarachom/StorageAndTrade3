

/*
        АктВиконанихРобіт_Функції.cs
        Функції
*/

using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Документи;

namespace StorageAndTrade
{
    static class АктВиконанихРобіт_Функції
    {
        public static List<Where> Відбори(string searchText)
        {
            return
            [
                
                //Назва
                new Where(АктВиконанихРобіт_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //Коментар
                new Where(Comparison.OR, АктВиконанихРобіт_Const.Коментар, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //КлючовіСловаДляПошуку
                new Where(Comparison.OR, АктВиконанихРобіт_Const.КлючовіСловаДляПошуку, Comparison.LIKE, searchText) { FuncToField = "LOWER" },

            ];
        }

        public static async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null,
            Action<UnigueID?>? сallBack_LoadRecords = null)
        {
            АктВиконанихРобіт_Елемент page = new АктВиконанихРобіт_Елемент
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
            await NotebookFunction.AddLockObjectFunc(Program.GeneralNotebook, page.Name, page.Елемент);
            await page.LockInfo(page.Елемент);
            page.SetValue();
        }

        public static async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            АктВиконанихРобіт_Pointer Вказівник = new(unigueID);
            bool? label = await Вказівник.GetDeletionLabel();
            if (label.HasValue) await Вказівник.SetDeletionLabel(!label.Value);
        }

        public static async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            АктВиконанихРобіт_Objest Обєкт = new АктВиконанихРобіт_Objest();
            if (await Обєкт.Read(unigueID))
            {
                АктВиконанихРобіт_Objest Новий = await Обєкт.Copy(true);
                await Новий.Save();

                await Новий.Послуги_TablePart.Save(false); // Таблична частина "Послуги"

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
