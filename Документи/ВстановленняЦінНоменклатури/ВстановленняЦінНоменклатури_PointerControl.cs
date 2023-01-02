
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ВстановленняЦінНоменклатури_PointerControl : PointerControl
    {
        public ВстановленняЦінНоменклатури_PointerControl()
        {
            pointer = new ВстановленняЦінНоменклатури_Pointer();
            WidthPresentation = 300;
            Caption = "Встановлення цін номенклатури:";
        }

        ВстановленняЦінНоменклатури_Pointer pointer;
        public ВстановленняЦінНоменклатури_Pointer Pointer
        {
            get
            {
                return pointer;
            }
            set
            {
                pointer = value;

                if (pointer != null)
                    Presentation = pointer.GetPresentation();
                else
                    Presentation = "";
            }
        }

        //Відбір по періоду в журналі
        public bool UseWherePeriod { get; set; } = false;

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            ВстановленняЦінНоменклатури page = new ВстановленняЦінНоменклатури(true);

            page.DocumentPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (ВстановленняЦінНоменклатури_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Встановлення цін номенклатури", () => { return page; });

            if (UseWherePeriod)
                page.SetValue();
            else
                page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ВстановленняЦінНоменклатури_Pointer();
        }
    }
}