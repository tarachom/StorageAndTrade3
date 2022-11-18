
using StorageAndTrade_1_0.Довідники;

namespace StorageAndTrade
{
    class Контрагенти_Папки_PointerControl : PointerControl
    {
        public Контрагенти_Папки_PointerControl()
        {
            pointer = new Контрагенти_Папки_Pointer();
            WidthPresentation = 300;
            Caption = "Контрагент папка:";
        }

        public string UidOpenFolder { get; set; } = "";

        Контрагенти_Папки_Pointer pointer;
        public Контрагенти_Папки_Pointer Pointer
        {
            get
            {
                return pointer;
            }
            set
            {
                pointer = value;

                if (pointer != null)
                    Presentation = pointer.GetPresentation();
                else
                    Presentation = "";
            }
        }

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            Program.GeneralForm?.CreateNotebookPage("Вибір - Довідник: Контрагенти Папки", () =>
            {
                Контрагенти_Папки_Дерево page = new Контрагенти_Папки_Дерево(true);

                page.DirectoryPointerItem = Pointer;
                page.UidOpenFolder = UidOpenFolder;
                page.CallBack_OnSelectPointer = (Контрагенти_Папки_Pointer selectPointer) =>
                {
                    Pointer = selectPointer;
                };

                page.LoadTree();

                return page;
            });
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new Контрагенти_Папки_Pointer();
        }
    }
}