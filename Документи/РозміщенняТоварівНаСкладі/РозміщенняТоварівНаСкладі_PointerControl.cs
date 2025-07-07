
/*
        РозміщенняТоварівНаСкладі_PointerControl.cs
        PointerControl
*/

using InterfaceGtk3;
using AccountingSoftware;

using GeneratedCode.Документи;

namespace StorageAndTrade
{
    class РозміщенняТоварівНаСкладі_PointerControl : PointerControl
    {
        event EventHandler<РозміщенняТоварівНаСкладі_Pointer> PointerChanged;

        public РозміщенняТоварівНаСкладі_PointerControl()
        {
            pointer = new РозміщенняТоварівНаСкладі_Pointer();
            WidthPresentation = 300;
            Caption = $"{РозміщенняТоварівНаСкладі_Const.FULLNAME}:";
            PointerChanged += async (object? _, РозміщенняТоварівНаСкладі_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        РозміщенняТоварівНаСкладі_Pointer pointer;
        public РозміщенняТоварівНаСкладі_Pointer Pointer
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
            РозміщенняТоварівНаСкладі page = new РозміщенняТоварівНаСкладі
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) => { Pointer = new РозміщенняТоварівНаСкладі_Pointer(selectPointer); }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {РозміщенняТоварівНаСкладі_Const.FULLNAME}", () => page);
            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new РозміщенняТоварівНаСкладі_Pointer();
        }
    }
}