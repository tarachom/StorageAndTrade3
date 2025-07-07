/*

Побудова сторінки звіту

*/

using Gtk;
using InterfaceGtk3;
using GeneratedCode;
using AccountingSoftware;
using GeneratedCode.Довідники;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace StorageAndTrade
{
    class ЗвітСторінка : InterfaceGtk3.ЗвітСторінка
    {
        public ЗвітСторінка() : base(Config.Kernel) { }

        protected override void ВідкритиДокументВідповідноДоВиду(string name, UnigueID? unigueID, string keyForSetting = "", ФункціїДляДинамічногоВідкриття.TypeForm typeForm = ФункціїДляДинамічногоВідкриття.TypeForm.Journal)
        {
            new ФункціїДляДинамічногоВідкриття().ВідкритиДокументВідповідноДоВиду(name, unigueID, keyForSetting, typeForm);
        }

        protected override void ВідкритиДовідникВідповідноДоВиду(string name, UnigueID? unigueID, ФункціїДляДинамічногоВідкриття.TypeForm typeForm = ФункціїДляДинамічногоВідкриття.TypeForm.Journal)
        {
            new ФункціїДляДинамічногоВідкриття().ВідкритиДовідникВідповідноДоВиду(name, unigueID, typeForm);
        }

        protected override async ValueTask ВигрузитиВФайл_PDF(InterfaceGtk3.ЗвітСторінка звіт, (Dictionary<string, PDFColumnsSettings> Settings, List<string[]> Rows) settingsAndRows)
        {
            string Назва = "Звіт: " + звіт.ReportName + " / " + звіт.Caption;
            string ДодатковаІнформація = звіт.GetInfo != null ? await звіт.GetInfo() : "";

            PDFColumnsSettings[] settings = [.. settingsAndRows.Settings.Values];

            QuestPDF.Settings.License = LicenseType.Community;
            Document doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(10, QuestPDF.Infrastructure.Unit.Point);

                    page.Content().Column(x =>
                    {
                        //Назва
                        x.Item().Text(Назва).FontSize(8).Bold();

                        //Параметри
                        x.Item().Text(ДодатковаІнформація).FontSize(6).Bold();

                        //Пустий рядок
                        x.Item().Text("");

                        x.Item().Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                if (settings.Length > 0)
                                {
                                    foreach (var item in settings)
                                        if (item.Type == TypePDFColumn.Relative)
                                            cols.RelativeColumn(item.Width >= 1 ? item.Width : 1);
                                        else if (item.Type == TypePDFColumn.Constant)
                                            cols.ConstantColumn(item.Width);
                                }
                                else
                                    cols.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                if (settings.Length > 0)
                                    foreach (var item in settings)
                                        header.Cell().Border(1).Padding(2).Text(item.Caption).Bold().FontSize(6).AlignCenter();
                                else
                                    header.Cell().Border(1).Text("Для звіту не задані налаштування колонок").FontSize(6).AlignCenter();
                            });

                            foreach (string[] cols in settingsAndRows.Rows)
                                for (int i = 0; i < cols.Length; i++)
                                {
                                    PDFColumnsSettings settingsItem = settings[i];

                                    var cell = table.Cell().Border(1).Padding(1).Text(cols[i]).FontSize(6);

                                    if (settingsItem.Xalign == 0)
                                        cell.AlignLeft();
                                    else if (settingsItem.Xalign == 0.5f)
                                        cell.AlignCenter();
                                    else if (settingsItem.Xalign == 1f)
                                        cell.AlignRight();
                                }
                        });
                    });
                });
            });

            doc.GeneratePdfAndShow();
        }

        protected override async ValueTask ВигрузитиВФайл_Excel(InterfaceGtk3.ЗвітСторінка звіт, (Dictionary<string, ExcelColumnsSettings> Settings, List<string[]> Rows) settingsAndRows)
        {
            string currentFolder = "";

            FileChooserDialog fc = new FileChooserDialog("Виберіть каталог", Program.GeneralForm,
                FileChooserAction.SelectFolder, "Закрити", ResponseType.Cancel, "Вибрати", ResponseType.Accept);

            if (fc.Run() == (int)ResponseType.Accept)
                currentFolder = fc.CurrentFolder;

            fc.Dispose();
            fc.Destroy();

            if (!string.IsNullOrEmpty(currentFolder) && Directory.Exists(currentFolder))
            {
                string reportFileName = звіт.ReportName + "_" + звіт.Caption;
                string extension = ".xlsx";
                string fullPath = "";

                //Підбір назви файлу на випадок наявності вже такого файлу
                //До назви добавляється номер версії. Максимально 100
                {
                    for (int i = 0; i < 100; i++)
                    {
                        string version = i != 0 ? "_" + i.ToString() : "";
                        string tmpFullPath = System.IO.Path.Combine(currentFolder, reportFileName + version + extension);

                        if (!File.Exists(tmpFullPath))
                        {
                            fullPath = tmpFullPath;
                            break;
                        }
                    }

                    //Якщо невдалось підібрати назву файлу, тоді випадкова назва
                    if (string.IsNullOrEmpty(fullPath))
                        fullPath = System.IO.Path.Combine(currentFolder, reportFileName + Guid.NewGuid().ToString().Replace("-", "") + extension);
                }

                await Task.Run(async () => await ВигрузкаВЕксель(звіт, settingsAndRows, fullPath));

                ФункціїДляПовідомлень.ДодатиІнформаційнеПовідомлення(null, "Вигрузка звіту в Excel", $"Вигружено у файл: {fullPath}");
            }
        }

        async ValueTask ВигрузкаВЕксель(InterfaceGtk3.ЗвітСторінка звіт, (Dictionary<string, ExcelColumnsSettings> Settings, List<string[]> Rows) settingsAndRows, string fullPath)
        {
            static void CreateCell(IRow CurrentRow, int CellIndex, string Value, XSSFCellStyle Style)
            {
                ICell Cell = CurrentRow.CreateCell(CellIndex);
                Cell.SetCellValue(Value);
                Cell.CellStyle = Style;
            }

            static void CreateNumberCell(IRow CurrentRow, int CellIndex, double Value, XSSFCellStyle Style)
            {
                ICell Cell = CurrentRow.CreateCell(CellIndex);

                if (Value != 0)
                    Cell.SetCellValue(Value);
                else
                    Cell.SetBlank();

                Cell.CellStyle = Style;
            }

            static void CreateDateTimeCell(IRow CurrentRow, int CellIndex, DateTime Value, XSSFCellStyle Style)
            {
                ICell Cell = CurrentRow.CreateCell(CellIndex);

                if (Value != DateTime.MinValue)
                    Cell.SetCellValue(Value);
                else
                    Cell.SetBlank();

                Cell.CellStyle = Style;
            }

            ExcelColumnsSettings[] settings = [.. settingsAndRows.Settings.Values];

            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet(звіт.Caption.Length >= 30 ? звіт.Caption[..30] : звіт.Caption);

            string Назва = "Звіт: " + звіт.ReportName + " / " + звіт.Caption;
            string ДодатковаІнформація = звіт.GetInfo != null ? await звіт.GetInfo() : "";

            if (settings.Length > 0)
            {
                int currRow = 0;
                int rowAutoFilter;
                string[] nameCols = [.. settings.Select(x => x.Caption)];

                //Caption
                {
                    XSSFFont font = (XSSFFont)workbook.CreateFont();
                    font.FontHeightInPoints = 12;
                    font.FontName = "Arial";

                    XSSFCellStyle cellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    cellStyle.SetFont(font);
                    cellStyle.WrapText = true;

                    CreateCell(sheet.CreateRow(currRow++), 0, Назва, cellStyle);
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(currRow - 1, currRow - 1, 0, nameCols.Length - 1));
                    sheet.AutoSizeRow(currRow - 1);

                    CreateCell(sheet.CreateRow(currRow++), 0, ДодатковаІнформація, cellStyle);
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(currRow - 1, currRow - 1, 0, nameCols.Length - 1));
                    sheet.AutoSizeRow(currRow - 1);

                    CreateCell(sheet.CreateRow(currRow++), 0, "", cellStyle);
                }

                //Header
                {
                    XSSFFont font = (XSSFFont)workbook.CreateFont();
                    font.FontHeightInPoints = 11;
                    font.FontName = "Arial";
                    font.IsBold = true;

                    XSSFCellStyle cellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    cellStyle.WrapText = true;
                    cellStyle.SetFont(font);

                    cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Dashed;
                    cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Dashed;
                    cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Dashed;
                    cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Dashed;
                    cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

                    rowAutoFilter = currRow;
                    IRow row = sheet.CreateRow(currRow++);

                    for (int i = 0; i < nameCols.Length; i++)
                        CreateCell(row, i, nameCols[i], cellStyle);

                    //Закріпити заголовок
                    sheet.CreateFreezePane(0, currRow);
                    sheet.AutoSizeRow(currRow - 1);
                }

                //Body
                {
                    XSSFFont font = (XSSFFont)workbook.CreateFont();
                    font.FontHeightInPoints = 10;
                    font.FontName = "Arial";

                    XSSFCellStyle cellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                    cellStyle.WrapText = true;
                    cellStyle.SetFont(font);

                    XSSFCellStyle cellStyleDateTime = (XSSFCellStyle)workbook.CreateCellStyle();
                    cellStyleDateTime.DataFormat = workbook.CreateDataFormat().GetFormat("dd.MM.yyyy HH:mm:ss");
                    cellStyleDateTime.SetFont(font);

                    for (int r = 0; r < settingsAndRows.Rows.Count; r++)
                    {
                        IRow row = sheet.CreateRow(currRow++);

                        //sheet.GroupRow(0, currRow);

                        string[] Значення = settingsAndRows.Rows[r];
                        for (int i = 0; i < Значення.Length; i++)
                        {
                            ExcelColumnsSettings setting = settings[i];

                            switch (setting.Type.ToLower())
                            {
                                case "n":
                                case "number":
                                    {
                                        double value = double.TryParse(Значення[i], out double result) ? result : 0;
                                        CreateNumberCell(row, i, value, cellStyle);
                                        break;
                                    }
                                case "datetime":
                                    {
                                        DateTime value = DateTime.TryParse(Значення[i], out DateTime result) ? result : DateTime.MinValue;
                                        CreateDateTimeCell(row, i, value, cellStyleDateTime);
                                        break;
                                    }
                                default:
                                    {
                                        CreateCell(row, i, Значення[i], cellStyle);
                                        break;
                                    }
                            }
                        }

                        sheet.AutoSizeRow(currRow - 1);
                    }
                }

                if (settingsAndRows.Rows.Count > 0)
                    sheet.SetAutoFilter(new NPOI.SS.Util.CellRangeAddress(rowAutoFilter, rowAutoFilter + settingsAndRows.Rows.Count - 1, 0, nameCols.Length - 1));

                //Авторозмір для тих колонок для яких не заданий розмір
                for (int i = 0; i < nameCols.Length; i++)
                {
                    ExcelColumnsSettings setting = settings[i];
                    if (setting.Width > 0)
                        sheet.SetColumnWidth(i, (setting.Width > 255 ? 255 : setting.Width) * 256);
                    else
                        sheet.AutoSizeColumn(i);
                }

                GC.Collect();
            }
            else
                CreateCell(sheet.CreateRow(0), 0, "Для звіту не задані налаштування колонок", (XSSFCellStyle)workbook.CreateCellStyle());

            using FileStream stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
            workbook.Write(stream);
        }

        protected override void AppendToolbar(Toolbar toolbar)
        {
            ToolItem separator = [new Separator(Orientation.Horizontal)];
            toolbar.Add(separator);

            //Зберегти в довіднику "Збережені звіти"
            {
                string label = "Зберегти в довіднику \"Збережені звіти\"";

                ToolButton button = new ToolButton(new Gtk.Image(Stock.Save, IconSize.Menu), label) { TooltipText = label };
                toolbar.Add(button);
                button.Clicked += async (sender, args) =>
                {
                    button.Sensitive = false;

                    await Select();
                    await ЗберегтиЗвіт(this, FillList());
                };
            }

            //Довідник "Збережені звіти"
            {
                string label = "Відкрити довідник \"Збережені звіти\"";

                ToolButton button = new ToolButton(new Gtk.Image(Stock.GoForward, IconSize.Menu), label) { TooltipText = label };
                toolbar.Add(button);
                button.Clicked += async (sender, args) => await ВідкритиЗбереженіЗвіти();
            }
        }

        protected async ValueTask ВідкритиЗбереженіЗвіти()
        {
            ЗбереженіЗвіти page = new ЗбереженіЗвіти();
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Збережені звіти", () => page);
            await page.SetValue();
        }

        protected async ValueTask ЗберегтиЗвіт(InterfaceGtk3.ЗвітСторінка звіт, List<string[]> rows)
        {
            ЗбереженіЗвіти_Objest Новий = new ЗбереженіЗвіти_Objest();
            await Новий.New();

            Новий.Назва = звіт.ReportName + " - " + звіт.Caption;

            if (звіт.GetInfo != null)
                Новий.Інформація = await звіт.GetInfo();

            foreach (string[] cols in rows)
            {
                ЗбереженіЗвіти_ЗвітСторінка_TablePart.Record Рядок = new();

                for (int i = 0; i < cols.Length; i++)
                    switch (i)
                    {
                        case 0: Рядок.А = cols[i]; break;
                        case 1: Рядок.Б = cols[i]; break;
                        case 2: Рядок.В = cols[i]; break;
                        case 3: Рядок.Г = cols[i]; break;
                        case 4: Рядок.Ґ = cols[i]; break;
                        case 5: Рядок.Д = cols[i]; break;
                        case 6: Рядок.Е = cols[i]; break;
                        case 7: Рядок.Є = cols[i]; break;
                        case 8: Рядок.Ж = cols[i]; break;
                        case 9: Рядок.З = cols[i]; break;
                        case 10: Рядок.И = cols[i]; break;
                        case 11: Рядок.І = cols[i]; break;
                        case 12: Рядок.Ї = cols[i]; break;
                        case 13: Рядок.Й = cols[i]; break;
                        case 14: Рядок.К = cols[i]; break;
                        case 15: Рядок.Л = cols[i]; break;
                        case 16: Рядок.М = cols[i]; break;
                        case 17: Рядок.Н = cols[i]; break;
                        case 18: Рядок.О = cols[i]; break;
                        case 19: Рядок.П = cols[i]; break;
                        case 20: Рядок.Р = cols[i]; break;
                        case 21: Рядок.С = cols[i]; break;
                        case 22: Рядок.Т = cols[i]; break;
                        case 23: Рядок.У = cols[i]; break;
                        case 24: Рядок.Ф = cols[i]; break;
                        case 25: Рядок.Х = cols[i]; break;
                        case 26: Рядок.Ц = cols[i]; break;
                        case 27: Рядок.Ч = cols[i]; break;
                        case 28: Рядок.Ш = cols[i]; break;
                        case 29: Рядок.Щ = cols[i]; break;
                        case 30: Рядок.Ь = cols[i]; break;
                        case 31: Рядок.Ю = cols[i]; break;
                        case 32: Рядок.Я = cols[i]; break;
                    }

                Новий.ЗвітСторінка_TablePart.Records.Add(Рядок);
            }

            if (await Новий.Save())
                await Новий.ЗвітСторінка_TablePart.Save(false);
        }

    }
}