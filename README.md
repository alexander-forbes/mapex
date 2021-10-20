# Mapex
A dynamic mapping engine to extract, transform and load data using YAML directives from various data sources.

- [Mapex](#mapex)
  - [What is a directive?](#what-is-a-directive)
  - [Where](#where)
    - [Syntax](#syntax)
      - [!from_subject](#from_subject)
        - [Syntax](#syntax-1)
      - [!from_subject_attachment](#from_subject_attachment)
        - [Syntax](#syntax-2)
      - [!file_system](#file_system)
        - [Syntax](#syntax-3)
  - [Extract](#extract)
    - [!html_email_body](#html_email_body)
      - [Syntax](#syntax-4)
    - [!excel](#excel)
      - [Syntax](#syntax-5)
    - [Options](#options)
    - [Data](#data)
    - [Aggregations](#aggregations)
      - [Using](#using)
      - [Where](#where-1)
      - [Group-By](#group-by)
      - [Include](#include)
    - [Attributes](#attributes)
  - [Filter](#filter)
    - [!default_filter](#default_filter)
      - [Syntax](#syntax-6)
    - [Operators](#operators)
  - [Transform](#transform)
    - [!default_transformer](#default_transformer)
      - [Syntax](#syntax-7)
      - [Create](#create)
        - [Syntax](#syntax-8)
      - [Append](#append)
        - [Syntax](#syntax-9)
      - [Yank](#yank)
        - [Syntax](#syntax-10)
      - [Trim](#trim)
        - [Syntax](#syntax-11)
      - [Translate](#translate)
        - [Syntax](#syntax-12)
      - [Convert](#convert)
        - [Syntax](#syntax-13)
      - [Resolve](#resolve)
  - [Map](#map)
    - [Syntax](#syntax-14)
  - [Intercept](#intercept)
  - [Writing directives](#writing-directives)

## What is a directive?
A directive is a YAML structured document. It is essentially, a recipe for processing an item. By "item" we mean an email, a file, an attachment, a downloaded document from a web service and so on...

A directive can be seen as a pipeline, with pluggable components, that starts with a where section, followed by an extract section and then followed by a transform section and finally, a map section. A directive may, optionally, have a filter section and an intercept section.

Essentially, every directive must have, at least, the following tags:

```
---
where:
extract:
map:
```

## Where
Mandatory. The where section of the directive will determine whether this directive can be used to process a particular item. It is the matching criteria that selects a directive. In other words, if an email comes from a specific sender or has a specific subject or attachment we need to use this directive to process the item. 

The where section of a directive determines whether this directive is able to process a particular item. Basically, it is the matching criteria that selects a directive based on the source of the item. 

The following where clauses are available, viz.:
- !from_subject_attachment
- !from_subject
- !file_system

### Syntax
Below is the the general syntax of the where block:

```
where:
```

#### !from_subject
This where block is used to match an email message based on the sender's email address and the subject of the email message.

##### Syntax
```
where:
    !from_subject
    from: {regular-expression}
    subject: {regular-expression}
```

|Tag|Description|Examples|
|---|---|---|
|`from`|A regular expression that matches the sender's email address|`^joe@test.com$`|
|`subject`|A regular expression that matches the subject of an email|`^ Daily Valuations$`|

#### !from_subject_attachment
This where block is used to match an email message based on the sender's email address, the subject of the email and the name of an attachment.

##### Syntax
```
where:
    !from_subject_attachment
    from: {regular-expression}
    subject: {regular-expression}
    attachment: {regular-expression}
```

|Tag|Description|Examples|
|---|---|---|
|`from`|A regular expression that matches the sender's email address|`^joe@test.com$`|
|`subject`|A regular expression that matches the subject of an email|`^Daily Valuations$`|
|`attachment`|A regular expression that matches the name of an attachment in the email message|`^Data \(FS[\d]+U\)_[\d]+-[\d]+-[\d]+.xlsx$`|

#### !file_system
This where block is used to match a file from the file system based on the path, filename or extension.

##### Syntax
```
where:
    !file_system
    filename: {regular-expression}
    path: {regular-expression}
    extension: {regular-expression}
```

|Tag|Description|Examples|
|---|---|---|
|`filename`|A regular expression that matches the name of the file to process.|`^AA 2800 Jun 2018.xlsx$`|
|`path`|Optional. A regular expression that matches the path of where the file resides.|`^c:\\temp\\$`|
|`extension`|Optional. A regular expression that matches the extension of the file to process.|`^.xls[x]*$`|

## Extract
Mandatory. The extract section of the directive provides the how-to for extracting information from the item. In other words, it is where we specify how-to we want the data to the grouped or summed and which data to select from the item. 

A number of extractors exist for extracting data from items, viz.:
- !html_email_body, and
- !excel.

### !html_email_body
To extract data from a HTML email body, the following block of code can be used:

#### Syntax
```
extract:
    !html_email_body
    using: {regular-expression}
        options:
            preserve_newlines: {true|false}
```
|Tag|Description|Examples|
|---|---|---|
|`using`|Must specify a [regular expression](http://regexhero.net/reference/), with capture groups, to extract the relevant parts of the email body after all HTML tags have been stripped from the input.|`(?<Date>[\d]+/[\d]+/[\d]+)`|
|`preserve_newlines`|When `preserve_newlines` is set to true, `\r\n` characters will *not* be stripped from the email body. The regular expression must then be written in such a manner that it will deal with whitespace.|`true` or `false`|

### !excel
To extract data from a comma-separated value file, the following block of code can be used. In most cases, auto-mapping can be used to import the data, from CSV, using the headers as property names.

#### Syntax
```
extract:
    !excel
    options:
        format: {excel|csv}
        automap: {true|false}
        delimiter: {delimiter}
        rowheader: {true|false}
        skip-last-row: {true|false}
    data:
        aggregations: {aggregations}
        attributes: {attributes}
```
There are two main tags that can be specified under the `extract` tag, viz. `options` and `data`.

### Options
This tag configures the Excel extractor by specifying the type of file that is being extracted, viz. Excel or CSV. In the case of a CSV file, it provides additional information such as the delimiter.

|Tag|Description|Examples|
|---|---|---|
|`format`|Specifies the type of file that is being extracted.|`excel` or `csv`|
|`automap`|Use the column headers as property names, after all spaces have been removed.|`true` or `false`|
|`delimiter`|Specifies the delimiter, in double-quotes, that is used to delimit the fields in the source file|A tab should be specified as `"\t"` and comma as `","`|
|`rowheader`|Indicates that the file has a header row which should be skipped during the data import.|`true` or `false`|
|`skip-last-row`|Indicates whether the last row should be removed from the table before processing. Only applies to CSV.|`true` or `false`|

### Data
Identifies the data that needs to be extracted from the file as well as any aggregations to perform on the data during the extraction process.

|Tag|Description|Examples|
|---|---|---|
|`aggregations`|Aggregations to perform on the data such as sum, min or max. See Aggregations table.||
|`attributes`|Attributes to select from the source file. See Attributes table.||

### Aggregations
```
aggregations:
    - function: {sum|min|max|nop}
      sheet: {string}
      rows: {rows}
      column: {column}
      type: {type}
      using: {using}
      include: {include}
      as: {alias}
```
|Tag|Description|Examples|
|---|---|---|
|`function`|Aggregation function to apply on the specified rows.|`sum`, `min`, `max` or `nop`|
|`sheet`|Name of the sheet from which to extract data. This tag is only required if there are multiple sheets in the file.|`Sheet1`|
|`rows`|Specifies the rows from which to extract data. A question mark can be used when there are an unknown number of rows.|`2..3`, `1..?` or `5..?`|
|`column`|The column on which to perform the aggregation.|`L`|
|`type`|Type of the column on which the aggregation is being performed.|`System.Decimal, mscorlib`|
|`using`|See Using table.||
|`include`|See Include table.||
|`as`|The name of the aggregation result property.|`Total`|

#### Using
Use the `using` clause to specify the data on which to perform the aggregation function.
```
using:
    where: {where}
    group-by: {group-by}
```
|Tag|Description|Examples|
|---|---|---|
|`where`|See Where table.||
|`group-by`|See the GroupBy table.||

#### Where
Use the `where` clause to specify which data to include when grouping data.
```
where:
    column: {column-1}
    - is: {value-1}
    - is: {value-2}
```

|Tag|Description|Examples|
|---|---|---|
|`column`|Name of the column to evaluate|`L`|
|`is`|Values to compare a cell against.|`PARCAS`|

#### Group-By
Use the `group-by` clause to group data based on the values of the specified columns. 
```
group-by:
    - column: {column-1}
      as: {alias-1}
    - column: {column-2}
      as: {alias-2}
```

|Tag|Description|Examples|
|---|---|---|
|`column`|Name of the column to use when grouping data. The order in the directive determines the grouping order.|`L`|
|`as`|Property name to use for the key of the grouping.|`PortfolioID`|

#### Include
Use the `include` clause to include additional columns into the group.
```
include:
    - column: {column-1}
      as: {alias-1}
    - column: {column-2}
      as: {alias-2}
```

|Tag|Description|Examples|
|---|---|---|
|`column`|Name of the column to include into the grouping.|`L`|
|`as`|Property name to use for the included column.|`PortfolioID`|

### Attributes
Attributes are used to get the values from specific cells in an Excel file.
```
attributes:
    - sheet: {sheet}
      cell: {cell}
      where: {where}
      as: {alias}
```

|Tag|Description|Examples|
|---|---|---|
|`sheet`|Name of the sheet from which to extract cell value.|`Transactions`|
|`cell`|The cell from which to extract the value.|`C2`|
|`where`|See the Where table.||
|`as`|Property name to use for the extracted value.|'PortfolioCode'|

## Filter
Optional. A set of criteria that specifies which data to include from the extracted data.

A filter is a set of criteria that specifies which data to include from the extracted data. 

The following filters are available, viz.:
- !default_filter

### !default_filter

#### Syntax
Below is the the general syntax of the filter block and its available tags:

```
filter:
    !default_filter
    where: {criteria}
```

|Tag|Description|Examples|
|---|---|---|
|`where`|Criteria to use when deciding which data to include. Mandatory. |`Item["TypeCode"].ToString() == "HO" OR Item["TypeCode"].ToString() == "HC"` |
|||`Item["DocAmount"].ToString() != "" AND Item["DocumentDate"].ToString() != "" AND NOT Item["Narrative"].ToString().StartsWith("MFS")`|

To select a property use the format `Item[{property-name}]`.

### Operators

|Operator|Description|
|---|---|
|`==`|equals|
|`!=`|not equal
|`>`|greater than|
|`>=`|greater than or equal|
|`<`|less than|
|`<=`|less than or equal|
|`&&`|and|
|`and`|and|
|`||`|or|
|`or`|or|

## Transform
Optional. A series of transformations to apply to the extracted data in order to shape it into the final form before mapping it to the destination data structure.

A property transformer may used to transform a property value from one value to another. For example, by appending a value to a property, by taking away something from the property value or to replace it with something else.

The default transformer (`!default_transformer`) has a number of built-in property transformers, viz.:
- Append
- Yank
- Trim
- Translate
- Convert
- Resolve
- Alias
- Create

### !default_transformer

#### Syntax
Below is the the general syntax of the transform block and its available tags:

```
transform:
    !default_transformer
    properties:
        - property: {property}
          create: {create}
          yank: {regular-expression}
          trim: {regular-expression}
          translate: {translate}
          convert: {convert}
          append: {append}
          resolve: {resolve}
          as: {alias}
```

|Tag|Description|Examples|
|---|---|---|
|`property`|Name of the property whose value must be transformed. Mandatory. |`PortfolioCode`|
|`as`|Name of the property after the transformation has been applied.|`FundId`|

#### Create

The create transformer is used to create properties with a specific value.

##### Syntax
```
transform:
    !default_transformer
    properties:
        - property: {property}
          create:
              value: {value}
              overwrite: {true|false}
```

|Tag|Description|Examples|
|---|---|---|
|`value`|Value to set on the created property.|Use `"[null]"` for null, or any other value.|
|`overwrite`|Overwrite the property with the given value, if it already exists on the object.|`{true|false}`|

#### Append

The appender transformer is used to either append a value to the front of a property or to the back of a property.

##### Syntax
```
transform:
    !default_transformer
    properties:
        - property: {property}
          append:
              value: {value}
              postfix: {true|false}
          as: {alias}
```

|Tag|Description|Examples|
|---|---|---|
|`value`|Value to append to the specified property.|`my-`|
|`postfix`|Specifies whether the value must be appended to the front or the back of the property value.|`{true|false}`|

#### Yank

The yanker is used to extract a part of a property value, using regular expressions, and replace the original value with the yanked value. 

##### Syntax
```
transform:
    !default_transformer
    properties:
        - property: {property}
          yank: {regular-expression}
          as: {alias}
```

|Tag|Description|Examples|
|---|---|---|
|`yank`|Regular expression with a capture group that matches the property name to extract from the property value.|`(?<property>[\d]+)`|

#### Trim

The trimmer is used to remove a part of a string.

##### Syntax
```
transform:
    !default_transformer
    properties:
        - property: {property}
           trim: {regular-expression}
           as: {alias}
```

|Tag|Description|Examples|
|---|---|---|
|`trim`|Regular expression that matches the part of the string to remove.|`.+[ to ]`|

#### Translate

The translator is used to translate a property value from one value to another.

##### Syntax
```
transform:
    !default_transformer
    properties:
        - property: {property}
          translate:
              - when: {value-1|[empty]}
                then: {alternative-value-1}
              - when: {value-2|[empty]}
                then: {alternative-value-2}
          as: {alias}
```

|Tag|Description|Examples|
|---|---|---|
|`when`|Value to match. Use `[empty]` to match `null` or empty values |`10` or `[empty]`|
|`then`|Value to replace the matched value with|`Contribution`|

#### Convert

The converter is used to convert a property value from one type to another.

##### Syntax
```
transform:
    !default_transformer
    properties:
        - property: {property}
          convert:
              to: {type}
              format: {format}
          as: {alias}
```

|Tag|Description|Examples|
|---|---|---|
|`type`|Type to convert the value to|`System.Decimal, mscorlib` or ``System.Nullable`1[System.Decimal], mscorlib``|
|`format`|Specifies the format of the source date. Mandatory for dates.|`dd/MM/yyyy`|

#### Resolve

The resolver is used to resolve a property value to another value. 

## Map
Mandatory. This is the final step in processing an item where it is mapped to a data structure that can be stored in the Document Exchange database. 

This is the final step in processing an item where it is mapped to a data structure that can be stored in a database. 

The following mappers are available, viz.:
- !default_mapper

### Syntax

Below is the the general syntax of the map block and its available tags:

```
map:
    !default_mapper
    as: {data-structure}
```

|Tag|Description|Examples|
|---|---|---|
|`as`|Canonical type to the map the extracted object to. Mandatory. |`DataModels.Transaction, DataModels`|

## Intercept
Optional. The processing pipeline can be intercepted at various points to perform additional processing before passing the data to the next stage in the pipeline. For example, resolving identifiers using a line-of-business system, or performing corrections on records and so on...

## Writing directives
There are a number of things to remember when writing directives:

Indentation matters (a lot!)
Do not use tabs, always use spaces
Keep indentation consistent

