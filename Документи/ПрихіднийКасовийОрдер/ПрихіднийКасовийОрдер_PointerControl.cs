
/*

        ПрихіднийКасовийОрдер_PointerControl.cs
        PointerControl

*/

using InterfaceGtk3;
using AccountingSoftware;

using GeneratedCode.Документи;

namespace StorageAndTrade
{
    class ПрихіднийКасовийОрдер_PointerControl : PointerControl
    {
        event EventHandler<ПрихіднийКасовийОрдер_Pointer> PointerChanged;

        public ПрихіднийКасовийОрдер_PointerControl()
        {
            pointer = new ПрихіднийКасовийОрдер_Pointer();
            WidthPresentation = 300;
            Caption = $"{ПрихіднийКасовийОрдер_Const.FULLNAME}:";
            PointerChanged += async (object? _, ПрихіднийКасовийОрдер_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ПрихіднийКасовийОрдер_Pointer pointer;
        public ПрихіднийКасовийОрдер_Pointer Pointer
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
            ПрихіднийКасовийОрдер page = new ПрихіднийКасовийОрдер
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) => { Pointer = new ПрихіднийКасовийОрдер_Pointer(selectPointer); }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ПрихіднийКасовийОрдер_Const.FULLNAME}", () => page);
            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ПрихіднийКасовийОрдер_Pointer();
        }
    }
}