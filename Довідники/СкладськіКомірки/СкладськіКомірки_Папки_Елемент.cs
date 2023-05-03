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

using Gtk;

using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class СкладськіКомірки_Папки_Елемент : ДовідникЕлемент
    {
        public СкладськіКомірки_Папки_Objest СкладськіКомірки_Папки_Objest { get; set; } = new СкладськіКомірки_Папки_Objest();
        public СкладськіКомірки_Папки_Pointer РодичДляНового { get; set; } = new СкладськіКомірки_Папки_Pointer();
        public СкладськіПриміщення_Pointer СкладськеПриміщенняДляНового { get; set; } = new СкладськіПриміщення_Pointer();

        Entry Код = new Entry() { WidthRequest = 100 };
        Entry Назва = new Entry() { WidthRequest = 500 };
        СкладськіКомірки_Папки_PointerControl Родич = new СкладськіКомірки_Папки_PointerControl();
        СкладськіПриміщення_PointerControl СкладськеПриміщення = new СкладськіПриміщення_PointerControl();

        public СкладськіКомірки_Папки_Елемент() : base() { }

        protected override void CreatePack1()
        {
            VBox vBox = new VBox();
            HPanedTop.Pack1(vBox, false, false);

            //Код
            HBox hBoxCode = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxCode, false, false, 5);

            hBoxCode.PackStart(new Label("Код:"), false, false, 5);
            hBoxCode.PackStart(Код, false, false, 5);

            //Назва
            HBox hBoxName = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxName, false, false, 5);

            hBoxName.PackStart(new Label("Назва:"), false, false, 5);
            hBoxName.PackStart(Назва, false, false, 5);

            //СкладськеПриміщення
            HBox hBoxSkaldPrem = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxSkaldPrem, false, false, 5);

            hBoxSkaldPrem.PackStart(СкладськеПриміщення, false, false, 5);

            //Родич
            HBox hBoxParent = new HBox() { Halign = Align.End };
            vBox.PackStart(hBoxParent, false, false, 5);

            hBoxParent.PackStart(Родич, false, false, 5);
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
            {
                СкладськіКомірки_Папки_Objest.New();
                СкладськіКомірки_Папки_Objest.Родич = РодичДляНового;
                СкладськіКомірки_Папки_Objest.Приміщення = СкладськеПриміщенняДляНового;
            }
            else
                Родич.UidOpenFolder = СкладськіКомірки_Папки_Objest.UnigueID.ToString();

            Код.Text = СкладськіКомірки_Папки_Objest.Код;
            Назва.Text = СкладськіКомірки_Папки_Objest.Назва;
            Родич.Pointer = СкладськіКомірки_Папки_Objest.Родич;
            СкладськеПриміщення.Pointer = СкладськіКомірки_Папки_Objest.Приміщення;
        }

        protected override void GetValue()
        {
            UnigueID = СкладськіКомірки_Папки_Objest.UnigueID;
            Caption = Назва.Text;

            СкладськіКомірки_Папки_Objest.Код = Код.Text;
            СкладськіКомірки_Папки_Objest.Назва = Назва.Text;
            СкладськіКомірки_Папки_Objest.Родич = Родич.Pointer;
            СкладськіКомірки_Папки_Objest.Приміщення = СкладськеПриміщення.Pointer;
        }

        #endregion

        protected override void Save()
        {
            try
            {
                СкладськіКомірки_Папки_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }
        }
    }
}