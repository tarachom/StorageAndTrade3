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
using ClosedXML.Excel;

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
                using var wbook = new XLWorkbook();

                var ws = wbook.Worksheets.Add("Sheet1");

                ws.Cell("A1").Value = "Sunny day";
                ws.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                ws.Range("A1:F1").Merge();

                ws.Column("B").Width = 3;
                ws.Column("C").Width = 30;

                ws.Cell("B3").Value = "war";
                ws.Cell("B4").Value = "snow";
                ws.Cell("B5").Value = "tree";
                ws.Cell("B6").Value = "ten";
                ws.Cell("B7").Value = "ten";

                ws.Cell("C3").Value = "book";
                ws.Cell("C4").Value = "cup";
                ws.Cell("C5").Value = "snake";
                ws.Cell("C6").Value = "falcon";
                ws.Cell("C7").Value = "cloud";

                var r = ws.Range("B3:C7");

                r.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                r.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                r.Style.Border.OutsideBorderColor = XLColor.Gray;
                r.Style.Border.InsideBorderColor = XLColor.Gray;

                string path = System.IO.Path.Combine(AppContext.BaseDirectory, "../../../merged.xlsx");

                wbook.SaveAs(path);
            }

            ShowAll();
        }
    }
}