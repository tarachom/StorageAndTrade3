

/*
        ПрихіднийКасовийОрдер_Друк.cs
        Друк
*/

using AccountingSoftware;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Документи;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    static class ПрихіднийКасовийОрдер_Друк
    {
        public static async ValueTask PDF(UnigueID unigueID)
        {
            ПрихіднийКасовийОрдер_Objest? Обєкт = await new ПрихіднийКасовийОрдер_Pointer(unigueID).GetDocumentObject();
            if (Обєкт != null)
            {
                await Обєкт.Організація.GetPresentation();
                await Обєкт.Контрагент.GetPresentation();
                await Обєкт.Валюта.GetPresentation();
                await Обєкт.Каса.GetPresentation();
                await Обєкт.КасаВідправник.GetPresentation();

                string ГосподарськаОперація = ПсевдонімиПерелічення.ГосподарськіОперації_Alias(Обєкт.ГосподарськаОперація);

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
                            //Каса
                            x.Item().Text("Каса: " + Обєкт.Каса.Назва).FontSize(8);
                            //Валюта
                            x.Item().Text("Валюта: " + Обєкт.Валюта.Назва).FontSize(8);
                            //ГосподарськаОперація
                            x.Item().Text("Господарська операція: " + ГосподарськаОперація).FontSize(8);

                            if (Обєкт.ГосподарськаОперація == ГосподарськіОперації.ПоступленняКоштівЗІншоїКаси)
                            {
                                //КасаВідправник
                                x.Item().Text("Каса відправник: " + Обєкт.КасаВідправник.Назва).FontSize(8);
                            }

                            x.Item().PaddingVertical(5).LineHorizontal(1);

                            //Сума
                            x.Item().Text("Сума: " + Обєкт.СумаДокументу).FontSize(8).Bold();
                        });
                    });
                });

                doc.GeneratePdfAndShow();
            }
        }
    }
}
