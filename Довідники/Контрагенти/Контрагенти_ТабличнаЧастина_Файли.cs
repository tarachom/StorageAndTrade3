/*
Copyright (C) 2019-2024 TARAKHOMYN YURIY IVANOVYCH
All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

/*
Автор:    Тарахомин Юрій Іванович
Адреса:   Україна, м. Львів
Сайт:     accounting.org.ua
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Контрагенти_ТабличнаЧастина_Файли : ДовідникТабличнаЧастина
    {
        public Контрагенти_Objest? Контрагенти_Objest { get; set; }

        #region Запис

        enum Columns
        {
            Image,
            UID,
            Файл
        }

        ListStore Store = new ListStore(
            typeof(Gdk.Pixbuf), /* Image */
            typeof(string),     //UID
            typeof(string)      //Файл
        );

        List<Запис> Записи = new List<Запис>();

        private class Запис
        {
            public string Image { get; set; } = AppContext.BaseDirectory + "images/doc.png";
            public Guid ID { get; set; } = Guid.Empty;
            public Файли_Pointer Файл { get; set; } = new Файли_Pointer();

            public object[] ToArray()
            {
                return
                [
                    new Gdk.Pixbuf(Image),
                    ID.ToString(),
                    Файл.Назва
                ];
            }

            public static Запис Clone(Запис запис)
            {
                return new Запис
                {
                    ID = Guid.Empty,
                    Файл = запис.Файл.Copy()
                };
            }

            public static async ValueTask ПісляЗміни_Файл(Запис запис)
            {
                await запис.Файл.GetPresentation();
            }
        }

        #endregion

        public Контрагенти_ТабличнаЧастина_Файли() : base()
        {
            TreeViewGrid.Model = Store;
            AddColumn();
        }

        public override async ValueTask LoadRecords()
        {
            Store.Clear();
            Записи.Clear();

            if (Контрагенти_Objest != null)
            {
                Query querySelect = Контрагенти_Objest.Файли_TablePart.QuerySelect;
                querySelect.Clear();

                //JOIN 1
                querySelect.FieldAndAlias.Add(
                    new NameValue<string>(Файли_Const.TABLE + "." + Файли_Const.Назва, "file_name"));
                querySelect.Joins.Add(
                    new Join(Файли_Const.TABLE, Контрагенти_Файли_TablePart.Файл, querySelect.Table));

                await Контрагенти_Objest.Файли_TablePart.Read();

                Dictionary<string, Dictionary<string, string>> join = Контрагенти_Objest.Файли_TablePart.JoinValue;

                foreach (Контрагенти_Файли_TablePart.Record record in Контрагенти_Objest.Файли_TablePart.Records)
                {
                    record.Файл.Назва = join[record.UID.ToString()]["file_name"];

                    Запис запис = new Запис
                    {
                        ID = record.UID,
                        Файл = record.Файл
                    };

                    Записи.Add(запис);
                    Store.AppendValues(запис.ToArray());
                }
            }
        }

        public override async ValueTask SaveRecords()
        {
            if (Контрагенти_Objest != null)
            {
                Контрагенти_Objest.Файли_TablePart.Records.Clear();

                foreach (Запис запис in Записи)
                {
                    Контрагенти_Файли_TablePart.Record record = new Контрагенти_Файли_TablePart.Record();
                    Контрагенти_Objest.Файли_TablePart.Records.Add(record);

                    record.UID = запис.ID;
                    record.Файл = запис.Файл;
                }

                await Контрагенти_Objest.Файли_TablePart.Save(true);
            }
        }

        #region TreeView

        void AddColumn()
        {
            TreeViewGrid.AppendColumn(new TreeViewColumn("", new CellRendererPixbuf(), "pixbuf", 0));

            //Файл
            {
                TreeViewColumn Файл = new TreeViewColumn("Файл", new CellRendererText(), "text", (int)Columns.Файл) { MinWidth = 300 };
                Файл.Data.Add("Column", Columns.Файл);

                TreeViewGrid.AppendColumn(Файл);
            }

            //Колонка пустишка для заповнення вільного простору
            TreeViewGrid.AppendColumn(new TreeViewColumn());
        }

        protected override async void ButtonSelect(TreeIter iter, int rowNumber, int colNumber, Popover popoverSmallSelect)
        {
            Запис запис = Записи[rowNumber];

            switch ((Columns)colNumber)
            {
                case Columns.Файл:
                    {
                        Файли_ШвидкийВибір page = new Файли_ШвидкийВибір
                        {
                            PopoverParent = popoverSmallSelect,
                            DirectoryPointerItem = запис.Файл.UnigueID,
                            CallBack_OnSelectPointer = async (UnigueID selectPointer) =>
                            {
                                запис.Файл = new Файли_Pointer(selectPointer);
                                await Запис.ПісляЗміни_Файл(запис);
                                Store.SetValues(iter, запис.ToArray());
                            }
                        };

                        popoverSmallSelect.Add(page);
                        popoverSmallSelect.ShowAll();

                        await page.LoadRecords();
                        break;
                    }
            }
        }

        protected override void ButtonPopupClear(TreeIter iter, int rowNumber, int colNumber)
        {
            Запис запис = Записи[rowNumber];

            switch ((Columns)colNumber)
            {
                case Columns.Файл:
                    {
                        запис.Файл.Clear();
                        break;
                    }
            }

            Store.SetValues(iter, запис.ToArray());
        }

        #endregion

        #region ToolBar

        protected override void AddRecord()
        {
            Запис запис = new Запис();
            Записи.Add(запис);

            TreeIter iter = Store.AppendValues(запис.ToArray());
            TreeViewGrid.SetCursor(Store.GetPath(iter), TreeViewGrid.Columns[0], false);
        }

        protected override void CopyRecord(int rowNumber)
        {
            Запис запис = Записи[rowNumber];

            Запис записНовий = Запис.Clone(запис);

            Записи.Add(записНовий);

            TreeIter iter = Store.AppendValues(записНовий.ToArray());
            TreeViewGrid.SetCursor(Store.GetPath(iter), TreeViewGrid.Columns[0], false);
        }

        protected override void DeleteRecord(TreeIter iter, int rowNumber)
        {
            Запис запис = Записи[rowNumber];

            Записи.Remove(запис);
            Store.Remove(ref iter);
        }

        #endregion

    }
}