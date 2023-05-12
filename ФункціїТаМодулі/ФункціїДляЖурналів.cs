/*
Copyright (C) 2019-2023 TARAKHOMYN YURIY IVANOVYCH
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

Функції для журналів

*/
using Gtk;

using System.Reflection;

using AccountingSoftware;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class ФункціїДляЖурналів
    {
        public static void ВідкритиСписокДокументів(Widget relative_to, Dictionary<string, string> allowDocument, ТипПеріодуДляЖурналівДокументів periodWhere = 0)
        {
            VBox vBox = new VBox();

            foreach (KeyValuePair<string, string> typeDoc in allowDocument)
            {
                LinkButton lb = new LinkButton(typeDoc.Value, " " + typeDoc.Value) { Halign = Align.Start };
                vBox.PackStart(lb, false, false, 0);

                lb.Clicked += (object? sender, EventArgs args) =>
                {
                    ФункціїДляЖурналів.ВідкритиЖурналВідповідноДоВидуДокументу(typeDoc.Key, new UnigueID(), periodWhere);
                };
            }

            Popover PopoverSelect = new Popover(relative_to) { Position = PositionType.Bottom, BorderWidth = 2 };

            PopoverSelect.Add(vBox);
            PopoverSelect.ShowAll();
        }

        public static void ВідкритиЖурналВідповідноДоВидуДокументу(string typeDoc, UnigueID unigueID, ТипПеріодуДляЖурналівДокументів periodWhere = 0)
        {
            Assembly ExecutingAssembly = Assembly.GetExecutingAssembly();

            //Простір імен програми
            string NameSpacePage = "StorageAndTrade";

            //Простір імен конфігурації
            string NameSpaceConfig = "StorageAndTrade_1_0.Документи";

            object? listPage;

            try
            {
                listPage = ExecutingAssembly.CreateInstance($"{NameSpacePage}.{typeDoc}");
            }
            catch (Exception ex)
            {
                Message.Error(Program.GeneralForm, ex.Message);
                return;
            }

            if (listPage != null)
            {
                //Документ який потрібно виділити в списку
                listPage.GetType().GetProperty("SelectPointerItem")?.SetValue(listPage, unigueID);

                //Заголовок журналу з константи конфігурації
                string listName = "Список документів";
                {
                    Type? documentConst = Type.GetType($"{NameSpaceConfig}.{typeDoc}_Const");
                    if (documentConst != null)
                        listName = documentConst.GetField("FULLNAME")?.GetValue(null)?.ToString() ?? listName;
                }

                Program.GeneralForm?.CreateNotebookPage(listName, () => { return (Widget)listPage; }, true);

                listPage.GetType().GetProperty("PeriodWhere")?.SetValue(listPage, (periodWhere != 0 ? periodWhere : ТипПеріодуДляЖурналівДокументів.ВесьПеріод));
                listPage.GetType().InvokeMember("SetValue", BindingFlags.InvokeMethod, null, listPage, null);
            }
        }
    }
}
