/*

Повнотекстовий пошук

*/

using StorageAndTrade_1_0;

namespace StorageAndTrade
{
    class PageFullTextSearch : InterfaceGtk.PageFullTextSearch
    {
        public PageFullTextSearch() : base(Config.Kernel, Config.NameSpageProgram, Config.NameSpageCodeGeneration) { }
    }
}