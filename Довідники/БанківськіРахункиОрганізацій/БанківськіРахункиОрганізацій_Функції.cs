
/*
        БанківськіРахункиОрганізацій_Функції.cs
        Функції
*/

using InterfaceGtk;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    static class БанківськіРахункиОрганізацій_Функції
    {
        public static List<Where> Відбори(string searchText)
        {
            return
            [
                
                //Код
                new Where(БанківськіРахункиОрганізацій_Const.Код, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //Назва
                new Where(Comparison.OR, БанківськіРахункиОрганізацій_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" },

            ];
        }

        public static async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null,
            Action<UnigueID?>? сallBack_LoadRecords = null,
            Action<UnigueID>? сallBack_OnSelectPointer = null)
        {
            БанківськіРахункиОрганізацій_Елемент page = new БанківськіРахункиОрганізацій_Елемент
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
            БанківськіРахункиОрганізацій_Objest Обєкт = new БанківськіРахункиОрганізацій_Objest();
            if (await Обєкт.Read(unigueID))
                await Обєкт.SetDeletionLabel(!Обєкт.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        public static async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            БанківськіРахункиОрганізацій_Objest Обєкт = new БанківськіРахункиОрганізацій_Objest();
            if (await Обєкт.Read(unigueID))
            {
                БанківськіРахункиОрганізацій_Objest Новий = await Обєкт.Copy(true);
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
