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

using InterfaceGtk;
using StorageAndTrade_1_0;
using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

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
                    page.Margin(MarginPage, Unit.Point);

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