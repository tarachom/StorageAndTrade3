
/*

        ПсуванняТоварів_PointerControl.cs
        PointerControl

*/

using InterfaceGtk;
using AccountingSoftware;

using GeneratedCode.Документи;

namespace StorageAndTrade
{
    class ПсуванняТоварів_PointerControl : PointerControl
    {
        event EventHandler<ПсуванняТоварів_Pointer> PointerChanged;

        public ПсуванняТоварів_PointerControl()
        {
            pointer = new ПсуванняТоварів_Pointer();
            WidthPresentation = 300;
            Caption = $"{ПсуванняТоварів_Const.FULLNAME}:";
            PointerChanged += async (object? _, ПсуванняТоварів_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ПсуванняТоварів_Pointer pointer;
        public ПсуванняТоварів_Pointer Pointer
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
            ПсуванняТоварів page = new ПсуванняТоварів
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) => { Pointer = new ПсуванняТоварів_Pointer(selectPointer); }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ПсуванняТоварів_Const.FULLNAME}", () =>  page);
            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ПсуванняТоварів_Pointer();
        }
    }
}