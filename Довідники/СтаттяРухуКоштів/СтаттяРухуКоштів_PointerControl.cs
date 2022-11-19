
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class СтаттяРухуКоштів_PointerControl : PointerControl
    {
        public СтаттяРухуКоштів_PointerControl()
        {
            pointer = new СтаттяРухуКоштів_Pointer();
            WidthPresentation = 300;
            Caption = "Стаття:";
        }

        СтаттяРухуКоштів_Pointer pointer;
        public СтаттяРухуКоштів_Pointer Pointer
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
            СтаттяРухуКоштів page = new СтаттяРухуКоштів(true);

            page.DirectoryPointerItem = Pointer;
            page.CallBack_OnSelectPointer = (СтаттяРухуКоштів_Pointer selectPointer) =>
            {
                Pointer = selectPointer;
            };

            Program.GeneralForm?.CreateNotebookPage("Вибір - Стаття руху коштів", () => { return page; });

            page.LoadRecords();

        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new СтаттяРухуКоштів_Pointer();
        }
    }
}