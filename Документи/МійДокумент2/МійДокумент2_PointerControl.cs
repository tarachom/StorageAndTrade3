

/*     
        МійДокумент2_PointerControl.cs
        PointerControl
*/
using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class МійДокумент2_PointerControl : PointerControl
    {
        event EventHandler<МійДокумент2_Pointer> PointerChanged;

        public МійДокумент2_PointerControl()
        {
            pointer = new МійДокумент2_Pointer();
            WidthPresentation = 300;
            Caption = $"{МійДокумент2_Const.FULLNAME}:";
            PointerChanged += async (object? _, МійДокумент2_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        МійДокумент2_Pointer pointer;
        public МійДокумент2_Pointer Pointer
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

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            BeforeClickOpenFunc?.Invoke();
            МійДокумент2 page = new МійДокумент2
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new МійДокумент2_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {МійДокумент2_Const.FULLNAME}", () => page);
            page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new МійДокумент2_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
    