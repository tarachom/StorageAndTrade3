
/*

        РахунокФактура_PointerControl.cs
        PointerControl

*/

using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class РахунокФактура_PointerControl : PointerControl
    {
        event EventHandler<РахунокФактура_Pointer> PointerChanged;

        public РахунокФактура_PointerControl()
        {
            pointer = new РахунокФактура_Pointer();
            WidthPresentation = 300;
            Caption = $"{РахунокФактура_Const.FULLNAME}:";
            PointerChanged += async (object? _, РахунокФактура_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        РахунокФактура_Pointer pointer;
        public РахунокФактура_Pointer Pointer
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
            РахунокФактура page = new РахунокФактура
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) => { Pointer = new РахунокФактура_Pointer(selectPointer); }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {РахунокФактура_Const.FULLNAME}", () => page);
            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new РахунокФактура_Pointer();
        }
    }
}