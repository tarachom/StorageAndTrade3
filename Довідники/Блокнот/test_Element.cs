

/*     файл:     test_Елемент.cs     */

using Gtk;

using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class test_Елемент : ДовідникЕлемент
    {
        public test_Objest test_Objest { get; set; } = new test_Objest();

        #region Fields

        Entry Код = new Entry() { WidthRequest = 500 };

        Entry Назва = new Entry() { WidthRequest = 500 };

        Entry one = new Entry() { WidthRequest = 500 };

        IntegerControl too = new IntegerControl();

        NumericControl thee = new NumericControl();

        bool dfgdfg;

        DateTimeControl sdefsd = new DateTimeControl() { OnlyDate = true };

        DateTimeControl gfwsdg = new DateTimeControl();

        TimeControl sdsd = new TimeControl();

        CompositePointerControl sdfgsd = new CompositePointerControl();

        Guid sdfsdfs = new Guid();

        byte[] sdfsd = new byte[] { };

        string[] sdfsdfssd = new string[] { };

        bool xdfgfdsgsd;
        Номенклатура_PointerControl Номенклатура = new Номенклатура_PointerControl() { Caption = "ggggg", WidthPresentation = 300 };

        ComboBoxText ТипиСкладів = new ComboBoxText();

        Guid wertwetwetwe = new Guid();

        ComboBoxText ГосподарськіОперації = new ComboBoxText();

        #endregion

        #region TabularParts

        #endregion

        public test_Елемент() : base()
        {

            foreach (var field in ПсевдонімиПерелічення.ТипиСкладів_List())
                ТипиСкладів.Append(field.Value.ToString(), field.Name);

            foreach (var field in ПсевдонімиПерелічення.ГосподарськіОперації_List())
                ГосподарськіОперації.Append(field.Value.ToString(), field.Name);

        }

        protected override void CreatePack1(VBox vBox)
        {

            CreateField(vBox, "Код:", Код);

            CreateField(vBox, "Назва:", Назва);

            CreateField(vBox, "one:", one);

            CreateField(vBox, "too:", too);

            CreateField(vBox, "thee:", thee);

            CreateField(vBox, "sdefsd:", sdefsd);

            CreateField(vBox, "gfwsdg:", gfwsdg);

            CreateField(vBox, "sdsd:", sdsd);

            CreateField(vBox, null, sdfgsd);

            CreateField(vBox, null, Номенклатура);

            CreateField(vBox, "sdgfsdgfsdgsdg:", ТипиСкладів);

            CreateField(vBox, "fdgsdgsdgsd:", ГосподарськіОперації);

        }

        protected override void CreatePack2(VBox vBox)
        {

        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
                test_Objest.New();

            Код.Text = test_Objest.Код;
            Назва.Text = test_Objest.Назва;
            one.Text = test_Objest.one;
            too.Value = test_Objest.too;
            thee.Value = test_Objest.thee;
            dfgdfg = test_Objest.dfgdfg;
            sdefsd.Value = test_Objest.sdefsd;
            gfwsdg.Value = test_Objest.gfwsdg;
            sdsd.Value = test_Objest.sdsd;
            sdfgsd.Pointer = test_Objest.sdfgsd;
            sdfsdfs = test_Objest.sdfsdfs;
            sdfsd = test_Objest.sdfsd;
            sdfsdfssd = test_Objest.sdfsdfssd;
            xdfgfdsgsd = test_Objest.xdfgfdsgsd;
            Номенклатура.Pointer = test_Objest.ggggg;
            ТипиСкладів.ActiveId = test_Objest.sdgfsdgfsdgsdg.ToString();
            if (ТипиСкладів.Active == -1) ТипиСкладів.Active = 0;
            wertwetwetwe = test_Objest.wertwetwetwe;
            ГосподарськіОперації.ActiveId = test_Objest.fdgsdgsdgsd.ToString();
            if (ГосподарськіОперації.Active == -1) ГосподарськіОперації.Active = 0;

        }

        protected override void GetValue()
        {
            UnigueID = test_Objest.UnigueID;
            Caption = Назва.Text;

            test_Objest.Код = Код.Text;
            test_Objest.Назва = Назва.Text;
            test_Objest.one = one.Text;
            test_Objest.too = too.Value;
            test_Objest.thee = thee.Value;
            test_Objest.dfgdfg = dfgdfg;
            test_Objest.sdefsd = sdefsd.Value;
            test_Objest.gfwsdg = gfwsdg.Value;
            test_Objest.sdsd = sdsd.Value;
            test_Objest.sdfgsd = sdfgsd.Pointer;
            test_Objest.sdfsdfs = sdfsdfs;
            test_Objest.sdfsd = sdfsd;
            test_Objest.sdfsdfssd = sdfsdfssd;
            test_Objest.xdfgfdsgsd = xdfgfdsgsd;
            test_Objest.ggggg = Номенклатура.Pointer;
            if (ТипиСкладів.Active != -1)
                test_Objest.sdgfsdgfsdgsdg = Enum.Parse<ТипиСкладів>(ТипиСкладів.ActiveId);
            test_Objest.wertwetwetwe = wertwetwetwe;
            if (ГосподарськіОперації.Active != -1)
                test_Objest.fdgsdgsdgsd = Enum.Parse<ГосподарськіОперації>(ГосподарськіОперації.ActiveId);

        }

        #endregion

        protected override void Save()
        {
            try
            {
                test_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }


        }
    }
}
