<?xml version="1.0" encoding="utf-8"?>
<!--
/*
Copyright (C) 2019-2023 TARAKHOMYN YURIY IVANOVYCH
All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

/*
Автор:    Тарахомин Юрій Іванович
Адреса:   Україна, м. Львів
Сайт:     accounting.org.ua
*/
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="text" indent="yes" />

    <!-- Файл -->
    <xsl:param name="File" />

    <xsl:template match="root">

        <xsl:choose>

            <xsl:when test="$File = 'Element'">
                <xsl:call-template name="DocumentElement" />
            </xsl:when>
            <xsl:when test="$File = 'List'">
                <xsl:call-template name="DocumentList" />
            </xsl:when>
            <xsl:when test="$File = 'PointerControl'">
                <xsl:call-template name="DocumentPointerControl" />
            </xsl:when>

        </xsl:choose>

    </xsl:template>

<!--- 
//
// ============================ Елемент ============================
//
-->

    <!-- Елемент -->
    <xsl:template name="DocumentElement">
        <xsl:variable name="DocumentName" select="Document/Name"/>
        <xsl:variable name="Fields" select="Document/Fields/Field"/>
        <xsl:variable name="TabularParts" select="Document/TabularParts/TablePart"/>

/*
        <xsl:value-of select="$DocumentName"/>_Елемент.cs
        Елемент
*/

using Gtk;

using AccountingSoftware;

using StorageAndTrade_1_0;
using StorageAndTrade_1_0.Константи;
using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class <xsl:value-of select="$DocumentName"/>_Елемент : ДокументЕлемент
    {
        public <xsl:value-of select="$DocumentName"/>_Objest <xsl:value-of select="$DocumentName"/>_Objest { get; set; } = new <xsl:value-of select="$DocumentName"/>_Objest();

        #region Fields
        <xsl:for-each select="$Fields">
            <xsl:choose>
                <xsl:when test="Type = 'string'">
                     <xsl:choose>
                        <xsl:when test="Multiline = '1'">
                    TextView <xsl:value-of select="Name"/> = new TextView();
                        </xsl:when>
                        <xsl:otherwise>
                    Entry <xsl:value-of select="Name"/> = new Entry() { WidthRequest = 500 };
                        </xsl:otherwise>
                    </xsl:choose>
                </xsl:when>
                <xsl:when test="Type = 'integer'">
                    IntegerControl <xsl:value-of select="Name"/> = new IntegerControl();
                </xsl:when>
                <xsl:when test="Type = 'numeric'">
                    NumericControl <xsl:value-of select="Name"/> = new NumericControl();
                </xsl:when>
                <xsl:when test="Type = 'boolean'">
                    CheckButton <xsl:value-of select="Name"/> = new CheckButton("<xsl:value-of select="Name"/>");
                </xsl:when>
                <xsl:when test="Type = 'date' or Type = 'datetime'">
                    DateTimeControl <xsl:value-of select="Name"/> = new DateTimeControl()<xsl:if test="Type = 'date'">{ OnlyDate = true }</xsl:if>;
                </xsl:when>
                <xsl:when test="Type = 'time'">
                    TimeControl <xsl:value-of select="Name"/> = new TimeControl();
                </xsl:when>
                <xsl:when test="Type = 'composite_pointer'">
                    CompositePointerControl <xsl:value-of select="Name"/> = new CompositePointerControl();
                </xsl:when>
                <xsl:when test="Type = 'pointer'">
                    <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                    <xsl:value-of select="$namePointer"/>_PointerControl <xsl:value-of select="Name"/> = new <xsl:value-of select="$namePointer"/>_PointerControl() { Caption = "<xsl:value-of select="Name"/>", WidthPresentation = 300 };
                </xsl:when>
                <xsl:when test="Type = 'enum'">
                    <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                    ComboBoxText <xsl:value-of select="Name"/> = new ComboBoxText();
                </xsl:when>
                <xsl:when test="Type = 'any_pointer'">
                    //Guid <xsl:value-of select="Name"/> = new Guid();
                </xsl:when>
                <xsl:when test="Type = 'bytea'">
                    //byte[] <xsl:value-of select="Name"/> = new byte[]{ };
                </xsl:when>
                <xsl:when test="Type = 'string[]'">
                    //string[] <xsl:value-of select="Name"/> = new string[]{ };
                </xsl:when>
                <xsl:when test="Type = 'integer'">
                    //int[] <xsl:value-of select="Name"/> = new int[]{ };
                </xsl:when>
                <xsl:when test="Type = 'numeric'">
                    //decimal[] <xsl:value-of select="Name"/> = new decimal[]{ };
                </xsl:when>
            </xsl:choose>
        </xsl:for-each>
        #endregion

        #region TabularParts
        <xsl:for-each select="$TabularParts">
            <xsl:value-of select="$DocumentName"/>_ТабличнаЧастина_<xsl:value-of select="Name"/><xsl:text> </xsl:text>
            <xsl:value-of select="Name"/> = new <xsl:value-of select="$DocumentName"/>_ТабличнаЧастина_<xsl:value-of select="Name"/>();
        </xsl:for-each>
        #endregion

        public <xsl:value-of select="$DocumentName"/>_Елемент() : base()
        {
            CreateDocName(<xsl:value-of select="$DocumentName"/>_Const.FULLNAME, НомерДок, ДатаДок);

            <xsl:if test="count($Fields[Name = 'Коментар']) != 0">
                CreateField(HBoxComment, "Коментар:", Коментар);
            </xsl:if>

            <xsl:for-each select="$TabularParts">
                NotebookTablePart.InsertPage(<xsl:value-of select="Name"/>, new Label("<xsl:value-of select="Name"/>"), <xsl:value-of select="position() - 1"/>);
            </xsl:for-each>
            <xsl:if test="count($TabularParts) != 0">
                NotebookTablePart.CurrentPage = 0;
            </xsl:if>

            FillComboBoxes();
        }

        void FillComboBoxes()
        {
            <xsl:for-each select="$Fields">
                <xsl:if test="Type = 'enum'">
                    <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                    foreach (var field in ПсевдонімиПерелічення.<xsl:value-of select="$namePointer"/>_List())
                        <xsl:value-of select="Name"/>.Append(field.Value.ToString(), field.Name);
                </xsl:if>
            </xsl:for-each>
        }

        protected override void CreateContainer1(VBox vBox)
        {
            
        }

        protected override void CreateContainer2(VBox vBox)
        {
           
        }

        protected override void CreateContainer3(VBox vBox)
        {
            <!-- Крім полів які зразу добавляються в шапку НомерДок, ДатаДок, Коментар -->
            <xsl:for-each select="$Fields[Name != 'НомерДок' and Name != 'ДатаДок' and Name != 'Коментар']">
                //<xsl:value-of select="Name"/>
                <xsl:choose>
                    <xsl:when test="Type = 'string' or Type = 'integer' or Type = 'numeric' or Type = 'date' or Type = 'datetime' or Type = 'time'">
                        <xsl:choose>
                            <xsl:when test="Type = 'string' and Multiline = '1'">
                CreateFieldView(vBox, "<xsl:value-of select="Name"/>:", <xsl:value-of select="Name"/>, 500, 200);
                            </xsl:when>
                            <xsl:otherwise>
                CreateField(vBox, "<xsl:value-of select="Name"/>:", <xsl:value-of select="Name"/>);
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:when>
                    <xsl:when test="Type = 'composite_pointer' or Type = 'boolean'">
                CreateField(vBox, null, <xsl:value-of select="Name"/>);
                    </xsl:when>
                    <xsl:when test="Type = 'pointer'">
                CreateField(vBox, null, <xsl:value-of select="Name"/>);
                    </xsl:when>
                    <xsl:when test="Type = 'enum'">
                CreateField(vBox, "<xsl:value-of select="Name"/>:", <xsl:value-of select="Name"/>);
                    </xsl:when>
                </xsl:choose>
            </xsl:for-each>
        }

        protected override void CreateContainer4(VBox vBox)
        {
            
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
                <xsl:value-of select="$DocumentName"/>_Objest.New();

            <xsl:for-each select="$Fields">
                <xsl:choose>
                    <xsl:when test="Type = 'string'">
                        <xsl:choose>
                            <xsl:when test="Multiline = '1'">
                                <xsl:value-of select="Name"/>.Buffer.Text = <xsl:value-of select="$DocumentName"/>_Objest.<xsl:value-of select="Name"/>;
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:value-of select="Name"/>.Text = <xsl:value-of select="$DocumentName"/>_Objest.<xsl:value-of select="Name"/>;
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:when>
                    <xsl:when test="Type = 'integer' or Type = 'numeric' or Type = 'date' or Type = 'datetime' or Type = 'time'">
                        <xsl:value-of select="Name"/>.Value = <xsl:value-of select="$DocumentName"/>_Objest.<xsl:value-of select="Name"/>;
                    </xsl:when>
                    <xsl:when test="Type = 'boolean'">
                        <xsl:value-of select="Name"/>.Active = <xsl:value-of select="$DocumentName"/>_Objest.<xsl:value-of select="Name"/>;
                    </xsl:when>
                    <xsl:when test="Type = 'composite_pointer'">
                        <xsl:value-of select="Name"/>.Pointer = <xsl:value-of select="$DocumentName"/>_Objest.<xsl:value-of select="Name"/>;
                    </xsl:when>
                    <xsl:when test="Type = 'pointer'">
                        <xsl:value-of select="Name"/>.Pointer = <xsl:value-of select="$DocumentName"/>_Objest.<xsl:value-of select="Name"/>;
                    </xsl:when>
                    <xsl:when test="Type = 'enum'">
                        <xsl:value-of select="Name"/>.ActiveId = <xsl:value-of select="$DocumentName"/>_Objest.<xsl:value-of select="Name"/>.ToString();
                        if (<xsl:value-of select="Name"/>.Active == -1) <xsl:value-of select="Name"/>.Active = 0;
                    </xsl:when>
                    <xsl:when test="Type = 'boolean' or Type = 'any_pointer' or Type = 'bytea' or Type = 'string[]' or Type = 'integer' or Type = 'numeric'">
                        //<xsl:value-of select="Name"/> = <xsl:value-of select="$DocumentName"/>_Objest.<xsl:value-of select="Name"/>;
                    </xsl:when>
                </xsl:choose>
            </xsl:for-each>

            <xsl:for-each select="$TabularParts">
                /* Таблична частина: <xsl:value-of select="Name"/> */
                <xsl:value-of select="Name"/>.<xsl:value-of select="$DocumentName"/>_Objest = <xsl:value-of select="$DocumentName"/>_Objest;
                <xsl:value-of select="Name"/>.LoadRecords();
            </xsl:for-each>
        }

        protected override void GetValue()
        {
            <xsl:for-each select="$Fields">
                <xsl:choose>
                    <xsl:when test="Type = 'string'">
                        <xsl:choose>
                            <xsl:when test="Multiline = '1'">
                                <xsl:value-of select="$DocumentName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Buffer.Text;
                            </xsl:when>
                            <xsl:otherwise>
                                <xsl:value-of select="$DocumentName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Text;
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:when>
                     <xsl:when test="Type = 'integer' or Type = 'numeric' or Type = 'date' or Type = 'datetime' or Type = 'time'">
                        <xsl:value-of select="$DocumentName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Value;
                    </xsl:when>
                    <xsl:when test="Type = 'boolean'">
                        <xsl:value-of select="$DocumentName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Active;
                    </xsl:when>
                    <xsl:when test="Type = 'composite_pointer'">
                        <xsl:value-of select="$DocumentName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Pointer;
                    </xsl:when>
                    <xsl:when test="Type = 'pointer'">
                        <xsl:value-of select="$DocumentName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Pointer;
                    </xsl:when>
                    <xsl:when test="Type = 'enum'">
                        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                        <xsl:text>if (</xsl:text><xsl:value-of select="Name"/>.Active != -1) 
                            <xsl:value-of select="$DocumentName"/>_Objest.<xsl:value-of select="Name"/> = Enum.Parse&lt;<xsl:value-of select="$namePointer"/>&gt;(<xsl:value-of select="Name"/>.ActiveId);
                    </xsl:when>
                    <xsl:when test="Type = 'boolean' or Type = 'any_pointer' or Type = 'bytea' or Type = 'string[]' or Type = 'integer' or Type = 'numeric'">
                        //<xsl:value-of select="$DocumentName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>;
                    </xsl:when>
                </xsl:choose>
            </xsl:for-each>
        }

        #endregion

        protected override bool Save()
        {
            bool isSave = false;

            try
            {
                isSave = <xsl:value-of select="$DocumentName"/>_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
                return false;
            }

            if (isSave)
            {
                <xsl:for-each select="$TabularParts">
                    <xsl:value-of select="Name"/>.SaveRecords();
                </xsl:for-each>
            }

            UnigueID = <xsl:value-of select="$DocumentName"/>_Objest.UnigueID;
            Caption = <xsl:value-of select="$DocumentName"/>_Objest.Назва;

            return isSave;
        }

        protected override bool SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = <xsl:value-of select="$DocumentName"/>_Objest.SpendTheDocument(<xsl:value-of select="$DocumentName"/>_Objest.ДатаДок);

                if (!isSpend)
                    ФункціїДляПовідомлень.ВідкритиТермінал();

                return isSpend;
            }
            else
            {
                <xsl:value-of select="$DocumentName"/>_Objest.ClearSpendTheDocument();
                
                return true;
            }
        }
    }
}
    </xsl:template>

<!--- 
//
// ============================ Список ============================
//
-->

    <!-- Список -->
    <xsl:template name="DocumentList">
        <xsl:variable name="DocumentName" select="Document/Name"/>
        <xsl:variable name="TabularParts" select="Document/TabularParts/TablePart"/>

/*     
        <xsl:value-of select="$DocumentName"/>.cs
        Список
*/

using Gtk;

using AccountingSoftware;

using ТабличніСписки = StorageAndTrade_1_0.Документи.ТабличніСписки;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    public class <xsl:value-of select="$DocumentName"/> : ДокументЖурнал
    {
        public <xsl:value-of select="$DocumentName"/>() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.<xsl:value-of select="$DocumentName"/>_Записи.Store;
            ТабличніСписки.<xsl:value-of select="$DocumentName"/>_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.<xsl:value-of select="$DocumentName"/>_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.<xsl:value-of select="$DocumentName"/>_Записи.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.<xsl:value-of select="$DocumentName"/>_Записи.LoadRecords();

            if (ТабличніСписки.<xsl:value-of select="$DocumentName"/>_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.<xsl:value-of select="$DocumentName"/>_Записи.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.<xsl:value-of select="$DocumentName"/>_Записи.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.<xsl:value-of select="$DocumentName"/>_Записи.CurrentPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length &lt; 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.<xsl:value-of select="$DocumentName"/>_Записи.Where.Clear();

            //Назва
            ТабличніСписки.<xsl:value-of select="$DocumentName"/>_Записи.Where.Add(
                new Where(<xsl:value-of select="$DocumentName"/>_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.<xsl:value-of select="$DocumentName"/>_Записи.LoadRecords();

            if (ТабличніСписки.<xsl:value-of select="$DocumentName"/>_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.<xsl:value-of select="$DocumentName"/>_Записи.FirstPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{<xsl:value-of select="$DocumentName"/>_Const.FULLNAME} *", () =>
                {
                    <xsl:value-of select="$DocumentName"/>_Елемент page = new <xsl:value-of select="$DocumentName"/>_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadRecords,
                        IsNew = true
                    };

                    page.SetValue();

                    return page;
                });
            }
            else if (unigueID != null)
            {
                <xsl:value-of select="$DocumentName"/>_Objest <xsl:value-of select="$DocumentName"/>_Objest = new <xsl:value-of select="$DocumentName"/>_Objest();
                if (<xsl:value-of select="$DocumentName"/>_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{<xsl:value-of select="$DocumentName"/>_Objest.Назва}", () =>
                    {
                        <xsl:value-of select="$DocumentName"/>_Елемент page = new <xsl:value-of select="$DocumentName"/>_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            <xsl:value-of select="$DocumentName"/>_Objest = <xsl:value-of select="$DocumentName"/>_Objest,
                        };

                        page.SetValue();

                        return page;
                    });
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }

        protected override void SetDeletionLabel(UnigueID unigueID)
        {
            <xsl:value-of select="$DocumentName"/>_Objest <xsl:value-of select="$DocumentName"/>_Objest = new <xsl:value-of select="$DocumentName"/>_Objest();
            if (<xsl:value-of select="$DocumentName"/>_Objest.Read(unigueID))
                <xsl:value-of select="$DocumentName"/>_Objest.SetDeletionLabel(!<xsl:value-of select="$DocumentName"/>_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override UnigueID? Copy(UnigueID unigueID)
        {
            <xsl:value-of select="$DocumentName"/>_Objest <xsl:value-of select="$DocumentName"/>_Objest = new <xsl:value-of select="$DocumentName"/>_Objest();
            if (<xsl:value-of select="$DocumentName"/>_Objest.Read(unigueID))
            {
                <xsl:value-of select="$DocumentName"/>_Objest <xsl:value-of select="$DocumentName"/>_Objest_Новий = <xsl:value-of select="$DocumentName"/>_Objest.Copy(true);
                <xsl:value-of select="$DocumentName"/>_Objest_Новий.Save();
                <xsl:for-each select="$TabularParts">
                    /* Таблична частина: <xsl:value-of select="Name"/> */
                    <xsl:value-of select="$DocumentName"/>_Objest_Новий.<xsl:value-of select="Name"/>_TablePart.Save(false);
                </xsl:for-each>
                return <xsl:value-of select="$DocumentName"/>_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        protected override void PeriodWhereChanged()
        {
            ТабличніСписки.<xsl:value-of select="$DocumentName"/>_Записи.ДодатиВідбірПоПеріоду(Enum.Parse&lt;ТипПеріодуДляЖурналівДокументів&gt;(ComboBoxPeriodWhere.ActiveId));
            LoadRecords();
        }

        protected override void SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            <xsl:value-of select="$DocumentName"/>_Pointer <xsl:value-of select="$DocumentName"/>_Pointer = new <xsl:value-of select="$DocumentName"/>_Pointer(unigueID);
            <xsl:value-of select="$DocumentName"/>_Objest? <xsl:value-of select="$DocumentName"/>_Objest = <xsl:value-of select="$DocumentName"/>_Pointer.GetDocumentObject(true);
            if (<xsl:value-of select="$DocumentName"/>_Objest == null) return;

            if (spendDoc)
            {
                if (!<xsl:value-of select="$DocumentName"/>_Objest.SpendTheDocument(<xsl:value-of select="$DocumentName"/>_Objest.ДатаДок))
                    ФункціїДляПовідомлень.ВідкритиТермінал();
            }
            else
                <xsl:value-of select="$DocumentName"/>_Objest.ClearSpendTheDocument();
        }

        protected override DocumentPointer? ReportSpendTheDocument(UnigueID unigueID)
        {
            return new <xsl:value-of select="$DocumentName"/>_Pointer(unigueID);
        }

        protected override void ExportXML(UnigueID unigueID)
        {
            string pathToSave = System.IO.Path.Combine(AppContext.BaseDirectory, $"{<xsl:value-of select="$DocumentName"/>_Const.FULLNAME}_{unigueID}.xml");
            <xsl:value-of select="$DocumentName"/>_Export.ToXmlFile(new <xsl:value-of select="$DocumentName"/>_Pointer(unigueID), pathToSave);
        }

        #endregion
    }
}
    </xsl:template>

<!--- 
//
// ============================ PointerControl ============================
//
-->

    <!-- PointerControl -->
    <xsl:template name="DocumentPointerControl">
        <xsl:variable name="DocumentName" select="Document/Name"/>

/*     
        <xsl:value-of select="$DocumentName"/>_PointerControl.cs
        PointerControl
*/

using AccountingSoftware;
using StorageAndTrade_1_0.Документи;

namespace StorageAndTrade
{
    class <xsl:value-of select="$DocumentName"/>_PointerControl : PointerControl
    {
        public <xsl:value-of select="$DocumentName"/>_PointerControl()
        {
            pointer = new <xsl:value-of select="$DocumentName"/>_Pointer();
            WidthPresentation = 300;
            Caption = $"{<xsl:value-of select="$DocumentName"/>_Const.FULLNAME}:";
        }

        <xsl:value-of select="$DocumentName"/>_Pointer pointer;
        public <xsl:value-of select="$DocumentName"/>_Pointer Pointer
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

        //Відбір по періоду в журналі
        public bool UseWherePeriod { get; set; } = true;

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            <xsl:value-of select="$DocumentName"/> page = new <xsl:value-of select="$DocumentName"/>();

            page.DocumentPointerItem = Pointer.UnigueID;
            page.CallBack_OnSelectPointer = (UnigueID selectPointer) =&gt;
            {
                Pointer = new <xsl:value-of select="$DocumentName"/>_Pointer(selectPointer);
            };

            Program.GeneralForm?.CreateNotebookPage($"Вибір - {<xsl:value-of select="$DocumentName"/>_Const.FULLNAME}", () =&gt; { return page; }, true);

            if (UseWherePeriod)
                page.SetValue();
            else
                page.LoadRecords();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new <xsl:value-of select="$DocumentName"/>_Pointer();
        }
    }
}
    </xsl:template>

</xsl:stylesheet>