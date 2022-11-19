
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class ВидиНоменклатури_PointerControl : PointerControl
    {
        public ВидиНоменклатури_PointerControl()
        {
            pointer = new ВидиНоменклатури_Pointer();
            WidthPresentation = 300;
            Caption = "Види номенклатури:";
        }

        ВидиНоменклатури_Pointer pointer;
        public ВидиНоменклатури_Pointer Pointer
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
            ВидиНоменклатури page = new ВидиНоменклатури(true);

            page.DirectoryPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (ВидиНоменклатури_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Види номенклатури", () => { return page; });

            page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ВидиНоменклатури_Pointer();
        }
    }
}