

/*
        ПереміщенняТоварів_Друк.cs
        Друк
*/

using AccountingSoftware;
using GeneratedCode.Константи;
using GeneratedCode.Документи;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace StorageAndTrade
{
    static class ПереміщенняТоварів_Друк
    {
        public static async ValueTask PDF(UnigueID unigueID)
        {
            ПереміщенняТоварів_Objest? Обєкт = await new ПереміщенняТоварів_Pointer(unigueID).GetDocumentObject();
            if (Обєкт != null)
            {
                await Обєкт.Організація.GetPresentation();
                await Обєкт.СкладВідправник.GetPresentation();
                await Обєкт.СкладОтримувач.GetPresentation();

                Обєкт.Товари_TablePart.FillJoin([ПереміщенняТоварів_Товари_TablePart.НомерРядка]);
                await Обєкт.Товари_TablePart.Read();

                //Назва та розмір колонок
                Dictionary<string, int> Columns = new()
                {
                    {"№", 20 },
                    {"Номенклатура", 150 }
                };

                if (Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                    Columns.Add("Характеристика", 100);

                if (Системні.ВестиОблікПоСеріяхНоменклатури_Const)
                    Columns.Add("Серія", 100);

                Columns.Add("Пак", 20);
                Columns.Add("Кількість", 50);

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
                            //СкладВідправник
                            x.Item().Text("Склад відправник: " + Обєкт.СкладВідправник.Назва).FontSize(8);
                            //СкладОтримувач
                            x.Item().Text("Склад отримувач: " + Обєкт.СкладОтримувач.Назва).FontSize(8);

                            x.Item().PaddingVertical(5).LineHorizontal(1);

                            x.Item().Table(table =>
                            {
                                table.ColumnsDefinition(cols =>
                                {
                                    foreach (var item in Columns.Values)
                                        cols.ConstantColumn(item);
                                });

                                table.Header(cell =>
                                {
                                    foreach (var item in Columns.Keys)
                                        cell.Cell().Border(1).Padding(1).Text(item).FontSize(8).AlignCenter();
                                });

                                foreach (var record in Обєкт.Товари_TablePart.Records)
                                {
                                    table.Cell().Border(1).Padding(1).Text(record.НомерРядка.ToString()).FontSize(8).AlignCenter();
                                    table.Cell().Border(1).Padding(1).Text(record.Номенклатура.Назва).FontSize(8);

                                    if (Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                                        table.Cell().Border(1).Padding(1).Text(record.ХарактеристикаНоменклатури.Назва).FontSize(8);

                                    if (Системні.ВестиОблікПоСеріяхНоменклатури_Const)
                                        table.Cell().Border(1).Padding(1).Text(record.Серія.Назва).FontSize(8);

                                    table.Cell().Border(1).Padding(1).Text(record.Пакування.Назва).FontSize(8).AlignCenter();
                                    table.Cell().Border(1).Padding(1).Text(record.Кількість.ToString()).FontSize(8).AlignCenter();
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
