

/*
        АктВиконанихРобіт_Друк.cs
        Друк
*/

using AccountingSoftware;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Документи;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace StorageAndTrade
{
    static class АктВиконанихРобіт_Друк
    {
        public static async ValueTask PDF(UnigueID unigueID)
        {
            АктВиконанихРобіт_Objest? Обєкт = await new АктВиконанихРобіт_Pointer(unigueID).GetDocumentObject();
            if (Обєкт != null)
            {
                await Обєкт.Організація.GetPresentation();
                await Обєкт.Контрагент.GetPresentation();
                await Обєкт.Валюта.GetPresentation();

                Обєкт.Послуги_TablePart.FillJoin([АктВиконанихРобіт_Послуги_TablePart.НомерРядка]);
                await Обєкт.Послуги_TablePart.Read();

                //Назва та розмір колонок
                Dictionary<string, int> Columns = new()
                {
                    {"№", 20 },
                    {"Номенклатура", 150 }
                };

                if (Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                    Columns.Add("Характеристика", 100);

                Columns.Add("Кількість", 50);
                Columns.Add("Ціна", 50);
                Columns.Add("Сума", 50);

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
                            //Контрагент
                            x.Item().Text("Контрагент: " + Обєкт.Контрагент.Назва).FontSize(8);
                            //Валюта
                            x.Item().Text("Валюта: " + Обєкт.Валюта.Назва).FontSize(8);

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

                                decimal Сума = 0;

                                foreach (var record in Обєкт.Послуги_TablePart.Records)
                                {
                                    table.Cell().Border(1).Padding(1).Text(record.НомерРядка.ToString()).FontSize(8).AlignCenter();
                                    table.Cell().Border(1).Padding(1).Text(record.Номенклатура.Назва).FontSize(8);

                                    if (Системні.ВестиОблікПоХарактеристикахНоменклатури_Const)
                                        table.Cell().Border(1).Padding(1).Text(record.ХарактеристикаНоменклатури.Назва).FontSize(8);

                                    table.Cell().Border(1).Padding(1).Text(record.Кількість.ToString()).FontSize(8).AlignCenter();
                                    table.Cell().Border(1).Padding(1).Text(record.Ціна.ToString()).FontSize(8).AlignRight();
                                    table.Cell().Border(1).Padding(1).Text(record.Сума.ToString()).FontSize(8).AlignRight();
                                    Сума += record.Сума;
                                }

                                for (int i = 1; i < Columns.Count; i++)
                                    table.Cell().Border(0).Padding(1);

                                table.Cell().Border(1).Padding(1).Text(Сума.ToString()).FontSize(8).AlignRight();
                            });
                        });
                    });
                });

                doc.GeneratePdfAndShow();
            }
        }
    }
}
