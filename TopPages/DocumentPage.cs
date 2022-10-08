using Gtk;
using System;
using System.IO;

using AccountingSoftware;

namespace StorageAndTrade
{
    class DocumentPage : VBox
    {
        #region Notebook

        private Notebook DocumentNotebook;
        private Dictionary<int, NotebookPage> NotebookPagesDictionary;
        private Dictionary<int, NameValue<System.Action<NotebookPage>>> PageAndActionDictionary;

        #endregion

        public DocumentPage() : base()
        {
            DocumentNotebook = new Notebook() { BorderWidth = 0, ShowBorder = false };
            DocumentNotebook.TabPos = PositionType.Top;

            NotebookPagesDictionary = new Dictionary<int, NotebookPage>();
            PageAndActionDictionary = new Dictionary<int, NameValue<Action<NotebookPage>>>();

            int counter = 0;

            PageAndActionDictionary.Add(counter, new NameValue<Action<NotebookPage>>("Головна", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Продажі", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            // PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Рахунок", (NotebookPage page) =>
            //     {
            //         /**/
            //     }
            // ));

            // PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Акт", (NotebookPage page) =>
            //     {
            //         /**/
            //     }
            // ));

            // PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Реалізація", (NotebookPage page) =>
            //     {
            //         /**/
            //     }
            // ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Покупки", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Каса", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Склад", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Внутрішні", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            // PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Переміщення", (NotebookPage page) =>
            //     {
            //         /**/
            //     }
            // ));

            // PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Внутрінє", (NotebookPage page) =>
            //     {
            //         /**/
            //     }
            // ));

            // PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Псування", (NotebookPage page) =>
            //     {
            //         /**/
            //     }
            // ));

            CreateTopNotebookPages();

            PackStart(DocumentNotebook, true, true, 0);

            ShowAll();

            DocumentNotebook.SwitchPage += OnTopNotebookSelectPage;
            OnTopNotebookSelectPage(null, new SwitchPageArgs());
        }

        void CreateTopNotebookPages()
        {
            foreach (NameValue<Action<NotebookPage>> page in PageAndActionDictionary.Values)
            {
                ScrolledWindow scroll = new ScrolledWindow() { ShadowType = ShadowType.In };
                scroll.SetPolicy(PolicyType.Never, PolicyType.Automatic);

                Label lb = new Label { Text = page.Name, Expand = false, Halign = Align.Start };

                int numPage = DocumentNotebook.AppendPage(scroll, lb);

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
            NotebookPage notebookPage = NotebookPagesDictionary[DocumentNotebook.CurrentPage];

            if (!notebookPage.IsConstruct)
                PageAndActionDictionary[DocumentNotebook.CurrentPage]?.Value?.Invoke(notebookPage);
        }
    }
}
