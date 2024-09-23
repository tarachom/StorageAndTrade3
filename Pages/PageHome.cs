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

/*

Стартова сторінка.

*/

using Gtk;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace StorageAndTrade
{
    class PageHome : Box
    {
        public БлокДляСторінки_КурсиВалют БлокКурсиВалют = new БлокДляСторінки_КурсиВалют() { WidthRequest = 500 };
        public БлокДляСторінки_АктивніКористувачі АктивніКористувачі = new БлокДляСторінки_АктивніКористувачі() { WidthRequest = 500 };

        public PageHome() : base(Orientation.Vertical, 0)
        {
            {
                Box hBox = new Box(Orientation.Horizontal, 0);
                hBox.PackStart(БлокКурсиВалют, false, false, 5);

                PackStart(hBox, false, false, 5);
            }

            {
                Box hBox = new Box(Orientation.Horizontal, 0);
                hBox.PackStart(АктивніКористувачі, false, false, 5);
                PackStart(hBox, false, false, 5);
            }

            {
                QuestPDF.Settings.License = LicenseType.Community;
                
                var doc = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(10, QuestPDF.Infrastructure.Unit.Point);

                        page.Content().Column(x =>
                        {
                            x.Item().Text("Поступлення товарів та послуг №0000000454 від 12.10.2024 р.")
                                .FontSize(12).FontFamily("Arial").Bold();

                            x.Item().PaddingVertical(5).LineHorizontal(1);

                            x.Item().Table(table =>
                            {
                                table.ColumnsDefinition(cols =>
                                {
                                    cols.ConstantColumn(20);
                                    cols.ConstantColumn(150);
                                    cols.ConstantColumn(250);
                                    cols.ConstantColumn(50);
                                    cols.ConstantColumn(50);
                                });

                                table.Header(cell =>
                                {
                                    cell.Cell().Border(1).Padding(1).Text("№").AlignCenter();
                                    cell.Cell().Border(1).Padding(1).Text("Назва").AlignCenter();
                                    cell.Cell().Border(1).Padding(1).Text("Опис").AlignCenter();
                                    cell.Cell().Border(1).Padding(1).Text("Ціна").AlignCenter();
                                    cell.Cell().Border(1).Padding(1).Text("Сума").AlignCenter();
                                });

                                for (int i = 1; i < 5; i++)
                                    foreach (var font in new string[] { i.ToString(), "text text text text text text text text text text text text text text text text ", "test", "test", "test" })
                                    {
                                        var a = table.Cell().Border(1).Padding(1).Text(font).FontSize(8).FontFamily("Arial");
                                        //a.AlignCenter(); //
                                    }
                            });
                        });
                    });
                });

                doc.GeneratePdf("lines.pdf");
            }

            ShowAll();
        }
    }
}