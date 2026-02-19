
/*
        СкладськіКомірки_Папки_Функції.cs
        Функції
*/

using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    static class СкладськіКомірки_Папки_Функції
    {
        public static List<Where> Відбори(string searchText)
        {
            return
            [
                
                //Код
                new Where(СкладськіКомірки_Папки_Const.Код, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //Назва
                new Where(Comparison.OR, СкладськіКомірки_Папки_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" },

            ];
        }

        public static async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null,
            Action<UnigueID?>? сallBack_LoadRecords = null,
            Action<UnigueID>? сallBack_OnSelectPointer = null,
            СкладськіПриміщення_Pointer? Власник = null
            )
        {
            СкладськіКомірки_Папки_Елемент page = new СкладськіКомірки_Папки_Елемент
            {
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
            СкладськіКомірки_Папки_Pointer Вказівник = new(unigueID);
            bool? label = await Вказівник.GetDeletionLabel();
            if (label.HasValue) await Вказівник.SetDeletionLabel(!label.Value);
        }

        public static async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            СкладськіКомірки_Папки_Objest Обєкт = new СкладськіКомірки_Папки_Objest();
            if (await Обєкт.Read(unigueID))
            {
                СкладськіКомірки_Папки_Objest Новий = await Обєкт.Copy(true);
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
