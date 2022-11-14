using Gtk;

using AccountingSoftware;
using StorageAndTrade_1_0;

namespace StorageAndTrade
{
    class Звіт_РухДокументівПоРегістрах : VBox
    {
        public Звіт_РухДокументівПоРегістрах() : base()
        {
            //Кнопки
            HBox hBoxBotton = new HBox();

            Button bClose = new Button("Закрити");
            bClose.Clicked += (object? sender, EventArgs args) => { Program.GeneralForm?.CloseCurrentPageNotebook(); };

            hBoxBotton.PackStart(bClose, false, false, 10);

            PackStart(hBoxBotton, false, false, 10);

            ShowAll();
        }

        public void CreateReport(DocumentPointer ДокументВказівник)
        {
            List<string> allowRegisterAccumulation = Config.Kernel!.Conf.Documents[ДокументВказівник.TypeDocument].AllowRegisterAccumulation;

            Dictionary<string, object> paramQuery = new Dictionary<string, object>();
            paramQuery.Add("ДокументВказівник", ДокументВказівник.UnigueID.UGuid);

            foreach (string regAccumName in allowRegisterAccumulation)
            {
                bool exist = false;
                string blockCaption = "";

                string[] columnsName = new string[] { };
                List<Dictionary<string, object>> listRow = new List<Dictionary<string, object>>();

                Dictionary<string, string> visibleColumn = new Dictionary<string, string>();
                Dictionary<string, string> dataColumn = new Dictionary<string, string>();

                switch (regAccumName)
                {
                    case "ТовариНаСкладах":
                        {
                            exist = true;
                            blockCaption = "Товари на складах";

                            visibleColumn = РухДокументівПоРегістрах.ТовариНаСкладах_ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.ТовариНаСкладах_КолонкиДаних();

                            Config.Kernel.DataBase.SelectRequest(РухДокументівПоРегістрах.ТовариНаСкладах_Запит, paramQuery, out columnsName, out listRow);

                            break;
                        }
                    case "ПартіїТоварів":
                        {
                            exist = true;
                            blockCaption = "Партії товарів";

                            visibleColumn = РухДокументівПоРегістрах.ПартіїТоварів_ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.ПартіїТоварів_КолонкиДаних();

                            Config.Kernel.DataBase.SelectRequest(РухДокументівПоРегістрах.ПартіїТоварів_Запит, paramQuery, out columnsName, out listRow);

                            break;
                        }
                    case "РухТоварів":
                        {
                            exist = true;
                            blockCaption = "Рух товарів";

                            visibleColumn = РухДокументівПоРегістрах.РухТоварів_ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.РухТоварів_КолонкиДаних();

                            Config.Kernel.DataBase.SelectRequest(РухДокументівПоРегістрах.РухТоварів_Запит, paramQuery, out columnsName, out listRow);

                            break;
                        }
                    case "ЗамовленняКлієнтів":
                        {
                            exist = true;
                            blockCaption = "Замовлення клієнтів";

                            visibleColumn = РухДокументівПоРегістрах.ЗамовленняКлієнтів_ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.ЗамовленняКлієнтів_КолонкиДаних();

                            Config.Kernel.DataBase.SelectRequest(РухДокументівПоРегістрах.ЗамовленняКлієнтів_Запит, paramQuery, out columnsName, out listRow);

                            break;
                        }
                    case "РозрахункиЗКлієнтами":
                        {
                            exist = true;
                            blockCaption = "Розрахунки з клієнтами";

                            visibleColumn = РухДокументівПоРегістрах.РозрахункиЗКлієнтами_ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.РозрахункиЗКлієнтами_КолонкиДаних();

                            Config.Kernel.DataBase.SelectRequest(РухДокументівПоРегістрах.РозрахункиЗКлієнтами_Запит, paramQuery, out columnsName, out listRow);

                            break;
                        }
                    case "ВільніЗалишки":
                        {
                            exist = true;
                            blockCaption = "Вільні залишки";

                            visibleColumn = РухДокументівПоРегістрах.ВільніЗалишки_ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.ВільніЗалишки_КолонкиДаних();

                            Config.Kernel.DataBase.SelectRequest(РухДокументівПоРегістрах.ВільніЗалишки_Запит, paramQuery, out columnsName, out listRow);

                            break;
                        }
                    case "ЗамовленняПостачальникам":
                        {
                            exist = true;
                            blockCaption = "Замовлення постачальникам";

                            visibleColumn = РухДокументівПоРегістрах.ЗамовленняПостачальникам_ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.ЗамовленняПостачальникам_КолонкиДаних();

                            Config.Kernel.DataBase.SelectRequest(РухДокументівПоРегістрах.ЗамовленняПостачальникам_Запит, paramQuery, out columnsName, out listRow);

                            break;
                        }
                    case "РозрахункиЗПостачальниками":
                        {
                            exist = true;
                            blockCaption = "Розрахунки з постачальниками";

                            visibleColumn = РухДокументівПоРегістрах.РозрахункиЗПостачальниками_ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.РозрахункиЗПостачальниками_КолонкиДаних();

                            Config.Kernel.DataBase.SelectRequest(РухДокументівПоРегістрах.РозрахункиЗПостачальниками_Запит, paramQuery, out columnsName, out listRow);

                            break;
                        }
                    case "ТовариДоПоступлення":
                        {
                            exist = true;
                            blockCaption = "Товари до поступлення";

                            visibleColumn = РухДокументівПоРегістрах.ТовариДоПоступлення_ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.ТовариДоПоступлення_КолонкиДаних();

                            Config.Kernel.DataBase.SelectRequest(РухДокументівПоРегістрах.ТовариДоПоступлення_Запит, paramQuery, out columnsName, out listRow);

                            break;
                        }
                    case "РухКоштів":
                        {
                            exist = true;
                            blockCaption = "Рух коштів";

                            visibleColumn = РухДокументівПоРегістрах.РухКоштів_ВидиміКолонки();
                            dataColumn = РухДокументівПоРегістрах.РухКоштів_КолонкиДаних();

                            Config.Kernel.DataBase.SelectRequest(РухДокументівПоРегістрах.РухКоштів_Запит, paramQuery, out columnsName, out listRow);

                            break;
                        }
                    default:
                        {
                            exist = false;
                            break;
                        }
                }

                if (exist)
                {
                    //Store
                    Type[] type = new Type[columnsName.Length];
                    for (int i = 0; i < columnsName.Length; i++)
                        type[i] = typeof(string);

                    ListStore store = new ListStore(type);

                    //Tree
                    TreeView treeView = new TreeView(store);
                    treeView.ButtonPressEvent += ФункціїДляЗвітів.OpenPageDirectoryOrDocument;

                    //Columns
                    for (int i = 0; i < columnsName.Length; i++)
                    {
                        string columnName = columnsName[i];
                        bool isVisible = visibleColumn.ContainsKey(columnName);

                        TreeViewColumn treeColumn = new TreeViewColumn(isVisible ? visibleColumn[columnName] : "", new CellRendererText() { Ypad = 4, Xpad = 8 }, "text", i);
                        treeColumn.Visible = isVisible;

                        if (dataColumn.ContainsKey(columnName))
                        {
                            string dataColumName = dataColumn[columnName];
                            for (int j = 0; j < columnsName.Length; j++)
                            {
                                if (dataColumName == columnsName[j])
                                {
                                    treeColumn.Data.Add("Column", new NameValue<int>(columnName, j));
                                    break;
                                }
                            }
                        }

                        treeView.AppendColumn(treeColumn);
                    }

                    //Data
                    foreach (Dictionary<string, object> row in listRow)
                    {
                        string[] value = new string[columnsName.Length];

                        for (int i = 0; i < columnsName.Length; i++)
                        {
                            string column = columnsName[i];

                            if (column == "income")
                                value[i] = (bool)row[column] ? "+" : "-";
                            else
                                value[i] = row[column]?.ToString() ?? "";
                        }

                        store.AppendValues(value);
                    }

                    WriteBlock(blockCaption, treeView);
                }
            }
        }

        void WriteBlock(string blockName, TreeView treeView)
        {
            VBox vBox = new VBox();

            Expander expander = new Expander(blockName) { Expanded = true };
            expander.Add(vBox);

            vBox.PackStart(treeView, false, false, 10);

            PackStart(expander, false, false, 10);

            ShowAll();
        }

    }
}