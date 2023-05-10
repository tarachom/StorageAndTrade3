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

    <!-- Розділ -->
    <xsl:param name="Section" />

    <!-- Файл -->
    <xsl:param name="File" />

    <xsl:template match="root">

        <xsl:choose>
            <xsl:when test="$Section = 'Directory'">

                <xsl:choose>
                    <xsl:when test="$File = 'Element'">
                        <xsl:call-template name="DirectoryElement" />
                    </xsl:when>
                    <xsl:when test="$File = 'List'">
                        <xsl:call-template name="DirectoryList" />
                    </xsl:when>
                </xsl:choose>

            </xsl:when>
        </xsl:choose>

    </xsl:template>

    <xsl:template name="DirectoryList">
        <xsl:variable name="DirectoryName" select="Directory/Name"/>
        <xsl:variable name="Fields" select="Directory/Fields/Field"/>

/*     файл:     <xsl:value-of select="$DirectoryName"/>.cs     */

using AccountingSoftware;
using StorageAndTrade_1_0.Довідники;
using ТабличніСписки = StorageAndTrade_1_0.Довідники.ТабличніСписки;

namespace StorageAndTrade
{
    public class <xsl:value-of select="$DirectoryName"/> : ДовідникЖурнал
    {
        public <xsl:value-of select="$DirectoryName"/>() : base()
        {
            TreeViewGrid.Model = ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.Store;
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.AddColumns(TreeViewGrid);
        }

        #region Override

        public override void LoadRecords()
        {
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.Where.Clear();

            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.LoadRecords();

            if (ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.SelectPath, TreeViewGrid.Columns[0], false);

            TreeViewGrid.GrabFocus();
        }

        protected override void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length &lt; 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.Where.Clear();

            //Назва
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.Where.Add(
                new Where(<xsl:value-of select="$DirectoryName"/>_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.LoadRecords();

            if (ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.FirstPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_Записи.FirstPath, TreeViewGrid.Columns[0], false);
        }

        protected override void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                Program.GeneralForm?.CreateNotebookPage($"{<xsl:value-of select="$DirectoryName"/>_Const.FULLNAME} *", () =>
                {
                    <xsl:value-of select="$DirectoryName"/>_Елемент page = new <xsl:value-of select="$DirectoryName"/>_Елемент
                    {
                        CallBack_LoadRecords = CallBack_LoadRecords,
                        IsNew = true
                    };

                    page.SetValue();

                    return page;
                }, true);
            }
            else if (unigueID != null)
            {
                <xsl:value-of select="$DirectoryName"/>_Objest <xsl:value-of select="$DirectoryName"/>_Objest = new <xsl:value-of select="$DirectoryName"/>_Objest();
                if (<xsl:value-of select="$DirectoryName"/>_Objest.Read(unigueID))
                {
                    Program.GeneralForm?.CreateNotebookPage($"{<xsl:value-of select="$DirectoryName"/>_Objest.Назва}", () =>
                    {
                        <xsl:value-of select="$DirectoryName"/>_Елемент page = new <xsl:value-of select="$DirectoryName"/>_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            <xsl:value-of select="$DirectoryName"/>_Objest = <xsl:value-of select="$DirectoryName"/>_Objest,
                        };

                        page.SetValue();

                        return page;
                    }, true);
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }

        protected override void SetDeletionLabel(UnigueID unigueID)
        {
            <xsl:value-of select="$DirectoryName"/>_Objest <xsl:value-of select="$DirectoryName"/>_Objest = new <xsl:value-of select="$DirectoryName"/>_Objest();
            if (<xsl:value-of select="$DirectoryName"/>_Objest.Read(unigueID))
                <xsl:value-of select="$DirectoryName"/>_Objest.SetDeletionLabel(!<xsl:value-of select="$DirectoryName"/>_Objest.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override UnigueID? Copy(UnigueID unigueID)
        {
            <xsl:value-of select="$DirectoryName"/>_Objest <xsl:value-of select="$DirectoryName"/>_Objest = new <xsl:value-of select="$DirectoryName"/>_Objest();
            if (<xsl:value-of select="$DirectoryName"/>_Objest.Read(unigueID))
            {
                <xsl:value-of select="$DirectoryName"/>_Objest <xsl:value-of select="$DirectoryName"/>_Objest_Новий = <xsl:value-of select="$DirectoryName"/>_Objest.Copy(true);
                <xsl:value-of select="$DirectoryName"/>_Objest_Новий.Save();

                return <xsl:value-of select="$DirectoryName"/>_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        #endregion
    }
}
    </xsl:template>

    <xsl:template name="DirectoryElement">
        <xsl:variable name="DirectoryName" select="Directory/Name"/>
        <xsl:variable name="Fields" select="Directory/Fields/Field"/>
        <xsl:variable name="TabularParts" select="Directory/TabularParts/TablePart"/>

/*     файл:     <xsl:value-of select="$DirectoryName"/>_Елемент.cs     */

using Gtk;

using StorageAndTrade_1_0.Довідники;
using StorageAndTrade_1_0.Документи;
using StorageAndTrade_1_0.Перелічення;

namespace StorageAndTrade
{
    class <xsl:value-of select="$DirectoryName"/>_Елемент : ДовідникЕлемент
    {
        public <xsl:value-of select="$DirectoryName"/>_Objest <xsl:value-of select="$DirectoryName"/>_Objest { get; set; } = new <xsl:value-of select="$DirectoryName"/>_Objest();
        
        #region Fields
        <xsl:for-each select="$Fields">
            <xsl:choose>
                <xsl:when test="Type = 'string'">
                    Entry <xsl:value-of select="Name"/> = new Entry() { WidthRequest = 500 };
                </xsl:when>
                <xsl:when test="Type = 'integer'">
                    IntegerControl <xsl:value-of select="Name"/> = new IntegerControl();
                </xsl:when>
                <xsl:when test="Type = 'numeric'">
                    NumericControl <xsl:value-of select="Name"/> = new NumericControl();
                </xsl:when>
                <xsl:when test="Type = 'boolean'">
                    bool <xsl:value-of select="Name"/>;
                </xsl:when>
                <xsl:when test="Type = 'date' or Type = 'datetime'">
                    DateTimeControl <xsl:value-of select="Name"/> = new DateTimeControl()<xsl:if test="Type = 'date'">{ OnlyDate = true }</xsl:if>;
                </xsl:when>
                <xsl:when test="Type = 'time'">
                    TimeControl <xsl:value-of select="Name"/> = new TimeControl();
                </xsl:when>
                <xsl:when test="Type = 'composite_pointer'">
                    Basis_PointerControl <xsl:value-of select="Name"/> = new Basis_PointerControl();
                </xsl:when>
                <xsl:when test="Type = 'pointer'">
                    <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                    <xsl:value-of select="$namePointer"/>_PointerControl <xsl:value-of select="$namePointer"/> = new <xsl:value-of select="$namePointer"/>_PointerControl() { Caption = "<xsl:value-of select="Name"/>", WidthPresentation = 300 };
                </xsl:when>
                <xsl:when test="Type = 'enum'">
                    <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                    ComboBoxText <xsl:value-of select="$namePointer"/> = new ComboBoxText();
                </xsl:when>
                <xsl:when test="Type = 'any_pointer'">
                    Guid <xsl:value-of select="Name"/> = new Guid();
                </xsl:when>
                <xsl:when test="Type = 'bytea'">
                    byte[] <xsl:value-of select="Name"/> = new byte[]{ };
                </xsl:when>
                <xsl:when test="Type = 'string[]'">
                    string[] <xsl:value-of select="Name"/> = new string[]{ };
                </xsl:when>
                <xsl:when test="Type = 'integer'">
                    int[] <xsl:value-of select="Name"/> = new int[]{ };
                </xsl:when>
                <xsl:when test="Type = 'numeric'">
                    decimal[] <xsl:value-of select="Name"/> = new decimal[]{ };
                </xsl:when>
            </xsl:choose>
        </xsl:for-each>
        #endregion

        #region TabularParts
        <xsl:for-each select="$TabularParts">
            <xsl:value-of select="$DirectoryName"/>_ТабличнаЧастина_<xsl:value-of select="Name"/><xsl:text> </xsl:text>
                <xsl:value-of select="Name"/> = new <xsl:value-of select="$DirectoryName"/>_ТабличнаЧастина_<xsl:value-of select="Name"/>();
        </xsl:for-each>
        #endregion

        public <xsl:value-of select="$DirectoryName"/>_Елемент() : base() 
        { 
            <xsl:for-each select="$Fields">
                <xsl:if test="Type = 'enum'">
                    <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                    foreach (var field in ПсевдонімиПерелічення.<xsl:value-of select="$namePointer"/>_List())
                        <xsl:value-of select="$namePointer"/>.Append(field.Value.ToString(), field.Name);
                </xsl:if>
            </xsl:for-each>
        }

        protected override void CreatePack1(VBox vBox)
        {
            <xsl:for-each select="$Fields">
                <xsl:choose>
                    <xsl:when test="Type = 'string' or Type = 'integer' or Type = 'numeric' or Type = 'date' or Type = 'datetime' or Type = 'time'">
                        CreateField(vBox, "<xsl:value-of select="Name"/>:", <xsl:value-of select="Name"/>);
                    </xsl:when>
                    <xsl:when test="Type = 'composite_pointer'">
                        CreateField(vBox, null, <xsl:value-of select="Name"/>);
                    </xsl:when>
                    <xsl:when test="Type = 'pointer'">
                        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                        CreateField(vBox, null, <xsl:value-of select="$namePointer"/>);
                    </xsl:when>
                    <xsl:when test="Type = 'enum'">
                        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                        CreateField(vBox, "<xsl:value-of select="Name"/>:", <xsl:value-of select="$namePointer"/>);
                    </xsl:when>
                </xsl:choose>
            </xsl:for-each>
        }

        protected override void CreatePack2(VBox vBox)
        {
            <xsl:for-each select="$TabularParts">
                CreateTablePart(vBox, "<xsl:value-of select="Name"/>:", <xsl:value-of select="Name"/>);
            </xsl:for-each>
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {
            if (IsNew)
                <xsl:value-of select="$DirectoryName"/>_Objest.New();

            <xsl:for-each select="$Fields">
                <xsl:choose>
                    <xsl:when test="Type = 'string'">
                        <xsl:value-of select="Name"/>.Text = <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/>;
                    </xsl:when>
                    <xsl:when test="Type = 'integer' or Type = 'numeric' or Type = 'date' or Type = 'datetime' or Type = 'time'">
                        <xsl:value-of select="Name"/>.Value = <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/>;
                    </xsl:when>
                    <xsl:when test="Type = 'composite_pointer'">
                        <xsl:value-of select="Name"/>.Pointer = <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/>;
                    </xsl:when>
                    <xsl:when test="Type = 'pointer'">
                        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                        <xsl:value-of select="$namePointer"/>.Pointer = <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/>;
                    </xsl:when>
                    <xsl:when test="Type = 'enum'">
                        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                        <xsl:value-of select="$namePointer"/>.ActiveId = <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/>.ToString();
                        if (<xsl:value-of select="$namePointer"/>.Active == -1) <xsl:value-of select="$namePointer"/>.Active = 0;
                    </xsl:when>
                    <xsl:when test="Type = 'boolean' or Type = 'any_pointer' or Type = 'bytea' or Type = 'string[]' or Type = 'integer' or Type = 'numeric'">
                        <xsl:value-of select="Name"/> = <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/>;
                    </xsl:when>
                </xsl:choose>
            </xsl:for-each>

            <xsl:for-each select="$TabularParts">
                /* Таблична частина: <xsl:value-of select="Name"/> */
                <xsl:value-of select="Name"/>.<xsl:value-of select="$DirectoryName"/>_Objest = <xsl:value-of select="$DirectoryName"/>_Objest;
                <xsl:value-of select="Name"/>.LoadRecords();
            </xsl:for-each>
        }

        protected override void GetValue()
        {
            UnigueID = <xsl:value-of select="$DirectoryName"/>_Objest.UnigueID;
            Caption = Назва.Text;

            <xsl:for-each select="$Fields">
                <xsl:choose>
                    <xsl:when test="Type = 'string'">
                        <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Text;
                    </xsl:when>
                     <xsl:when test="Type = 'integer' or Type = 'numeric' or Type = 'date' or Type = 'datetime' or Type = 'time'">
                        <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Value;
                    </xsl:when>
                    <xsl:when test="Type = 'composite_pointer'">
                        <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Pointer;
                    </xsl:when>
                    <xsl:when test="Type = 'pointer'">
                        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                        <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="$namePointer"/>.Pointer;
                    </xsl:when>
                    <xsl:when test="Type = 'enum'">
                        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                        <xsl:text>if (</xsl:text><xsl:value-of select="$namePointer"/>.Active != -1) 
                            <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/> = Enum.Parse&lt;<xsl:value-of select="$namePointer"/>&gt;(<xsl:value-of select="$namePointer"/>.ActiveId);
                    </xsl:when>
                    <xsl:when test="Type = 'boolean' or Type = 'any_pointer' or Type = 'bytea' or Type = 'string[]' or Type = 'integer' or Type = 'numeric'">
                        <xsl:value-of select="$DirectoryName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>;
                    </xsl:when>
                </xsl:choose>
            </xsl:for-each>
        }

        #endregion

        protected override void Save()
        {
            try
            {
                <xsl:value-of select="$DirectoryName"/>_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
            }

            <xsl:for-each select="$TabularParts">
                <xsl:value-of select="Name"/>.SaveRecords();
            </xsl:for-each>
        }
    }
}
    </xsl:template>

</xsl:stylesheet>