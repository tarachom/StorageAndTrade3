using Gtk;
using System;
using System.IO;

using AccountingSoftware;

namespace StorageAndTrade
{
    class DirectoryPage : VBox
    {
        #region Notebook

        private Notebook DirectoryNotebook;
        private Dictionary<int, NotebookPage> NotebookPagesDictionary;
        private Dictionary<int, NameValue<System.Action<NotebookPage>>> PageAndActionDictionary;

        #endregion

        public DirectoryPage() : base()
        {
            DirectoryNotebook = new Notebook();
            DirectoryNotebook.TabPos = PositionType.Top;

            NotebookPagesDictionary = new Dictionary<int, NotebookPage>();
            PageAndActionDictionary = new Dictionary<int, NameValue<Action<NotebookPage>>>();

            int counter = 0;

            PageAndActionDictionary.Add(counter, new NameValue<Action<NotebookPage>>("Головна", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Організації", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Валюти", (NotebookPage page) =>
                {
                    Валюти валюти = new Валюти();
                    page.AddVBox(валюти);

                    валюти.LoadRecords();
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Номенклатура", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Контрагенти", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Склади", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            // PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Файли", (NotebookPage page) =>
            //     {
            //         /**/
            //     }
            // ));

            CreateTopNotebookPages();

            PackStart(DirectoryNotebook, true, true, 0);

            ShowAll();

            DirectoryNotebook.SwitchPage += OnTopNotebookSelectPage;
            OnTopNotebookSelectPage(null, new SwitchPageArgs());
        }

        void CreateTopNotebookPages()
        {
            foreach (NameValue<Action<NotebookPage>> page in PageAndActionDictionary.Values)
            {
                ScrolledWindow scroll = new ScrolledWindow() { ShadowType = ShadowType.In };
                scroll.SetPolicy(PolicyType.Never, PolicyType.Automatic);

                int numPage = DirectoryNotebook.AppendPage(scroll, new Label { Text = page.Name, Expand = false, Halign = Align.End });

                NotebookPagesDictionary.Add(numPage,
                    new NotebookPage
                    {
                        NumPage = numPage,
                        NamePage = page.Name,
                        ScrolledWindow = scroll
                    });
            }
        }

        void OnTopNotebookSelectPage(object? sender, SwitchPageArgs args)
        {
            NotebookPage notebookPage = NotebookPagesDictionary[DirectoryNotebook.CurrentPage];

            if (!notebookPage.IsConstruct)
                PageAndActionDictionary[DirectoryNotebook.CurrentPage]?.Value?.Invoke(notebookPage);
        }
    }
}
