/*

Документи

*/

using Gtk;
using InterfaceGtk;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class Menu_Document : Форма
    {
        public Menu_Document() : base()
        {
            //Список
            Box hBoxList = new Box(Orientation.Horizontal, 0);

            Box vLeft = new Box(Orientation.Vertical, 0);
            hBoxList.PackStart(vLeft, false, false, 5);

            {
                CreateCaptionLink(vLeft, "Продажі", async () =>
                {
                    Журнал_Продажі page = new Журнал_Продажі();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Продажі", () => page);
                    await page.SetValue();
                });

                CreateLink(vLeft, ЗамовленняКлієнта_Const.FULLNAME, async () =>
                {
                    ЗамовленняКлієнта page = new ЗамовленняКлієнта();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ЗамовленняКлієнта_Const.FULLNAME, () => page);
                    await page.SetValue();
                });

                CreateLink(vLeft, РахунокФактура_Const.FULLNAME, async () =>
                {
                    РахунокФактура page = new РахунокФактура();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, РахунокФактура_Const.FULLNAME, () => page);
                    await page.SetValue();
                });

                CreateLink(vLeft, РеалізаціяТоварівТаПослуг_Const.FULLNAME, async () =>
                {
                    РеалізаціяТоварівТаПослуг page = new РеалізаціяТоварівТаПослуг();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, РеалізаціяТоварівТаПослуг_Const.FULLNAME, () => page);
                    await page.SetValue();
                });

                CreateLink(vLeft, АктВиконанихРобіт_Const.FULLNAME, async () =>
                {
                    АктВиконанихРобіт page = new АктВиконанихРобіт();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, АктВиконанихРобіт_Const.FULLNAME, () => page);
                    await page.SetValue();
                });

                CreateLink(vLeft, ПоверненняТоварівВідКлієнта_Const.FULLNAME, async () =>
                {
                    ПоверненняТоварівВідКлієнта page = new ПоверненняТоварівВідКлієнта();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ПоверненняТоварівВідКлієнта_Const.FULLNAME, () => page);
                    await page.SetValue();
                });
            }

            {
                CreateCaptionLink(vLeft, "Закупівлі", async () =>
                {
                    Журнал_Закупівлі page = new Журнал_Закупівлі();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Закупівлі", () => page);
                    await page.SetValue();
                });

                CreateLink(vLeft, ЗамовленняПостачальнику_Const.FULLNAME, async () =>
                {
                    ЗамовленняПостачальнику page = new ЗамовленняПостачальнику();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ЗамовленняПостачальнику_Const.FULLNAME, () => page);
                    await page.SetValue();
                });

                CreateLink(vLeft, ПоступленняТоварівТаПослуг_Const.FULLNAME, async () =>
                {
                    ПоступленняТоварівТаПослуг page = new ПоступленняТоварівТаПослуг();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ПоступленняТоварівТаПослуг_Const.FULLNAME, () => page);
                    await page.SetValue();
                });

                CreateLink(vLeft, ПоверненняТоварівПостачальнику_Const.FULLNAME, async () =>
                {
                    ПоверненняТоварівПостачальнику page = new ПоверненняТоварівПостачальнику();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ПоверненняТоварівПостачальнику_Const.FULLNAME, () => page);
                    await page.SetValue();
                });
            }

            {
                CreateCaptionLink(vLeft, "Ціноутворення");

                CreateLink(vLeft, ВстановленняЦінНоменклатури_Const.FULLNAME, async () =>
                {
                    ВстановленняЦінНоменклатури page = new ВстановленняЦінНоменклатури();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ВстановленняЦінНоменклатури_Const.FULLNAME, () => page);
                    await page.SetValue();
                });
            }

            CreateSeparator(hBoxList);

            Box vRight = new Box(Orientation.Vertical, 0);
            hBoxList.PackStart(vRight, false, false, 5);

            {
                CreateCaptionLink(vRight, "Каса", async () =>
                {
                    Журнал_Каса page = new Журнал_Каса();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Каса", () => page);
                    await page.SetValue();
                });

                CreateLink(vRight, ПрихіднийКасовийОрдер_Const.FULLNAME, async () =>
                {
                    ПрихіднийКасовийОрдер page = new ПрихіднийКасовийОрдер();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ПрихіднийКасовийОрдер_Const.FULLNAME, () => page);
                    await page.SetValue();
                });

                CreateLink(vRight, РозхіднийКасовийОрдер_Const.FULLNAME, async () =>
                {
                    РозхіднийКасовийОрдер page = new РозхіднийКасовийОрдер();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, РозхіднийКасовийОрдер_Const.FULLNAME, () => page);
                    await page.SetValue();
                });

                CreateLink(vRight, КорегуванняБоргу_Const.FULLNAME, async () =>
                {
                    КорегуванняБоргу page = new КорегуванняБоргу();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, КорегуванняБоргу_Const.FULLNAME, () => page);
                    await page.SetValue();
                });
            }

            {
                CreateCaptionLink(vRight, "Склад", async () =>
                {
                    Журнал_Склад page = new Журнал_Склад();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Склад", () => page);
                    await page.SetValue();
                });

                CreateLink(vRight, ПереміщенняТоварів_Const.FULLNAME, async () =>
                {
                    ПереміщенняТоварів page = new ПереміщенняТоварів();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ПереміщенняТоварів_Const.FULLNAME, () => page);
                    await page.SetValue();
                });

                CreateLink(vRight, ВведенняЗалишків_Const.FULLNAME, async () =>
                {
                    ВведенняЗалишків page = new ВведенняЗалишків();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ВведенняЗалишків_Const.FULLNAME, () => page);
                    await page.SetValue();
                });

                CreateLink(vRight, ВнутрішнєСпоживанняТоварів_Const.FULLNAME, async () =>
                {
                    ВнутрішнєСпоживанняТоварів page = new ВнутрішнєСпоживанняТоварів();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ВнутрішнєСпоживанняТоварів_Const.FULLNAME, () => page);
                    await page.SetValue();
                });

                CreateLink(vRight, ПсуванняТоварів_Const.FULLNAME, async () =>
                {
                    ПсуванняТоварів page = new ПсуванняТоварів();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ПсуванняТоварів_Const.FULLNAME, () => page);
                    await page.SetValue();
                });

                CreateLink(vRight, ПерерахунокТоварів_Const.FULLNAME, async () =>
                {
                    ПерерахунокТоварів page = new ПерерахунокТоварів();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ПерерахунокТоварів_Const.FULLNAME, () => page);
                    await page.SetValue();
                });
            }

            {
                CreateCaptionLink(vRight, "Адресне зберігання", async () =>
                {
                    Журнал_АдреснеЗберігання page = new Журнал_АдреснеЗберігання();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, "Адресне зберігання", () => page);
                    await page.SetValue();
                });

                CreateLink(vRight, РозміщенняТоварівНаСкладі_Const.FULLNAME, async () =>
                {
                    РозміщенняТоварівНаСкладі page = new РозміщенняТоварівНаСкладі();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, РозміщенняТоварівНаСкладі_Const.FULLNAME, () => page);
                    await page.SetValue();
                });

                CreateLink(vRight, ЗбіркаТоварівНаСкладі_Const.FULLNAME, async () =>
                {
                    ЗбіркаТоварівНаСкладі page = new ЗбіркаТоварівНаСкладі();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ЗбіркаТоварівНаСкладі_Const.FULLNAME, () => page);
                    await page.SetValue();
                });

                CreateLink(vRight, ПереміщенняТоварівНаСкладі_Const.FULLNAME, async () =>
                {
                    ПереміщенняТоварівНаСкладі page = new ПереміщенняТоварівНаСкладі();
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, ПереміщенняТоварівНаСкладі_Const.FULLNAME, () => page);
                    await page.SetValue();
                });
            }

            PackStart(hBoxList, false, false, 10);
            ShowAll();
        }
    }
}