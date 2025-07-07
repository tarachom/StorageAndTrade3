<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"> <!-- xmlns:exslt="http://exslt.org/common" -->
    <xsl:output method="text" indent="yes" />

    <!-- Файл -->
    <xsl:param name="File" />

    <!-- Простори імен -->
    <xsl:param name="NameSpaceGeneratedCode" />
    <xsl:param name="NameSpace" />

    <xsl:template match="root">

        <xsl:choose>
            <xsl:when test="$File = 'Triggers'">
                <xsl:call-template name="DocumentTriggers" />
            </xsl:when>
            <xsl:when test="$File = 'SpendTheDocument'">
                <xsl:call-template name="DocumentSpendTheDocument" />
            </xsl:when>
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
            <xsl:when test="$File = 'Report'">
                <xsl:call-template name="DocumentReport" />
            </xsl:when>
        </xsl:choose>

    </xsl:template>

<!--- 
//
// ============================ Triggers ============================
//
-->

    <xsl:template name="DocumentTriggers">
        <xsl:variable name="DocumentName" select="Document/Name"/>
        <xsl:variable name="Fields" select="Document/Fields/Field"/>
        <xsl:variable name="DocumentAutomaticNumeration" select="Document/AutomaticNumeration"/>

        <!-- Назви функцій -->
        <xsl:variable name="TriggerFunctions" select="Document/TriggerFunctions"/>

/*
        <xsl:value-of select="$DocumentName"/>_Triggers.cs
        Тригери
*/

using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Константи;
using AccountingSoftware;

namespace <xsl:value-of select="$NameSpaceGeneratedCode"/>.Документи
{
    static class <xsl:value-of select="$DocumentName"/>_Triggers
    {
        public static async ValueTask <xsl:value-of select="$TriggerFunctions/New"/>(<xsl:value-of select="$DocumentName"/>_Objest ДокументОбєкт)
        {
            <xsl:if test="$DocumentAutomaticNumeration = '1'">
                <xsl:text>ДокументОбєкт.НомерДок = (++НумераціяДокументів.</xsl:text>
                <xsl:value-of select="$DocumentName"/>
                <xsl:text>_Const).ToString("D8");</xsl:text>
            </xsl:if>
            ДокументОбєкт.ДатаДок = DateTime.Now;
            await ValueTask.FromResult(true);
        }

        public static async ValueTask <xsl:value-of select="$TriggerFunctions/Copying"/>(<xsl:value-of select="$DocumentName"/>_Objest ДокументОбєкт, <xsl:value-of select="$DocumentName"/>_Objest Основа)
        {
            <xsl:if test="$Fields[Name = 'Назва']">
            ДокументОбєкт.Назва += " - Копія";
            </xsl:if>
            await ValueTask.FromResult(true);
        }

        public static async ValueTask <xsl:value-of select="$TriggerFunctions/BeforeSave"/>(<xsl:value-of select="$DocumentName"/>_Objest ДокументОбєкт)
        {
            ДокументОбєкт.Назва = $"{<xsl:value-of select="$DocumentName"/>_Const.FULLNAME} №{ДокументОбєкт.НомерДок} від {ДокументОбєкт.ДатаДок.ToString("dd.MM.yyyy")}";
            await ValueTask.FromResult(true);
        }

        public static async ValueTask <xsl:value-of select="$TriggerFunctions/AfterSave"/>(<xsl:value-of select="$DocumentName"/>_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask <xsl:value-of select="$TriggerFunctions/SetDeletionLabel"/>(<xsl:value-of select="$DocumentName"/>_Objest ДокументОбєкт, bool label)
        {
            await ValueTask.FromResult(true);
        }

        public static async ValueTask <xsl:value-of select="$TriggerFunctions/BeforeDelete"/>(<xsl:value-of select="$DocumentName"/>_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
    </xsl:template>


    <!--- 
//
// ============================ SpendTheDocument ============================
//
-->

    <xsl:template name="DocumentSpendTheDocument">
        <xsl:variable name="DocumentName" select="Document/Name"/>

        <!-- Назви функцій -->
        <xsl:variable name="SpendFunctions" select="Document/SpendFunctions"/>

/*
        <xsl:value-of select="$DocumentName"/>_SpendTheDocument.cs
        Модуль проведення документу
*/

using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>;

using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Довідники;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.РегістриНакопичення;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.РегістриВідомостей;

namespace <xsl:value-of select="$NameSpaceGeneratedCode"/>.Документи
{
    static class <xsl:value-of select="$DocumentName"/>_SpendTheDocument
    {
        public static async ValueTask&lt;bool&gt; <xsl:value-of select="$SpendFunctions/Spend"/>(<xsl:value-of select="$DocumentName"/>_Objest ДокументОбєкт)
        {
            try
            {
                // Проведення документу
                // ...

                return true;
            }
            catch (Exception ex)
            {
                await СпільніФункції.ДокументНеПроводиться(ДокументОбєкт, ДокументОбєкт.Назва, ex.Message);
                return false;
            }
        }

        public static async ValueTask <xsl:value-of select="$SpendFunctions/ClearSpend"/>(<xsl:value-of select="$DocumentName"/>_Objest ДокументОбєкт)
        {
            await ValueTask.FromResult(true);
        }
    }
}
    </xsl:template>

<!--- 
//
// ============================ Function ============================
//
-->

    <xsl:template name="Function_FuncToField">
        <xsl:choose>
            <xsl:when test="Type = 'string'">FuncToField = "LOWER"</xsl:when>
            <xsl:otherwise>FuncToField = "TO_CHAR", FuncToField_Param1 = "''"</xsl:otherwise>
        </xsl:choose>
    </xsl:template>

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

using InterfaceGtk3;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Документи;

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
                new Where(<xsl:if test="position() != 1">Comparison.OR, </xsl:if><xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="Name"/>, Comparison.LIKE, searchText) { <xsl:call-template name="Function_FuncToField" /> },
                        </xsl:for-each>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:if test="$FieldsFilter[Name = 'Назва']">
                //Назва
                new Where(<xsl:if test="$FieldsFilter[Name = 'Код']">Comparison.OR, </xsl:if><xsl:value-of select="$DocumentName"/>_Const.Назва, Comparison.LIKE, searchText) { <xsl:call-template name="Function_FuncToField" /> },
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
            await NotebookFunction.AddLockObjectFunc(Program.GeneralNotebook, page.Name, page.Елемент);
            await page.LockInfo(page.Елемент);
            page.SetValue();
        }

        public static async ValueTask SetDeletionLabel(UnigueID unigueID)
        {
            <xsl:value-of select="$DocumentName"/>_Pointer Вказівник = new(unigueID);
            bool? label = await Вказівник.GetDeletionLabel();
            if (label.HasValue) await Вказівник.SetDeletionLabel(!label.Value);
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
        <xsl:variable name="FieldsTL" select="Document/ElementFields/Field"/>
        <xsl:variable name="TabularPartsTL" select="Document/ElementTableParts/TablePart"/>
/*
        <xsl:value-of select="$DocumentName"/>_Елемент.cs
        Елемент
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;

using <xsl:value-of select="$NameSpaceGeneratedCode"/>;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Константи;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Довідники;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Документи;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Перелічення;

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
                <xsl:when test="Type = 'composite_text'">
                    <xsl:text>//NameAndText </xsl:text><xsl:value-of select="Name"/> = new NameAndText();
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
                    <xsl:text>//byte[] </xsl:text><xsl:value-of select="Name"/> = [];
                </xsl:when>
                <xsl:when test="Type = 'string[]'">
                    <xsl:text>//string[] </xsl:text><xsl:value-of select="Name"/> = [];
                </xsl:when>
                <xsl:when test="Type = 'integer[]'">
                    <xsl:text>//int[] </xsl:text><xsl:value-of select="Name"/> = [];
                </xsl:when>
                <xsl:when test="Type = 'numeric[]'">
                    <xsl:text>//decimal[] </xsl:text><xsl:value-of select="Name"/> = [];
                </xsl:when>
                <xsl:when test="Type = 'uuid[]'">
                    <xsl:text>//Guid[] </xsl:text><xsl:value-of select="Name"/> = [];
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
                        <xsl:text>//if (</xsl:text><xsl:value-of select="Name"/>.Active == -1) <xsl:value-of select="Name"/>.Active = 0;
                    </xsl:when>
                    <xsl:when test="Type = 'any_pointer' or Type = 'composite_text' or Type = 'bytea' or Type = 'string[]' or Type = 'integer[]' or Type = 'numeric[]' or Type = 'uuid[]'">
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
                        <xsl:text>Елемент.</xsl:text><xsl:value-of select="Name"/> = ПсевдонімиПерелічення.<xsl:value-of select="$namePointer"/>_FindByName(<xsl:value-of select="Name"/>.ActiveId);
                    </xsl:when>
                    <xsl:when test="Type = 'any_pointer' or Type = 'composite_text' or Type = 'bytea' or Type = 'string[]' or Type = 'integer[]' or Type = 'numeric[]' or Type = 'uuid[]'">
                        <xsl:text>//Елемент.</xsl:text><xsl:value-of select="Name"/> = <xsl:value-of select="Name"/>;
                    </xsl:when>
                </xsl:choose>
            </xsl:for-each>
        }

        <xsl:if test="count($FieldsTL[Type = 'pointer']) != 0 or count($TabularPartsTL) != 0">
        /*string КлючовіСловаДляПошуку()
        {
            return $"\n<xsl:for-each select="$FieldsTL[Type = 'pointer']">
                <xsl:choose>
                    <xsl:when test="Type = 'pointer'"> {<xsl:value-of select="Name"/>.Pointer.Назва}</xsl:when>
                </xsl:choose>
            </xsl:for-each>"
            <xsl:for-each select="$TabularPartsTL"> + <xsl:value-of select="Name"/>.КлючовіСловаДляПошуку()</xsl:for-each>;
        }*/
        </xsl:if>

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

        protected override async ValueTask InJournal(UnigueID unigueID)
        {
            <xsl:value-of select="$DocumentName"/> page = new <xsl:value-of select="$DocumentName"/>() { SelectPointerItem = unigueID };
            NotebookFunction.CreateNotebookPage(Program.GeneralNotebook, <xsl:value-of select="$DocumentName"/>_Const.FULLNAME, () => page);
            await page.SetValue();
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
using InterfaceGtk3;
using AccountingSoftware;

using <xsl:value-of select="$NameSpaceGeneratedCode"/>;
using ТабличніСписки = <xsl:value-of select="$NameSpaceGeneratedCode"/>.Документи.ТабличніСписки;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Документи;

namespace <xsl:value-of select="$NameSpace"/>
{
    public class <xsl:value-of select="$DocumentName"/> : ДокументЖурнал
    {
        public <xsl:value-of select="$DocumentName"/>() : base()
        {
            ТабличніСписки.<xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularList"/>.AddColumns(TreeViewGrid);
        }

        #region Override

        public override async ValueTask LoadRecords()
        {
            ТабличніСписки.<xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбірПоПеріоду(TreeViewGrid, Період.Period, Період.DateStart, Період.DateStop);

            await ТабличніСписки.<xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid, SelectPointerItem, DocumentPointerItem);
        }

        public override async ValueTask LoadRecords_OnSearch(string searchText)
        {
            ТабличніСписки.<xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularList"/>.ОчиститиВідбір(TreeViewGrid);
            
            //Відбори
            ТабличніСписки.<xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularList"/>.ДодатиВідбір(TreeViewGrid, <xsl:value-of select="$DocumentName"/>_Функції.Відбори(searchText));

            await ТабличніСписки.<xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid);
        }

        public async override ValueTask LoadRecords_OnFilter()
        {
            await ТабличніСписки.<xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularList"/>.LoadRecords(TreeViewGrid, SelectPointerItem, DocumentPointerItem);
        }

        public async ValueTask UpdateRecords(List&lt;ObjectChanged&gt; recordsChanged)
        {
            await ТабличніСписки.<xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularList"/>.UpdateRecords(TreeViewGrid, recordsChanged);
        }

        protected override void FillFilterList(ListFilterControl filterControl)
        {
            ТабличніСписки.<xsl:value-of select="$DocumentName"/>_<xsl:value-of select="$TabularList"/>.CreateFilter(TreeViewGrid, filterControl);
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

        protected override async ValueTask VersionsHistory(UnigueID unigueID)
        {
            await СпільніФорми_ІсторіяЗміниДаних_Список.Сформувати(new <xsl:value-of select="$DocumentName"/>_Pointer(unigueID).GetBasis());
        }

        const string КлючНалаштуванняКористувача = "Документи.<xsl:value-of select="$DocumentName"/>";

        protected override async ValueTask BeforeSetValue()
        {
            await ФункціїНалаштуванняКористувача.ОтриматиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період);
            NotebookFunction.AddChangeFunc(Program.GeneralNotebook, Name, UpdateRecords, <xsl:value-of select="$DocumentName"/>_Const.POINTER);
        }

        protected override async void PeriodChanged()
        {
            ФункціїНалаштуванняКористувача.ЗаписатиПеріодДляЖурналу(КлючНалаштуванняКористувача + KeyForSetting, Період.Period.ToString(), Період.DateStart, Період.DateStop);
            await BeforeLoadRecords();
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
using InterfaceGtk3;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Документи;

namespace <xsl:value-of select="$NameSpace"/>
{
    public class <xsl:value-of select="$DocumentName"/>_PointerControl : PointerControl
    {
        event EventHandler&lt;<xsl:value-of select="$DocumentName"/>_Pointer&gt; PointerChanged;

        public <xsl:value-of select="$DocumentName"/>_PointerControl()
        {
            pointer = new <xsl:value-of select="$DocumentName"/>_Pointer();
            WidthPresentation = 300;
            Caption = $"{<xsl:value-of select="$DocumentName"/>_Const.FULLNAME}:";
            PointerChanged += async (_, pointer) =&gt; Presentation = pointer != null ? await pointer.GetPresentation() : "";
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
                CallBack_OnSelectPointer = selectPointer =&gt;
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
            AfterClearFunc?.Invoke();
        }
    }
}
    </xsl:template>

<!--- 
//
// ============================ Звіт ============================
//
-->

    <!-- Список -->
    <xsl:template name="DocumentReport">
        <xsl:variable name="DocumentName" select="Document/Name"/>
        <xsl:variable name="FieldsTL" select="Document/ElementFields/Field"/>

/*
        <xsl:value-of select="$DocumentName"/>_Звіт.cs
        Звіт
*/

using Gtk;
using InterfaceGtk3;
using AccountingSoftware;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Довідники;
using <xsl:value-of select="$NameSpaceGeneratedCode"/>.Документи;

namespace <xsl:value-of select="$NameSpace"/>
{
    public static class <xsl:value-of select="$DocumentName"/>_Звіт
    {
        public static async ValueTask Сформувати()
        {
            <xsl:variable name="CountFieldsTL" select="count($FieldsTL)"/>
            string query = $@"
SELECT
    <xsl:for-each select="$FieldsTL">
    <xsl:choose>
        <xsl:when test="Type = 'pointer'">
            <xsl:variable name="name" select="Name" />
            <xsl:variable name="nameGroup" select="substring-before(Pointer, '.')" />
            <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
            <xsl:variable name="index">
                <xsl:for-each select="$FieldsTL[Type = 'pointer' and $namePointer = substring-after(Pointer, '.')]">
                    <xsl:if test="$name = Name and position() &gt; 1"><xsl:value-of select="position()"/></xsl:if>
                </xsl:for-each>
            </xsl:variable>
            <xsl:value-of select="$DocumentName"/>.{<xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="Name"/>} AS <xsl:value-of select="Name"/>,
            <xsl:choose>
                <xsl:when test="PresetntationFields/@Count = 1">
                    <xsl:value-of select="$nameGroup"/>_<xsl:value-of select="$namePointer"/><xsl:value-of select="$index"/>.{<xsl:value-of select="$namePointer"/>_Const.<xsl:value-of select="PresetntationFields/Field"/><xsl:text>}</xsl:text>
                </xsl:when>
                <xsl:when test="PresetntationFields/@Count &gt; 1">
                    <xsl:text>concat_ws (', '</xsl:text>
                    <xsl:for-each select="PresetntationFields/Field">
                        <xsl:value-of select="concat(', ', $nameGroup, '_', $namePointer, $index, '.{', $namePointer, '_Const.', text(), '}')"/>
                    </xsl:for-each>
                    <xsl:text>)</xsl:text>
                </xsl:when>
                <xsl:otherwise>'#'</xsl:otherwise>
            </xsl:choose> AS <xsl:value-of select="concat(Name, '_Назва')"/>
        </xsl:when>
        <xsl:otherwise>
            <xsl:value-of select="$DocumentName"/>.{<xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="Name"/>} AS <xsl:value-of select="Name"/>
        </xsl:otherwise>
    </xsl:choose>
    <xsl:if test="position() != $CountFieldsTL">,
    </xsl:if>
    </xsl:for-each>
FROM
    {<xsl:value-of select="$DocumentName"/>_Const.TABLE} AS <xsl:value-of select="$DocumentName"/>
    <xsl:for-each select="$FieldsTL[Type = 'pointer']">
        <xsl:variable name="name" select="Name" />
        <xsl:variable name="nameGroup" select="substring-before(Pointer, '.')" />
        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
        <xsl:variable name="index">
            <xsl:for-each select="$FieldsTL[Type = 'pointer' and $namePointer = substring-after(Pointer, '.')]">
                <xsl:if test="$name = Name and position() &gt; 1"><xsl:value-of select="position()"/></xsl:if>
            </xsl:for-each>
        </xsl:variable>
    LEFT JOIN {<xsl:value-of select="$namePointer"/>_Const.TABLE} AS <xsl:value-of select="$nameGroup"/>_<xsl:value-of select="$namePointer"/><xsl:value-of select="$index"/> ON <xsl:value-of select="$nameGroup"/>_<xsl:value-of select="$namePointer"/><xsl:value-of select="$index"/>.uid = 
        <xsl:value-of select="$DocumentName"/>.{<xsl:value-of select="$DocumentName"/>_Const.<xsl:value-of select="Name"/>}
    </xsl:for-each>
";
            ЗвітСторінка Звіт = new ЗвітСторінка()
            {
                ReportName = "<xsl:value-of select="$DocumentName"/>_Звіт",
                Caption = "<xsl:value-of select="$DocumentName"/>",
                Query = query,
                GetInfo = () =&gt; ValueTask.FromResult("")
            };

            <xsl:for-each select="$FieldsTL">
                <xsl:choose>
                    <xsl:when test="Type = 'pointer'">
                        <xsl:variable name="namePointer" select="substring-after(Pointer, '.')" />
                        <xsl:text>Звіт.ColumnSettings.Add("</xsl:text><xsl:value-of select="Name"/>_Назва", new("<xsl:value-of select="Caption"/>", "<xsl:value-of select="Name"/>", <xsl:value-of select="$namePointer"/>_Const.POINTER));
                    </xsl:when>
                    <xsl:when test="Type = 'integer' or Type = 'numeric'">
                        <xsl:text>Звіт.ColumnSettings.Add("</xsl:text><xsl:value-of select="Name"/>", new("<xsl:value-of select="Caption"/>", "", "", 1, ЗвітСторінка.ФункціяДляКолонкиБазоваДляЧисла));
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:text>Звіт.ColumnSettings.Add("</xsl:text><xsl:value-of select="Name"/>", new("<xsl:value-of select="Caption"/>"));
                    </xsl:otherwise>
                </xsl:choose>
            </xsl:for-each>
            await Звіт.Select();

            Звіт.FillTreeView();
            await Звіт.View(Program.GeneralNotebook);
        }
    }
}
    </xsl:template>

</xsl:stylesheet>