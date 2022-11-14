using Gtk;

using AccountingSoftware;
using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Довідники;

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
                    treeView.ButtonPressEvent += OnButtonPressEvent;

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

        void OnButtonPressEvent(object sender, ButtonPressEventArgs args)
        {
            if (args.Event.Type == Gdk.EventType.DoubleButtonPress)
            {
                TreeView treeView = (TreeView)sender;

                TreePath itemPath;
                TreeViewColumn treeColumn;

                treeView.GetCursor(out itemPath, out treeColumn);

                if (treeColumn.Data.ContainsKey("Column"))
                {
                    TreeIter iter;
                    treeView.Model.GetIter(out iter, itemPath);

                    int rowNumber = int.Parse(itemPath.ToString());

                    NameValue<int> valueDataColumn = (NameValue<int>)treeColumn.Data["Column"]!;
                    string uid = treeView.Model.GetValue(iter, valueDataColumn.Value).ToString()!;

                    switch (valueDataColumn.Name)
                    {
                        case "Номенклатура_Назва":
                            {
                                Program.GeneralForm?.CreateNotebookPage("Довідник: Номенклатура", () =>
                                {
                                    Номенклатура page = new Номенклатура(true);
                                    page.SelectPointerItem = new Номенклатура_Pointer(new UnigueID(uid));
                                    page.LoadTree();

                                    return page;
                                });

                                break;
                            }

                            // case Columns.ХарактеристикаНазва:
                            //     {
                            //         Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Характеристика", () =>
                            //         {
                            //             ХарактеристикиНоменклатури page = new ХарактеристикиНоменклатури(true);

                            //             page.НоменклатураВласник.Pointer = запис.Номенклатура;
                            //             page.DirectoryPointerItem = запис.Характеристика;
                            //             page.CallBack_OnSelectPointer = (ХарактеристикиНоменклатури_Pointer selectPointer) =>
                            //             {
                            //                 запис.Характеристика = selectPointer;
                            //                 Запис.ПісляЗміни_Характеристика(запис);

                            //                 Store.SetValues(iter, запис.ToArray());
                            //             };

                            //             page.LoadRecords();

                            //             return page;
                            //         });

                            //         break;
                            //     }
                            // case Columns.СеріяНазва:
                            //     {
                            //         Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Серія", () =>
                            //         {
                            //             СеріїНоменклатури page = new СеріїНоменклатури(true);

                            //             page.DirectoryPointerItem = запис.Серія;
                            //             page.CallBack_OnSelectPointer = (СеріїНоменклатури_Pointer selectPointer) =>
                            //             {
                            //                 запис.Серія = selectPointer;
                            //                 Запис.ПісляЗміни_Серія(запис);

                            //                 Store.SetValues(iter, запис.ToArray());
                            //             };

                            //             page.LoadRecords();

                            //             return page;
                            //         });

                            //         break;
                            //     }
                            // case Columns.ПакуванняНазва:
                            //     {
                            //         Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Пакування", () =>
                            //         {
                            //             ПакуванняОдиниціВиміру page = new ПакуванняОдиниціВиміру(true);

                            //             page.DirectoryPointerItem = запис.Пакування;
                            //             page.CallBack_OnSelectPointer = (ПакуванняОдиниціВиміру_Pointer selectPointer) =>
                            //             {
                            //                 запис.Пакування = selectPointer;
                            //                 Запис.ПісляЗміни_Пакування(запис);

                            //                 Store.SetValues(iter, запис.ToArray());
                            //             };

                            //             page.LoadRecords();

                            //             return page;
                            //         });

                            //         break;
                            //     }
                            // case Columns.СкладНазва:
                            //     {
                            //         Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Склад", () =>
                            //         {
                            //             Склади page = new Склади(true);

                            //             page.DirectoryPointerItem = запис.Склад;
                            //             page.CallBack_OnSelectPointer = (Склади_Pointer selectPointer) =>
                            //             {
                            //                 запис.Склад = selectPointer;
                            //                 Запис.ПісляЗміни_Склад(запис);

                            //                 Store.SetValues(iter, запис.ToArray());
                            //             };

                            //             page.LoadRecords();

                            //             return page;
                            //         });

                            //         break;
                            //     }
                    }
                }
            }
        }
    }
}