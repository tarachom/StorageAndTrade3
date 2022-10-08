using Gtk;
using System;
using System.IO;
using System.ComponentModel;

using AccountingSoftware;
using Константи = StorageAndTrade_1_0.Константи;
using Довідники = StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Валюти : VBox
    {
        ListStore Store;
        TreeView ViewGrid;

        private class Записи
        {
            public string Icon = "doc_text_image.png";
            public string ID = "";
            public string Назва = "";
            public string КороткаНазва = "";
            public string Код_R030 = "";
            public string Код = "";

            public Array ToArray()
            {
                return new object[] { new Gdk.Pixbuf(GetIconByte()),
                ID, Назва, КороткаНазва,
                Код_R030, Код
            };
            }

            private byte[]? cashe_icon;
            private byte[] GetIconByte()
            {
                if (cashe_icon == null)
                {
                    string fileIco = "doc_text_image.png";
                    if (File.Exists(fileIco))
                        return File.ReadAllBytes(fileIco);
                    else
                        return new byte[] { };
                }
                else return cashe_icon;
            }

            public static ListStore CreateModel()
            {
                return new ListStore(typeof(Gdk.Pixbuf),
                   typeof(string), //ID
                   typeof(string), //Назва
                   typeof(string), //КороткаНазва
                   typeof(string), //R030
                   typeof(string)  //Код
                );
            }

            public static void AddColumns(TreeView treeView)
            {
                treeView.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));

                treeView.AppendColumn(new TreeViewColumn("ID", new CellRendererText(), "text", 1) { Visible = false });
                treeView.AppendColumn(new TreeViewColumn("Назва", new CellRendererText() { Editable = true }, "text", 2) { FixedWidth = 300 });
                treeView.AppendColumn(new TreeViewColumn("КороткаНазва", new CellRendererText(), "text", 3));
                treeView.AppendColumn(new TreeViewColumn("R030", new CellRendererText(), "text", 4));
                treeView.AppendColumn(new TreeViewColumn("Код", new CellRendererText(), "text", 5));
            }
        }

        public Валюти() : base()
        {
            new VBox(false, 0);
            BorderWidth = 0;

            Toolbar toolbar = new Toolbar();
            PackStart(toolbar, false, false, 0);

            ToolButton upButton = new ToolButton(Stock.Add) { Label = "Додати", IsImportant = true };
            upButton.Clicked += OnAddClick;
            toolbar.Add(upButton);

            ToolButton refreshButton = new ToolButton(Stock.Refresh) { Label = "Обновити", IsImportant = true };
            refreshButton.Clicked += OnRefreshClick;
            toolbar.Add(refreshButton);

            ToolButton deleteButton = new ToolButton(Stock.Delete) { Label = "Видалити", IsImportant = true };
            deleteButton.Clicked += OnDeleteClick;
            toolbar.Add(deleteButton);

            ToolButton copyButton = new ToolButton(Stock.Copy) { Label = "Копіювати", IsImportant = true };
            copyButton.Clicked += OnCopyClick;
            toolbar.Add(copyButton);

            Store = Записи.CreateModel();

            ScrolledWindow scroll = new ScrolledWindow() { ShadowType = ShadowType.In };
            scroll.SetPolicy(PolicyType.Never, PolicyType.Automatic);

            ViewGrid = new TreeView(Store);
            ViewGrid.Selection.Mode = SelectionMode.Multiple;
            ViewGrid.RowActivated += OnRowActivated;
            Записи.AddColumns(ViewGrid);

            scroll.Add(ViewGrid);

            PackStart(scroll, true, true, 0);

            //LoadRecords();

            ShowAll();
        }

        void OnRefreshClick(object? sender, EventArgs args)
        {
            LoadRecords();
        }

        void OnAddClick(object? sender, EventArgs args)
        {
            if (ViewGrid.Selection.CountSelectedRows() != 0)
            {
                TreeIter iter;
                TreePath[] selectionRows = ViewGrid.Selection.GetSelectedRows();

                Console.WriteLine("--------------------------");

                foreach (TreePath itemPath in selectionRows)
                {
                    ViewGrid.Model.GetIter(out iter, itemPath);
                    string uid = (string)ViewGrid.Model.GetValue(iter, 1);

                    Console.WriteLine(uid);
                }
            }
        }

        void OnCopyClick(object? sender, EventArgs args)
        {
            if (ViewGrid.Selection.CountSelectedRows() != 0)
            {
                MessageDialog md = new MessageDialog(null, DialogFlags.DestroyWithParent, MessageType.Question, ButtonsType.YesNo, "Копіювати вибрані рядки?");
                ResponseType response = (ResponseType)md.Run();
                md.Destroy();

                if (response == ResponseType.Yes)
                {
                    TreePath[] selectionRows = ViewGrid.Selection.GetSelectedRows();

                    foreach (TreePath itemPath in selectionRows)
                    {
                        TreeIter iter;
                        ViewGrid.Model.GetIter(out iter, itemPath);

                        string uid = (string)ViewGrid.Model.GetValue(iter, 1);

                        Довідники.Валюти_Objest валюти_Objest = new Довідники.Валюти_Objest();
                        if (валюти_Objest.Read(new UnigueID(uid)))
                        {
                            Довідники.Валюти_Objest валюти_Objest_Новий = валюти_Objest.Copy();
                            валюти_Objest_Новий.Назва = валюти_Objest_Новий.Назва + " - Копія";
                            валюти_Objest_Новий.Код = (++Константи.НумераціяДовідників.Валюти_Const).ToString("D6");
                            валюти_Objest_Новий.Save();
                        }
                        else
                        {
                            MessageDialog mdError = new MessageDialog(null, DialogFlags.DestroyWithParent, MessageType.Warning, ButtonsType.Close,
                                "Не вдалось прочитати!");

                            mdError.Run();
                            mdError.Destroy();
                            break;
                        }
                    }

                    LoadRecords();
                }
            }
        }

        void OnDeleteClick(object? sender, EventArgs args)
        {
            if (ViewGrid.Selection.CountSelectedRows() != 0)
            {
                MessageDialog md = new MessageDialog(null, DialogFlags.DestroyWithParent, MessageType.Question, ButtonsType.YesNo, "Видалити вибрані рядки?");
                ResponseType response = (ResponseType)md.Run();
                md.Destroy();

                if (response == ResponseType.Yes)
                {
                    TreePath[] selectionRows = ViewGrid.Selection.GetSelectedRows();

                    foreach (TreePath itemPath in selectionRows)
                    {
                        TreeIter iter;
                        ViewGrid.Model.GetIter(out iter, itemPath);

                        string uid = (string)ViewGrid.Model.GetValue(iter, 1);

                        Довідники.Валюти_Objest валюти_Objest = new Довідники.Валюти_Objest();
                        if (валюти_Objest.Read(new UnigueID(uid)))
                        {
                            валюти_Objest.Delete();
                        }
                        else
                        {
                            MessageDialog mdError = new MessageDialog(null, DialogFlags.DestroyWithParent, MessageType.Warning, ButtonsType.Close,
                                "Не вдалось прочитати!");

                            mdError.Run();
                            mdError.Destroy();
                            break;
                        }
                    }

                    LoadRecords();
                }
            }
        }

        void OnRowActivated(object sender, RowActivatedArgs args)
        {
            // TreeIter iter;
            // TreeView treeView = (TreeView)sender;

            // if (treeView.Model.GetIter(out iter, args.Path))
            // {
            //     string row = (string)treeView.Model.GetValue(iter, 1);
            // }
        }

        public void LoadRecords()
        {
            Store.Clear();

            Довідники.Валюти_Select валюти_Select = new Довідники.Валюти_Select();
            валюти_Select.QuerySelect.Field.AddRange(
                new string[]
                {
                Довідники.Валюти_Const.Назва,
                Довідники.Валюти_Const.Код_R030,
                Довідники.Валюти_Const.Код,
                Довідники.Валюти_Const.КороткаНазва
                });

            //ORDER
            валюти_Select.QuerySelect.Order.Add(Довідники.Валюти_Const.Код, SelectOrder.ASC);

            валюти_Select.Select();
            while (валюти_Select.MoveNext())
            {
                Довідники.Валюти_Pointer cur = валюти_Select.Current;

                Store.AppendValues(new Записи
                {
                    ID = cur.UnigueID.ToString(),
                    Назва = cur.Fields?[Довідники.Валюти_Const.Назва].ToString() ?? "",
                    КороткаНазва = cur.Fields?[Довідники.Валюти_Const.КороткаНазва].ToString() ?? "",
                    Код = cur.Fields?[Довідники.Валюти_Const.Код].ToString() ?? "",
                    Код_R030 = cur.Fields?[Довідники.Валюти_Const.Код_R030].ToString() ?? ""
                }.ToArray());
            }
        }


    }
}