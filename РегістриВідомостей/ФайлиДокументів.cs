

/*     
        ФайлиДокументів.cs
        Список

        Табличний список - Записи
*/

using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.РегістриВідомостей;
using ТабличніСписки = GeneratedCode.РегістриВідомостей.ТабличніСписки;

namespace StorageAndTrade.РегістриВідомостей
{
    public class ФайлиДокументів : РегістриВідомостейЖурнал
    {
        public ФайлиДокументів() : base()
        {
            ТабличніСписки.ФайлиДокументів_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.ФайлиДокументів_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.ФайлиДокументів_Записи.LoadRecords(TreeViewGrid, SelectPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.ФайлиДокументів_Записи.ОчиститиВідбір(TreeViewGrid);

            //period
            ТабличніСписки.ФайлиДокументів_Записи.ДодатиВідбір(TreeViewGrid,
                new Where("period", Comparison.LIKE, searchText) { FuncToField = "to_char", FuncToField_Param1 = "'DD.MM.YYYY'" });

            await ТабличніСписки.ФайлиДокументів_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UniqueID? uniqueID = null)
        {
            // ФайлиДокументів_Елемент page = new ФайлиДокументів_Елемент
            // {
            //     CallBack_LoadRecords = CallBack_LoadRecords,
            //     IsNew = IsNew
            // };

            // if (IsNew)
            //     await page.Елемент.New();
            // else if (uniqueID == null || !await page.Елемент.Read(uniqueID))
            // {
            //     Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            //     return;
            // }

            // NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);
            // page.SetValue();
            await ValueTask.FromResult(true);
        }
        protected override async ValueTask Delete(UniqueID uniqueID)
        {
            ФайлиДокументів_Objest Обєкт = new ФайлиДокументів_Objest();
            if (await Обєкт.Read(uniqueID))
                await Обєкт.Delete();
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UniqueID?> Copy(UniqueID uniqueID)
        {
            ФайлиДокументів_Objest Обєкт = new ФайлиДокументів_Objest();
            if (await Обєкт.Read(uniqueID))
            {
                ФайлиДокументів_Objest Новий = Обєкт.Copy();
                await Новий.Save();
                return Новий.UniqueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "РегістриВідомостей.ФайлиДокументів";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await BeforeLoadRecords();
        }

        #endregion
    }
}
