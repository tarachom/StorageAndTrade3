
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class СтруктураПідприємства_PointerControl : PointerControl
    {
        public СтруктураПідприємства_PointerControl()
        {
            pointer = new СтруктураПідприємства_Pointer();
            WidthPresentation = 300;
            Caption = "Структура підприємства:";
        }

        СтруктураПідприємства_Pointer pointer;
        public СтруктураПідприємства_Pointer Pointer
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
            Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Структура підприємства", () =>
            {
                СтруктураПідприємства page = new СтруктураПідприємства(true);

                page.DirectoryPointerItem = Pointer;
                page.CallBack_OnSelectPointer = (СтруктураПідприємства_Pointer selectPointer) =>
                {
                    Pointer = selectPointer;
                };

                page.LoadRecords();

                return page;
            });
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new СтруктураПідприємства_Pointer();
        }
    }
}