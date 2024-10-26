
/*

        ПереміщенняТоварівНаСкладі_PointerControl.cs
        PointerControl

*/

using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ПереміщенняТоварівНаСкладі_PointerControl : PointerControl
    {
        event EventHandler<ПереміщенняТоварівНаСкладі_Pointer> PointerChanged;

        public ПереміщенняТоварівНаСкладі_PointerControl()
        {
            pointer = new ПереміщенняТоварівНаСкладі_Pointer();
            WidthPresentation = 300;
            Caption = $"{ПереміщенняТоварівНаСкладі_Const.FULLNAME}:";
            PointerChanged += async (object? _, ПереміщенняТоварівНаСкладі_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ПереміщенняТоварівНаСкладі_Pointer pointer;
        public ПереміщенняТоварівНаСкладі_Pointer Pointer
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
            ПереміщенняТоварівНаСкладі page = new ПереміщенняТоварівНаСкладі
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) => { Pointer = new ПереміщенняТоварівНаСкладі_Pointer(selectPointer); }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ПереміщенняТоварівНаСкладі_Const.FULLNAME}", () => page);
            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ПереміщенняТоварівНаСкладі_Pointer();
        }
    }
}