
/*

        ПоступленняТоварівТаПослуг_PointerControl.cs
        PointerControl

*/

using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class ПоступленняТоварівТаПослуг_PointerControl : PointerControl
    {
        event EventHandler<ПоступленняТоварівТаПослуг_Pointer> PointerChanged;

        public ПоступленняТоварівТаПослуг_PointerControl()
        {
            pointer = new ПоступленняТоварівТаПослуг_Pointer();
            WidthPresentation = 300;
            Caption = $"{ПоступленняТоварівТаПослуг_Const.FULLNAME}:";
            PointerChanged += async (object? _, ПоступленняТоварівТаПослуг_Pointer pointer)=> 
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ПоступленняТоварівТаПослуг_Pointer pointer;
        public ПоступленняТоварівТаПослуг_Pointer Pointer
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
            ПоступленняТоварівТаПослуг page = new ПоступленняТоварівТаПослуг
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) => { Pointer = new ПоступленняТоварівТаПослуг_Pointer(selectPointer); }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {ПоступленняТоварівТаПослуг_Const.FULLNAME}", () => page);
            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ПоступленняТоварівТаПослуг_Pointer();
        }
    }
}