
/*
        Організації_Функції.cs
        Функції
*/

using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    static class Організації_Функції
    {
        public static List<Where> Відбори(string searchText)
        {
            return
            [
                
                //Назва
                new Where(Організації_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //Код
                new Where(Comparison.OR, Організації_Const.Код, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //НазваСкорочена
                new Where(Comparison.OR, Організації_Const.НазваСкорочена, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //СвідоцтвоСеріяНомер
                new Where(Comparison.OR, Організації_Const.СвідоцтвоСеріяНомер, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //СвідоцтвоДатаВидачі
                new Where(Comparison.OR, Організації_Const.СвідоцтвоДатаВидачі, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        
                //КлючовіСловаДляПошуку
                new Where(Comparison.OR, Організації_Const.КлючовіСловаДляПошуку, Comparison.LIKE, searchText) { FuncToField = "LOWER" },

            ];
        }

        public static async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null,
            Action<UnigueID?>? сallBack_LoadRecords = null,
            Action<UnigueID>? сallBack_OnSelectPointer = null)
        {
            Організації_Елемент page = new Організації_Елемент
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
            await NotebookFunction.AddLockObjectFunc(Program.GeneralNotebook, page.Name, page.Елемент);
            await page.LockInfo(page.Елемент);
            page.SetValue();
        }

        public static async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            Організації_Pointer Вказівник = new(unigueID);
            bool? label = await Вказівник.GetDeletionLabel();
            if (label.HasValue) await Вказівник.SetDeletionLabel(!label.Value);
        }

        public static async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            Організації_Objest Обєкт = new Організації_Objest();
            if (await Обєкт.Read(unigueID))
            {
                Організації_Objest Новий = await Обєкт.Copy(true);
                await Новий.Save();

                await Новий.Контакти_TablePart.Save(false); // Таблична частина "Контакти"

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
