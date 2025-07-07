
/*
        ХарактеристикиНоменклатури_Функції.cs
        Функції
*/

using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    static class ХарактеристикиНоменклатури_Функції
    {
        public static List<Where> Відбори(string searchText)
        {
            return
            [
                
                //Назва
                new Where(ХарактеристикиНоменклатури_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //Код
                new Where(Comparison.OR, ХарактеристикиНоменклатури_Const.Код, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //НазваПовна
                new Where(Comparison.OR, ХарактеристикиНоменклатури_Const.НазваПовна, Comparison.LIKE, searchText) { FuncToField = "LOWER" },

            ];
        }

        public static async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null,
            Action<UnigueID?>? сallBack_LoadRecords = null,
            Action<UnigueID>? сallBack_OnSelectPointer = null,
            Номенклатура_Pointer? Власник = null)
        {
            ХарактеристикиНоменклатури_Елемент page = new ХарактеристикиНоменклатури_Елемент
            {
                IsNew = IsNew,
                CallBack_LoadRecords = сallBack_LoadRecords,
                CallBack_OnSelectPointer = сallBack_OnSelectPointer
            };

            if (IsNew)
            {
                await page.Елемент.New();

                if (Власник != null)
                    page.ВласникДляНового = Власник;

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
            ХарактеристикиНоменклатури_Pointer Вказівник = new(unigueID);
            bool? label = await Вказівник.GetDeletionLabel();
            if (label.HasValue) await Вказівник.SetDeletionLabel(!label.Value);
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
