

/*
        КорегуванняБоргу_Друк.cs
        Друк
*/

using AccountingSoftware;
using GeneratedCode.Константи;
using GeneratedCode.Документи;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using GeneratedCode.Перелічення;

namespace StorageAndTrade
{
    static class КорегуванняБоргу_Друк
    {
        public static async ValueTask PDF(UnigueID unigueID)
        {
            КорегуванняБоргу_Objest? Обєкт = await new КорегуванняБоргу_Pointer(unigueID).GetDocumentObject();
            if (Обєкт != null)
            {
                await Обєкт.Організація.GetPresentation();

                Обєкт.РозрахункиЗКонтрагентами_TablePart.FillJoin([КорегуванняБоргу_РозрахункиЗКонтрагентами_TablePart.НомерРядка]);
                await Обєкт.РозрахункиЗКонтрагентами_TablePart.Read();

                //Назва та розмір колонок
                Dictionary<string, int> Columns = new()
                {
                    { "№", 20 },
                    { "Тип", 100 },
                    { "Контрагент", 150 },
                    { "Валюта", 80 },
                    { "Сума", 50 },
                };

                //Колонки які мають відносний розмір
                Dictionary<string, int> ColumnsRelative = new()
                {
                    {"Контрагент", 5 }
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
                            //Назва
                            x.Item().Text(Обєкт.Назва).FontSize(14).Bold();

                            //Організація
                            x.Item().Text("Організація: " + Обєкт.Організація.Назва).FontSize(8);

                            x.Item().PaddingVertical(5).LineHorizontal(1);

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

                                foreach (var record in Обєкт.РозрахункиЗКонтрагентами_TablePart.Records)
                                {
                                    table.Cell().Border(1).Padding(1).Text(record.НомерРядка.ToString()).FontSize(8).AlignCenter();
                                    table.Cell().Border(1).Padding(1).Text(ПсевдонімиПерелічення.ТипиКонтрагентів_Alias(record.ТипКонтрагента)).FontSize(8);
                                    table.Cell().Border(1).Padding(1).Text(record.Контрагент.Назва).FontSize(8);
                                    table.Cell().Border(1).Padding(1).Text(record.Валюта.Назва).FontSize(8).AlignCenter();
                                    table.Cell().Border(1).Padding(1).Text(record.Сума.ToString()).FontSize(8).AlignCenter();
                                }
                            });
                        });
                    });
                });

                doc.GeneratePdfAndShow();
            }
        }
    }
}
