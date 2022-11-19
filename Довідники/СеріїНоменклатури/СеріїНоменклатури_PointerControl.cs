
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class СеріїНоменклатури_PointerControl : PointerControl
    {
        public СеріїНоменклатури_PointerControl()
        {
            pointer = new СеріїНоменклатури_Pointer();
            WidthPresentation = 300;
            Caption = "Серія:";
        }

        СеріїНоменклатури_Pointer pointer;
        public СеріїНоменклатури_Pointer Pointer
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
            СеріїНоменклатури page = new СеріїНоменклатури(true);

            page.DirectoryPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (СеріїНоменклатури_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - СеріїНоменклатури", () => { return page; });

            page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new СеріїНоменклатури_Pointer();
        }
    }
}