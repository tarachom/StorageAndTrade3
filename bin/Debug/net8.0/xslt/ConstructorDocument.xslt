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
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"> <!-- xmlns:exslt="http://exslt.org/common" -->
    <xsl:output method="text" indent="yes" />

    <!-- Файл -->
    <xsl:param name="File" />

    <!-- Простори імен -->
    <xsl:param name="NameSpaceGenerationCode" />
    <xsl:param name="NameSpace" />

    <xsl:template match="root">

        <xsl:choose>
            <xsl:when test="$File = 'Function'">
                <xsl:call-template name="DocumentFunction" />
            </xsl:when>
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
// ============================ Function ============================
//
-->

    <!-- Елемент -->
    <xsl:template name="DocumentFunction">
        <xsl:variable name="DocumentName" select="Document/Name"/>
        <xsl:variable name="Fields" select="Document/Fields/Field"/>
        <xsl:variable name="TabularParts" select="Document/TabularParts/TablePart"/>
        <xsl:variable name="TabularList" select="Document/TabularList"/>

        <!-- Відфільтровані поля по типу даних -->
        <xsl:variable name="FieldsFilter" select="$Fields[Type = 'string' or Type = 'integer' or Type = 'numeric' or Type = 'date' or Type = 'datetime' or Type = 'time']"/>

/*
        <xsl:value-of select="$DocumentName"/>_Функції.cs
        Функції
*/

using InterfaceGtk;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGenerationCode"/>.Документи;

namespace <xsl:value-of select="$NameSpace"/>
{
    static class <xsl:value-of select="$DocumentName"/>_Функції
    {
        public static List&lt;Where&gt; Відбори(string searchText)
        {
            return
            [
                <xsl:choose>
                    <xsl:when test="$FieldsFilter[IsSearch = '1']">
                        <xsl:for-each select="$FieldsFilter[IsSearch = '1']">
                //<xsl:value-of select="Name"/>
                new Where(<xsl:if test="position() != 1">Comparison.OR, </xsl:if><xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="Name"/>, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        </xsl:for-each>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:if test="$FieldsFilter[Name = 'Назва']">
                //Назва
                new Where(<xsl:if test="$FieldsFilter[Name = 'Код']">Comparison.OR, </xsl:if><xsl:value-of select="$DocumentName"/>_Const.Назва, Comparison.LIKE, searchText) { FuncToField = "LOWER" },
                        </xsl:if>
                    </xsl:otherwise>
                </xsl:choose>
            ];
        }

        public static async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null, 
            Action&lt;UnigueID?&gt;? сallBack_LoadRecords = null)
        {
            <xsl:value-of select="$DocumentName"/>_Елемент page = new <xsl:value-of select="$DocumentName"/>_Елемент
            {
                IsNew = IsNew,
                CallBack_LoadRecords = сallBack_LoadRecords
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

        public static async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            <xsl:value-of select="$DocumentName"/>_Objest Обєкт = new <xsl:value-of select="$DocumentName"/>_Objest();
            if (await Обєкт.Read(unigueID))
                await Обєкт.SetDeletionLabel(!Обєкт.DeletionLabel);
            else
                Message.Error(Program.GeneralForm, "Не вдалось прочитати!");
        }

        public static async ValueTask&lt;UnigueID?&gt; Copy(UnigueID unigueID)
        {
            <xsl:value-of select="$DocumentName"/>_Objest Обєкт = new <xsl:value-of select="$DocumentName"/>_Objest();
            if (await Обєкт.Read(unigueID))
            {
                <xsl:value-of select="$DocumentName"/>_Objest Новий = await Обєкт.Copy(true);
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
    }
}
    </xsl:template>

<!--- 
//
// ============================ Елемент ============================
//
-->

    <!-- Елемент -->
    <xsl:template name="DocumentElement">
        <xsl:variable name="DocumentName" select="Document/Name"/>
        <!--<xsl:variable name="Fields" select="Document/Fields/Field"/>-->
        <!--<xsl:variable name="TabularParts" select="Document/TabularParts/TablePart"/>-->
        <xsl:variable name="FieldsTL" select="Document/ElementFields/Field"/>
        <xsl:variable name="TabularPartsTL" select="Document/ElementTableParts/TablePart"/>
/*
        <xsl:value-of select="$DocumentName"/>_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using <xsl:value-of select="$NameSpaceGenerationCode"/>;
using <xsl:value-of select="$NameSpaceGenerationCode"/>.Константи;
using <xsl:value-of select="$NameSpaceGenerationCode"/>.Довідники;
using <xsl:value-of select="$NameSpaceGenerationCode"/>.Документи;
using <xsl:value-of select="$NameSpaceGenerationCode"/>.Перелічення;

namespace <xsl:value-of select="$NameSpace"/>
{
    class <xsl:value-of select="$DocumentName"/>_Елемент : ДокументЕлемент
    {
        public <xsl:value-of select="$DocumentName"/>_Objest Елемент { get; init; } = new <xsl:value-of select="$DocumentName"/>_Objest();

        #region Fields
        <!-- Крім поля Назва -->
        <xsl:for-each select="$FieldsTL[Name != 'Назва']">
            <xsl:variable name="Size">
                <xsl:choose>
                    <xsl:when test="Size != '0'"><xsl:value-of select="Size"/></xsl:when>
                    <xsl:otherwise>500</xsl:otherwise>
                </xsl:choose>
            </xsl:variable>
            <xsl:choose>
                <xsl:when test="Type = 'string'">
                    <xsl:choose>
                        <xsl:when test="Multiline = '1'">
                    <xsl:text>TextView </xsl:text><xsl:value-of select="Name"/> = new TextView() { WrapMode = WrapMode.Word };
                        </xsl:when>
                        <xsl:otherwise>
                    <xsl:text>Entry </xsl:text><xsl:value-of select="Name"/> = new Entry() { WidthRequest = <xsl:value-of select="$Size"/> };
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
                    <xsl:text>CompositePointerControl </xsl:text><xsl:value-of select="Name"/> = new CompositePointerControl() { BoundConfType = "Документи.<xsl:value-of select="$DocumentName"/>.<xsl:value-of select="Name"/>" };
                </xsl:when>
                <xsl:when test="Type = 'pointer'">
                    <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                    <xsl:value-of select="$namePointer"/>_PointerControl <xsl:value-of select="Name"/> = new <xsl:value-of select="$namePointer"/>_PointerControl() { Caption = "<xsl:value-of select="Caption"/>", WidthPresentation = <xsl:value-of select="$Size"/> };
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
        </xsl:for-each>
        #endregion

        #region TabularParts
        <xsl:for-each select="$TabularPartsTL">
            <xsl:variable name="Size">
                <xsl:choose>
                    <xsl:when test="Size != '0'"><xsl:value-of select="Size"/></xsl:when>
                    <xsl:otherwise>500</xsl:otherwise>
                </xsl:choose>
            </xsl:variable>
            <xsl:variable name="Height">
                <xsl:choose>
                    <xsl:when test="Height != '0'"><xsl:value-of select="Height"/></xsl:when>
                    <xsl:otherwise>300</xsl:otherwise>
                </xsl:choose>
            </xsl:variable>
            // Таблична частина "<xsl:value-of select="Name"/>" 
            <xsl:value-of select="$DocumentName"/>_ТабличнаЧастина_<xsl:value-of select="Name"/><xsl:text> </xsl:text><xsl:value-of select="Name"/> = new <xsl:value-of select="$DocumentName"/>_ТабличнаЧастина_<xsl:value-of select="Name"/>() { WidthRequest = <xsl:value-of select="$Size"/>, HeightRequest = <xsl:value-of select="$Height"/> };
        </xsl:for-each>
        #endregion

        public <xsl:value-of select="$DocumentName"/>_Елемент()
        {
            Елемент.UnigueIDChanged += UnigueIDChanged;
            Елемент.CaptionChanged += CaptionChanged;

            CreateDocName(<xsl:value-of select="$DocumentName"/>_Const.FULLNAME, НомерДок, ДатаДок);
            <xsl:if test="$FieldsTL[Name = 'Коментар']">
            CreateField(HBoxComment, "<xsl:value-of select="$FieldsTL[Name = 'Коментар']/Caption"/>:", Коментар);
            </xsl:if>

            <xsl:if test="count($TabularPartsTL) != 0">
                <xsl:for-each select="$TabularPartsTL">
                // Таблична частина "<xsl:value-of select="Name"/>" 
                NotebookTablePart.InsertPage(<xsl:value-of select="Name"/>, new Label("<xsl:value-of select="Caption"/>"), <xsl:value-of select="position() - 1"/>);
                </xsl:for-each>
                NotebookTablePart.CurrentPage = 0;
            </xsl:if>

            <!-- Заповнення випадаючих списків для перелічень -->
            <xsl:for-each select="$FieldsTL[Type = 'enum']">
                foreach (var field in ПсевдонімиПерелічення.<xsl:value-of select="substring-after(Pointer, '.')"/>_List())
                    <xsl:value-of select="Name"/>.Append(field.Value.ToString(), field.Name);
            </xsl:for-each>
        }

        protected override void CreateContainer1(Box vBox)
        {
            
        }

        protected override void CreateContainer2(Box vBox)
        {
           
        }

        protected override void CreateContainer3(Box vBox)
        {
            <!-- Крім полів які зразу добавляються в шапку НомерДок, ДатаДок, Коментар -->
            <!-- та скритого поля Назва яке формується перед збереженням -->
            <xsl:for-each select="$FieldsTL[Name != 'Назва' and Name != 'НомерДок' and Name != 'ДатаДок' and Name != 'Коментар']">
                // <xsl:value-of select="Name"/>
                <xsl:choose>
                    <xsl:when test="Type = 'string' or Type = 'integer' or Type = 'numeric' or Type = 'date' or Type = 'datetime' or Type = 'time'">
                        <xsl:choose>
                            <xsl:when test="Type = 'string' and Multiline = '1'">
                            <xsl:variable name="Size">
                                <xsl:choose>
                                    <xsl:when test="Size != '0'"><xsl:value-of select="Size"/></xsl:when>
                                    <xsl:otherwise>500</xsl:otherwise>
                                </xsl:choose>
                            </xsl:variable>
                            <xsl:variable name="Height">
                                <xsl:choose>
                                    <xsl:when test="Height != '0'"><xsl:value-of select="Height"/></xsl:when>
                                    <xsl:otherwise>200</xsl:otherwise>
                                </xsl:choose>
                            </xsl:variable>
                CreateFieldView(vBox, "<xsl:value-of select="Caption"/>:", <xsl:value-of select="Name"/>, <xsl:value-of select="$Size"/>, <xsl:value-of select="$Height"/>);
                            </xsl:when>
                            <xsl:otherwise>
                CreateField(vBox, "<xsl:value-of select="Caption"/>:", <xsl:value-of select="Name"/>);
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
                CreateField(vBox, "<xsl:value-of select="Caption"/>:", <xsl:value-of select="Name"/>);
                    </xsl:when>
                </xsl:choose>
            </xsl:for-each>
        }

        protected override void CreateContainer4(Box vBox)
        {
            
        }

        #region Присвоєння / зчитування значень

        public override <xsl:if test="count($TabularPartsTL) != 0">async</xsl:if> void SetValue()
        {
            <!-- Крім скритого поля Назва яке формується перед збереженням -->
            <xsl:for-each select="$FieldsTL[Name != 'Назва']">
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
            </xsl:for-each>

            <xsl:for-each select="$TabularPartsTL">
                // Таблична частина "<xsl:value-of select="Name"/>" 
                <xsl:value-of select="Name"/>.ЕлементВласник = Елемент; 
                <xsl:text>await </xsl:text><xsl:value-of select="Name"/>.LoadRecords();
            </xsl:for-each>
        }

        protected override void GetValue()
        {
            <!-- Крім скритого поля Назва яке формується перед збереженням -->
            <xsl:for-each select="$FieldsTL[Name != 'Назва']">
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
                        <xsl:text>if (</xsl:text><xsl:value-of select="Name"/><xsl:text>.Active != -1) </xsl:text>
                        <xsl:text>Елемент.</xsl:text><xsl:value-of select="Name"/> = Enum.Parse&lt;<xsl:value-of select="$namePointer"/>&gt;(<xsl:value-of select="Name"/>.ActiveId);
                    </xsl:when>
                    <xsl:when test="Type = 'boolean' or Type = 'any_pointer' or Type = 'bytea' or Type = 'string[]' or Type = 'integer' or Type = 'numeric'">
                        <xsl:text>//Елемент.</xsl:text><xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>;
                    </xsl:when>
                </xsl:choose>
            </xsl:for-each>
        }

        /*string КлючовіСловаДляПошуку()
        {
            return "\n<xsl:for-each select="$FieldsTL[Type = 'pointer']">
                <xsl:choose>
                    <xsl:when test="Type = 'pointer'"> {<xsl:value-of select="Name"/>.Назва}</xsl:when>
                </xsl:choose>
            </xsl:for-each>"
            <xsl:for-each select="$TabularPartsTL"> + <xsl:value-of select="Name"/>.КлючовіСловаДляПошуку()</xsl:for-each>;
        }*/

        #endregion
        
        protected override async ValueTask&lt;bool&gt; Save()
        {
            bool isSaved = false;
            try
            {
                if(await Елемент.Save())
                {
                    <xsl:for-each select="$TabularPartsTL">
                        <xsl:text>await </xsl:text><xsl:value-of select="Name"/>.SaveRecords(); // Таблична частина "<xsl:value-of select="Name"/>"
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

        protected override async ValueTask&lt;bool&gt; SpendTheDocument(bool spendDoc)
        {
            if (spendDoc)
            {
                bool isSpend = await Елемент.SpendTheDocument(Елемент.ДатаДок);
                if (!isSpend) ФункціїДляПовідомлень.ПоказатиПовідомлення(Елемент.UnigueID);
                return isSpend;
            }
            else
            {
                await Елемент.ClearSpendTheDocument();
                return true;
            }
        }

        protected override void ReportSpendTheDocument(UnigueID unigueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new <xsl:value-of select="$DocumentName"/>_Pointer(unigueID));
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
        <xsl:variable name="TabularList" select="Document/TabularList"/>

        <!-- Додатова інформація -->
        <xsl:variable name="DocumentExportXML" select="Document/ExportXML"/>

/*     
        <xsl:value-of select="$DocumentName"/>.cs
        Список
*/

using Gtk;
using InterfaceGtk;
using AccountingSoftware;

using ТабличніСписки = <xsl:value-of select="$NameSpaceGenerationCode"/>.Документи.ТабличніСписки;
using <xsl:value-of select="$NameSpaceGenerationCode"/>.Документи;

namespace <xsl:value-of select="$NameSpace"/>
{
    public class <xsl:value-of select="$DocumentName"/> : ДокументЖурнал
    {
        public <xsl:value-of select="$DocumentName"/>() : base()
        {
            ТабличніСписки.<xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularList"/>.AddColumns(TreeViewGrid);
        }

        #region Override

        protected override async ValueTask LoadRecords()
        {
            ТабличніСписки.<xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularList"/>.SelectPointerItem = SelectPointerItem;
            ТабличніСписки.<xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularList"/>.DocumentPointerItem = DocumentPointerItem;

            ТабличніСписки.<xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.<xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid);
        }

        protected override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.<xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularList"/>.ОчиститиВідбір(TreeViewGrid);
            
            //Відбори
            ТабличніСписки.<xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбір(TreeViewGrid, <xsl:value-of select="$DocumentName"/>_Функції.Відбори(searchText));

            await ТабличніСписки.<xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid);
        }

        protected override Widget? FilterRecords(Box hBox)
        {
            return ТабличніСписки.<xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularList"/>.CreateFilter(TreeViewGrid);
        }

        protected override async ValueTask OpenPageElement(bool IsNew, UnigueID? unigueID = null)
        {
            await <xsl:value-of select="$DocumentName"/>_Функції.OpenPageElement(IsNew, unigueID, CallBack_LoadRecords);
        }

        protected override async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            await <xsl:value-of select="$DocumentName"/>_Функції.SetDeletionLabel(unigueID);
        }

        protected override async ValueTask&lt;UnigueID?&gt; Copy(UnigueID unigueID)
        {
            return await <xsl:value-of select="$DocumentName"/>_Функції.Copy(unigueID);
        }

        const string КлючНалаштуванняКористувача = "Документи.<xsl:value-of select="$DocumentName"/>";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await LoadRecords();
        }

        protected override async ValueTask SpendTheDocument(UnigueID unigueID, bool spendDoc)
        {
            <xsl:value-of select="$DocumentName"/>_Objest? Обєкт = await new <xsl:value-of select="$DocumentName"/>_Pointer(unigueID).GetDocumentObject(true);
            if (Обєкт == null) return;

            if (spendDoc)
            {
                if (!await Обєкт.SpendTheDocument(Обєкт.ДатаДок))
                    ФункціїДляПовідомлень.ПоказатиПовідомлення(Обєкт.UnigueID);
            }
            else
                await Обєкт.ClearSpendTheDocument();
        }

        protected override void ReportSpendTheDocument(UnigueID unigueID)
        {
            СпільніФорми_РухДокументуПоРегістрах.СформуватиЗвіт(new <xsl:value-of select="$DocumentName"/>_Pointer(unigueID));
        }
        
        protected override bool IsExportXML() { return <xsl:choose><xsl:when test="$DocumentExportXML = '1'">true</xsl:when><xsl:otherwise>false</xsl:otherwise></xsl:choose>; } //Дозволити експорт документу
        protected override async ValueTask ExportXML(UnigueID unigueID, string pathToFolder)
        {
            <xsl:value-of select="$DocumentName"/>_Pointer Вказівник = new <xsl:value-of select="$DocumentName"/>_Pointer(unigueID);
            await Вказівник.GetPresentation();
            string path = System.IO.Path.Combine(pathToFolder, $"{Вказівник.Назва}.xml");

            await <xsl:value-of select="$DocumentName"/>_Export.ToXmlFile(Вказівник, path);
            ФункціїДляПовідомлень.ДодатиІнформаційнеПовідомлення(Вказівник.GetBasis(), Вказівник.Назва, $"Вигружено у файл: {path}");
        }

        /*
        protected override async ValueTask PrintingDoc(UnigueID unigueID)
        {
            await <xsl:value-of select="$DocumentName"/>_Друк.PDF(unigueID);
        }
        */

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
using Gtk;
using InterfaceGtk;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGenerationCode"/>.Документи;

namespace <xsl:value-of select="$NameSpace"/>
{
    class <xsl:value-of select="$DocumentName"/>_PointerControl : PointerControl
    {
        event EventHandler&lt;<xsl:value-of select="$DocumentName"/>_Pointer&gt; PointerChanged;

        public <xsl:value-of select="$DocumentName"/>_PointerControl()
        {
            pointer = new <xsl:value-of select="$DocumentName"/>_Pointer();
            WidthPresentation = 300;
            Caption = $"{<xsl:value-of select="$DocumentName"/>_Const.FULLNAME}:";
            PointerChanged += async (object? _, <xsl:value-of select="$DocumentName"/>_Pointer pointer) =&gt;
            {
                Presentation = pointer != null ? await pointer.GetPresentation() : "";
            };
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
                PointerChanged?.Invoke(null, pointer);
            }
        }

        protected override void OpenSelect(object? sender, EventArgs args)
        {
            BeforeClickOpenFunc?.Invoke();
            <xsl:value-of select="$DocumentName"/> page = new <xsl:value-of select="$DocumentName"/>
            {
                DocumentPointerItem = Pointer.UnigueID,
                CallBack_OnSelectPointer = (UnigueID selectPointer) =&gt;
                {
                    Pointer = new <xsl:value-of select="$DocumentName"/>_Pointer(selectPointer);
                    AfterSelectFunc?.Invoke();
                }
            };

            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, $"Вибір - {<xsl:value-of select="$DocumentName"/>_Const.FULLNAME}", () =&gt; page);
            page.SetValue();
        }

        protected override void OnClear(object? sender, EventArgs args)
        {
            Pointer = new <xsl:value-of select="$DocumentName"/>_Pointer();
            AfterSelectFunc?.Invoke();
        }
    }
}
    </xsl:template>

</xsl:stylesheet>