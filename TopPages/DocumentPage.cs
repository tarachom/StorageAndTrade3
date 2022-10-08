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
            DocumentNotebook = new Notebook();
            DocumentNotebook.TabPos = PositionType.Left;

            NotebookPagesDictionary = new Dictionary<int, NotebookPage>();
            PageAndActionDictionary = new Dictionary<int, NameValue<Action<NotebookPage>>>();

            int counter = 0;

            PageAndActionDictionary.Add(counter, new NameValue<Action<NotebookPage>>("Головна", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Замовлення клієнтів", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Рахунок фактура", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Акт виконаних робіт", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Реалізація товарів", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Замовлення постач.", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Поступлення товарів", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Прихідний ордер", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Розхідний ордер", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Переміщення тов.", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Внутрінє спож.", (NotebookPage page) =>
                {
                    /**/
                }
            ));

            PageAndActionDictionary.Add(++counter, new NameValue<Action<NotebookPage>>("Псування товарів", (NotebookPage page) =>
                {
                    /**/
                }
            ));

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
