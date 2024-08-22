<?xml version="1.0" encoding="utf-8"?>
<!--
/*
Copyright (C) 2019-2024 TARAKHOMYN YURIY IVANOVYCH
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

    <!-- Простори імен -->
    <xsl:param name="NameSpaceGenerationCode" />
    <xsl:param name="NameSpace" />

    <xsl:template match="root">

        <xsl:choose>
            <xsl:when test="$File = 'Element'">
                <xsl:call-template name="RegisterInformationElement" />
            </xsl:when>
            <xsl:when test="$File = 'List'">
                <xsl:call-template name="RegisterInformationList" />
            </xsl:when>
        </xsl:choose>

    </xsl:template>

<!--- 
//
// ============================ Елемент ============================
//
-->

    <!-- Елемент -->
    <xsl:template name="RegisterInformationElement">
        <xsl:variable name="RegisterInformationName" select="RegisterInformation/Name"/>
        <xsl:variable name="Fields" select="RegisterInformation/Fields/Field"/>
        <xsl:variable name="FormElementField" select="RegisterInformation/ElementFields/ElementField"/>

/*
        <xsl:value-of select="$RegisterInformationName"/>_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk;
using <xsl:value-of select="$NameSpaceGenerationCode"/>.РегістриВідомостей;

namespace <xsl:value-of select="$NameSpace"/>
{
    class <xsl:value-of select="$RegisterInformationName"/>_Елемент : РегістриЕлемент
    {
        public <xsl:value-of select="$RegisterInformationName"/>_Objest <xsl:value-of select="$RegisterInformationName"/>_Objest { get; set; } = new <xsl:value-of select="$RegisterInformationName"/>_Objest();
        DateTimeControl Період = new DateTimeControl();

        #region Fields
        <xsl:for-each select="$Fields">
            <xsl:variable name="FieldName" select="Name" />
            <xsl:if test="$FormElementField[Name = $FieldName]">
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
                        CompositePointerControl <xsl:value-of select="Name"/> = new CompositePointerControl() { BoundConfType = "Довідники.<xsl:value-of select="$RegisterInformationName"/>.<xsl:value-of select="Name"/>" };
                    </xsl:when>
                    <xsl:when test="Type = 'pointer'">
                        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                        <xsl:value-of select="$namePointer"/>_PointerControl <xsl:value-of select="Name"/> = new <xsl:value-of select="$namePointer"/>_PointerControl() { Caption = "<xsl:value-of select="$FormElementField[Name = $FieldName]/Caption"/>", WidthPresentation = 300 };
                    </xsl:when>
                    <xsl:when test="Type = 'enum'">
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
            </xsl:if>
        </xsl:for-each>
        #endregion

        public <xsl:value-of select="$RegisterInformationName"/>_Елемент() : base() 
        { 
            <xsl:for-each select="$Fields">
                <xsl:variable name="FieldName" select="Name" />
                <xsl:if test="$FormElementField[Name = $FieldName] and Type = 'enum'">
                    <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                    foreach (var field in ПсевдонімиПерелічення.<xsl:value-of select="$namePointer"/>_List())
                        <xsl:value-of select="Name"/>.Append(field.Value.ToString(), field.Name);
                </xsl:if>
            </xsl:for-each>
        }

        protected override void CreatePack1(Box vBox)
        {
            //Період
            CreateField(vBox, "Період:", Період);
            <xsl:for-each select="$Fields">
                <xsl:variable name="FieldName" select="Name" />
                <xsl:if test="$FormElementField[Name = $FieldName]">
                    <xsl:variable name="Caption" select="$FormElementField[Name = $FieldName]/Caption" />
                    //<xsl:value-of select="Name"/>
                    <xsl:choose>
                        <xsl:when test="Type = 'string' or Type = 'integer' or Type = 'numeric' or Type = 'date' or Type = 'datetime' or Type = 'time'">
                            <xsl:choose>
                                <xsl:when test="Type = 'string' and Multiline = '1'">
                    CreateFieldView(vBox, "<xsl:value-of select="$Caption"/>:", <xsl:value-of select="Name"/>, 500, 200);
                                </xsl:when>
                                <xsl:otherwise>
                    CreateField(vBox, "<xsl:value-of select="$Caption"/>:", <xsl:value-of select="Name"/>);
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
                    CreateField(vBox, "<xsl:value-of select="$Caption"/>:", <xsl:value-of select="Name"/>);
                        </xsl:when>
                    </xsl:choose>
                </xsl:if>
            </xsl:for-each>
        }

        protected override void CreatePack2(Box vBox)
        {
            
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {            
            if (IsNew)
                <xsl:value-of select="$RegisterInformationName"/>_Objest.New();

            Період.Value = <xsl:value-of select="$RegisterInformationName"/>_Objest.Period;
            
            <xsl:for-each select="$Fields">
                <xsl:variable name="FieldName" select="Name" />
                <xsl:if test="$FormElementField[Name = $FieldName]">
                    <xsl:choose>
                        <xsl:when test="Type = 'string'">
                            <xsl:choose>
                                <xsl:when test="Multiline = '1'">
                                    <xsl:value-of select="Name"/>.Buffer.Text = <xsl:value-of select="$RegisterInformationName"/>_Objest.<xsl:value-of select="Name"/>;
                                </xsl:when>
                                <xsl:otherwise>
                                    <xsl:value-of select="Name"/>.Text = <xsl:value-of select="$RegisterInformationName"/>_Objest.<xsl:value-of select="Name"/>;
                                </xsl:otherwise>
                            </xsl:choose>
                        </xsl:when>
                        <xsl:when test="Type = 'integer' or Type = 'numeric' or Type = 'date' or Type = 'datetime' or Type = 'time'">
                            <xsl:value-of select="Name"/>.Value = <xsl:value-of select="$RegisterInformationName"/>_Objest.<xsl:value-of select="Name"/>;
                        </xsl:when>
                        <xsl:when test="Type = 'boolean'">
                            <xsl:value-of select="Name"/>.Active = <xsl:value-of select="$RegisterInformationName"/>_Objest.<xsl:value-of select="Name"/>;
                        </xsl:when>
                        <xsl:when test="Type = 'composite_pointer'">
                            <xsl:value-of select="Name"/>.Pointer = <xsl:value-of select="$RegisterInformationName"/>_Objest.<xsl:value-of select="Name"/>;
                        </xsl:when>
                        <xsl:when test="Type = 'pointer'">
                            <xsl:value-of select="Name"/>.Pointer = <xsl:value-of select="$RegisterInformationName"/>_Objest.<xsl:value-of select="Name"/>;
                        </xsl:when>
                        <xsl:when test="Type = 'enum'">
                            <xsl:value-of select="Name"/>.ActiveId = <xsl:value-of select="$RegisterInformationName"/>_Objest.<xsl:value-of select="Name"/>.ToString();
                            if (<xsl:value-of select="Name"/>.Active == -1) <xsl:value-of select="Name"/>.Active = 0;
                        </xsl:when>
                        <xsl:when test="Type = 'boolean' or Type = 'any_pointer' or Type = 'bytea' or Type = 'string[]' or Type = 'integer' or Type = 'numeric'">
                            //<xsl:value-of select="Name"/> = <xsl:value-of select="$RegisterInformationName"/>_Objest.<xsl:value-of select="Name"/>;
                        </xsl:when>
                    </xsl:choose>
                </xsl:if>
            </xsl:for-each>
        }

        protected override void GetValue()
        {
            UnigueID = <xsl:value-of select="$RegisterInformationName"/>_Objest.UnigueID;
            Caption = Період.Value.ToString();

            <xsl:for-each select="$Fields">
                <xsl:variable name="FieldName" select="Name" />
                <xsl:if test="$FormElementField[Name = $FieldName]">
                    <xsl:choose>
                        <xsl:when test="Type = 'string'">
                            <xsl:choose>
                                <xsl:when test="Multiline = '1'">
                                    <xsl:value-of select="$RegisterInformationName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Buffer.Text;
                                </xsl:when>
                                <xsl:otherwise>
                                    <xsl:value-of select="$RegisterInformationName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Text;
                                </xsl:otherwise>
                            </xsl:choose>
                        </xsl:when>
                        <xsl:when test="Type = 'integer' or Type = 'numeric' or Type = 'date' or Type = 'datetime' or Type = 'time'">
                            <xsl:value-of select="$RegisterInformationName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Value;
                        </xsl:when>
                        <xsl:when test="Type = 'boolean'">
                            <xsl:value-of select="$RegisterInformationName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Active;
                        </xsl:when>
                        <xsl:when test="Type = 'composite_pointer'">
                            <xsl:value-of select="$RegisterInformationName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Pointer;
                        </xsl:when>
                        <xsl:when test="Type = 'pointer'">
                            <xsl:value-of select="$RegisterInformationName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Pointer;
                        </xsl:when>
                        <xsl:when test="Type = 'enum'">
                            <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                            <xsl:text>if (</xsl:text><xsl:value-of select="Name"/>.Active != -1) 
                                <xsl:value-of select="$RegisterInformationName"/>_Objest.<xsl:value-of select="Name"/> = Enum.Parse&lt;<xsl:value-of select="$namePointer"/>&gt;(<xsl:value-of select="Name"/>.ActiveId);
                        </xsl:when>
                        <xsl:when test="Type = 'boolean' or Type = 'any_pointer' or Type = 'bytea' or Type = 'string[]' or Type = 'integer' or Type = 'numeric'">
                            //<xsl:value-of select="$RegisterInformationName"/>_Objest.<xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>;
                        </xsl:when>
                    </xsl:choose>
                </xsl:if>
            </xsl:for-each>
        }

        #endregion

        protected override async ValueTask Save()
        {
            try
            {
                await <xsl:value-of select="$RegisterInformationName"/>_Objest.Save();
            }
            catch (Exception ex)
            {
                MsgError(ex);
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
    <xsl:template name="RegisterInformationList">
        <xsl:variable name="RegisterInformationName" select="RegisterInformation/Name"/>
        <xsl:variable name="TabularList" select="RegisterInformation/TabularList"/>

/*     
        <xsl:value-of select="$RegisterInformationName"/>.cs
        Список

        Табличний список - <xsl:value-of select="$TabularList"/>
*/

using InterfaceGtk;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGenerationCode"/>.РегістриВідомостей;
using ТабличніСписки = <xsl:value-of select="$NameSpaceGenerationCode"/>.РегістриВідомостей.ТабличніСписки;

namespace <xsl:value-of select="$NameSpace"/>
{
    public class <xsl:value-of select="$RegisterInformationName"/> : РегістриЖурнал
    {
        public <xsl:value-of select="$RegisterInformationName"/>() : base()
        {
            ТабличніСписки.<xsl:value-of select="$RegisterInformationName"/>_<xsl:value-of select="$TabularList"/>.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async void LoadRecords()
        {
            ТабличніСписки.<xsl:value-of select="$RegisterInformationName"/>_<xsl:value-of select="$TabularList"/>.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.<xsl:value-of select="$RegisterInformationName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.<xsl:value-of select="$RegisterInformationName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid);

            if (ТабличніСписки.<xsl:value-of select="$RegisterInformationName"/>_<xsl:value-of select="$TabularList"/>.SelectPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.<xsl:value-of select="$RegisterInformationName"/>_<xsl:value-of select="$TabularList"/>.SelectPath, TreeViewGrid.Columns[0], false);
            else if (ТабличніСписки.<xsl:value-of select="$RegisterInformationName"/>_<xsl:value-of select="$TabularList"/>.CurrentPath != null)
                TreeViewGrid.SetCursor(ТабличніСписки.<xsl:value-of select="$RegisterInformationName"/>_<xsl:value-of select="$TabularList"/>.CurrentPath, TreeViewGrid.Columns[0], false);
        }

        protected override async void LoadRecords_OnSearch(string searchText)
        {
            searchText = searchText.ToLower().Trim();

            if (searchText.Length &lt; 1)
                return;

            searchText = "%" + searchText.Replace(" ", "%") + "%";

            ТабличніСписки.<xsl:value-of select="$RegisterInformationName"/>_<xsl:value-of select="$TabularList"/>.ОчиститиВідбір(TreeViewGrid);

            //period
            ТабличніСписки.<xsl:value-of select="$RegisterInformationName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбір(TreeViewGrid,
                new Where("period", Comparison.LIKE, searchText)
                {
                    FuncToField = "to_char",
                    FuncToField_Param1 = "'DD.MM.YYYY'"
                }
            );

            await ТабличніСписки.<xsl:value-of select="$RegisterInformationName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid);
        }

        protected override async void OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            if (IsNew)
            {
                NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{<xsl:value-of select="$RegisterInformationName"/>_Const.FULLNAME} *", () =>
                {
                    <xsl:value-of select="$RegisterInformationName"/>_Елемент page = new <xsl:value-of select="$RegisterInformationName"/>_Елемент
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
                <xsl:value-of select="$RegisterInformationName"/>_Objest <xsl:value-of select="$RegisterInformationName"/>_Objest = new <xsl:value-of select="$RegisterInformationName"/>_Objest();
                if (await <xsl:value-of select="$RegisterInformationName"/>_Objest.Read(unigueID))
                {
                    NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"{<xsl:value-of select="$RegisterInformationName"/>_Objest.Period}", () =>
                    {
                        <xsl:value-of select="$RegisterInformationName"/>_Елемент page = new <xsl:value-of select="$RegisterInformationName"/>_Елемент
                        {
                            CallBack_LoadRecords = CallBack_LoadRecords,
                            IsNew = false,
                            <xsl:value-of select="$RegisterInformationName"/>_Objest = <xsl:value-of select="$RegisterInformationName"/>_Objest,
                        };

                        page.SetValue();

                        return page;
                    });
                }
                else
                    Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
            }
        }

        protected override async ValueTask Delete(UnigueID unigueID)
        {
            <xsl:value-of select="$RegisterInformationName"/>_Objest <xsl:value-of select="$RegisterInformationName"/>_Objest = new <xsl:value-of select="$RegisterInformationName"/>_Objest();
            if (await <xsl:value-of select="$RegisterInformationName"/>_Objest.Read(unigueID))
                await <xsl:value-of select="$RegisterInformationName"/>_Objest.Delete();
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask&lt;UnigueID?&gt; Copy(UnigueID unigueID)
        {
            <xsl:value-of select="$RegisterInformationName"/>_Objest <xsl:value-of select="$RegisterInformationName"/>_Objest = new <xsl:value-of select="$RegisterInformationName"/>_Objest();
            if (await <xsl:value-of select="$RegisterInformationName"/>_Objest.Read(unigueID))
            {
                <xsl:value-of select="$RegisterInformationName"/>_Objest <xsl:value-of select="$RegisterInformationName"/>_Objest_Новий = <xsl:value-of select="$RegisterInformationName"/>_Objest.Copy();
                await <xsl:value-of select="$RegisterInformationName"/>_Objest_Новий.Save();
                return <xsl:value-of select="$RegisterInformationName"/>_Objest_Новий.UnigueID;
            }
            else
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return null;
            }
        }

        const string КлючНалаштуванняКористувача = "РегістриВідомостей.<xsl:value-of select="$RegisterInformationName"/>";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період);
        }

        protected override void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            LoadRecords();
        }

        #endregion
    }
}
    </xsl:template>

</xsl:stylesheet>