grammar ST;

startRule: stProgram;

stProgram: outerOperations* program outerOperations*;
program: PROGRAM programName? (operations SEMICOLON?)* END_PROGRAM;
programName: ID;

namespace: NAMESPACE namespaceName (namespaceOperations SEMICOLON?)* END_NAMESPACE;
namespaceName: ID;
namespaceOperations: operations | outerOperations;

operations: variables
          | variableAssign
          | bracketedAssignOperation
          | assignOperation          
          | ifExpression
          | caseExpression
          | forLoop
          | whileLoop
          | repeatLoop
          | EXIT
          | RETURN
          | structAssign
          | functionCall
          | method
          | classPropertyAssign
          | classMethodCall
          | classObjectMethodCall
          | parentClassMethodCall
          | callInheritedClass
          | namespaceAccess
          ;

outerOperations: classDeclaration
               | interfaceDeclaration
               | types
               | functionBlock
               | namespace
               | function
               ;

// VARIABLES
variables: variableKeywords qualifiers? variableDeclaration* END_VAR SEMICOLON?;
variableDeclaration: (variableName COMMA?)* COLON variableType lengthDeclaration* assignment? SEMICOLON?;
variableAssign: value* ASSIGN_OPERATOR assignValue SEMICOLON?;
assignment: ASSIGN_OPERATOR assignValue;
variableType: dataType
            | customDataType
            | reference
            ;

variableName: ID (AT ADDRESS)?
            | AT ADDRESS
            ;

// TYPES
types: TYPE typeDeclaration* END_TYPE;
typeDeclaration: ID (COLON (customDataType | dataType | reference))? (ASSIGN_OPERATOR  assignValue)? SEMICOLON?;

customDataType: enumeratedDataType
              | subrangeDataType
              | arrayDataType
              | structuredDataType
              ;

enumeratedDataType: dataType? BRACKET_OPEN (ID (ASSIGN_OPERATOR assignValue)? COMMA?)+ BRACKET_CLOSE;
subrangeDataType: dataType? BRACKET_OPEN NUMBER_VALUE DOT DOT NUMBER_VALUE BRACKET_CLOSE;
arrayDataType: ARRAY SQUARE_BRACKET_OPEN (arrayValues COMMA?)+ SQUARE_BRACKET_CLOSE OF (dataType | typeDeclaration);
structuredDataType: STRUCT OVERLAP? variableDeclaration*  END_STRUCT;
structAssign: BRACKET_OPEN (variableAssign COMMA?)* BRACKET_CLOSE;
namespaceAccess: ID lengthDeclaration* ((HASH | DOT) ID lengthDeclaration*)+;
partialBitAccess: DOT PARTIALBITACCESS;
registryAccess: NUMBER_VALUE HASH ID;

fragment PARTIALBITACCESS:
    '%'? [a-zA-Z0-9*]* ('.' [0-9]+)*;

expression: ID arithmeticOperator ID SEMICOLON?;

ifExpression: ifStatement elsifStatement* elseStatement? END_IF SEMICOLON?;
ifStatement: IF normalBracketedOperation+ THEN (operations SEMICOLON?)*;
elsifStatement: ELSIF normalBracketedOperation+ THEN (operations SEMICOLON?)*;
elseStatement: ELSE (operations SEMICOLON?)*;
normalBracketedOperation: bracketedOperation | operation;

caseExpression: CASE value OF caseOption* (ELSE (operations SEMICOLON?)*)? END_CASE;
caseOption: value (',' value)* COLON (operations SEMICOLON?)*;   

forLoop: FOR variableAssign TO numberValue (BY NUMBER_VALUE)? DO? (operations SEMICOLON?)* END_FOR SEMICOLON?;
whileLoop: WHILE normalBracketedOperation* DO (operations SEMICOLON?)* END_WHILE SEMICOLON?;
repeatLoop: REPEAT (operations SEMICOLON?)* UNTIL normalBracketedOperation* END_REPEAT SEMICOLON?;

reference: REF_TO (ID | dataType) SEMICOLON?;
referenceAssignment: REF BRACKET_OPEN assignValue BRACKET_CLOSE SEMICOLON?;
referenceAccess: ID lengthDeclaration* CARET ((HASH | DOT) ID lengthDeclaration*)+;

function: FUNCTION INTERNAL? ID (SEMICOLON? COLON (dataType | ID))? SEMICOLON? (operations SEMICOLON?)* END_FUNCTION;
functionBlock: FUNCTION_BLOCK ID inheritance? (SEMICOLON? COLON (dataType | ID))? SEMICOLON? (operations SEMICOLON?)* END_FUNCTION_BLOCK;
functionCall: ID BRACKET_OPEN (functionValue COMMA?)* BRACKET_CLOSE SEMICOLON?;
anonymousFunction: ID ANONYMOUS_FN_OPERATOR (bracketedValue | value);
functionValue: variableAssign
             | assignValue
             ;

classDeclaration: CLASS className inheritance? interfaceImplementation? (operations SEMICOLON?)* END_CLASS;
className: ID;
method: METHOD qualifiers* ID (SEMICOLON? COLON (dataType | ID))? SEMICOLON? (operations SEMICOLON?)* END_METHOD;
classPropertyAccess: THIS DOT ID;
classPropertyAssign: classPropertyAccess ASSIGN_OPERATOR assignValue;
classMethodCall: classPropertyAccess BRACKET_OPEN (functionValue COMMA?)* BRACKET_CLOSE SEMICOLON?;
parentClassMethodCall: SUPER DOT ID BRACKET_OPEN (functionValue COMMA?)* BRACKET_CLOSE SEMICOLON?;
classObjectMethodCall: ID (DOT ID)* BRACKET_OPEN (functionValue COMMA?)* BRACKET_CLOSE SEMICOLON?;

inheritance: EXTENDS className;
callInheritedClass: SUPER BRACKET_OPEN BRACKET_CLOSE SEMICOLON;

interfaceDeclaration: INTERFACE interfaceName interfaceMethodDeclaration* END_INTERFACE SEMICOLON?;
interfaceMethodDeclaration: METHOD interfaceMethodName (COLON (dataType | ID))? END_METHOD SEMICOLON?;
interfaceName: ID;
interfaceMethodName: ID;
interfaceImplementation: IMPLEMENTS ID (COMMA ID)*;
                            
dataType: SINT
        | INT
        | DINT
        | LINT
        | USINT
        | UINT
        | LDINT
        | ULINT
        | REAL
        | LREAL
        | TIME
        | DATE
        | TIME_OF_DAY
        | DATE_AND_TIME
        | STRING
        | BOOL (R_EDGE | F_EDGE)?
        | BYTE
        | WORD
        | DWORD
        | LWORD
        | UDINT
        | LTIME
        | LDATE
        | WSTRING
        | CHAR
        | WCHAR
        | ID
        ;

genericDataType: ANY
               | ANY_DERIVED
               | ANY_ELEMENTARY
               | ANY_MAGNITUDE
               | ANY_NUM
               | ANY_REAL
               | ANY_INT
               | ANY_UNSIGNED
               | ANY_SIGNED
               | ANY_DURATION
               | ANY_BIT
               | ANY_CHARS
               | ANY_STRING
               | ANY_CHAR
               | ANY_DATE
               ;
                 
specialCharacters: COLON
                 | SEMICOLON
                 | DOT
                 | COMMA
                 ;

assignValue: bracketedAssignOperation
           | assignOperation
           | bracketedValue
           | value
           | variableChoice
           | structAssign
           | referenceAssignment
	       | classPropertyAccess
           ;

bracketedOperation: BRACKET_OPEN operation BRACKET_CLOSE;
operation: bracketedArithmeticOperation
         | arithmeticOperation
         | bracketedNegatedOperation
         | negatedOperation
         | bracketedLogicalRelationalOperation
         | logicalRelationalOperation
         | trueOrFalseOperation
         ;

bracketedAssignOperation: BRACKET_OPEN assignOperation BRACKET_CLOSE;
assignOperation: bracketedArithmeticOperation
               | arithmeticOperation
               | bracketedNegatedOperation
               | negatedOperation
               | bracketedLogicalRelationalOperation
               | logicalRelationalOperation
               ;


arithmeticOperation: value (arithmeticOperator arithmeticOperationValue)+ SEMICOLON?;
bracketedArithmeticOperation: BRACKET_OPEN value (arithmeticOperator arithmeticOperationValue)+ BRACKET_CLOSE SEMICOLON?;
arithmeticOperationValue: ID | value | bracketedArithmeticOperation;

logicalRelationalOperation: logicalRelationalValue logicalOperation+ SEMICOLON?;
bracketedLogicalRelationalOperation: BRACKET_OPEN logicalRelationalValue logicalOperation* BRACKET_CLOSE SEMICOLON? logicalOperationOutside*;
logicalOperation: logicalRelationalOperator logicalRelationalValue;
logicalOperationOutside: logicalOperation;
logicalRelationalOperator: logicalOperator | relationalOperator;
logicalRelationalValue: arithmeticOperation | value | bracketedLogicalRelationalOperation;

bracketedNegatedOperation: NEGATION_OPERATOR BRACKET_OPEN (logicalRelationalOperation | bracketedNegatedOperation | trueOrFalseOperation) BRACKET_CLOSE SEMICOLON?;
negatedOperation: NEGATION_OPERATOR BRACKET_OPEN? (logicalRelationalOperation | trueOrFalseOperation) BRACKET_CLOSE? SEMICOLON?;
trueOrFalseOperation: ID | TRUE | FALSE;

bracketedValue: BRACKET_OPEN value BRACKET_CLOSE;
value: TRUE
     | FALSE
     | STRING_VALUE
     | NUMBER_VALUE
     | THIS
     | idWithLength
     | ID partialBitAccess?
     | ADDRESS   
     | functionCall
     | classMethodCall
     | parentClassMethodCall
     | anonymousFunction
     | referenceValue
     | valueWithQuantity     
     | arrayValue
     | classObjectMethodCall
     | namespaceAccess
     | referenceAccess
     | registryAccess
     | trueOrFalseOperation
     ;

arrayValues: NUMBER_VALUE DOT DOT NUMBER_VALUE
           | ID
           | MULTIPLY_OPERATOR
           ;

idWithLength: ID lengthDeclaration+;
referenceValue: ID CARET lengthDeclaration*;
valueWithQuantity: (NUMBER_VALUE | ID) (BRACKET_OPEN (COMMA? (NUMBER_VALUE | ID | operations))+ BRACKET_CLOSE)?;
variableChoice: ID (logicalOperator ID)*;
arrayValue: SQUARE_BRACKET_OPEN (arrayValueValue COMMA?)+ SQUARE_BRACKET_CLOSE;
arrayValueValue: NUMBER_VALUE | valueWithQuantity;

numberValue: NUMBER_VALUE
           | ID
           | functionCall
           | classMethodCall
           | parentClassMethodCall
           ;

arithmeticOperator: ADD_OPERATOR
                  | SUBTRACT_OPERATOR
                  | MULTIPLY_OPERATOR
                  | DIVISION_OPERATOR
                  | MODULO_OPERATOR
                  | EXPONENT_OPERATOR
                  ;

logicalOperator: AND_OPERATOR
               | OR_OPERATOR
               | XOR_OPERATOR
               | NEGATION_OPERATOR
               ;

relationalOperator: EQUAL_OPERATOR
                  | LESS_THAN_OPERATOR
                  | LESS_THAN_EQUAL_OPERATOR
                  | GREATER_THAN_OPERATOR
                  | GREATER_THAN_EQUAL_OPERATOR
                  | NOT_EQUAL_OPERATOR
                  ;

lengthDeclaration: SQUARE_BRACKET_OPEN (COMMA? lengthDeclarationValue)+ SQUARE_BRACKET_CLOSE;
lengthDeclarationValue: numberValue | ID;

variableKeywords: VAR
                | VAR_INPUT
                | VAR_OUTPUT
                | VAR_IN_OUT
                | VAR_EXTERNAL
                | VAR_GLOBAL
                | VAR_ACCESS
                | VAR_TEMP
                | VAR_CONFIG
                ;

qualifiers: RETAIN
          | NON_RETAIN
          | PROTECTED
          | PUBLIC
          | PRIVATE
          | INTERNAL
          | CONSTANT
          | OVERRIDE
          | IMPLEMENTS
          ;


// KEYWORDS
PROGRAM:                            'PROGRAM';
END_PROGRAM:                        'END_PROGRAM';
VAR:                                'VAR';
VAR_INPUT:                          'VAR_INPUT';
VAR_OUTPUT:                         'VAR_OUTPUT';
VAR_IN_OUT:                         'VAR_IN_OUT';
VAR_EXTERNAL:                       'VAR_EXTERNAL';
VAR_GLOBAL:                         'VAR_GLOBAL';
VAR_ACCESS:                         'VAR_ACCESS';
VAR_TEMP:                           'VAR_TEMP';
VAR_CONFIG:                         'VAR_CONFIG';
END_VAR:                            'END_VAR';
RETAIN:                             'RETAIN';
NON_RETAIN:                         'NON_RETAIN';
PROTECTED:                          'PROTECTED';
PUBLIC:                             'PUBLIC';
PRIVATE:                            'PRIVATE';
INTERNAL:                           'INTERNAL';
CONSTANT:                           'CONSTANT';
IF:                                 'IF';
ELSIF:                              'ELSIF';
THEN:                               'THEN';
ELSE:                               'ELSE';
END_IF:                             'END_IF';
CASE:                               'CASE';
OF:                                 'OF';
END_CASE:                           'END_CASE';
FOR:                                'FOR';
TO:                                 'TO';
BY:                                 'BY';
DO:                                 'DO';
END_FOR:                            'END_FOR';
EXIT:                               'EXIT';
RETURN:                             'RETURN';
WHILE:                              'WHILE';
END_WHILE:                          'END_WHILE';
REPEAT:                             'REPEAT';
UNTIL:                              'UNTIL';
END_REPEAT:                         'END_REPEAT';
TYPE:                               'TYPE';
END_TYPE:                           'END_TYPE';
ARRAY:                              'ARRAY';
STRUCT:                             'STRUCT';
END_STRUCT:                         'END_STRUCT';
OVERLAP:                            'OVERLAP';
AT:                                 'AT';
REF_TO:                             'REF_TO';
REF:                                'REF';
FUNCTION:                           'FUNCTION';
END_FUNCTION:                       'END_FUNCTION';
FUNCTION_BLOCK:                     'FUNCTION_BLOCK';
END_FUNCTION_BLOCK:                 'END_FUNCTION_BLOCK';
CLASS:                              'CLASS';
END_CLASS:                          'END_CLASS';
FINAL:                              'FINAL';
METHOD:                             'METHOD';
END_METHOD:                         'END_METHOD';
EXTENDS:                            'EXTENDS';
OVERRIDE:                           'OVERRIDE';
ABSTRACT:                           'ABSTRACT';
THIS:                               'THIS';
SUPER:                              'SUPER';
INTERFACE:                          'INTERFACE';
END_INTERFACE:                      'END_INTERFACE';
IMPLEMENTS:                         'IMPLEMENTS';
READ_WRITE:                         'READ_WRITE';
READ_ONLY:                          'READ_ONLY';
NAMESPACE:                          'NAMESPACE';
END_NAMESPACE:                      'END_NAMESPACE';

// DATA TYPES
SINT:                               'SINT';
INT:                                'INT';
DINT:                               'DINT';
LINT:                               'LINT';
USINT:                              'USINT';
UINT:                               'UINT';
UDINT:                              'UDINT';
LDINT:                              'LDINT';
ULINT:                              'ULINT';
REAL:                               'REAL';
LREAL:                              'LREAL';
TIME:                               'TIME';
DATE:                               'DATE';
TIME_OF_DAY:                        'TIME_OF_DAY' | 'TOD';
LTIME_OF_DAY:                       'LTIME_OF_DAY' | 'LTOD';
DATE_AND_TIME:                      'DATE_AND_TIME' | 'DT';
LDATE_AND_TIME:                     'LDATE_AND_TIME' 'LDT';
STRING:                             'STRING';
BOOL:                               'BOOL';
R_EDGE:                             'R_EDGE';
F_EDGE:                             'F_EDGE';
BYTE:                               'BYTE';
WORD:                               'WORD';
DWORD:                              'DWORD';
LWORD:                              'LWORD';
LTIME:                              'LTIME';
LDATE:                              'LDATE';
WSTRING:                            'WSTRING';
CHAR:                               'CHAR';
WCHAR:                              'WCHAR';

// GENERIC DATA TYPES
ANY:                                'ANY';
ANY_DERIVED:                        'ANY_DERIVED';
ANY_ELEMENTARY:                     'ANY_ELEMENTARY';
ANY_MAGNITUDE:                      'ANY_MAGNITUDE';
ANY_NUM:                            'ANY_NUM';
ANY_REAL:                           'ANY_REAL';
ANY_INT:                            'ANY_INT';
ANY_UNSIGNED:                       'ANY_UNSIGNED';
ANY_SIGNED:                         'ANY_SIGNED';
ANY_DURATION:                       'ANY_DURATION';
ANY_BIT:                            'ANY_BIT';
ANY_CHARS:                          'ANY_CHARS';
ANY_STRING:                         'ANY_STRING';
ANY_CHAR:                           'ANY_CHAR';
ANY_DATE:                           'ANY_DATE';

// COMMENTS
SINGLE_LINE_COMMENT:                '//'  InputCharacter*    -> skip;
DELIMITED_COMMENT_SLASH:            '/*'  .*? '*/'           -> skip;
DELIMITED_COMMENT_BRACKET:          '(*'  .*? '*)'           -> skip;

// SPECIAL CHARACTERS
COLON:                              ':';
SEMICOLON:                          ';';
DOT:                                '.';
COMMA:                              ',';
BRACKET_OPEN:                       '(';
BRACKET_CLOSE:                      ')';
SQUARE_BRACKET_OPEN:                '[';
SQUARE_BRACKET_CLOSE:               ']';
CURLY_BRACKET_OPEN:                 '{';
CURLY_BRACKET_CLOSE:                '}';
HASH:                               '#';
CARET:                              '^';
PERCENT:                            '%';

//OPERATORS
ASSIGN_OPERATOR:                    ':=';
ANONYMOUS_FN_OPERATOR:              '=>';

// ARITHMETIC OPERATORS
ADD_OPERATOR:                       '+';
SUBTRACT_OPERATOR:                  '-';
MULTIPLY_OPERATOR:                  '*';
DIVISION_OPERATOR:                  '/';
MODULO_OPERATOR:                    'MOD';
EXPONENT_OPERATOR:                  '**';

// RELATIONAL OPERATORS
EQUAL_OPERATOR:                     '=';
LESS_THAN_OPERATOR:                 '<';
LESS_THAN_EQUAL_OPERATOR:           '<=';
GREATER_THAN_OPERATOR:              '>';
GREATER_THAN_EQUAL_OPERATOR:        '>=';
NOT_EQUAL_OPERATOR:                 '<>';

// LOGICAL AND BITWISE OPERATORS
AND_OPERATOR:                       '&' | A N D;
OR_OPERATOR:                        'OR';
XOR_OPERATOR:                       'XOR';
NEGATION_OPERATOR:                  'NOT';

fragment A: 'A' | 'a';
fragment N: 'N' | 'n';
fragment D: 'D' | 'd';
    
// VALUES
TRUE:                               'TRUE';
FALSE:                              'FALSE';
STRING_VALUE:                       ('"' | '\'')  InputCharacter* ('"' | '\'');
NUMBER_VALUE:                       '-'? [0-9]+ ('.' [0-9]+)?;
ADDRESS:                            '%' [a-zA-Z0-9*]+ ('.' [0-9]+)?;

fragment InputCharacter:            ~[\r\n\u0085\u2028\u2029];

ID :                                [a-zA-Z0-9_]+ ;             
WS :                                [ \t\r\n]+ -> skip ; 