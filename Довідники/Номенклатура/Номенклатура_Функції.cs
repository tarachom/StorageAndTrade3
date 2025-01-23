
/*
        Номенклатура_Функції.cs
        Функції
*/

using InterfaceGtk;
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

        public static async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null,
            Action<UnigueID?>? сallBack_LoadRecords = null,
            Action<UnigueID>? сallBack_OnSelectPointer = null)
        {
            Номенклатура_Елемент page = new Номенклатура_Елемент
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

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page, false,
                async () => await page.Елемент.Lock(),
                async () => await page.Елемент.UnLock());

            page.SetValue();
        }

        public static async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            Номенклатура_Objest Обєкт = new Номенклатура_Objest();
            if (await Обєкт.Read(unigueID))
                await Обєкт.SetDeletionLabel(!Обєкт.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        public static async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            Номенклатура_Objest Обєкт = new Номенклатура_Objest();
            if (await Обєкт.Read(unigueID))
            {
                Номенклатура_Objest Новий = await Обєкт.Copy(true);
                await Новий.Save();

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
