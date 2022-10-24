using Gtk;

using AccountingSoftware;
using Довідники = StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Валюти : VBox
    {
        public FormStorageAndTrade? GeneralForm { get; set; }

        TreeView ViewGrid;
        
        public Валюти() : base()
        {
            new VBox(false, 0);
            BorderWidth = 0;

            //Кнопки
            HBox hBoxBotton = new HBox();

            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { GeneralForm?.CloseCurrentPageNotebook(); };

            hBoxBotton.PackStart(bClose, false, false, 10);

            PackStart(hBoxBotton, false, false, 10);

            //Список
            Toolbar toolbar = new Toolbar();
            PackStart(toolbar, false, false, 0);

            ToolButton upButton = new ToolButton(Stock.Add) { Label = "Додати", IsImportant = true };
            //upButton.Clicked += OnAddClick;
            toolbar.Add(upButton);

            ToolButton refreshButton = new ToolButton(Stock.Refresh) { Label = "Обновити", IsImportant = true };
            refreshButton.Clicked += OnRefreshClick;
            toolbar.Add(refreshButton);

            ToolButton deleteButton = new ToolButton(Stock.Delete) { Label = "Видалити", IsImportant = true };
            //deleteButton.Clicked += OnDeleteClick;
            toolbar.Add(deleteButton);

            ToolButton copyButton = new ToolButton(Stock.Copy) { Label = "Копіювати", IsImportant = true };
            //copyButton.Clicked += OnCopyClick;
            toolbar.Add(copyButton);

            ScrolledWindow scroll = new ScrolledWindow() { ShadowType = ShadowType.In };
            scroll.SetPolicy(PolicyType.Never, PolicyType.Automatic);

            ViewGrid = new TreeView(Довідники.ТабличніСписки.Валюти_Записи.Store);
            ViewGrid.Selection.Mode = SelectionMode.Multiple;
            //ViewGrid.RowActivated += OnRowActivated;
            Довідники.ТабличніСписки.Валюти_Записи.AddColumns(ViewGrid);

            scroll.Add(ViewGrid);

            PackStart(scroll, true, true, 0);

            ShowAll();
        }

        public void LoadRecords()
        {
            Довідники.ТабличніСписки.Валюти_Записи.LoadRecords();
        }

        void OnRefreshClick(object? sender, EventArgs args)
        {
            Довідники.ТабличніСписки.Валюти_Записи.LoadRecords();
        }

        //void OnAddClick(object? sender, EventArgs args)
        // {
        //     if (ViewGrid.Selection.CountSelectedRows() != 0)
        //     {
        //         TreeIter iter;
        //         TreePath[] selectionRows = ViewGrid.Selection.GetSelectedRows();

        //         Console.WriteLine("--------------------------");

        //         foreach (TreePath itemPath in selectionRows)
        //         {
        //             ViewGrid.Model.GetIter(out iter, itemPath);
        //             string uid = (string)ViewGrid.Model.GetValue(iter, 1);

        //             Console.WriteLine(uid);
        //         }
        //     }
        // }

        // void OnCopyClick(object? sender, EventArgs args)
        // {
        //     if (ViewGrid.Selection.CountSelectedRows() != 0)
        //     {
        //         MessageDialog md = new MessageDialog(null, DialogFlags.DestroyWithParent, MessageType.Question, ButtonsType.YesNo, "Копіювати вибрані рядки?");
        //         ResponseType response = (ResponseType)md.Run();
        //         md.Destroy();

        //         if (response == ResponseType.Yes)
        //         {
        //             TreePath[] selectionRows = ViewGrid.Selection.GetSelectedRows();

        //             foreach (TreePath itemPath in selectionRows)
        //             {
        //                 TreeIter iter;
        //                 ViewGrid.Model.GetIter(out iter, itemPath);

        //                 string uid = (string)ViewGrid.Model.GetValue(iter, 1);

        //                 Довідники.Валюти_Objest валюти_Objest = new Довідники.Валюти_Objest();
        //                 if (валюти_Objest.Read(new UnigueID(uid)))
        //                 {
        //                     Довідники.Валюти_Objest валюти_Objest_Новий = валюти_Objest.Copy();
        //                     валюти_Objest_Новий.Назва = валюти_Objest_Новий.Назва + " - Копія";
        //                     валюти_Objest_Новий.Код = (++Константи.НумераціяДовідників.Валюти_Const).ToString("D6");
        //                     валюти_Objest_Новий.Save();
        //                 }
        //                 else
        //                 {
        //                     MessageDialog mdError = new MessageDialog(null, DialogFlags.DestroyWithParent, MessageType.Warning, ButtonsType.Close,
        //                         "Не вдалось прочитати!");

        //                     mdError.Run();
        //                     mdError.Destroy();
        //                     break;
        //                 }
        //             }

        //             LoadRecords();
        //         }
        //     }
        // }

        // void OnDeleteClick(object? sender, EventArgs args)
        // {
        //     if (ViewGrid.Selection.CountSelectedRows() != 0)
        //     {
        //         MessageDialog md = new MessageDialog(null, DialogFlags.DestroyWithParent, MessageType.Question, ButtonsType.YesNo, "Видалити вибрані рядки?");
        //         ResponseType response = (ResponseType)md.Run();
        //         md.Destroy();

        //         if (response == ResponseType.Yes)
        //         {
        //             TreePath[] selectionRows = ViewGrid.Selection.GetSelectedRows();

        //             foreach (TreePath itemPath in selectionRows)
        //             {
        //                 TreeIter iter;
        //                 ViewGrid.Model.GetIter(out iter, itemPath);

        //                 string uid = (string)ViewGrid.Model.GetValue(iter, 1);

        //                 Довідники.Валюти_Objest валюти_Objest = new Довідники.Валюти_Objest();
        //                 if (валюти_Objest.Read(new UnigueID(uid)))
        //                 {
        //                     валюти_Objest.Delete();
        //                 }
        //                 else
        //                 {
        //                     MessageDialog mdError = new MessageDialog(null, DialogFlags.DestroyWithParent, MessageType.Warning, ButtonsType.Close,
        //                         "Не вдалось прочитати!");

        //                     mdError.Run();
        //                     mdError.Destroy();
        //                     break;
        //                 }
        //             }

        //             LoadRecords();
        //         }
        //     }
        // }

        //void OnRowActivated(object sender, RowActivatedArgs args)
        //{
        // TreeIter iter;
        // TreeView treeView = (TreeView)sender;

        // if (treeView.Model.GetIter(out iter, args.Path))
        // {
        //     string row = (string)treeView.Model.GetValue(iter, 1);
        // }
        //}

        // public void LoadRecords()
        // {
        //     //Store.Clear();

        //     Довідники.Валюти_Select валюти_Select = new Довідники.Валюти_Select();
        //     валюти_Select.QuerySelect.Field.AddRange(
        //         new string[]
        //         {
        //             Довідники.Валюти_Const.Назва,
        //             Довідники.Валюти_Const.Код_R030,
        //             Довідники.Валюти_Const.Код,
        //             Довідники.Валюти_Const.КороткаНазва
        //         });

        //     //ORDER
        //     валюти_Select.QuerySelect.Order.Add(Довідники.Валюти_Const.Код, SelectOrder.ASC);

        //     валюти_Select.Select();
        //     while (валюти_Select.MoveNext())
        //     {
        //         Довідники.Валюти_Pointer? cur = валюти_Select.Current;

        //         if (cur != null)
        //             Store.AppendValues(new Записи
        //             {
        //                 ID = cur.UnigueID.ToString(),
        //                 Назва = cur.Fields?[Довідники.Валюти_Const.Назва].ToString() ?? "",
        //                 КороткаНазва = cur.Fields?[Довідники.Валюти_Const.КороткаНазва].ToString() ?? "",
        //                 Код = cur.Fields?[Довідники.Валюти_Const.Код].ToString() ?? "",
        //                 Код_R030 = cur.Fields?[Довідники.Валюти_Const.Код_R030].ToString() ?? ""
        //             }.ToArray());
        //     }
        // }


    }
}