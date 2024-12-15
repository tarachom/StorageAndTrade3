/*

        Каси_PointerControl.cs
        PointerControl

*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Каси_MultiplePointerControl : MultiplePointerControl
    {
        event EventHandler<List<Каси_Pointer>> PointerChanged;

        public Каси_MultiplePointerControl()
        {
            Pointer = [];
            WidthPresentation = 300;
            Caption = $"{Каси_Const.FULLNAME}:";
            PointerChanged += (object? _, List<Каси_Pointer> pointer) =>
            {
                //Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        public List<Каси_Pointer> Pointer;
        
        // public List<Каси_Pointer> Pointer
        // {
        //     get
        //     {
        //         return pointer;
        //     }
        //     set
        //     {
        //         pointer = value;
        //         PointerChanged?.Invoke(null, pointer);
        //     }
        // }

        protected override async void OpenSelect(object? sender, EventArgs args)
        {
            Popover popover = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };

            BeforeClickOpenFunc?.Invoke();

            Каси_ШвидкийВибір page = new Каси_ШвидкийВибір
            {
                PopoverParent = popover,
                //DirectoryPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer.Add(new Каси_Pointer(selectPointer));
                    AfterSelectFunc?.Invoke();
                },
                CallBack_OnMultipleSelectPointer = (UnigueID[] selectPointers) =>
                {
                    foreach (var selectPointer in selectPointers)
                        Pointer.Add(new Каси_Pointer(selectPointer));
                }
            };

            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = [];
        }
    }
}