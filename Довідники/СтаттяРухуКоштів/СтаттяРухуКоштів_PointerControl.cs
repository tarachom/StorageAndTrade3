
/*     
        СтаттяРухуКоштів_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class СтаттяРухуКоштів_PointerControl : PointerControl
    {
        event EventHandler<СтаттяРухуКоштів_Pointer> PointerChanged;

        public СтаттяРухуКоштів_PointerControl()
        {
            pointer = new СтаттяРухуКоштів_Pointer();
            WidthPresentation = 300;
            Caption = $"{СтаттяРухуКоштів_Const.FULLNAME}:";
            PointerChanged += async (object? _, СтаттяРухуКоштів_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        СтаттяРухуКоштів_Pointer pointer;
        public СтаттяРухуКоштів_Pointer Pointer
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
            Popover popover = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };
            BeforeClickOpenFunc?.Invoke();
            СтаттяРухуКоштів_ШвидкийВибір page = new СтаттяРухуКоштів_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new СтаттяРухуКоштів_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new СтаттяРухуКоштів_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
