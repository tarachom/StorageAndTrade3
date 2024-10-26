
/*

        ПереміщенняТоварів_PointerControl.cs
        PointerControl

*/

using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ПереміщенняТоварів_PointerControl : PointerControl
    {
        event EventHandler<ПереміщенняТоварів_Pointer> PointerChanged;

        public ПереміщенняТоварів_PointerControl()
        {
            pointer = new ПереміщенняТоварів_Pointer();
            WidthPresentation = 300;
            Caption = $"{ПереміщенняТоварів_Const.FULLNAME}:";
            PointerChanged += async (object? _, ПереміщенняТоварів_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ПереміщенняТоварів_Pointer pointer;
        public ПереміщенняТоварів_Pointer Pointer
        {
            get
            {
                return pointer;
            }
            set
            {
                pointer = value;
                PointerChanged?.Invoke(null, pointer);
            }
        }

        protected override async void OpenSelect(object? sender, EventArgs args)
        {
            ПереміщенняТоварів page = new ПереміщенняТоварів
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) => { Pointer = new ПереміщенняТоварів_Pointer(selectPointer); }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ПереміщенняТоварів_Const.FULLNAME}", () => page);
            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ПереміщенняТоварів_Pointer();
        }
    }
}