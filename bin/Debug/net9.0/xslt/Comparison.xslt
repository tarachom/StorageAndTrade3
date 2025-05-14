<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" indent="yes" />

  <xsl:template name="FieldsControl">
    <xsl:param name="TableName" />
    <xsl:param name="InfoSchemaFieldList" />
    <xsl:param name="InfoSchemaIndexList" />
    <xsl:param name="ConfigurationFieldList" />

    <xsl:for-each select="$ConfigurationFieldList">
      <xsl:variable name="ConfFieldName" select="NameInTable" />

      <Control_Field>
        <Name>
          <xsl:value-of select="Name"/>
        </Name>
        <NameInTable>
          <xsl:value-of select="$ConfFieldName"/>
        </NameInTable>
        <Index>
          <xsl:value-of select="IsIndex"/>
        </Index>
        <IndexExist>
          <xsl:if test="$InfoSchemaIndexList[Name = concat($TableName , '_', $ConfFieldName, '_idx')]">
            <xsl:text>yes</xsl:text>
          </xsl:if>
        </IndexExist>
        <xsl:choose>
          <xsl:when test="$InfoSchemaFieldList[Name = $ConfFieldName]">
            <IsExist>yes</IsExist>
            <Type>
              <xsl:variable name="ConfFieldType" select="Type" />
              <xsl:variable name="InfoSchemaFieldDataType" select="$InfoSchemaFieldList[Name = $ConfFieldName]/DataType" />
              <xsl:variable name="InfoSchemaFieldUdtName" select="$InfoSchemaFieldList[Name = $ConfFieldName]/UdtName" />

              <ConfType>
                <xsl:value-of select="$ConfFieldType"/>
              </ConfType>

              <DataType>
                <xsl:value-of select="$InfoSchemaFieldDataType"/>
              </DataType>

              <UdtName>
                <xsl:value-of select="$InfoSchemaFieldUdtName"/>
              </UdtName>

              <xsl:if test="$ConfFieldType = 'string'">
                <xsl:choose>
                  <xsl:when test="$InfoSchemaFieldDataType = 'text' and $InfoSchemaFieldUdtName = 'text'">
                    <Coincide>yes</Coincide>
                  </xsl:when>
                  <xsl:otherwise>
                    <Coincide>no</Coincide>
                    <DataTypeCreate>text</DataTypeCreate>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:if>

              <xsl:if test="$ConfFieldType = 'string[]'">
                <xsl:choose>
                  <xsl:when test="$InfoSchemaFieldDataType = 'ARRAY' and $InfoSchemaFieldUdtName = '_text'">
                    <Coincide>yes</Coincide>
                  </xsl:when>
                  <xsl:otherwise>
                    <Coincide>no</Coincide>
                    <DataTypeCreate>text[]</DataTypeCreate>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:if>

              <xsl:if test="$ConfFieldType = 'integer'">
                <xsl:choose>
                  <xsl:when test="$InfoSchemaFieldDataType = 'integer' and $InfoSchemaFieldUdtName = 'int4'">
                    <Coincide>yes</Coincide>
                  </xsl:when>
                  <xsl:otherwise>
                    <Coincide>no</Coincide>
                    <DataTypeCreate>integer</DataTypeCreate>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:if>

              <xsl:if test="$ConfFieldType = 'integer[]'">
                <xsl:choose>
                  <xsl:when test="$InfoSchemaFieldDataType = 'ARRAY' and $InfoSchemaFieldUdtName = '_int4'">
                    <Coincide>yes</Coincide>
                  </xsl:when>
                  <xsl:otherwise>
                    <Coincide>no</Coincide>
                    <DataTypeCreate>integer[]</DataTypeCreate>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:if>

              <xsl:if test="$ConfFieldType = 'numeric'">
                <xsl:choose>
                  <xsl:when test="$InfoSchemaFieldDataType = 'numeric' and $InfoSchemaFieldUdtName = 'numeric'">
                    <Coincide>yes</Coincide>
                  </xsl:when>
                  <xsl:otherwise>
                    <Coincide>no</Coincide>
                    <DataTypeCreate>numeric</DataTypeCreate>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:if>

              <xsl:if test="$ConfFieldType = 'numeric[]'">
                <xsl:choose>
                  <xsl:when test="$InfoSchemaFieldDataType = 'ARRAY' and $InfoSchemaFieldUdtName = '_numeric'">
                    <Coincide>yes</Coincide>
                  </xsl:when>
                  <xsl:otherwise>
                    <Coincide>no</Coincide>
                    <DataTypeCreate>numeric[]</DataTypeCreate>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:if>

              <xsl:if test="$ConfFieldType = 'boolean'">
                <xsl:choose>
                  <xsl:when test="$InfoSchemaFieldDataType = 'boolean' and $InfoSchemaFieldUdtName = 'bool'">
                    <Coincide>yes</Coincide>
                  </xsl:when>
                  <xsl:otherwise>
                    <Coincide>no</Coincide>
                    <DataTypeCreate>boolean</DataTypeCreate>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:if>

              <xsl:if test="$ConfFieldType = 'date'">
                <xsl:choose>
                  <xsl:when test="$InfoSchemaFieldDataType = 'date' and $InfoSchemaFieldUdtName = 'date'">
                    <Coincide>yes</Coincide>
                  </xsl:when>
                  <xsl:otherwise>
                    <Coincide>no</Coincide>
                    <DataTypeCreate>date</DataTypeCreate>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:if>

              <xsl:if test="$ConfFieldType = 'time'">
                <xsl:choose>
                  <xsl:when test="$InfoSchemaFieldDataType = 'time without time zone' and $InfoSchemaFieldUdtName = 'time'">
                    <Coincide>yes</Coincide>
                  </xsl:when>
                  <xsl:otherwise>
                    <Coincide>no</Coincide>
                    <DataTypeCreate>time without time zone</DataTypeCreate>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:if>

              <xsl:if test="$ConfFieldType = 'datetime'">
                <xsl:choose>
                  <xsl:when test="$InfoSchemaFieldDataType = 'timestamp without time zone' and $InfoSchemaFieldUdtName = 'timestamp'">
                    <Coincide>yes</Coincide>
                  </xsl:when>
                  <xsl:otherwise>
                    <Coincide>no</Coincide>
                    <DataTypeCreate>timestamp without time zone</DataTypeCreate>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:if>

              <xsl:if test="$ConfFieldType = 'pointer'">
                <xsl:choose>
                  <xsl:when test="$InfoSchemaFieldDataType = 'uuid' and $InfoSchemaFieldUdtName = 'uuid'">
                    <Coincide>yes</Coincide>
                  </xsl:when>
                  <xsl:otherwise>
                    <Coincide>no</Coincide>
                    <DataTypeCreate>uuid</DataTypeCreate>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:if>

			        <xsl:if test="$ConfFieldType = 'any_pointer'">
                <xsl:choose>
                  <xsl:when test="$InfoSchemaFieldDataType = 'uuid' and $InfoSchemaFieldUdtName = 'uuid'">
                    <Coincide>yes</Coincide>
                  </xsl:when>
                  <xsl:otherwise>
                    <Coincide>no</Coincide>
                    <DataTypeCreate>uuid</DataTypeCreate>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:if>
				
			        <xsl:if test="$ConfFieldType = 'composite_pointer'">
                <xsl:choose>
                  <xsl:when test="$InfoSchemaFieldDataType = 'USER-DEFINED' and $InfoSchemaFieldUdtName = 'uuidtext'">
                    <Coincide>yes</Coincide>
                  </xsl:when>
                  <xsl:otherwise>
                    <Coincide>no</Coincide>
                    <DataTypeCreate>uuidtext</DataTypeCreate>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:if>

              <xsl:if test="$ConfFieldType = 'composite_text'">
                <xsl:choose>
                  <xsl:when test="$InfoSchemaFieldDataType = 'USER-DEFINED' and $InfoSchemaFieldUdtName = 'nametext'">
                    <Coincide>yes</Coincide>
                  </xsl:when>
                  <xsl:otherwise>
                    <Coincide>no</Coincide>
                    <DataTypeCreate>nametext</DataTypeCreate>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:if>
				
              <xsl:if test="$ConfFieldType = 'enum'">
                <xsl:choose>
                  <xsl:when test="$InfoSchemaFieldDataType = 'integer' and $InfoSchemaFieldUdtName = 'int4'">
                    <Coincide>yes</Coincide>
                  </xsl:when>
                  <xsl:otherwise>
                    <Coincide>no</Coincide>
                    <DataTypeCreate>integer</DataTypeCreate>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:if>

			        <xsl:if test="$ConfFieldType = 'bytea'">
                <xsl:choose>
                  <xsl:when test="$InfoSchemaFieldDataType = 'bytea' and $InfoSchemaFieldUdtName = 'bytea'">
                    <Coincide>yes</Coincide>
                  </xsl:when>
                  <xsl:otherwise>
                    <Coincide>no</Coincide>
                    <DataTypeCreate>bytea</DataTypeCreate>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:if>

              <xsl:if test="$ConfFieldType = 'uuid[]'">
                <xsl:choose>
                  <xsl:when test="$InfoSchemaFieldDataType = 'ARRAY' and $InfoSchemaFieldUdtName = '_uuid'">
                    <Coincide>yes</Coincide>
                  </xsl:when>
                  <xsl:otherwise>
                    <Coincide>no</Coincide>
                    <DataTypeCreate>uuid[]</DataTypeCreate>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:if>
				
            </Type>
          </xsl:when>
          <xsl:otherwise>
            <IsExist>no</IsExist>

            <xsl:call-template name="FieldCreate">
              <xsl:with-param name="ConfFieldName" select="Name" />
              <xsl:with-param name="ConfFieldNameInTable" select="$ConfFieldName" />
              <xsl:with-param name="ConfFieldType" select="Type" />
		          <xsl:with-param name="ConfFieldIndex" select="IsIndex" />
              <xsl:with-param name="ConfFieldNotNull" select="IsNotNull" />
            </xsl:call-template>

          </xsl:otherwise>
        </xsl:choose>
      
      </Control_Field>

    </xsl:for-each>
  </xsl:template>

  <xsl:template name="FieldCreate">
    <xsl:param name="ConfFieldName" />
    <xsl:param name="ConfFieldNameInTable" />
    <xsl:param name="ConfFieldType" />
	  <xsl:param name="ConfFieldIndex" />
    <xsl:param name="ConfFieldNotNull" />
	  
    <FieldCreate>
      <Name>
        <xsl:value-of select="$ConfFieldName"/>
      </Name>

      <NameInTable>
        <xsl:value-of select="$ConfFieldNameInTable"/>
      </NameInTable>

      <ConfType>
        <xsl:value-of select="$ConfFieldType"/>
      </ConfType>

      <DataType>
        <xsl:choose>
          <xsl:when test="Type = 'string'">
            <xsl:text>text</xsl:text>
          </xsl:when>
          <xsl:when test="Type = 'string[]'">
            <xsl:text>text[]</xsl:text>
          </xsl:when>
          <xsl:when test="Type = 'integer'">
            <xsl:text>integer</xsl:text>
          </xsl:when>
          <xsl:when test="Type = 'integer[]'">
            <xsl:text>integer[]</xsl:text>
          </xsl:when>
          <xsl:when test="Type = 'numeric'">
            <xsl:text>numeric</xsl:text>
          </xsl:when>
          <xsl:when test="Type = 'numeric[]'">
            <xsl:text>numeric[]</xsl:text>
          </xsl:when>
          <xsl:when test="Type = 'boolean'">
            <xsl:text>boolean</xsl:text>
          </xsl:when>
          <xsl:when test="Type = 'date'">
            <xsl:text>date</xsl:text>
          </xsl:when>
          <xsl:when test="Type = 'time'">
            <xsl:text>time without time zone</xsl:text>
          </xsl:when>
          <xsl:when test="Type = 'datetime'">
            <xsl:text>timestamp without time zone</xsl:text>
          </xsl:when>
          <xsl:when test="Type = 'pointer'">
            <xsl:text>uuid</xsl:text>
          </xsl:when>
		      <xsl:when test="Type = 'any_pointer'">
            <xsl:text>uuid</xsl:text>
          </xsl:when>
		      <xsl:when test="Type = 'composite_pointer'">
            <xsl:text>uuidtext</xsl:text>
          </xsl:when>
          <xsl:when test="Type = 'composite_text'">
            <xsl:text>nametext</xsl:text>
          </xsl:when>
          <xsl:when test="Type = 'enum'">
            <xsl:text>integer</xsl:text>
          </xsl:when>
		      <xsl:when test="Type = 'bytea'">
            <xsl:text>bytea</xsl:text>
          </xsl:when>
          <xsl:when test="Type = 'uuid[]'">
            <xsl:text>uuid[]</xsl:text>
          </xsl:when>
        </xsl:choose>
      </DataType>

      <Index>
		    <xsl:value-of select="$ConfFieldIndex"/>
      </Index>

      <NotNull>
		    <xsl:value-of select="$ConfFieldNotNull"/>
      </NotNull>
    </FieldCreate>

  </xsl:template>

  <xsl:template name="TabularPartsControl">
    <xsl:param name="InfoSchemaTableList" />
    <xsl:param name="ConfigurationTablePartList" />
    <xsl:param name="IsCreateOwner" />

    <xsl:for-each select="$ConfigurationTablePartList">
      <xsl:variable name="ConTablePart" select="Name" />
      <xsl:variable name="ConfTablePartTable" select="Table" />

      <Control_TabularParts>
        <Name>
          <xsl:value-of select="$ConTablePart"/>
        </Name>
        <Table>
          <xsl:value-of select="$ConfTablePartTable"/>
        </Table>
        <IsCreateOwner>
          <xsl:value-of select="$IsCreateOwner"/>
        </IsCreateOwner>
        <xsl:choose>
          <xsl:when test="$InfoSchemaTableList[Name = $ConfTablePartTable]">
            <IsExist>yes</IsExist>

            <xsl:choose>
              <xsl:when test="$IsCreateOwner = 'yes'">
                <xsl:call-template name="FieldsControl">
                  <xsl:with-param name="TableName" select="$ConfTablePartTable" />
                  <xsl:with-param name="ConfigurationFieldList" select="PredefinedFields/PredefinedField[Name != 'uid']" />
                  <xsl:with-param name="InfoSchemaFieldList" select="$InfoSchemaTableList[Name = $ConfTablePartTable]/Column" />
                  <xsl:with-param name="InfoSchemaIndexList" select="$InfoSchemaTableList[Name = $ConfTablePartTable]/Index" />
                </xsl:call-template>
              </xsl:when>
              <!-- 
              В табличній частині на даний момент два поля, тому цей блок непотрібний, бо полів для перевірки немає
              <xsl:otherwise>
                <xsl:call-template name="FieldsControl">
                  <xsl:with-param name="TableName" select="$ConfTablePartTable" />
                  <xsl:with-param name="ConfigurationFieldList" select="PredefinedFields/PredefinedField[Name != 'uid' and Name != 'owner']" />
                  <xsl:with-param name="InfoSchemaFieldList" select="$InfoSchemaTableList[Name = $ConfTablePartTable]/Column" />
                  <xsl:with-param name="InfoSchemaIndexList" select="$InfoSchemaTableList[Name = $ConfTablePartTable]/Index" />
                </xsl:call-template>
              </xsl:otherwise>
              -->
            </xsl:choose>

            <xsl:call-template name="FieldsControl">
			        <xsl:with-param name="TableName" select="$ConfTablePartTable" />
              <xsl:with-param name="ConfigurationFieldList" select="Fields/Field" />
              <xsl:with-param name="InfoSchemaFieldList" select="$InfoSchemaTableList[Name = $ConfTablePartTable]/Column" />
			        <xsl:with-param name="InfoSchemaIndexList" select="$InfoSchemaTableList[Name = $ConfTablePartTable]/Index" />
            </xsl:call-template>

          </xsl:when>
          <xsl:otherwise>
            <IsExist>no</IsExist>

            <TableCreate>

              <xsl:for-each select="Fields/Field">
                <xsl:call-template name="FieldCreate">
                  <xsl:with-param name="ConfFieldName" select="Name" />
                  <xsl:with-param name="ConfFieldNameInTable" select="NameInTable" />
                  <xsl:with-param name="ConfFieldType" select="Type" />
				          <xsl:with-param name="ConfFieldIndex" select="IsIndex" />
                  <xsl:with-param name="ConfFieldNotNull" select="IsNotNull" />
                </xsl:call-template>
              </xsl:for-each>

            </TableCreate>

          </xsl:otherwise>
        </xsl:choose>
      </Control_TabularParts>

    </xsl:for-each>

  </xsl:template>

  <xsl:template name="SecondConfigurationFields">
    <xsl:param name="InfoSchemaTableList" />
    <xsl:param name="DocumentConfigurationFieldNodes" />
    <xsl:param name="SecondConfigurationFieldsNodes" />
    <!-- Document, Directory ... -->
    <xsl:param name="Type" />
    <xsl:param name="Name" />
    <xsl:param name="TableName" />

    <xsl:for-each select="$SecondConfigurationFieldsNodes">
      <xsl:variable name="SecondConfFieldName" select="Name" />
      <xsl:variable name="SecondConfFieldNameInTable" select="NameInTable" />
      <xsl:variable name="SecondConfFieldType" select="Type" />

      <xsl:variable name="CountField" select="count($DocumentConfigurationFieldNodes[NameInTable = $SecondConfFieldNameInTable])" />

      <xsl:choose>
        <xsl:when test="$CountField = 0">

          <Control_Table>
            <Type>
              <xsl:value-of select="$Type"/>
            </Type>
            <Name>
              <xsl:value-of select="$Name"/>
            </Name>
            <Table>
              <xsl:value-of select="$TableName"/>
            </Table>
            <IsExist>yes</IsExist>
            <Control_Field>
              <Name>
                <xsl:value-of select="$SecondConfFieldName"/>
              </Name>
              <NameInTable>
                <xsl:value-of select="$SecondConfFieldNameInTable"/>
              </NameInTable>
              <IsExist>delete</IsExist>
            </Control_Field>
          </Control_Table>

        </xsl:when>
        <xsl:otherwise>

          <xsl:if test="$SecondConfFieldType = 'pointer' or $SecondConfFieldType = 'any_pointer' or $SecondConfFieldType = 'enum'">

            <xsl:variable name="DocumentConfigurationType" select="$DocumentConfigurationFieldNodes[NameInTable = $SecondConfFieldNameInTable]/Type" />

            <!-- Вказівник з конфігурації за умови що типи не відрізняються -->
            <xsl:variable name="DocumentConfigurationPointer" select="$DocumentConfigurationFieldNodes
                                [NameInTable = $SecondConfFieldNameInTable and Type = $SecondConfFieldType]/Pointer" />

            <!-- Вказівник з копії конфігурації -->
            <xsl:variable name="SecondConfFieldPointer" select="Pointer" />

            <xsl:if test="($SecondConfFieldPointer != $DocumentConfigurationPointer)">

              <Control_Table>
                <Type>
                  <xsl:value-of select="$Type"/>
                  <xsl:text>.TablePart</xsl:text>
                </Type>
                <Name>
                  <xsl:value-of select="$Name"/>
                </Name>
                <Table>
                  <xsl:value-of select="$TableName"/>
                </Table>
                <IsExist>yes</IsExist>
                <Control_Field>
                  <Name>
                    <xsl:value-of select="$SecondConfFieldName"/>
                  </Name>
                  <NameInTable>
                    <xsl:value-of select="$SecondConfFieldNameInTable"/>
                  </NameInTable>
                  <IsExist>yes</IsExist>
                  <Type>
                    <Coincide>clear</Coincide>

                    <xsl:choose>
                      <xsl:when test="$SecondConfFieldType = 'pointer'">
                        <DataTypeCreate>uuid</DataTypeCreate>
                      </xsl:when>
                      <xsl:when test="$SecondConfFieldType = 'enum'">
                        <DataTypeCreate>integer</DataTypeCreate>
                      </xsl:when>
                    </xsl:choose>

                  </Type>
                </Control_Field>
              </Control_Table>

            </xsl:if>
          </xsl:if>

        </xsl:otherwise>
      </xsl:choose>

    </xsl:for-each>

  </xsl:template>

  <xsl:template name="SecondConfigurationTabularParts">
    <xsl:param name="InfoSchemaTableList" />
    <xsl:param name="DocumentConfigurationTabularParts" />
    <xsl:param name="SecondConfigurationTabularParts" />
    <xsl:param name="IsExistOwner" />
    <xsl:param name="Type" />

    <xsl:for-each select="$SecondConfigurationTabularParts">
      <xsl:variable name="SecondConfTablePartName" select="Name" />
      <xsl:variable name="SecondConfTablePartTable" select="Table" />

      <xsl:variable name="CountTablePart" select="count($DocumentConfigurationTabularParts[Table = $SecondConfTablePartTable])" />
      <xsl:variable name="CountTablePartInfoSchema" select="count($InfoSchemaTableList[Name = $SecondConfTablePartTable])" />

      <xsl:choose>

        <!-- Якщо таблична частина відсутня або обєкт відсутній в конфігурації та є наявна таблиця в базі даних -->
        <xsl:when test="($CountTablePart = 0 or $IsExistOwner = 0) and $CountTablePartInfoSchema = 1">

          <Control_Table>
            <Type>
              <xsl:value-of select="$Type"/>
              <xsl:text>.TablePart</xsl:text>
            </Type>
            <Name>
              <xsl:value-of select="$SecondConfTablePartName"/>
            </Name>
            <Table>
              <xsl:value-of select="$SecondConfTablePartTable"/>
            </Table>
            <IsExist>delete</IsExist>
          </Control_Table>

        </xsl:when>
        <xsl:otherwise>

          <xsl:call-template name="SecondConfigurationFields">
            <xsl:with-param name="InfoSchemaTableList" select="$InfoSchemaTableList" />
            <xsl:with-param name="DocumentConfigurationFieldNodes" select="$DocumentConfigurationTabularParts[Table = $SecondConfTablePartTable]/Fields/Field" />
            <xsl:with-param name="SecondConfigurationFieldsNodes" select="Fields/Field" />
            <xsl:with-param name="Type" select="concat($Type, '.TablePart')"/>
            <xsl:with-param name="Name" select="$SecondConfTablePartName" />
            <xsl:with-param name="TableName" select="$SecondConfTablePartTable" />
          </xsl:call-template>

        </xsl:otherwise>
      </xsl:choose>

    </xsl:for-each>

  </xsl:template>

  <xsl:template name="SecondConfiguration">
    <xsl:param name="InfoSchemaTableList" />
    <xsl:param name="DocumentConfigurationNodes" />
    <xsl:param name="SecondConfigurationNodes" />
    <xsl:param name="Type" />

    <xsl:for-each select="$SecondConfigurationNodes">
      <xsl:variable name="SecondConfDirectoryName" select="Name" />
      <xsl:variable name="SecondConfDirectoryTable" select="Table" />

      <xsl:variable name="CountObject"
           select="count($DocumentConfigurationNodes[Table = $SecondConfDirectoryTable])" />

      <xsl:variable name="CountTableInfoSchema"
          select="count($InfoSchemaTableList[Name = $SecondConfDirectoryTable])" />

      <!-- Якщо обєкт відсутній в конфігурації, але є наявна таблиця в базі даних -->
      <xsl:if test="$CountObject = 0 and $CountTableInfoSchema = 1">
        <Control_Table>
          <Type>
            <xsl:value-of select="$Type"/>
          </Type>
          <Name>
            <xsl:value-of select="$SecondConfDirectoryName"/>
          </Name>
          <Table>
            <xsl:value-of select="$SecondConfDirectoryTable"/>
          </Table>
          <IsExist>delete</IsExist>
        </Control_Table>
      </xsl:if>

      <xsl:if test="$CountObject = 1">
		  
        <xsl:call-template name="SecondConfigurationFields">
          <xsl:with-param name="InfoSchemaTableList" select="$InfoSchemaTableList" />
          <xsl:with-param name="DocumentConfigurationFieldNodes" select="$DocumentConfigurationNodes[Table = $SecondConfDirectoryTable]/Fields/Field" />
          <xsl:with-param name="SecondConfigurationFieldsNodes" select="Fields/Field" />
          <xsl:with-param name="Type" select="$Type"/>
          <xsl:with-param name="Name" select="$SecondConfDirectoryName" />
          <xsl:with-param name="TableName" select="$SecondConfDirectoryTable" />
        </xsl:call-template>

      </xsl:if>

      <xsl:call-template name="SecondConfigurationTabularParts">
        <xsl:with-param name="InfoSchemaTableList" select="$InfoSchemaTableList" />
        <xsl:with-param name="DocumentConfigurationTabularParts" select="$DocumentConfigurationNodes[Table = $SecondConfDirectoryTable]/TabularParts/TablePart" />
        <xsl:with-param name="SecondConfigurationTabularParts" select="TabularParts/TablePart" />
        <xsl:with-param name="IsExistOwner" select="$CountObject" />
        <xsl:with-param name="Type" select="$Type" />
      </xsl:call-template>

    </xsl:for-each>
  </xsl:template>

  <xsl:template match="/">

    <root>

      <xsl:variable name="InfoSchemaTableList" select="root/InformationSchema/Table" />
      <xsl:variable name="documentConfiguration" select="root/NewConfiguration" />
      <xsl:variable name="documentSecondConfiguration" select="root/SecondConfiguration" />
      
      <xsl:for-each select="$documentConfiguration/Configuration/ConstantsBlocks">
        <xsl:variable name="ConfObjName">Константи</xsl:variable>
        <xsl:variable name="ConfObjTable">tab_constants</xsl:variable>

        <Control_Table>
          <Type>Constants</Type>
          <Name>
            <xsl:value-of select="$ConfObjName"/>
          </Name>
          <Table>
            <xsl:value-of select="$ConfObjTable"/>
          </Table>

          <xsl:choose>
            <xsl:when test="$InfoSchemaTableList[Name = $ConfObjTable]">
              <IsExist>yes</IsExist>

              <xsl:call-template name="FieldsControl">
				        <xsl:with-param name="TableName" select="$ConfObjTable" />
                <xsl:with-param name="ConfigurationFieldList" select="ConstantsBlock/./Constants/Constant" />
                <xsl:with-param name="InfoSchemaFieldList" select="$InfoSchemaTableList[Name = $ConfObjTable]/Column" />
				        <xsl:with-param name="InfoSchemaIndexList" select="$InfoSchemaTableList[Name = $ConfObjTable]/Index" />
              </xsl:call-template>

            </xsl:when>
            <xsl:otherwise>
              <IsExist>no</IsExist>

              <TableCreate>

                <xsl:for-each select="ConstantsBlock/./Constants/Constant">
                  <xsl:call-template name="FieldCreate">
                    <xsl:with-param name="ConfFieldName" select="Name" />
                    <xsl:with-param name="ConfFieldNameInTable" select="NameInTable" />
                    <xsl:with-param name="ConfFieldType" select="Type" />
					          <xsl:with-param name="ConfFieldIndex" select="IsIndex" />
                    <xsl:with-param name="ConfFieldNotNull" select="IsNotNull" />
				          </xsl:call-template>
                </xsl:for-each>

              </TableCreate>

            </xsl:otherwise>
          </xsl:choose>

          <xsl:for-each select="ConstantsBlock/./Constants/Constant">

            <xsl:call-template name="TabularPartsControl">
              <xsl:with-param name="ConfigurationTablePartList" select="TabularParts/TablePart" />
              <xsl:with-param name="InfoSchemaTableList" select="$InfoSchemaTableList" />
              <xsl:with-param name="IsCreateOwner">no</xsl:with-param>
            </xsl:call-template>

          </xsl:for-each>

        </Control_Table>

        <xsl:if test="$InfoSchemaTableList[Name = $ConfObjTable]">

          <xsl:call-template name="SecondConfigurationFields">
            <xsl:with-param name="InfoSchemaTableList" select="$InfoSchemaTableList" />
            <xsl:with-param name="DocumentConfigurationFieldNodes" select="ConstantsBlock/./Constants/Constant" />
            <xsl:with-param name="SecondConfigurationFieldsNodes" select="$documentSecondConfiguration/Configuration/ConstantsBlocks/ConstantsBlock/./Constants/Constant" />
            <xsl:with-param name="Type">Constants</xsl:with-param>
            <xsl:with-param name="Name" select="$ConfObjName" />
            <xsl:with-param name="TableName" select="$ConfObjTable" />
          </xsl:call-template>

          <xsl:for-each select="$documentSecondConfiguration/Configuration/ConstantsBlocks/ConstantsBlock/./Constants/Constant">
            <xsl:variable name="SecondConfBlockName" select="../../Name" />
            <xsl:variable name="SecondConfNameInTable" select="NameInTable" />

            <xsl:call-template name="SecondConfigurationTabularParts">
              <xsl:with-param name="InfoSchemaTableList" select="$InfoSchemaTableList" />
              <xsl:with-param name="DocumentConfigurationTabularParts"
                              select="$documentConfiguration/Configuration/ConstantsBlocks/ConstantsBlock
                                     [Name = $SecondConfBlockName]/Constants/Constant
                                     [NameInTable = $SecondConfNameInTable]/TabularParts/TablePart" />
              <xsl:with-param name="SecondConfigurationTabularParts" select="TabularParts/TablePart" />
              <xsl:with-param name="IsExistOwner" select="1" />
              <xsl:with-param name="Type">Constants</xsl:with-param>
            </xsl:call-template>

          </xsl:for-each>

        </xsl:if>

      </xsl:for-each>

      <xsl:for-each select="$documentConfiguration/Configuration/Directories/Directory">
        <xsl:variable name="ConfDirectoryName" select="Name" />
        <xsl:variable name="ConfDirectoryTable" select="Table" />

        <Control_Table>
          <Type>Directory</Type>
          <Name>
            <xsl:value-of select="$ConfDirectoryName"/>
          </Name>
          <Table>
            <xsl:value-of select="$ConfDirectoryTable"/>
          </Table>

          <xsl:choose>
            <xsl:when test="$InfoSchemaTableList[Name = $ConfDirectoryTable]">
              <IsExist>yes</IsExist>

              <!-- Перевірка наперед визначених полів, окрім первинного ключа -->
              <xsl:call-template name="FieldsControl">
				        <xsl:with-param name="TableName" select="$ConfDirectoryTable" />
                <xsl:with-param name="ConfigurationFieldList" select="PredefinedFields/PredefinedField[Name != 'uid']" />
                <xsl:with-param name="InfoSchemaFieldList" select="$InfoSchemaTableList[Name = $ConfDirectoryTable]/Column" />
				        <xsl:with-param name="InfoSchemaIndexList" select="$InfoSchemaTableList[Name = $ConfDirectoryTable]/Index" />
              </xsl:call-template>

              <xsl:call-template name="FieldsControl">
				        <xsl:with-param name="TableName" select="$ConfDirectoryTable" />
                <xsl:with-param name="ConfigurationFieldList" select="Fields/Field" />
                <xsl:with-param name="InfoSchemaFieldList" select="$InfoSchemaTableList[Name = $ConfDirectoryTable]/Column" />
				        <xsl:with-param name="InfoSchemaIndexList" select="$InfoSchemaTableList[Name = $ConfDirectoryTable]/Index" />
              </xsl:call-template>

            </xsl:when>
            <xsl:otherwise>
              <IsExist>no</IsExist>

              <TableCreate>

                <xsl:for-each select="Fields/Field">
                  <xsl:call-template name="FieldCreate">
                    <xsl:with-param name="ConfFieldName" select="Name" />
                    <xsl:with-param name="ConfFieldNameInTable" select="NameInTable" />
                    <xsl:with-param name="ConfFieldType" select="Type" />
					          <xsl:with-param name="ConfFieldIndex" select="IsIndex" />
                    <xsl:with-param name="ConfFieldNotNull" select="IsNotNull" />
                  </xsl:call-template>
                </xsl:for-each>

              </TableCreate>

            </xsl:otherwise>
          </xsl:choose>

          <xsl:call-template name="TabularPartsControl">
            <xsl:with-param name="ConfigurationTablePartList" select="TabularParts/TablePart" />
            <xsl:with-param name="InfoSchemaTableList" select="$InfoSchemaTableList" />
            <xsl:with-param name="IsCreateOwner">yes</xsl:with-param>
          </xsl:call-template>

        </Control_Table>

      </xsl:for-each>

      <xsl:call-template name="SecondConfiguration">
        <xsl:with-param name="InfoSchemaTableList" select="$InfoSchemaTableList" />
        <xsl:with-param name="DocumentConfigurationNodes" select="$documentConfiguration/Configuration/Directories/Directory" />
        <xsl:with-param name="SecondConfigurationNodes" select="$documentSecondConfiguration/Configuration/Directories/Directory" />
        <xsl:with-param name="Type">Directory</xsl:with-param>
      </xsl:call-template>

      <xsl:for-each select="$documentConfiguration/Configuration/Documents/Document">
        <xsl:variable name="ConfDirectoryName" select="Name" />
        <xsl:variable name="ConfDirectoryTable" select="Table" />

        <Control_Table>
          <Type>Document</Type>
          <Name>
            <xsl:value-of select="$ConfDirectoryName"/>
          </Name>
          <Table>
            <xsl:value-of select="$ConfDirectoryTable"/>
          </Table>

          <xsl:choose>
            <xsl:when test="$InfoSchemaTableList[Name = $ConfDirectoryTable]">
              <IsExist>yes</IsExist>

               <!-- Перевірка наперед визначених полів, окрім первинного ключа -->
              <xsl:call-template name="FieldsControl">
				        <xsl:with-param name="TableName" select="$ConfDirectoryTable" />
                <xsl:with-param name="ConfigurationFieldList" select="PredefinedFields/PredefinedField[Name != 'uid']" />
                <xsl:with-param name="InfoSchemaFieldList" select="$InfoSchemaTableList[Name = $ConfDirectoryTable]/Column" />
				        <xsl:with-param name="InfoSchemaIndexList" select="$InfoSchemaTableList[Name = $ConfDirectoryTable]/Index" />
              </xsl:call-template>

              <xsl:call-template name="FieldsControl">
				        <xsl:with-param name="TableName" select="$ConfDirectoryTable" />
                <xsl:with-param name="ConfigurationFieldList" select="Fields/Field" />
                <xsl:with-param name="InfoSchemaFieldList" select="$InfoSchemaTableList[Name = $ConfDirectoryTable]/Column" />
				        <xsl:with-param name="InfoSchemaIndexList" select="$InfoSchemaTableList[Name = $ConfDirectoryTable]/Index" />				  
              </xsl:call-template>

            </xsl:when>
            <xsl:otherwise>
              <IsExist>no</IsExist>

              <TableCreate>

                <xsl:for-each select="Fields/Field">
                  <xsl:call-template name="FieldCreate">
                    <xsl:with-param name="ConfFieldName" select="Name" />
                    <xsl:with-param name="ConfFieldNameInTable" select="NameInTable" />
                    <xsl:with-param name="ConfFieldType" select="Type" />
					          <xsl:with-param name="ConfFieldIndex" select="IsIndex" />
                    <xsl:with-param name="ConfFieldNotNull" select="IsNotNull" />
                  </xsl:call-template>
                </xsl:for-each>

              </TableCreate>

            </xsl:otherwise>
          </xsl:choose>

          <xsl:call-template name="TabularPartsControl">
            <xsl:with-param name="ConfigurationTablePartList" select="TabularParts/TablePart" />
            <xsl:with-param name="InfoSchemaTableList" select="$InfoSchemaTableList" />
            <xsl:with-param name="IsCreateOwner">yes</xsl:with-param>
          </xsl:call-template>

        </Control_Table>

      </xsl:for-each>

      <xsl:call-template name="SecondConfiguration">
        <xsl:with-param name="InfoSchemaTableList" select="$InfoSchemaTableList" />
        <xsl:with-param name="DocumentConfigurationNodes" select="$documentConfiguration/Configuration/Documents/Document" />
        <xsl:with-param name="SecondConfigurationNodes" select="$documentSecondConfiguration/Configuration/Documents/Document" />
        <xsl:with-param name="Type">Document</xsl:with-param>
      </xsl:call-template>

      <xsl:for-each select="$documentConfiguration/Configuration/RegistersInformation/RegisterInformation">
        <xsl:variable name="ConfDirectoryName" select="Name" />
        <xsl:variable name="ConfDirectoryTable" select="Table" />

        <Control_Table>
          <Type>RegisterInformation</Type>
          <Name>
            <xsl:value-of select="$ConfDirectoryName"/>
          </Name>
          <Table>
            <xsl:value-of select="$ConfDirectoryTable"/>
          </Table>

          <xsl:choose>
            <xsl:when test="$InfoSchemaTableList[Name = $ConfDirectoryTable]">
              <IsExist>yes</IsExist>

               <!-- Перевірка наперед визначених полів, окрім первинного ключа -->
              <xsl:call-template name="FieldsControl">
				        <xsl:with-param name="TableName" select="$ConfDirectoryTable" />
                <xsl:with-param name="ConfigurationFieldList" select="PredefinedFields/PredefinedField[Name != 'uid']" />
                <xsl:with-param name="InfoSchemaFieldList" select="$InfoSchemaTableList[Name = $ConfDirectoryTable]/Column" />
				        <xsl:with-param name="InfoSchemaIndexList" select="$InfoSchemaTableList[Name = $ConfDirectoryTable]/Index" />
              </xsl:call-template>

              <xsl:call-template name="FieldsControl">
				        <xsl:with-param name="TableName" select="$ConfDirectoryTable" />
                <xsl:with-param name="ConfigurationFieldList" select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field" />
                <xsl:with-param name="InfoSchemaFieldList" select="$InfoSchemaTableList[Name = $ConfDirectoryTable]/Column" />
				        <xsl:with-param name="InfoSchemaIndexList" select="$InfoSchemaTableList[Name = $ConfDirectoryTable]/Index" />
              </xsl:call-template>

            </xsl:when>
            <xsl:otherwise>
              <IsExist>no</IsExist>

              <TableCreate>

                <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
                  <xsl:call-template name="FieldCreate">
                    <xsl:with-param name="ConfFieldName" select="Name" />
                    <xsl:with-param name="ConfFieldNameInTable" select="NameInTable" />
                    <xsl:with-param name="ConfFieldType" select="Type" />
					          <xsl:with-param name="ConfFieldIndex" select="IsIndex" />
                    <xsl:with-param name="ConfFieldNotNull" select="IsNotNull" />
                  </xsl:call-template>
                </xsl:for-each>

              </TableCreate>

            </xsl:otherwise>
          </xsl:choose>

        </Control_Table>

        <xsl:call-template name="SecondConfigurationFields">
          <xsl:with-param name="InfoSchemaTableList" select="$InfoSchemaTableList" />
          <xsl:with-param name="DocumentConfigurationFieldNodes" select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field" />
          <xsl:with-param name="SecondConfigurationFieldsNodes" select="$documentSecondConfiguration/Configuration/RegistersInformation/RegisterInformation[Name = $ConfDirectoryName]/*/Fields/Field" />
          <xsl:with-param name="Type">RegisterInformation</xsl:with-param>
          <xsl:with-param name="Name" select="$ConfDirectoryName" />
          <xsl:with-param name="TableName" select="$ConfDirectoryTable" />
        </xsl:call-template>
		  
      </xsl:for-each>

      <xsl:call-template name="SecondConfiguration">
        <xsl:with-param name="InfoSchemaTableList" select="$InfoSchemaTableList" />
        <xsl:with-param name="DocumentConfigurationNodes" select="$documentConfiguration/Configuration/RegistersInformation/RegisterInformation" />
        <xsl:with-param name="SecondConfigurationNodes" select="$documentSecondConfiguration/Configuration/RegistersInformation/RegisterInformation" />
        <xsl:with-param name="Type">RegisterInformation</xsl:with-param>
      </xsl:call-template>
		
      <xsl:for-each select="$documentConfiguration/Configuration/RegistersAccumulation/RegisterAccumulation">
        <xsl:variable name="ConfDirectoryName" select="Name" />
        <xsl:variable name="ConfDirectoryTable" select="Table" />

        <Control_Table>
          <Type>RegisterAccumulation</Type>
          <Name>
            <xsl:value-of select="$ConfDirectoryName"/>
          </Name>
          <Table>
            <xsl:value-of select="$ConfDirectoryTable"/>
          </Table>

          <xsl:choose>
            <xsl:when test="$InfoSchemaTableList[Name = $ConfDirectoryTable]">
              <IsExist>yes</IsExist>

               <!-- Перевірка наперед визначених полів, окрім первинного ключа -->
              <xsl:call-template name="FieldsControl">
				        <xsl:with-param name="TableName" select="$ConfDirectoryTable" />
                <xsl:with-param name="ConfigurationFieldList" select="PredefinedFields/PredefinedField[Name != 'uid']" />
                <xsl:with-param name="InfoSchemaFieldList" select="$InfoSchemaTableList[Name = $ConfDirectoryTable]/Column" />
				        <xsl:with-param name="InfoSchemaIndexList" select="$InfoSchemaTableList[Name = $ConfDirectoryTable]/Index" />
              </xsl:call-template>

              <xsl:call-template name="FieldsControl">
				        <xsl:with-param name="TableName" select="$ConfDirectoryTable" />
                <xsl:with-param name="ConfigurationFieldList" select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field" />
                <xsl:with-param name="InfoSchemaFieldList" select="$InfoSchemaTableList[Name = $ConfDirectoryTable]/Column" />
				        <xsl:with-param name="InfoSchemaIndexList" select="$InfoSchemaTableList[Name = $ConfDirectoryTable]/Index" />
              </xsl:call-template>

            </xsl:when>
            <xsl:otherwise>
              <IsExist>no</IsExist>

              <TableCreate>

                <xsl:for-each select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field">
                  <xsl:call-template name="FieldCreate">
                    <xsl:with-param name="ConfFieldName" select="Name" />
                    <xsl:with-param name="ConfFieldNameInTable" select="NameInTable" />
                    <xsl:with-param name="ConfFieldType" select="Type" />
					          <xsl:with-param name="ConfFieldIndex" select="IsIndex" />
                    <xsl:with-param name="ConfFieldNotNull" select="IsNotNull" />
                  </xsl:call-template>
                </xsl:for-each>

              </TableCreate>

            </xsl:otherwise>
          </xsl:choose>

          <xsl:call-template name="TabularPartsControl">
            <xsl:with-param name="ConfigurationTablePartList" select="TabularParts/TablePart" />
            <xsl:with-param name="InfoSchemaTableList" select="$InfoSchemaTableList" />
            <xsl:with-param name="IsCreateOwner">no</xsl:with-param>
          </xsl:call-template>

        </Control_Table>
		  
        <xsl:call-template name="SecondConfigurationFields">
          <xsl:with-param name="InfoSchemaTableList" select="$InfoSchemaTableList" />
          <xsl:with-param name="DocumentConfigurationFieldNodes" select="(DimensionFields|ResourcesFields|PropertyFields)/Fields/Field" />
          <xsl:with-param name="SecondConfigurationFieldsNodes" select="$documentSecondConfiguration/Configuration/RegistersAccumulation/RegisterAccumulation[Name = $ConfDirectoryName]/*/Fields/Field" />
          <xsl:with-param name="Type">RegisterAccumulation</xsl:with-param>
          <xsl:with-param name="Name" select="$ConfDirectoryName" />
          <xsl:with-param name="TableName" select="$ConfDirectoryTable" />
        </xsl:call-template>
		  
      </xsl:for-each>

      <xsl:call-template name="SecondConfiguration">
        <xsl:with-param name="InfoSchemaTableList" select="$InfoSchemaTableList" />
        <xsl:with-param name="DocumentConfigurationNodes" select="$documentConfiguration/Configuration/RegistersAccumulation/RegisterAccumulation" />
        <xsl:with-param name="SecondConfigurationNodes" select="$documentSecondConfiguration/Configuration/RegistersAccumulation/RegisterAccumulation" />
        <xsl:with-param name="Type">RegisterAccumulation</xsl:with-param>
      </xsl:call-template>
		
    </root>

  </xsl:template>

</xsl:stylesheet>