
/*
        ЗбіркаТоварівНаСкладі_PointerControl.cs
        PointerControl
*/

using InterfaceGtk3;
using AccountingSoftware;

using GeneratedCode.Документи;

namespace StorageAndTrade
{
    class ЗбіркаТоварівНаСкладі_PointerControl : PointerControl
    {
        event EventHandler<ЗбіркаТоварівНаСкладі_Pointer> PointerChanged;

        public ЗбіркаТоварівНаСкладі_PointerControl()
        {
            pointer = new ЗбіркаТоварівНаСкладі_Pointer();
            WidthPresentation = 300;
            Caption = $"{ЗбіркаТоварівНаСкладі_Const.FULLNAME}:";
            PointerChanged += async (object? _, ЗбіркаТоварівНаСкладі_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ЗбіркаТоварівНаСкладі_Pointer pointer;
        public ЗбіркаТоварівНаСкладі_Pointer Pointer
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
            ЗбіркаТоварівНаСкладі page = new ЗбіркаТоварівНаСкладі
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) => { Pointer = new ЗбіркаТоварівНаСкладі_Pointer(selectPointer); }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ЗбіркаТоварівНаСкладі_Const.FULLNAME}", () => page);
            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ЗбіркаТоварівНаСкладі_Pointer();
        }
    }
}