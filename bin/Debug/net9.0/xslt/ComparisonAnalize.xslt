<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" indent="yes" />

  <!-- Унікальний ключ для створення копій колонок -->
  <xsl:param name="KeyUID" />

  <xsl:template name="Template_AddColumn">
    <xsl:param name="TableName" />
    <xsl:param name="FieldNameInTable" />
    <xsl:param name="DataTypeCreate" />

    <info>
      <xsl:text disable-output-escaping="yes"> -&gt; </xsl:text> Додати колонку <xsl:value-of select="$FieldNameInTable"/> в таблицю <xsl:value-of select="$TableName"/>
    </info>

    <sql>
      <xsl:text>ALTER TABLE </xsl:text>
      <xsl:value-of select="$TableName"/>
      <xsl:text> ADD COLUMN "</xsl:text>
      <xsl:value-of select="$FieldNameInTable"/>
      <xsl:text>" </xsl:text>
      <xsl:value-of select="$DataTypeCreate"/>
      <xsl:text>;</xsl:text>
    </sql>
    
  </xsl:template>
  
  <xsl:template name="Template_CopyColumn">
    <xsl:param name="TableName" />
    <xsl:param name="FieldNameInTable" />
    <xsl:param name="DataTypeCreate" />

    <info> <xsl:text disable-output-escaping="yes"> -&gt; </xsl:text> Перейменувати колонку <xsl:value-of select="$FieldNameInTable"/> в таблиці <xsl:value-of select="$TableName"/><xsl:text> на </xsl:text>
      <xsl:value-of select="$FieldNameInTable"/><xsl:text>_old_</xsl:text><xsl:value-of select="$KeyUID"/>
    </info>

    <sql>
      <xsl:text>ALTER TABLE </xsl:text>
      <xsl:value-of select="$TableName"/>
      <xsl:text> RENAME COLUMN "</xsl:text>
      <xsl:value-of select="$FieldNameInTable"/>
      <xsl:text>" TO "</xsl:text>
      <xsl:value-of select="$FieldNameInTable"/>
      <xsl:text>_old_</xsl:text>
      <xsl:value-of select="$KeyUID"/>
      <xsl:text>";</xsl:text>
    </sql>

    <xsl:call-template name="Template_AddColumn">
      <xsl:with-param name="TableName" select="$TableName" />
      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
      <xsl:with-param name="DataTypeCreate" select="Type/DataTypeCreate" />
    </xsl:call-template>

  </xsl:template>

  <xsl:template name="Template_DropColumn">
    <xsl:param name="TableName" />
    <xsl:param name="FieldNameInTable" />

    <info>
      <xsl:text disable-output-escaping="yes"> -&gt; </xsl:text> Видалити колонку <xsl:value-of select="$FieldNameInTable"/> в таблиці <xsl:value-of select="$TableName"/>
    </info>

    <sql>
      <xsl:text>ALTER TABLE </xsl:text>
      <xsl:value-of select="$TableName"/>
      <xsl:text> DROP COLUMN "</xsl:text>
      <xsl:value-of select="$FieldNameInTable"/>
      <xsl:text>";</xsl:text>
    </sql>
  </xsl:template>
  
  <xsl:template name="Template_DropOldColumn">
    <xsl:param name="TableName" />
    <xsl:param name="FieldNameInTable" />

    <info> <xsl:text disable-output-escaping="yes"> -&gt; </xsl:text> Видалити колонку <xsl:value-of select="$FieldNameInTable"/><xsl:text>_old_</xsl:text><xsl:value-of select="$KeyUID"/> в таблиці <xsl:value-of select="$TableName"/></info>

    <sql>
      <xsl:text>ALTER TABLE </xsl:text>
      <xsl:value-of select="$TableName"/>
      <xsl:text> DROP COLUMN "</xsl:text>
      <xsl:value-of select="$FieldNameInTable"/>
      <xsl:text>_old_</xsl:text>
      <xsl:value-of select="$KeyUID"/>
      <xsl:text>";</xsl:text>
    </sql>
  </xsl:template>

  <xsl:template name="Template_Control_Field">
    <xsl:param name="Control_Field" />
    <xsl:param name="Name" />
    <xsl:param name="TableName" />

    <xsl:for-each select="$Control_Field">

      <xsl:choose>
       <xsl:when test="IsExist = 'yes'">

        <xsl:choose>
          <xsl:when test="Type/Coincide = 'no'">

            <info>Таблиця <xsl:value-of select="$Name"/> (<xsl:value-of select="$TableName"/>), поле <xsl:value-of select="Name"/> (<xsl:value-of select="NameInTable"/><xsl:text>). </xsl:text>
              <xsl:text>Перетворити тип даних </xsl:text>(<xsl:value-of select="Type/DataType"/>, <xsl:value-of select="Type/UdtName"/>)<xsl:text disable-output-escaping="yes"> -&gt; </xsl:text> <xsl:value-of select="Type/ConfType"/></info>

            <xsl:choose>
              <xsl:when test="Type/DataType = 'text'">
                <xsl:choose>
                  <!-- Текст в масив -->
                  <xsl:when test="Type/ConfType = 'string[]'">
                    <info>Резструктуризація можлива: Текст в масив.</info>
                    <sql>BEGIN;</sql>

                    <xsl:call-template name="Template_CopyColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                      <xsl:with-param name="DataTypeCreate" select="Type/DataTypeCreate" />
                    </xsl:call-template>

                    <!--
                    UPDATE test
                        SET text_mas = 
                        (
                          SELECT array_agg(t.text) FROM test AS t 
                          WHERE t.uid = test.uid AND t.text != NULL
                        ) 
                    -->

                    <sql>
                      <xsl:value-of select="concat('UPDATE ', $TableName, ' SET ', NameInTable, ' = (')"/>
                      <xsl:value-of select="concat('SELECT array_agg(t.', NameInTable, '_old_', $KeyUID, ') FROM ', $TableName, ' AS t WHERE t.uid = ', $TableName, '.uid ')"/>
                      <xsl:value-of select="concat('AND t.', NameInTable, '_old_', $KeyUID, ' != NULL)')"/>
                    </sql>

                    <xsl:call-template name="Template_DropOldColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                    </xsl:call-template>

                    <sql>COMMIT;</sql>
                  </xsl:when>
                  <xsl:otherwise>

                    <info>Заміна колонки! Втрата даних!</info>
                    <sql>BEGIN;</sql>
                    <xsl:call-template name="Template_DropColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                    </xsl:call-template>
                    <xsl:call-template name="Template_AddColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                      <xsl:with-param name="DataTypeCreate" select="Type/DataTypeCreate" />
                    </xsl:call-template>
                    <sql>COMMIT;</sql>

                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>

              <xsl:when test="Type/DataType = 'integer' or 
                              Type/DataType = 'numeric'">
                <xsl:choose>
                  <!-- Число в масив -->
                  <xsl:when test="Type/ConfType = 'integer[]' or Type/ConfType = 'numeric[]'">
                    <info>Резструктуризація можлива: Число в масив.</info>
                    <sql>BEGIN;</sql>

                    <xsl:call-template name="Template_CopyColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                      <xsl:with-param name="DataTypeCreate" select="Type/DataTypeCreate" />
                    </xsl:call-template>

                    <!-- UPDATE public.test SET text_mas = (SELECT array_agg(test2.text) FROM public.test AS test2 WHERE test2.uid = test.uid) -->

                    <sql>
                      <xsl:text>UPDATE </xsl:text>
                      <xsl:value-of select="$TableName"/>
                      <xsl:text> SET </xsl:text>
                      <xsl:value-of select="NameInTable"/>
                      <xsl:value-of select="concat(' = (SELECT array_agg(', 't.', NameInTable, '_old_', $KeyUID, ') FROM ')"/>
                      <xsl:value-of select="$TableName"/>
                      <xsl:text> AS t WHERE t.uid = </xsl:text>
                      <xsl:value-of select="$TableName"/>
                      <xsl:text>.uid AND t.</xsl:text>
                      <xsl:value-of select="NameInTable"/>
                      <xsl:text>_old_</xsl:text>
                      <xsl:value-of select="$KeyUID"/>
                      <xsl:text> != NULL);</xsl:text>
                    </sql>

                    <xsl:call-template name="Template_DropOldColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                    </xsl:call-template>

                    <sql>COMMIT;</sql>
                  </xsl:when>
                  <!-- Ціле число в число з комою -->
                  <xsl:when test="Type/ConfType = 'numeric' and Type/DataType = 'integer'">
                    <info>Резструктуризація можлива: Ціле число в число з комою.</info>
                    <sql>BEGIN;</sql>

                    <xsl:call-template name="Template_CopyColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                      <xsl:with-param name="DataTypeCreate" select="Type/DataTypeCreate" />
                    </xsl:call-template>

                    <sql>
                      <xsl:text>UPDATE </xsl:text>
                      <xsl:value-of select="$TableName"/>
                      <xsl:text> SET </xsl:text>
                      <xsl:value-of select="NameInTable"/>
                      <xsl:text> = (SELECT t.</xsl:text>
                      <xsl:value-of select="NameInTable"/>
                      <xsl:text>_old_</xsl:text>
                      <xsl:value-of select="$KeyUID"/>
                      <xsl:text> FROM </xsl:text>
                      <xsl:value-of select="$TableName"/>
                      <xsl:text> AS t WHERE t.uid = </xsl:text>
                      <xsl:value-of select="$TableName"/>
                      <xsl:text>.uid);</xsl:text>
                    </sql>

                    <xsl:call-template name="Template_DropOldColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                    </xsl:call-template>

                    <sql>COMMIT;</sql>
                  </xsl:when>
                  <!-- Число з комою в ціле число -->
                  <xsl:when test="Type/ConfType = 'integer' and Type/DataType = 'numeric'">
                    <info>Резструктуризація можлива (Часткова втрата даних: значення після коми будуть втрачення): Число з комою в ціле число.</info>
                    <sql>BEGIN;</sql>

                    <xsl:call-template name="Template_CopyColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                      <xsl:with-param name="DataTypeCreate" select="Type/DataTypeCreate" />
                    </xsl:call-template>

                    <sql>
                      <xsl:text>UPDATE </xsl:text>
                      <xsl:value-of select="$TableName"/>
                      <xsl:text> SET </xsl:text>
                      <xsl:value-of select="NameInTable"/>
                      <xsl:text> = (SELECT trunc(t.</xsl:text>
                      <xsl:value-of select="NameInTable"/>
                      <xsl:text>_old_</xsl:text>
                      <xsl:value-of select="$KeyUID"/>
                      <xsl:text>) FROM </xsl:text>
                      <xsl:value-of select="$TableName"/>
                      <xsl:text> AS t WHERE t.uid = </xsl:text>
                      <xsl:value-of select="$TableName"/>
                      <xsl:text>.uid);</xsl:text>
                    </sql>

                    <xsl:call-template name="Template_DropOldColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                    </xsl:call-template>

                    <sql>COMMIT;</sql>
                  </xsl:when>
                  <xsl:otherwise>

                    <info>Заміна колонки! Втрата даних!</info>
                    <sql>BEGIN;</sql>
                    <xsl:call-template name="Template_DropColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                    </xsl:call-template>
                    <xsl:call-template name="Template_AddColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                      <xsl:with-param name="DataTypeCreate" select="Type/DataTypeCreate" />
                    </xsl:call-template>
                    <sql>COMMIT;</sql>

                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>

              <xsl:when test="(Type/DataType = 'ARRAY' and Type/UdtName = '_text') or
                              (Type/DataType = 'ARRAY' and Type/UdtName = '_int4') or 
                              (Type/DataType = 'ARRAY' and Type/UdtName = '_numeric') or
                              (Type/DataType = 'ARRAY' and Type/UdtName = '_uuid')">
                <xsl:choose>
                  <!-- Масив в текст -->
                  <xsl:when test="Type/ConfType = 'string'">
                    <info>Резструктуризація можлива: Масив в текст.</info>
                    <sql>BEGIN;</sql>

                    <xsl:call-template name="Template_CopyColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                      <xsl:with-param name="DataTypeCreate" select="Type/DataTypeCreate" />
                    </xsl:call-template>

                    <!-- UPDATE test SET text = (SELECT array_to_string(text_mas, ', ') FROM test AS t Where t.uid = test.uid); -->

                    <sql>
                      <xsl:text>UPDATE </xsl:text>
                      <xsl:value-of select="$TableName"/>
                      <xsl:text> SET </xsl:text>
                      <xsl:value-of select="NameInTable"/>
                      <xsl:text> = (SELECT array_to_string(t.</xsl:text>
                      <xsl:value-of select="NameInTable"/>
                      <xsl:text>_old_</xsl:text>
                      <xsl:value-of select="$KeyUID"/>
                      <xsl:text>, ', ') FROM </xsl:text>
                      <xsl:value-of select="$TableName"/>
                      <xsl:text> AS t WHERE t.uid = </xsl:text>
                      <xsl:value-of select="$TableName"/>
                      <xsl:text>.uid AND t.</xsl:text>
                      <xsl:value-of select="NameInTable"/>
                      <xsl:text>_old_</xsl:text>
                      <xsl:value-of select="$KeyUID"/>
                      <xsl:text> != NULL);</xsl:text>
                    </sql>

                    <xsl:call-template name="Template_DropOldColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                    </xsl:call-template>

                    <sql>COMMIT;</sql>
                  </xsl:when>
                  <xsl:otherwise>
                    
                    <info>Заміна колонки! Втрата даних!</info>
                    <sql>BEGIN;</sql>
                    <xsl:call-template name="Template_DropColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                    </xsl:call-template>
                    <xsl:call-template name="Template_AddColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                      <xsl:with-param name="DataTypeCreate" select="Type/DataTypeCreate" />
                    </xsl:call-template>
                    <sql>COMMIT;</sql>
                    
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              
              <xsl:when test="Type/DataType = 'boolean' or 
                              Type/DataType = 'date' or  
                              Type/DataType = 'time without time zone' or 
                              Type/DataType = 'timestamp without time zone' or 
                              Type/DataType = 'uuid' or
						                  (Type/DataType = 'USER-DEFINED' and Type/UdtName = 'uuidtext') or
						                  (Type/DataType = 'USER-DEFINED' and Type/UdtName = 'nametext') ">
                <xsl:choose>
                  <!-- Значення в Текст -->
                  <xsl:when test="Type/ConfType = 'string'">
                    <info>Резструктуризація можлива: Дані в текст.</info>
                    <sql>BEGIN;</sql>

                    <xsl:call-template name="Template_CopyColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                      <xsl:with-param name="DataTypeCreate" select="Type/DataTypeCreate" />
                    </xsl:call-template>

                    <sql>
                      <xsl:text>UPDATE </xsl:text>
                      <xsl:value-of select="$TableName"/>
                      <xsl:text> SET </xsl:text>
                      <xsl:value-of select="NameInTable"/>
                      <xsl:text> = (SELECT t.</xsl:text>
                      <xsl:value-of select="NameInTable"/>
                      <xsl:text>_old_</xsl:text>
                      <xsl:value-of select="$KeyUID"/>
                      <xsl:text> FROM </xsl:text>
                      <xsl:value-of select="$TableName"/>
                      <xsl:text> AS t WHERE t.uid = </xsl:text>
                      <xsl:value-of select="$TableName"/>
                      <xsl:text>.uid);</xsl:text>
                    </sql>

                    <xsl:call-template name="Template_DropOldColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                    </xsl:call-template>

                    <sql>COMMIT;</sql>
                  </xsl:when>
                  <!-- Дата -> Дата та час -->
                  <xsl:when test="Type/ConfType = 'datetime' and Type/DataType = 'date'">
                    <info>Резструктуризація можлива: Дата в Дата та час.</info>
                    <sql>BEGIN;</sql>

                    <xsl:call-template name="Template_CopyColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                      <xsl:with-param name="DataTypeCreate" select="Type/DataTypeCreate" />
                    </xsl:call-template>

                    <sql>
                      <xsl:text>UPDATE </xsl:text>
                      <xsl:value-of select="$TableName"/>
                      <xsl:text> SET </xsl:text>
                      <xsl:value-of select="NameInTable"/>
                      <xsl:text> = (SELECT t.</xsl:text>
                      <xsl:value-of select="NameInTable"/>
                      <xsl:text>_old_</xsl:text>
                      <xsl:value-of select="$KeyUID"/>
                      <xsl:text> FROM </xsl:text>
                      <xsl:value-of select="$TableName"/>
                      <xsl:text> AS t WHERE t.uid = </xsl:text>
                      <xsl:value-of select="$TableName"/>
                      <xsl:text>.uid);</xsl:text>
                    </sql>

                    <xsl:call-template name="Template_DropOldColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                    </xsl:call-template>

                    <sql>COMMIT;</sql>
                  </xsl:when>
                  <!-- Дата та час -> Дата -->
                  <xsl:when test="Type/ConfType = 'date' and Type/DataType = 'timestamp without time zone'">
                    <info>Резструктуризація можлива (часткова втрата даних): Дата та час в Дата.</info>
                    <sql>BEGIN;</sql>

                    <xsl:call-template name="Template_CopyColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                      <xsl:with-param name="DataTypeCreate" select="Type/DataTypeCreate" />
                    </xsl:call-template>

                    <sql>
                      <xsl:text>UPDATE </xsl:text>
                      <xsl:value-of select="$TableName"/>
                      <xsl:text> SET </xsl:text>
                      <xsl:value-of select="NameInTable"/>
                      <xsl:text> = (SELECT t.</xsl:text>
                      <xsl:value-of select="NameInTable"/>
                      <xsl:text>_old_</xsl:text>
                      <xsl:value-of select="$KeyUID"/>
                      <xsl:text> FROM </xsl:text>
                      <xsl:value-of select="$TableName"/>
                      <xsl:text> AS t WHERE t.uid = </xsl:text>
                      <xsl:value-of select="$TableName"/>
                      <xsl:text>.uid);</xsl:text>
                    </sql>

                    <xsl:call-template name="Template_DropOldColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                    </xsl:call-template>

                    <sql>COMMIT;</sql>
                  </xsl:when>
                  <!-- Дата та час -> Час -->
                  <xsl:when test="Type/ConfType = 'time' and Type/DataType = 'timestamp without time zone'">
                    <info>Резструктуризація можлива (часткова втрата даних): Дата та час в Час.</info>
                    <sql>BEGIN;</sql>

                    <xsl:call-template name="Template_CopyColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                      <xsl:with-param name="DataTypeCreate" select="Type/DataTypeCreate" />
                    </xsl:call-template>

                    <sql>
                      <xsl:text>UPDATE </xsl:text>
                      <xsl:value-of select="$TableName"/>
                      <xsl:text> SET </xsl:text>
                      <xsl:value-of select="NameInTable"/>
                      <xsl:text> = (SELECT t.</xsl:text>
                      <xsl:value-of select="NameInTable"/>
                      <xsl:text>_old_</xsl:text>
                      <xsl:value-of select="$KeyUID"/>
                      <xsl:text> FROM </xsl:text>
                      <xsl:value-of select="$TableName"/>
                      <xsl:text> AS t WHERE t.uid = </xsl:text>
                      <xsl:value-of select="$TableName"/>
                      <xsl:text>.uid);</xsl:text>
                    </sql>

                    <xsl:call-template name="Template_DropOldColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                    </xsl:call-template>

                    <sql>COMMIT;</sql>
                  </xsl:when>
                  <xsl:otherwise>

                    <info>Заміна колонки! Втрата даних!</info>
                    <sql>BEGIN;</sql>
                    <xsl:call-template name="Template_DropColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                    </xsl:call-template>
                    <xsl:call-template name="Template_AddColumn">
                      <xsl:with-param name="TableName" select="$TableName" />
                      <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                      <xsl:with-param name="DataTypeCreate" select="Type/DataTypeCreate" />
                    </xsl:call-template>
                    <sql>COMMIT;</sql>
                    
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
			  
			        <xsl:when test="Type/DataType = 'bytea'">				  
                <info>Заміна колонки! Втрата даних!</info>
                <sql>BEGIN;</sql>
                <xsl:call-template name="Template_DropColumn">
                    <xsl:with-param name="TableName" select="$TableName" />
                    <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                </xsl:call-template>
                <xsl:call-template name="Template_AddColumn">
                    <xsl:with-param name="TableName" select="$TableName" />
                    <xsl:with-param name="FieldNameInTable" select="NameInTable" />
                    <xsl:with-param name="DataTypeCreate" select="Type/DataTypeCreate" />
                </xsl:call-template>
                <sql>COMMIT;</sql>
              </xsl:when>
				
              <xsl:otherwise>
                <info>ПОМИЛКА! Не вдалось знайти спосіб реструктуризації для даного типу.</info>
              </xsl:otherwise>
            </xsl:choose>

          </xsl:when>
          <xsl:when test="Type/Coincide = 'clear'">
            
            <info>Таблиця <xsl:value-of select="$Name"/> (<xsl:value-of select="$TableName"/>), поле <xsl:value-of select="Name"/> (<xsl:value-of select="NameInTable"/><xsl:text>). </xsl:text></info>
            
            <info>Очищення колонки! Втрата даних!</info>
            <sql>BEGIN;</sql>
            <sql>
              <xsl:text>UPDATE </xsl:text>
              <xsl:value-of select="$TableName"/>
              <xsl:text> SET </xsl:text>
              <xsl:value-of select="NameInTable"/>
              <xsl:text> = NULL;</xsl:text>
            </sql>
            <sql>COMMIT;</sql>
            
          </xsl:when>
        </xsl:choose>

		<xsl:choose>
			<xsl:when test="Index = '1'">
				<xsl:if test="IndexExist != 'yes'">
				    <sql>
					    <xsl:value-of select="concat('CREATE INDEX IF NOT EXISTS ', $TableName, '_', NameInTable, '_idx ON ', $TableName, ' (', NameInTable, ');')"/>
				    </sql>
				</xsl:if>
			</xsl:when>
			<xsl:otherwise>
				<xsl:if test="IndexExist = 'yes'">
				    <sql>
					    <xsl:value-of select="concat('DROP INDEX IF EXISTS ', $TableName, '_', NameInTable, '_idx;')"/>
				    </sql>
				</xsl:if>
			</xsl:otherwise>
	   </xsl:choose>

       </xsl:when>
       <xsl:when test="IsExist = 'no'">

          <xsl:for-each select="FieldCreate">
            <info>Додати колонку <xsl:value-of select="Name"/> (<xsl:value-of select="NameInTable"/>, тип <xsl:value-of select="DataType"/>)  в таблицю <xsl:value-of select="$Name"/> (<xsl:value-of select="$TableName"/>)</info>
            <sql>
              <xsl:text>ALTER TABLE </xsl:text>
              <xsl:value-of select="$TableName"/>
              <xsl:text> ADD COLUMN "</xsl:text>
              <xsl:value-of select="NameInTable"/>
              <xsl:text>" </xsl:text>
              <xsl:value-of select="DataType"/>
              <xsl:if test="NotNull = '1'"> NOT NULL</xsl:if>
              <xsl:text>;</xsl:text>
            </sql>
            <xsl:if test="Index = '1'">
              <sql>
                <xsl:value-of select="concat('CREATE INDEX IF NOT EXISTS ', $TableName, '_', NameInTable, '_idx ON ', $TableName, ' (', NameInTable, ');')"/>
              </sql>
            </xsl:if>
          </xsl:for-each>

        </xsl:when>
       <xsl:when test="IsExist = 'delete'">

          <info>Видалити колонку <xsl:value-of select="Name"/> (<xsl:value-of select="NameInTable"/>)  в таблиці <xsl:value-of select="$Name"/> (<xsl:value-of select="$TableName"/>)</info>
          <sql>
            <xsl:text>ALTER TABLE </xsl:text>
            <xsl:value-of select="$TableName"/>
            <xsl:text> DROP COLUMN "</xsl:text>
            <xsl:value-of select="NameInTable"/>
            <xsl:text>" </xsl:text>
            <xsl:text>;</xsl:text>
          </sql>

        </xsl:when>
      </xsl:choose>

    </xsl:for-each>

  </xsl:template>

  <xsl:template match="/">

    <root>

      <xsl:for-each select="root/Control_Table">

        <xsl:variable name="DirectoryName" select="Name" />
        <xsl:variable name="TableName" select="Table" />
	      <xsl:variable name="TableType" select="Type" />
		  
        <xsl:choose>
          <xsl:when test="IsExist = 'yes'">

            <xsl:call-template name="Template_Control_Field">
              <xsl:with-param name="Control_Field" select="Control_Field" />
              <xsl:with-param name="Name" select="$DirectoryName" />
              <xsl:with-param name="TableName" select="$TableName" />
            </xsl:call-template>

          </xsl:when>
          <xsl:when test="IsExist = 'no'">

            <xsl:for-each select="TableCreate">
              <info>Створити таблицю <xsl:value-of select="$DirectoryName"/> (<xsl:value-of select="$TableName"/>)</info>
              <sql>
                <xsl:text>CREATE TABLE IF NOT EXISTS </xsl:text>
                <xsl:value-of select="$TableName"/>
                <xsl:text> (</xsl:text>
                <xsl:text>uid uuid NOT NULL, </xsl:text>
                <xsl:if test="$TableType = 'Directory'">
                  <xsl:text>deletion_label bool NOT NULL, </xsl:text>
                </xsl:if>
                <xsl:if test="$TableType = 'Document'">
                  <xsl:text>deletion_label bool NOT NULL, </xsl:text>
                  <xsl:text>spend bool NOT NULL, </xsl:text>
                  <xsl:text>spend_date timestamp without time zone NOT NULL, </xsl:text>
                </xsl:if>
                <xsl:if test="$TableType = 'RegisterInformation'">
                  <xsl:text>period timestamp without time zone NOT NULL, </xsl:text>
                  <xsl:text>owner uuid NOT NULL, </xsl:text>
                  <xsl:text>ownertype nametext NOT NULL, </xsl:text>
                </xsl:if>
                <xsl:if test="$TableType = 'RegisterAccumulation'">
                  <xsl:text>period timestamp without time zone NOT NULL, </xsl:text>
                  <xsl:text>income bool NOT NULL, </xsl:text>
                  <xsl:text>owner uuid NOT NULL, </xsl:text>
                  <xsl:text>ownertype nametext NOT NULL, </xsl:text>
                </xsl:if>
                <xsl:for-each select="FieldCreate">
                  <xsl:text> "</xsl:text>
                  <xsl:value-of select="NameInTable"/>
                  <xsl:text>" </xsl:text>
                  <xsl:value-of select="DataType"/>
                  <xsl:text>, </xsl:text>
                </xsl:for-each>
                <xsl:text>PRIMARY KEY(uid));</xsl:text>
              </sql>
              <xsl:for-each select="FieldCreate[Index = '1']">
                <sql>
                  <xsl:value-of select="concat('CREATE INDEX IF NOT EXISTS ', $TableName, '_', NameInTable, '_idx ON ', $TableName, ' (', NameInTable, ');')"/>
                </sql>
              </xsl:for-each>
              <xsl:if test="$TableType = 'Directory'">
                <sql>
                  <xsl:value-of select="concat('CREATE INDEX IF NOT EXISTS ', $TableName, '_deletion_label_idx ON ', $TableName, ' (deletion_label);')"/>
                </sql>
              </xsl:if>
              <xsl:if test="$TableType = 'Document'">
                <sql>
                  <xsl:value-of select="concat('CREATE INDEX IF NOT EXISTS ', $TableName, '_deletion_label_idx ON ', $TableName, ' (deletion_label);')"/>
                </sql>
                <sql>
                  <xsl:value-of select="concat('CREATE INDEX IF NOT EXISTS ', $TableName, '_spend_idx ON ', $TableName, ' (spend);')"/>
                </sql>
              </xsl:if>
              <xsl:if test="$TableType = 'RegisterInformation'">
                <sql>
                    <xsl:value-of select="concat('CREATE INDEX IF NOT EXISTS ', $TableName, '_period_idx ON ', $TableName, ' (period);')"/>
                </sql>
                <sql>
                  <xsl:value-of select="concat('CREATE INDEX IF NOT EXISTS ', $TableName, '_owner_idx ON ', $TableName, ' (owner);')"/>
                </sql>
                <sql>
                  <xsl:value-of select="concat('CREATE INDEX IF NOT EXISTS ', $TableName, '_ownertype_idx ON ', $TableName, ' (ownertype);')"/>
                </sql>
              </xsl:if>
              <xsl:if test="$TableType = 'RegisterAccumulation'">
                <sql>
                    <xsl:value-of select="concat('CREATE INDEX IF NOT EXISTS ', $TableName, '_period_idx ON ', $TableName, ' (period);')"/>
                </sql>
                <sql>
                  <xsl:value-of select="concat('CREATE INDEX IF NOT EXISTS ', $TableName, '_income_idx ON ', $TableName, ' (income);')"/>
                </sql>
                <sql>
                  <xsl:value-of select="concat('CREATE INDEX IF NOT EXISTS ', $TableName, '_owner_idx ON ', $TableName, ' (owner);')"/>
                </sql>
                <sql>
                  <xsl:value-of select="concat('CREATE INDEX IF NOT EXISTS ', $TableName, '_ownertype_idx ON ', $TableName, ' (ownertype);')"/>
                </sql>
              </xsl:if>
            </xsl:for-each>

          </xsl:when>
          <xsl:when test="IsExist = 'delete'">

            <info>Видалити таблицю <xsl:value-of select="$DirectoryName"/> (<xsl:value-of select="$TableName"/>)</info>
            <sql>
              <xsl:text>DROP TABLE </xsl:text>
              <xsl:value-of select="$TableName"/>
              <xsl:text>;</xsl:text>
            </sql>
            
          </xsl:when>
        </xsl:choose>

        <xsl:for-each select="Control_TabularParts">
          
          <xsl:variable name="TablePartName" select="Name" />
          <xsl:variable name="TabularParts_TableName" select="Table" />
          <xsl:variable name="IsCreateOwnerField" select="IsCreateOwner" />
          
          <xsl:choose>
            <xsl:when test="IsExist = 'yes'">

              <xsl:call-template name="Template_Control_Field">
                <xsl:with-param name="Control_Field" select="Control_Field" />
                <xsl:with-param name="Name" select="$TablePartName" />
                <xsl:with-param name="TableName" select="$TabularParts_TableName" />
              </xsl:call-template>

            </xsl:when>
            <xsl:when test="IsExist = 'no'">

              <xsl:for-each select="TableCreate">
                <info>Створити таблицю <xsl:value-of select="$TablePartName"/> (<xsl:value-of select="$TabularParts_TableName"/>)</info>
                <sql>
                  <xsl:text>CREATE TABLE IF NOT EXISTS </xsl:text>
                  <xsl:value-of select="$TabularParts_TableName"/>
                  <xsl:text> (</xsl:text>
                  <xsl:text>uid uuid NOT NULL, </xsl:text>
                  <xsl:if test="$IsCreateOwnerField = 'yes'">
                    <xsl:text>owner uuid NOT NULL, </xsl:text>
                  </xsl:if>
                  <xsl:for-each select="FieldCreate">
                    <xsl:text>"</xsl:text>
                    <xsl:value-of select="NameInTable"/>
                    <xsl:text>" </xsl:text>
                    <xsl:value-of select="DataType"/>
                    <xsl:text>, </xsl:text>
                  </xsl:for-each>
                  <xsl:text>PRIMARY KEY(uid));</xsl:text>
                </sql>
                <xsl:for-each select="FieldCreate[Index = '1']">
                  <sql>
                    <xsl:value-of select="concat('CREATE INDEX IF NOT EXISTS ', $TabularParts_TableName, '_', NameInTable, '_idx ON ', $TabularParts_TableName, ' (', NameInTable, ');')"/>
                  </sql>
                </xsl:for-each>
                <xsl:if test="$IsCreateOwnerField = 'yes'">
                  <sql>
					          <xsl:value-of select="concat('CREATE INDEX IF NOT EXISTS ', $TabularParts_TableName, '_owner_idx ON ', $TabularParts_TableName, ' (owner);')"/>
                  </sql>
                </xsl:if>
              </xsl:for-each>

            </xsl:when>
            <xsl:when test="IsExist = 'delete'">

              <info>Видалити таблицю <xsl:value-of select="$DirectoryName"/> (<xsl:value-of select="$TableName"/>)</info>
              <sql>
                <xsl:text>DROP TABLE </xsl:text>
                <xsl:value-of select="$TabularParts_TableName"/>
                <xsl:text>;</xsl:text>
              </sql>

            </xsl:when>
            
          </xsl:choose>

        </xsl:for-each>

      </xsl:for-each>

    </root>

  </xsl:template>

</xsl:stylesheet>