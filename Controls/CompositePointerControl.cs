/*

CompositePointerControl - контрол тип якого оприділяється в процесі
Використовується для Довідників та Документів
В базі даних цей тип представлений композитним типом UuidAndText

*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using GeneratedCode;

namespace StorageAndTrade
{
    class CompositePointerControl : InterfaceGtk3.CompositePointerControl
    {
        public CompositePointerControl() : base(Config.Kernel, Config.NameSpageProgram, Config.NameSpageCodeGeneration) { }

        protected override async ValueTask<CompositePointerPresentation_Record> CompositePointerPresentation(UuidAndText uuidAndText)
        {
            return await Functions.CompositePointerPresentation(uuidAndText);
        }
        protected override void CreateNotebookPage(string tabName, Func<Widget>? pageWidget)
        {
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, tabName, pageWidget);
        }
    }
}