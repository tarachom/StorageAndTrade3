
/*

        КурсиВалют.cs

*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using ТабличніСписки = GeneratedCode.РегістриВідомостей.ТабличніСписки;
using GeneratedCode.РегістриВідомостей;

namespace StorageAndTrade.РегістриВідомостей
{
    class КурсиВалют : РегістриВідомостейЖурнал
    {
        public Валюти_PointerControl ВалютаВласник = new Валюти_PointerControl();

        public КурсиВалют()
        {
            ТабличніСписки.КурсиВалют_Записи.AddColumns(TreeViewGrid);

            HBoxTop.PackStart(ВалютаВласник, false, false, 2);
            ВалютаВласник.AfterSelectFunc = async () =>
            {
                SelectPointerItem?.Clear();
                await BeforeLoadRecords();
            };
        }

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.КурсиВалют_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            if (!ВалютаВласник.Pointer.UniqueID.IsEmpty())
            {
                ТабличніСписки.КурсиВалют_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(КурсиВалют_Const.Валюта, Comparison.EQ, ВалютаВласник.Pointer.UniqueID.UGuid));
            }

            await ТабличніСписки.КурсиВалют_Записи.LoadRecords(TreeViewGrid, SelectPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.КурсиВалют_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!ВалютаВласник.Pointer.UniqueID.IsEmpty())
            {
                ТабличніСписки.КурсиВалют_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(КурсиВалют_Const.Валюта, Comparison.EQ, ВалютаВласник.Pointer.UniqueID.UGuid));
            }

            //period
            ТабличніСписки.КурсиВалют_Записи.ДодатиВідбір(TreeViewGrid,
                new Where("period", Comparison.LIKE, searchText)
                {
                    FuncToField = "to_char",
                    FuncToField_Param1 = "'DD.MM.YYYY'"
                }
            );

            await ТабличніСписки.КурсиВалют_Записи.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UniqueID? uniqueID = null)
        {
            КурсиВалют_Елемент page = new КурсиВалют_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords
            };

            if (IsNew)
                page.Елемент.New();
            else if (uniqueID == null || !await page.Елемент.Read(uniqueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return;
            }

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);

            page.SetValue();
        }

        protected override async ValueTask Delete(UniqueID uniqueID)
        {
            КурсиВалют_Objest КурсиВалют_Objest = new КурсиВалют_Objest();
            if (await КурсиВалют_Objest.Read(uniqueID))
                await КурсиВалют_Objest.Delete();
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UniqueID?> Copy(UniqueID uniqueID)
        {
            КурсиВалют_Objest КурсиВалют_Objest = new КурсиВалют_Objest();
            if (await КурсиВалют_Objest.Read(uniqueID))
            {
                КурсиВалют_Objest КурсиВалют_Objest_Новий = КурсиВалют_Objest.Copy();
                await КурсиВалют_Objest_Новий.Save();

                return КурсиВалют_Objest_Новий.UniqueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "РегістриВідомостей.КурсиВалют";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await BeforeLoadRecords();
        }
    }
}