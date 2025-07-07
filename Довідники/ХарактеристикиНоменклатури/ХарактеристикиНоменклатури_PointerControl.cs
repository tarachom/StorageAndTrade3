
/*     
        ХарактеристикиНоменклатури_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode.Довідники;

namespace StorageAndTrade
{
    class ХарактеристикиНоменклатури_PointerControl : PointerControl
    {
        event EventHandler<ХарактеристикиНоменклатури_Pointer> PointerChanged;

        public ХарактеристикиНоменклатури_PointerControl()
        {
            pointer = new ХарактеристикиНоменклатури_Pointer();
            WidthPresentation = 300;
            Caption = $"{ХарактеристикиНоменклатури_Const.FULLNAME}:";
            PointerChanged += async (object? _, ХарактеристикиНоменклатури_Pointer pointer) =>
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        ХарактеристикиНоменклатури_Pointer pointer;
        public ХарактеристикиНоменклатури_Pointer Pointer
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


        public Номенклатура_Pointer Власник { get; set; } = new Номенклатура_Pointer();


        protected override async void OpenSelect(object? sender, EventArgs args)
        {
            Popover popover = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };
            BeforeClickOpenFunc?.Invoke();
            ХарактеристикиНоменклатури_ШвидкийВибір page = new ХарактеристикиНоменклатури_ШвидкийВибір()
            {
                PopoverParent = popover,
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =>
                {
                    Pointer = new ХарактеристикиНоменклатури_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };

            page.Власник.Pointer = Власник;

            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new ХарактеристикиНоменклатури_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
