/*

Динамічне відкриття журналів

*/

using Gtk;
using InterfaceGtk;
using StorageAndTrade_1_0;

namespace StorageAndTrade
{
    class ФункціїДляДинамічногоВідкриття : InterfaceGtk.ФункціїДляДинамічногоВідкриття
    {
        public ФункціїДляДинамічногоВідкриття() : base(Config.NameSpageProgram, Config.NameSpageCodeGeneration) { }

        protected override void CreateNotebookPage(string tabName, Func<Widget>? pageWidget)
        {
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, tabName, pageWidget);
        }
    }
}