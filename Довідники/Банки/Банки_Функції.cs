
/*
        Банки_Функції.cs
        Функції
*/

using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    static class Банки_Функції
    {
        public static List<Where> Відбори(string searchText)
        {
            return
            [
                
                //Код
                new Where(Банки_Const.Код, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //Назва
                new Where(Comparison.OR, Банки_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //КодМФО
                new Where(Comparison.OR, Банки_Const.КодМФО, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //ПовнаНазва
                new Where(Comparison.OR, Банки_Const.ПовнаНазва, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //УнікальнийКодБанку
                new Where(Comparison.OR, Банки_Const.УнікальнийКодБанку, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //Адреса
                new Where(Comparison.OR, Банки_Const.Адреса, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //КодНБУ
                new Where(Comparison.OR, Банки_Const.КодНБУ, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //НомерЛіцензії
                new Where(Comparison.OR, Банки_Const.НомерЛіцензії, Comparison.LIKE, searchText) { FuncToField = "LOWER" },

            ];
        }

        public static async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null,
            Action<UnigueID?>? сallBack_LoadRecords = null,
            Action<UnigueID>? сallBack_OnSelectPointer = null)
        {
            Банки_Елемент page = new Банки_Елемент
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

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);
            page.SetValue();
        }

        public static async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            Банки_Objest Обєкт = new Банки_Objest();
            if (await Обєкт.Read(unigueID, false, true))
                await Обєкт.SetDeletionLabel(!Обєкт.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        public static async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            Банки_Objest Обєкт = new Банки_Objest();
            if (await Обєкт.Read(unigueID))
            {
                Банки_Objest Новий = await Обєкт.Copy(true);
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
