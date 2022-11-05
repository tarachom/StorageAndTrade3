
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class ХарактеристикиНоменклатури_PointerControl : PointerControl
    {
        public ХарактеристикиНоменклатури_PointerControl()
        {
            pointer = new ХарактеристикиНоменклатури_Pointer();
            WidthPresentation = 300;
            Caption = "Характеристика:";
        }

        ХарактеристикиНоменклатури_Pointer pointer;
        public ХарактеристикиНоменклатури_Pointer Pointer
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

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Характеристики", () =>
            {
                ХарактеристикиНоменклатури page = new ХарактеристикиНоменклатури();

                page.DirectoryPointerItem = Pointer;
                page.CallBack_OnSelectPointer = (ХарактеристикиНоменклатури_Pointer selectPointer) =>
                {
                    Pointer = selectPointer;
                };

                page.LoadRecords();

                return page;
            });
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ХарактеристикиНоменклатури_Pointer();
        }
    }
}