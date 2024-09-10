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

    <!-- Простори імен -->
    <xsl:param name="NameSpaceGenerationCode" />
    <xsl:param name="NameSpace" />

    <xsl:template match="root">

        <xsl:choose>
            <xsl:when test="$File = 'Element'">
                <xsl:call-template name="DirectoryElement" />
            </xsl:when>
            <xsl:when test="$File = 'List'">
                <xsl:call-template name="DirectoryList" />
            </xsl:when>
            <xsl:when test="$File = 'ListSmallSelect'">
                <xsl:call-template name="DirectoryListSmallSelect" />
            </xsl:when>
            <xsl:when test="$File = 'PointerControl'">
                <xsl:call-template name="DirectoryPointerControl" />
            </xsl:when>
            <xsl:when test="$File = 'ListAndTree'">
                <xsl:call-template name="DirectoryListAndTree" />
            </xsl:when>
        </xsl:choose>

    </xsl:template>

<!--- 
//
// ============================ Елемент ============================
//
-->

    <!-- Елемент -->
    <xsl:template name="DirectoryElement">
        <xsl:param name="IsTree" />
        <xsl:variable name="DirectoryName" select="Directory/Name"/>
        <xsl:variable name="Fields" select="Directory/Fields/Field"/>
        <xsl:variable name="TabularParts" select="Directory/TabularParts/TablePart"/>
        <xsl:variable name="FormElementField" select="Directory/ElementFields/ElementField"/>
        <xsl:variable name="FormElementTablePart" select="Directory/ElementTableParts/ElementTablePart"/>

        <!-- Додатова інформація для ієрархічного довідника -->
        <xsl:variable name="DirectoryType" select="Directory/Type"/>
        <xsl:variable name="ParentField" select="Directory/ParentField"/> <!-- Поле Родич (тільки для ієрархічного) -->
/*
        <xsl:value-of select="$DirectoryName"/>_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk;

using <xsl:value-of select="$NameSpaceGenerationCode"/>.Довідники;
using <xsl:value-of select="$NameSpaceGenerationCode"/>.Документи;
using <xsl:value-of select="$NameSpaceGenerationCode"/>.Перелічення;

namespace <xsl:value-of select="$NameSpace"/>
{
    class <xsl:value-of select="$DirectoryName"/>_Елемент : ДовідникЕлемент
    {
        public <xsl:value-of select="$DirectoryName"/>_Objest Елемент { get; set; } = new <xsl:value-of select="$DirectoryName"/>_Objest();
        <xsl:if test="$DirectoryType = 'Hierarchical'">
            public <xsl:value-of select="$DirectoryName"/>_Pointer РодичДляНового { get; set; } = new <xsl:value-of select="$DirectoryName"/>_Pointer();
        </xsl:if>
        #region Fields
        <xsl:for-each select="$Fields">
            <xsl:variable name="FieldName" select="Name" />
            <xsl:if test="$FormElementField[Name = $FieldName]">
                <xsl:choose>
                    <xsl:when test="Type = 'string'">
                        <xsl:choose>
                            <xsl:when test="Multiline = '1'">
                        <xsl:text>TextView </xsl:text><xsl:value-of select="Name"/> = new TextView();
                            </xsl:when>
                            <xsl:otherwise>
                        <xsl:text>Entry </xsl:text><xsl:value-of select="Name"/> = new Entry() { WidthRequest = 500 };
                            </xsl:otherwise>
                        </xsl:choose>
                    </xsl:when>
                    <xsl:when test="Type = 'integer'">
                        <xsl:text>IntegerControl </xsl:text><xsl:value-of select="Name"/> = new IntegerControl();
                    </xsl:when>
                    <xsl:when test="Type = 'numeric'">
                        <xsl:text>NumericControl </xsl:text><xsl:value-of select="Name"/> = new NumericControl();
                    </xsl:when>
                    <xsl:when test="Type = 'boolean'">
                        <xsl:text>CheckButton </xsl:text><xsl:value-of select="Name"/> = new CheckButton("<xsl:value-of select="Name"/>");
                    </xsl:when>
                    <xsl:when test="Type = 'date' or Type = 'datetime'">
                        <xsl:text>DateTimeControl </xsl:text><xsl:value-of select="Name"/> = new DateTimeControl()<xsl:if test="Type = 'date'">{ OnlyDate = true }</xsl:if>;
                    </xsl:when>
                    <xsl:when test="Type = 'time'">
                        <xsl:text>TimeControl </xsl:text><xsl:value-of select="Name"/> = new TimeControl();
                    </xsl:when>
                    <xsl:when test="Type = 'composite_pointer'">
                        <xsl:text>CompositePointerControl </xsl:text><xsl:value-of select="Name"/> = new CompositePointerControl() { BoundConfType = "Довідники.<xsl:value-of select="$DirectoryName"/>.<xsl:value-of select="Name"/>" };
                    </xsl:when>
                    <xsl:when test="Type = 'pointer'">
                        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                        <xsl:value-of select="$namePointer"/>_PointerControl <xsl:value-of select="Name"/> = new <xsl:value-of select="$namePointer"/>_PointerControl() { Caption = "<xsl:value-of select="$FormElementField[Name = $FieldName]/Caption"/>" };
                    </xsl:when>
                    <xsl:when test="Type = 'enum'">
                        <xsl:text>ComboBoxText </xsl:text><xsl:value-of select="Name"/> = new ComboBoxText();
                    </xsl:when>
                    <xsl:when test="Type = 'any_pointer'">
                        <xsl:text>//Guid </xsl:text><xsl:value-of select="Name"/> = new Guid();
                    </xsl:when>
                    <xsl:when test="Type = 'bytea'">
                        <xsl:text>//byte[] </xsl:text><xsl:value-of select="Name"/> = new byte[]{ };
                    </xsl:when>
                    <xsl:when test="Type = 'string[]'">
                        <xsl:text>//string[] </xsl:text><xsl:value-of select="Name"/> = new string[]{ };
                    </xsl:when>
                    <xsl:when test="Type = 'integer'">
                        <xsl:text>//int[] </xsl:text><xsl:value-of select="Name"/> = new int[]{ };
                    </xsl:when>
                    <xsl:when test="Type = 'numeric'">
                        <xsl:text>//decimal[] </xsl:text><xsl:value-of select="Name"/> = new decimal[]{ };
                    </xsl:when>
                </xsl:choose>
            </xsl:if>
        </xsl:for-each>
        #endregion

        #region TabularParts
        <xsl:for-each select="$TabularParts">
            <xsl:variable name="TablePartName" select="Name" />
            <xsl:if test="$FormElementTablePart[Name = $TablePartName]">
                <xsl:value-of select="$DirectoryName"/>_ТабличнаЧастина_<xsl:value-of select="Name"/><xsl:text> </xsl:text>
                <xsl:value-of select="Name"/> = new <xsl:value-of select="$DirectoryName"/>_ТабличнаЧастина_<xsl:value-of select="Name"/>();
            </xsl:if>
        </xsl:for-each>
        #endregion

        public <xsl:value-of select="$DirectoryName"/>_Елемент() : base() 
        { 
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;

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
            <xsl:for-each select="$TabularParts">
                <xsl:variable name="TablePartName" select="Name" />
                <xsl:if test="$FormElementTablePart[Name = $TablePartName]">
                CreateTablePart(vBox, "<xsl:value-of select="Name"/>:", <xsl:value-of select="Name"/>);
                </xsl:if>
            </xsl:for-each>
        }

        #region Присвоєння / зчитування значень

        public override void SetValue()
        {            
            <xsl:choose>
                <xsl:when test="$DirectoryType = 'Hierarchical'">
                    if (IsNew)
                        Елемент.<xsl:value-of select="$ParentField"/> = РодичДляНового;
                    else
                        <xsl:value-of select="$ParentField"/>.OpenFolder = Елемент.UnigueID;
                </xsl:when>
            </xsl:choose>

            <xsl:for-each select="$Fields">
                <xsl:variable name="FieldName" select="Name" />
                <xsl:if test="$FormElementField[Name = $FieldName]">
                    <xsl:choose>
                        <xsl:when test="Type = 'string'">
                            <xsl:choose>
                                <xsl:when test="Multiline = '1'">
                                    <xsl:value-of select="Name"/>.Buffer.Text = Елемент.<xsl:value-of select="Name"/>;
                                </xsl:when>
                                <xsl:otherwise>
                                    <xsl:value-of select="Name"/>.Text = Елемент.<xsl:value-of select="Name"/>;
                                </xsl:otherwise>
                            </xsl:choose>
                        </xsl:when>
                        <xsl:when test="Type = 'integer' or Type = 'numeric' or Type = 'date' or Type = 'datetime' or Type = 'time'">
                            <xsl:value-of select="Name"/>.Value = Елемент.<xsl:value-of select="Name"/>;
                        </xsl:when>
                        <xsl:when test="Type = 'boolean'">
                            <xsl:value-of select="Name"/>.Active = Елемент.<xsl:value-of select="Name"/>;
                        </xsl:when>
                        <xsl:when test="Type = 'composite_pointer'">
                            <xsl:value-of select="Name"/>.Pointer = Елемент.<xsl:value-of select="Name"/>;
                        </xsl:when>
                        <xsl:when test="Type = 'pointer'">
                            <xsl:value-of select="Name"/>.Pointer = Елемент.<xsl:value-of select="Name"/>;
                        </xsl:when>
                        <xsl:when test="Type = 'enum'">
                            <xsl:value-of select="Name"/>.ActiveId = Елемент.<xsl:value-of select="Name"/>.ToString();
                            <xsl:text>if (</xsl:text><xsl:value-of select="Name"/>.Active == -1) <xsl:value-of select="Name"/>.Active = 0;
                        </xsl:when>
                        <xsl:when test="Type = 'boolean' or Type = 'any_pointer' or Type = 'bytea' or Type = 'string[]' or Type = 'integer' or Type = 'numeric'">
                            <xsl:text>//</xsl:text><xsl:value-of select="Name"/> = Елемент.<xsl:value-of select="Name"/>;
                        </xsl:when>
                    </xsl:choose>
                </xsl:if>
            </xsl:for-each>

            <xsl:for-each select="$TabularParts">
                <xsl:variable name="TablePartName" select="Name" />
                <xsl:if test="$FormElementTablePart[Name = $TablePartName]">
                    <xsl:value-of select="Name"/>.ЕлементВласник = Елемент; // Таблична частина "<xsl:value-of select="Name"/>"
                    await <xsl:value-of select="Name"/>.LoadRecords();
                </xsl:if>
            </xsl:for-each>
        }

        protected override void GetValue()
        {
            <xsl:for-each select="$Fields">
                <xsl:variable name="FieldName" select="Name" />
                <xsl:if test="$FormElementField[Name = $FieldName]">
                    <xsl:choose>
                        <xsl:when test="Type = 'string'">
                            <xsl:choose>
                                <xsl:when test="Multiline = '1'">
                            <xsl:text>Елемент.</xsl:text><xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Buffer.Text;
                                </xsl:when>
                                <xsl:otherwise>
                            <xsl:text>Елемент.</xsl:text><xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Text;
                                </xsl:otherwise>
                            </xsl:choose>
                        </xsl:when>
                        <xsl:when test="Type = 'integer' or Type = 'numeric' or Type = 'date' or Type = 'datetime' or Type = 'time'">
                            <xsl:text>Елемент.</xsl:text><xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Value;
                        </xsl:when>
                        <xsl:when test="Type = 'boolean'">
                            <xsl:text>Елемент.</xsl:text><xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Active;
                        </xsl:when>
                        <xsl:when test="Type = 'composite_pointer'">
                            <xsl:text>Елемент.</xsl:text><xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Pointer;
                        </xsl:when>
                        <xsl:when test="Type = 'pointer'">
                            <xsl:text>Елемент.</xsl:text><xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>.Pointer;
                        </xsl:when>
                        <xsl:when test="Type = 'enum'">
                            <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                            <xsl:text>if (</xsl:text><xsl:value-of select="Name"/>.Active != -1) 
                            <xsl:text>Елемент.</xsl:text><xsl:value-of select="Name"/> = Enum.Parse&lt;<xsl:value-of select="$namePointer"/>&gt;(<xsl:value-of select="Name"/>.ActiveId);
                        </xsl:when>
                        <xsl:when test="Type = 'boolean' or Type = 'any_pointer' or Type = 'bytea' or Type = 'string[]' or Type = 'integer' or Type = 'numeric'">
                            <xsl:text>//Елемент.</xsl:text><xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>;
                        </xsl:when>
                    </xsl:choose>
                </xsl:if>
            </xsl:for-each>
        }

        #endregion

        protected override async ValueTask&lt;bool&gt; Save()
        {
            bool isSaved = false;

            try
            {
                if (await Елемент.Save())
                {
                    <xsl:for-each select="$TabularParts">
                        <xsl:variable name="TablePartName" select="Name" />
                        <xsl:if test="$FormElementTablePart[Name = $TablePartName]">
                            <xsl:text>await </xsl:text><xsl:value-of select="Name"/>.SaveRecords();
                        </xsl:if>
                    </xsl:for-each>
                    isSaved = true;
                }
            }
            catch (Exception ex)
            {
                ФункціїДляПовідомлень.ДодатиПовідомлення(Елемент.GetBasis(), Caption, ex);
            }

            return isSaved;
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
    <xsl:template name="DirectoryList">
        <xsl:variable name="DirectoryName" select="Directory/Name"/>
        <xsl:variable name="TabularParts" select="Directory/TabularParts/TablePart"/>
        <xsl:variable name="TabularList" select="Directory/TabularList"/>

        <!-- Додатова інформація для ієрархічного довідника -->
        <xsl:variable name="DirectoryType" select="Directory/Type"/>
        <xsl:variable name="ParentField" select="Directory/ParentField"/> <!-- Поле Родич (тільки для ієрархічного) -->
/*     
        <xsl:value-of select="$DirectoryName"/>.cs
        Список
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGenerationCode"/>.Довідники;
using ТабличніСписки = <xsl:value-of select="$NameSpaceGenerationCode"/>.Довідники.ТабличніСписки;

namespace <xsl:value-of select="$NameSpace"/>
{
    public class <xsl:value-of select="$DirectoryName"/> : ДовідникЖурнал
    {
        public <xsl:value-of select="$DirectoryName"/>() : base()
        {
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ОчиститиВідбір(TreeViewGrid);

            //Код
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбір(TreeViewGrid,
                new Where(<xsl:value-of select="$DirectoryName"/>_Const.Код, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            //Назва
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбір(TreeViewGrid,
                new Where(Comparison.OR, <xsl:value-of select="$DirectoryName"/>_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            await ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            <xsl:value-of select="$DirectoryName"/>_Елемент page = new <xsl:value-of select="$DirectoryName"/>_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                IsNew = IsNew
            };

            if (IsNew)
            {
                await page.Елемент.New();
                <xsl:if test="$DirectoryType = 'Hierarchical'">
                    <xsl:text>page.РодичДляНового = new </xsl:text>
                    <xsl:value-of select="$DirectoryName"/>_Pointer(SelectPointerItem ?? DirectoryPointerItem ?? new UnigueID());
                </xsl:if>
            }
            else if (unigueID == null || !await page.Елемент.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return;
            }

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () =&gt; page);
            page.SetValue();
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            <xsl:value-of select="$DirectoryName"/>_Objest Обєкт = new <xsl:value-of select="$DirectoryName"/>_Objest();
            if (await Обєкт.Read(unigueID))
                await Обєкт.SetDeletionLabel(!Обєкт.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask&lt;UnigueID?&gt; Copy(UnigueID unigueID)
        {
            <xsl:value-of select="$DirectoryName"/>_Objest Обєкт = new <xsl:value-of select="$DirectoryName"/>_Objest();
            if (await Обєкт.Read(unigueID))
            {
                <xsl:value-of select="$DirectoryName"/>_Objest Новий = await Обєкт.Copy(true);
                await Новий.Save();
                <xsl:for-each select="$TabularParts">
                    await Новий.<xsl:value-of select="Name"/>_TablePart.Save(false); // Таблична частина "<xsl:value-of select="Name"/>"
                </xsl:for-each>
                return Новий.UnigueID;
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

<!--- 
//
// ============================ ШвидкийВибір ============================
//
-->

    <!-- ШвидкийВибір -->
    <xsl:template name="DirectoryListSmallSelect">
        <xsl:variable name="DirectoryName" select="Directory/Name"/>
        <xsl:variable name="TabularList" select="Directory/TabularList"/>

        <!-- Додатова інформація для ієрархічного довідника -->
        <xsl:variable name="DirectoryType" select="Directory/Type"/>
        <xsl:variable name="ParentField" select="Directory/ParentField"/> <!-- Поле Родич (тільки для ієрархічного) -->
/*     
        <xsl:value-of select="$DirectoryName"/>_ШвидкийВибір.cs
        ШвидкийВибір
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGenerationCode"/>.Довідники;
using ТабличніСписки = <xsl:value-of select="$NameSpaceGenerationCode"/>.Довідники.ТабличніСписки;

namespace <xsl:value-of select="$NameSpace"/>
{
    class <xsl:value-of select="$DirectoryName"/>_ШвидкийВибір : ДовідникШвидкийВибір
    {
        public <xsl:value-of select="$DirectoryName"/>_ШвидкийВибір() : base()
        {
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.AddColumns(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.SelectPointerItem = null;
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ОчиститиВідбір(TreeViewGrid);

            await ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ОчиститиВідбір(TreeViewGrid);

            //Код
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбір(TreeViewGrid,
                new Where(<xsl:value-of select="$DirectoryName"/>_Const.Код, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            //Назва
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбір(TreeViewGrid,
                new Where(Comparison.OR, <xsl:value-of select="$DirectoryName"/>_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            await ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask OpenPageList(UnigueID? unigueID = null)
        {
            <xsl:value-of select="$DirectoryName"/> page = new <xsl:value-of select="$DirectoryName"/>()
            {
                DirectoryPointerItem = DirectoryPointerItem,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer,
                OpenFolder = OpenFolder
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {<xsl:value-of select="$DirectoryName"/>_Const.FULLNAME}", () =&gt; page);
            await page.SetValue();
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            <xsl:value-of select="$DirectoryName"/>_Елемент page = new <xsl:value-of select="$DirectoryName"/>_Елемент
            {
                IsNew = IsNew,
                CallBack_OnSelectPointer = CallBack_OnSelectPointer
            };

            if (IsNew)
                await page.Елемент.New();
            else if (unigueID == null || !await page.Елемент.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return;
            }

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () =&gt; page);
            page.SetValue();
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            <xsl:value-of select="$DirectoryName"/>_Objest Обєкт = new <xsl:value-of select="$DirectoryName"/>_Objest();
            if (await Обєкт.Read(unigueID))
                await Обєкт.SetDeletionLabel(!Обєкт.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }
    }
}
    </xsl:template>

<!--- 
//
// ============================ Список з Деревом ============================
//
-->

    <!-- Список з Деревом-->
    <xsl:template name="DirectoryListAndTree">
        <xsl:variable name="DirectoryName" select="Directory/Name"/>
        <xsl:variable name="TabularParts" select="Directory/TabularParts/TablePart"/>
        <xsl:variable name="TabularList" select="Directory/TabularList"/>

        <!-- Додатова інформація для ієрархічного довідника -->
        <xsl:variable name="DirectoryType" select="Directory/Type"/>
        <xsl:variable name="ParentField" select="Directory/ParentField"/> <!-- Поле Родич (тільки для ієрархічного) -->

        <xsl:variable name="PointerFolders" select="Directory/PointerFolders"/> <!-- Окремий довідник для ієрархії (тільки для ієрархії в окремому довіднику) -->
        <xsl:variable name="FieldFolder"> <!-- Поле Папка для ієрархії в окремому довіднику -->
            <xsl:choose>
                <xsl:when test="normalize-space(Directory/FieldFolder) != ''"><xsl:value-of select="Directory/FieldFolder"/></xsl:when>
                <xsl:otherwise>Папка</xsl:otherwise>
            </xsl:choose>
        </xsl:variable>
/*     
        <xsl:value-of select="$DirectoryName"/>.cs 
        Список з Деревом
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGenerationCode"/>.Довідники;
using ТабличніСписки = <xsl:value-of select="$NameSpaceGenerationCode"/>.Довідники.ТабличніСписки;

namespace <xsl:value-of select="$NameSpace"/>
{
    public class <xsl:value-of select="$DirectoryName"/> : ДовідникЖурнал
    {
        CheckButton checkButtonIsHierarchy = new CheckButton("Ієрархія папок") { Active = true };
        <xsl:value-of select="$PointerFolders"/> ДеревоПапок;

        public <xsl:value-of select="$DirectoryName"/>() : base()
        {
            //Враховувати ієрархію папок
            checkButtonIsHierarchy.Clicked += async (object? sender, EventArgs args) =&gt; await LoadRecords();
            HBoxTop.PackStart(checkButtonIsHierarchy, false, false, 10);

            //Дерево папок
            ДеревоПапок = new <xsl:value-of select="$PointerFolders"/>() { WidthRequest = 500, CallBack_RowActivated = LoadRecords_TreeCallBack };
            HPanedTable.Pack2(ДеревоПапок, false, true);

            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            if (DirectoryPointerItem != null || SelectPointerItem != null)
            {
                <xsl:value-of select="$DirectoryName"/>_Objest? Обєкт = await new <xsl:value-of select="$DirectoryName"/>_Pointer(SelectPointerItem ?? DirectoryPointerItem ?? new UnigueID()).GetDirectoryObject();
                if (Обєкт != null) ДеревоПапок.SelectPointerItem = Обєкт.<xsl:value-of select="$FieldFolder"/>.UnigueID;
            }

            await ДеревоПапок.SetValue();
        }

        async void LoadRecords_TreeCallBack()
        {
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.DirectoryPointerItem = DirectoryPointerItem;

            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ОчиститиВідбір(TreeViewGrid);

            if (checkButtonIsHierarchy.Active)
                ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбір(TreeViewGrid,
                    new Where(<xsl:value-of select="$DirectoryName"/>_Const.<xsl:value-of select="$FieldFolder"/>, Comparison.EQ, ДеревоПапок.SelectPointerItem?.UGuid ?? new UnigueID().UGuid));

            await ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ОчиститиВідбір(TreeViewGrid);

            //Код
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбір(TreeViewGrid,
                new Where(<xsl:value-of select="$DirectoryName"/>_Const.Код, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            //Назва
            ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбір(TreeViewGrid,
                new Where(Comparison.OR, <xsl:value-of select="$DirectoryName"/>_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" });

            await ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid, OpenFolder);
        }

        protected override void FilterRecords(Box hBox)
        {
            hBox.PackStart(ТабличніСписки.<xsl:value-of select="$DirectoryName"/>_<xsl:value-of select="$TabularList"/>.CreateFilter(TreeViewGrid), false, false, 5);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            <xsl:value-of select="$DirectoryName"/>_Елемент page = new <xsl:value-of select="$DirectoryName"/>_Елемент
            {
                CallBack_LoadRecords = CallBack_LoadRecords,
                IsNew = IsNew
            };

            if (IsNew)
                await page.Елемент.New();
            else if (unigueID == null || !await page.Елемент.Read(unigueID))
            {
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
                return;
            }

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, page.Caption, () =&gt; page);
            page.SetValue();
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            <xsl:value-of select="$DirectoryName"/>_Objest Обєкт = new <xsl:value-of select="$DirectoryName"/>_Objest();
            if (await Обєкт.Read(unigueID))
                await Обєкт.SetDeletionLabel(!Обєкт.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        protected override async ValueTask&lt;UnigueID?&gt; Copy(UnigueID unigueID)
        {
            <xsl:value-of select="$DirectoryName"/>_Objest Обєкт = new <xsl:value-of select="$DirectoryName"/>_Objest();
            if (await Обєкт.Read(unigueID))
            {
                <xsl:value-of select="$DirectoryName"/>_Objest Новий = await Обєкт.Copy(true);
                await Новий.Save();
                <xsl:for-each select="$TabularParts">
                    await Новий.<xsl:value-of select="Name"/>_TablePart.Save(false); // Таблична частина "<xsl:value-of select="Name"/>"
                </xsl:for-each>
                return Новий.UnigueID;
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

<!--- 
//
// ============================ PointerControl ============================
//
-->

    <!-- PointerControl -->
    <xsl:template name="DirectoryPointerControl">
        <xsl:variable name="DirectoryName" select="Directory/Name"/>
/*     
        <xsl:value-of select="$DirectoryName"/>_PointerControl.cs
        PointerControl (Список)
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGenerationCode"/>.Довідники;

namespace <xsl:value-of select="$NameSpace"/>
{
    class <xsl:value-of select="$DirectoryName"/>_PointerControl : PointerControl
    {
        event EventHandler&lt;<xsl:value-of select="$DirectoryName"/>_Pointer&gt; PointerChanged;

        public <xsl:value-of select="$DirectoryName"/>_PointerControl()
        {
            pointer = new <xsl:value-of select="$DirectoryName"/>_Pointer();
            WidthPresentation = 300;
            Caption = $"{<xsl:value-of select="$DirectoryName"/>_Const.FULLNAME}:";
            PointerChanged += async (object? _, <xsl:value-of select="$DirectoryName"/>_Pointer pointer) =&gt;
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
        }

        <xsl:value-of select="$DirectoryName"/>_Pointer pointer;
        public <xsl:value-of select="$DirectoryName"/>_Pointer Pointer
        {
            get
            {
                return pointer;
            }
            set
            {
                pointer = value;
                PointerChanged?.Invoke(null, pointer);
            }
        }

        protected override async void OpenSelect(object? sender, EventArgs args)
        {
            Popover popover = new Popover((Button)sender!) { Position = PositionType.Bottom, BorderWidth = 2 };
            BeforeClickOpenFunc?.Invoke();
            <xsl:value-of select="$DirectoryName"/>_ШвидкийВибір page = new <xsl:value-of select="$DirectoryName"/>_ШвидкийВибір() 
            { 
                PopoverParent = popover, 
                DirectoryPointerItem = Pointer.UnigueID,
                OpenFolder = OpenFolder,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =&gt;
                {
                    Pointer = new <xsl:value-of select="$DirectoryName"/>_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };
            popover.Add(page);
            popover.ShowAll();

            await page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new <xsl:value-of select="$DirectoryName"/>_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
    </xsl:template>

</xsl:stylesheet>