
/*
        ХарактеристикиНоменклатури_Функції.cs
        Функції
*/

using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    static class ХарактеристикиНоменклатури_Функції
    {
        public static List<Where> Відбори(string searchText)
        {
            return
            [
                //Код
                new Where(ХарактеристикиНоменклатури_Const.Код, Comparison.LIKE, searchText) { FuncToField = "LOWER" },

                //Назва
                new Where(Comparison.OR, ХарактеристикиНоменклатури_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
            ];
        }

        public static async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null, 
            Action<UnigueID?>? сallBack_LoadRecords = null, 
            Action<UnigueID>? сallBack_OnSelectPointer = null)
        {
            ХарактеристикиНоменклатури_Елемент page = new ХарактеристикиНоменклатури_Елемент
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
            ХарактеристикиНоменклатури_Objest Обєкт = new ХарактеристикиНоменклатури_Objest();
            if (await Обєкт.Read(unigueID))
                await Обєкт.SetDeletionLabel(!Обєкт.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        public static async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ХарактеристикиНоменклатури_Objest Обєкт = new ХарактеристикиНоменклатури_Objest();
            if (await Обєкт.Read(unigueID))
            {
                ХарактеристикиНоменклатури_Objest Новий = await Обєкт.Copy(true);
                await Новий.Save();
                
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
    