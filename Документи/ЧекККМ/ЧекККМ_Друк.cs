

/*
        АктВиконанихРобіт_Друк.cs
        Друк
*/

using AccountingSoftware;
using GeneratedCode.Документи;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace StorageAndTrade
{
    static class ЧекККМ_Друк
    {
        public static async ValueTask PDF(UnigueID unigueID)
        {
            ЧекККМ_Objest? Обєкт = await new ЧекККМ_Pointer(unigueID).GetDocumentObject();
            if (Обєкт != null)
            {
                //await Обєкт.Валюта.GetPresentation();

                Обєкт.Товари_TablePart.FillJoin();
                await Обєкт.Товари_TablePart.Read();

                //Назва та розмір колонок
                Dictionary<string, int> Columns = new()
                {
                    { "№", 20 },
                    { "Найменування товару", 150 },
                    { "Од. виміру", 50},
                    { "Кількість", 50 },
                    { "Ціна", 50 },
                    { "Сума", 50 }
                };

                //Колонки які мають відносний розмір
                Dictionary<string, int> ColumnsRelative = new()
                {
                    {"Найменування товару", 5 }
                };

                QuestPDF.Settings.License = LicenseType.Community;
                Document doc = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(10, Unit.Point);

                        page.Content().Column(x =>
                        {
                            //Дата
                            x.Item().Text(DateTime.Now.ToShortDateString()).AlignRight().FontSize(8);

                            //Назва
                            x.Item().Padding(10).Text("ТОВАРНИЙ ЧЕК").AlignCenter().FontSize(14).Bold();

                            decimal Сума = 0;

                            x.Item().Table(table =>
                            {
                                table.ColumnsDefinition(cols =>
                                {
                                    foreach (var item in Columns)
                                        if (ColumnsRelative.TryGetValue(item.Key, out int value))
                                            cols.RelativeColumn(value);
                                        else
                                            cols.ConstantColumn(item.Value);
                                });

                                table.Header(cell =>
                                {
                                    foreach (var item in Columns.Keys)
                                        cell.Cell().Border(1).Padding(1).Text(item).FontSize(8).AlignCenter();
                                });

                                foreach (var record in Обєкт.Товари_TablePart.Records)
                                {
                                    table.Cell().Border(1).Padding(1).Text(record.НомерРядка.ToString()).FontSize(8).AlignCenter();
                                    table.Cell().Border(1).Padding(1).Text(record.Номенклатура.Назва + " " + record.ХарактеристикаНоменклатури.Назва).FontSize(8);
                                    table.Cell().Border(1).Padding(1).Text(record.Пакування.Назва).FontSize(8).AlignCenter();
                                    table.Cell().Border(1).Padding(1).Text(record.Кількість.ToString()).FontSize(8).AlignCenter();
                                    table.Cell().Border(1).Padding(1).Text(record.Ціна.ToString()).FontSize(8).AlignRight();
                                    table.Cell().Border(1).Padding(1).Text(record.Сума.ToString()).FontSize(8).AlignRight();
                                    Сума += record.Сума;
                                }

                                for (int i = 1; i < Columns.Count - 1; i++)
                                    table.Cell().Border(0).Padding(1);

                                table.Cell().Border(1).Padding(1).Text("Разом").FontSize(8).AlignRight();
                                table.Cell().Border(1).Padding(1).Text(Сума.ToString()).FontSize(8).AlignRight();
                            });

                            x.Item().Text("");
                            x.Item().Text($"Всього на суму: {Сума} грн.").FontSize(8);
                            x.Item().Text("");

                            x.Item().Table(table =>
                            {
                                table.ColumnsDefinition(cols =>
                                {
                                    for (int i = 1; i <= 5; i++)
                                        cols.RelativeColumn(i == 3 || i == 4 ? 1 : 2);
                                });

                                table.Cell().Padding(1).Text("Товар відпустив").FontSize(8).AlignRight();
                                table.Cell().BorderBottom(1);
                                table.Cell();
                                table.Cell().Padding(1).Text("Товар прийняв").FontSize(8).AlignRight();
                                table.Cell().BorderBottom(1);
                            });
                        });
                    });
                });

                doc.GeneratePdfAndShow();
            }
        }
    }
}
