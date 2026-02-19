

/*
        РеалізаціяТоварівТаПослуг_Функції.cs
        Функції
*/

using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Документи;

namespace StorageAndTrade
{
    static class РеалізаціяТоварівТаПослуг_Функції
    {
        public static List<Where> Відбори(string searchText)
        {
            return
            [
                
                //Назва
                new Where(РеалізаціяТоварівТаПослуг_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //Коментар
                new Where(Comparison.OR, РеалізаціяТоварівТаПослуг_Const.Коментар, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //КлючовіСловаДляПошуку
                new Where(Comparison.OR, РеалізаціяТоварівТаПослуг_Const.КлючовіСловаДляПошуку, Comparison.LIKE, searchText) { FuncToField = "LOWER" },

            ];
        }

        public static async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null,
            Action<UnigueID?>? сallBack_LoadRecords = null)
        {
            РеалізаціяТоварівТаПослуг_Елемент page = new РеалізаціяТоварівТаПослуг_Елемент
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
            РеалізаціяТоварівТаПослуг_Pointer Вказівник = new(unigueID);
            bool? label = await Вказівник.GetDeletionLabel();
            if (label.HasValue) await Вказівник.SetDeletionLabel(!label.Value);
        }

        public static async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            РеалізаціяТоварівТаПослуг_Objest Обєкт = new РеалізаціяТоварівТаПослуг_Objest();
            if (await Обєкт.Read(unigueID))
            {
                РеалізаціяТоварівТаПослуг_Objest Новий = await Обєкт.Copy(true);
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
