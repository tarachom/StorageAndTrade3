
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

            if (!ВалютаВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.КурсиВалют_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(КурсиВалют_Const.Валюта, Comparison.EQ, ВалютаВласник.Pointer.UnigueID.UGuid));
            }

            await ТабличніСписки.КурсиВалют_Записи.LoadRecords(TreeViewGrid, SelectPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.КурсиВалют_Записи.ОчиститиВідбір(TreeViewGrid);

            if (!ВалютаВласник.Pointer.UnigueID.IsEmpty())
            {
                ТабличніСписки.КурсиВалют_Записи.ДодатиВідбір(TreeViewGrid,
                    new Where(КурсиВалют_Const.Валюта, Comparison.EQ, ВалютаВласник.Pointer.UnigueID.UGuid));
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

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            КурсиВалют_Елемент page = new КурсиВалют_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords
            };

            if (IsNew)
                page.Елемент.New();
            else if (unigueID == null || !await page.Елемент.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return;
            }

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () => page);

            page.SetValue();
        }

        protected override async ValueTask Delete(UnigueID unigueID)
        {
            КурсиВалют_Objest КурсиВалют_Objest = new КурсиВалют_Objest();
            if (await КурсиВалют_Objest.Read(unigueID))
                await КурсиВалют_Objest.Delete();
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            КурсиВалют_Objest КурсиВалют_Objest = new КурсиВалют_Objest();
            if (await КурсиВалют_Objest.Read(unigueID))
            {
                КурсиВалют_Objest КурсиВалют_Objest_Новий = КурсиВалют_Objest.Copy();
                await КурсиВалют_Objest_Новий.Save();

                return КурсиВалют_Objest_Новий.UnigueID;
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