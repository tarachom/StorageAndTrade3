/*

Побудова сторінки звіту

*/

using Gtk;
using InterfaceGtk;
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
    class ЗвітСторінка : InterfaceGtk.ЗвітСторінка
    {
        public ЗвітСторінка() : base(Config.Kernel) { }

        protected override void ВідкритиДокументВідповідноДоВиду(string name, UnigueID? unigueID, string keyForSetting = "")
        {
            new ФункціїДляДинамічногоВідкриття().ВідкритиДокументВідповідноДоВиду(name, unigueID);
        }

        protected override void ВідкритиДовідникВідповідноДоВиду(string name, UnigueID? unigueID)
        {
            new ФункціїДляДинамічногоВідкриття().ВідкритиДовідникВідповідноДоВиду(name, unigueID);
        }

        protected override async ValueTask ВигрузитиВФайл_PDF(InterfaceGtk.ЗвітСторінка звіт, List<string[]> rows)
        {
            const float MarginPage = 10;

            //Назва та розмір колонок
            Dictionary<string, int> Columns = [];
            if (rows.Count > 0)
            {
                string[] НазвиКолонок = rows[0];
                int СереднійРозмірКолонки = (int)Math.Round((PageSizes.A4.Width - MarginPage * 2) / НазвиКолонок.Length, 0) - 1;

                foreach (var item in НазвиКолонок)
                    Columns.Add(item, СереднійРозмірКолонки);
            }

            string Назва = звіт.ReportName + " - " + звіт.Caption;
            string ДодатковаІнформація = звіт.GetInfo != null ? await звіт.GetInfo() : "";

            QuestPDF.Settings.License = LicenseType.Community;
            Document doc = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(MarginPage, QuestPDF.Infrastructure.Unit.Point);

                    page.Content().Column(x =>
                    {
                        //Назва
                        x.Item().Text(Назва).FontSize(10).Bold();

                        //Параметри
                        x.Item().Text(ДодатковаІнформація).FontSize(8).Bold();

                        x.Item().Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                foreach (var item in Columns.Values)
                                    cols.ConstantColumn(item);
                            });

                            foreach (string[] cols in rows)
                                for (int i = 0; i < cols.Length; i++)
                                {
                                    var cell = table.Cell().Border(1).Padding(1).Text(cols[i]).FontSize(6);
                                }
                        });
                    });
                });
            });

            doc.GeneratePdfAndShow();
        }

        protected override async ValueTask ВигрузитиВФайл_Excel(InterfaceGtk.ЗвітСторінка звіт, List<string[]> rows)
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
                        string version = i != 0 ? "_" + i.ToString() + "_" : "";
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

                static void CreateCell(IRow CurrentRow, int CellIndex, string Value, XSSFCellStyle Style)
                {
                    ICell Cell = CurrentRow.CreateCell(CellIndex);
                    Cell.SetCellValue(Value);
                    Cell.CellStyle = Style;
                }

                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet(звіт.Caption.Length >= 30 ? звіт.Caption[..30] : звіт.Caption);

                if (rows.Count > 0)
                {
                    string[] nameCols = rows[0];

                    //Header
                    {
                        XSSFFont font = (XSSFFont)workbook.CreateFont();
                        font.FontHeightInPoints = 11;
                        font.FontName = "Arial";
                        font.IsBold = true;

                        XSSFCellStyle cellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                        cellStyle.SetFont(font);

                        cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Dashed;
                        cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Dashed;
                        cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Dashed;
                        cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Dashed;
                        cellStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;

                        IRow row = sheet.CreateRow(0);

                        for (int i = 0; i < nameCols.Length; i++)
                            CreateCell(row, i, nameCols[i], cellStyle);
                    }

                    //Body
                    {
                        XSSFFont font = (XSSFFont)workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.FontName = "Arial";

                        XSSFCellStyle cellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
                        cellStyle.SetFont(font);

                        for (int r = 1; r < rows.Count; r++)
                        {
                            IRow row = sheet.CreateRow(r);

                            string[] Значення = rows[r];
                            for (int i = 0; i < Значення.Length; i++)
                                CreateCell(row, i, Значення[i], cellStyle);
                        }
                    }

                    for (int i = 0; i < nameCols.Length; i++)
                        sheet.AutoSizeColumn(i);

                    sheet.SetAutoFilter(new NPOI.SS.Util.CellRangeAddress(0, rows.Count - 1, 0, nameCols.Length - 1));

                    GC.Collect();
                }

                using FileStream stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                workbook.Write(stream);

                ФункціїДляПовідомлень.ДодатиІнформаційнеПовідомлення(null, "Вигрузка звіту в Excel", $"Вигружено у файл: {fullPath}");
            }

            await ValueTask.FromResult(true);
        }

        protected override async ValueTask ВідкритиЗбереженіЗвіти()
        {
            ЗбереженіЗвіти page = new ЗбереженіЗвіти();
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Збережені звіти", () => page);
            await page.SetValue();
        }

        protected override async ValueTask ЗберегтиЗвіт(InterfaceGtk.ЗвітСторінка звіт, List<string[]> rows)
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