
/*

        ПоверненняТоварівВідКлієнта_PointerControl.cs
        PointerControl

*/

using InterfaceGtk3;
using AccountingSoftware;

using GeneratedCode.Документи;

namespace StorageAndTrade
{
    class ПоверненняТоварівВідКлієнта_PointerControl : PointerControl
    {
        event EventHandler<ПоверненняТоварівВідКлієнта_Pointer> PointerChanged;

        public ПоверненняТоварівВідКлієнта_PointerControl()
        {
            pointer = new ПоверненняТоварівВідКлієнта_Pointer();
            WidthPresentation = 300;
            Caption = $"{ПоверненняТоварівВідКлієнта_Const.FULLNAME}:";
            PointerChanged += async (object? _, ПоверненняТоварівВідКлієнта_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ПоверненняТоварівВідКлієнта_Pointer pointer;
        public ПоверненняТоварівВідКлієнта_Pointer Pointer
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
            ПоверненняТоварівВідКлієнта page = new ПоверненняТоварівВідКлієнта
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) => { Pointer = new ПоверненняТоварівВідКлієнта_Pointer(selectPointer); }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ПоверненняТоварівВідКлієнта_Const.FULLNAME}", () => page);
            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ПоверненняТоварівВідКлієнта_Pointer();
        }
    }
}