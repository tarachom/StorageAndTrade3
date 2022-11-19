
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class РеалізаціяТоварівТаПослуг_PointerControl : PointerControl
    {
        public РеалізаціяТоварівТаПослуг_PointerControl()
        {
            pointer = new РеалізаціяТоварівТаПослуг_Pointer();
            WidthPresentation = 300;
            Caption = "Реалізація товарів та послуг:";
        }

        РеалізаціяТоварівТаПослуг_Pointer pointer;
        public РеалізаціяТоварівТаПослуг_Pointer Pointer
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
            РеалізаціяТоварівТаПослуг page = new РеалізаціяТоварівТаПослуг(true);

            page.DocumentPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (РеалізаціяТоварівТаПослуг_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Реалізація товарів та послуг", () => { return page; });

            page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new РеалізаціяТоварівТаПослуг_Pointer();
        }
    }
}