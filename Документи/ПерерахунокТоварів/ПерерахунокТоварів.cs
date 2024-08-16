

/*     
        ПерерахунокТоварів.cs
        Список
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    public class ПерерахунокТоварів : ДокументЖурнал
    {
        public ПерерахунокТоварів() : base()
        {
            ТабличніСписки.ПерерахунокТоварів_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async void LoadRecords()
        {
            ТабличніСписки.ПерерахунокТоварів_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.ПерерахунокТоварів_Записи.DocumentPointerItem = DocumentPointerItem;

            await ТабличніСписки.ПерерахунокТоварів_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ПерерахунокТоварів_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПерерахунокТоварів_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.ПерерахунокТоварів_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПерерахунокТоварів_Записи.CurrentPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length < 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            //Назва
            ТабличніСписки.ПерерахунокТоварів_Записи.ДодатиВідбір(TreeViewGrid,
                new Where(ПерерахунокТоварів_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" }, true);

            await ТабличніСписки.ПерерахунокТоварів_Записи.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.ПерерахунокТоварів_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.ПерерахунокТоварів_Записи.FirstPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{ПерерахунокТоварів_Const.FULLNAME} *", () =>
                {
                    ПерерахунокТоварів_Елемент page = new ПерерахунокТоварів_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadRecords,
                        IsNew = true
                    };

                    page.SetValue();

                    return page;
                });
            }
            else if (unigueID != null)
            {
                ПерерахунокТоварів_Objest ПерерахунокТоварів_Objest = new ПерерахунокТоварів_Objest();
                if (await ПерерахунокТоварів_Objest.Read(unigueID))
                {
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook,$"{ПерерахунокТоварів_Objest.Назва}", () =>
                    {
                        ПерерахунокТоварів_Елемент page = new ПерерахунокТоварів_Елемент
                        {
                            UnigueID = unigueID,
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            ПерерахунокТоварів_Objest = ПерерахунокТоварів_Objest,
                        };

                        page.SetValue();

                        return page;
                    });
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            ПерерахунокТоварів_Objest ПерерахунокТоварів_Objest = new ПерерахунокТоварів_Objest();
            if (await ПерерахунокТоварів_Objest.Read(unigueID))
                await ПерерахунокТоварів_Objest.SetDeletionLabel(!ПерерахунокТоварів_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask<UnigueID?> Copy(UnigueID unigueID)
        {
            ПерерахунокТоварів_Objest ПерерахунокТоварів_Objest = new ПерерахунокТоварів_Objest();
            if (await ПерерахунокТоварів_Objest.Read(unigueID))
            {
                ПерерахунокТоварів_Objest ПерерахунокТоварів_Objest_Новий = await ПерерахунокТоварів_Objest.Copy(true);
                await ПерерахунокТоварів_Objest_Новий.Save();

                /* Таблична частина: Товари */
                await ПерерахунокТоварів_Objest_Новий.Товари_TablePart.Save(false);

                return ПерерахунокТоварів_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        protected override void PeriodWhereChanged()
        {
            ТабличніСписки.ПерерахунокТоварів_Записи.ДодатиВідбірПоПеріоду(TreeViewGrid, Enum.Parse<ПеріодДляЖурналу.ТипПеріоду>(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            ПерерахунокТоварів_Pointer ПерерахунокТоварів_Pointer = new ПерерахунокТоварів_Pointer(unigueID);
            ПерерахунокТоварів_Objest? ПерерахунокТоварів_Objest = await ПерерахунокТоварів_Pointer.GetDocumentObject(true);
            if (ПерерахунокТоварів_Objest == null) return;

            if (spendDoc)
            {
                if (!await ПерерахунокТоварів_Objest.SpendTheDocument(ПерерахунокТоварів_Objest.ДатаДок))
                    new ФункціїДляПовідомлень().ПоказатиПовідомлення(ПерерахунокТоварів_Objest.UnigueID);
            }
            else
                await ПерерахунокТоварів_Objest.ClearSpendTheDocument();
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new ПерерахунокТоварів_Pointer(unigueID);
        }

        protected override async void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{ПерерахунокТоварів_Const.FULLNAME}_{unigueID}.xml");
            await ПерерахунокТоварів_Export.ToXmlFile(new ПерерахунокТоварів_Pointer(unigueID), pathToSave);
        }

        #endregion
    }
}
