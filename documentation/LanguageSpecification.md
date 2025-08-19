# Language syntax specification

## Literals

Literals are the building blocks of expressions and statements in the language.

```ebnf
literal ::= number | string | boolean | identifier ;

```

### Numbers

Both integer and floating point numbers are supported. For example `0`, `123`, `123.0`.

```ebnf
number ::= integer | float ;
integer ::= digit, { digit } ;
float ::= digit, { digit }, ".", digit, { digit } ;
```

### Strings

String is a free text on a single line enclosed in quotes. 

```ebnf
string ::= '"' , { all characters - '"' }, '"' ;
```

### Boolean values

```ebnf
boolean ::= "True" | "False" ;
```

### Identifiers

Identifier is a single word, starting from a letter from the English alphabet and letters or digits. They can't be a keyword, defined later

```ebnf
identifier ::= letter, { letter | digit } ;
```

## Expressions

Are code blocks that can be evaluated and return a value; for simplicity whitespace characters are omitted in forms.   

### Unary expressions

#### Negation

TODO: This is not implemented yet

```ebnf
negation ::= "-" , number | identifier | grouping | array_element | function_call ;
```

#### Logical not

```ebnf
not ::= "not", boolean | identifier | grouping | array_element | function_call ;
```

### Array element access

```ebnf
array_element ::= identifier | function_call | grouping, "[", expression, "]" ;
```

### Binary expressions

#### Arithmetic expressions

```ebnf
addition ::= expression, "+", expression ;
subtraction ::= expression, "-", expression ;
multiplication ::= expression, "*", expression ;
division ::= expression, "/", expression ;
```

#### Logical expressions

```ebnf
conjunction ::= expression, "and", expression ;
disjunction ::= expression, "or", expression ;
exclusiveDisjunction ::= expression, "xor", expression ;
```

#### Comparison expressions

> [!NOTE]
> For equality operator there is a difference for this implementation to make it easier to implement and context independent. Instead of `=` equality operator is C-like - `==`

```ebnf
equality ::= expression, "==", expression ;
inequality ::= expression, "!=", expression ;
lessThan ::= expression, "<", expression ;
greaterThan ::= expression, ">", expression ;
lessOrEqualThan ::= expression, "<=", expression ;
greaterOrEqualThan ::= expression, ">=", expression ;
```

## Comments

Comments are a free text starting with a `!` character until the end of line.

```ebnf
comment ::= "!" , { all characters - "!" }, EoL ;
```

## Statements

### Program

Main statement to mark the beginning of a program in BASIC

```ebnf
program ::= "program", identifier, { comment }, EoL, (statement | comment)*, "end" ; 
```

### Variable assignment

```ebnf
assignment ::= identifier "=", expression ;
```

### Variable declaration

```ebnf
declaration ::= "let", assignment ; 
```

### Flow of control

#### Conditional statement

Conditional statements can have only a single statement for each branch

```ebnf
condition ::= "if", expression, "then", EoL, statement, EoL, (elseIf)*, {else} ;
elseIf ::= "elseIf", expression, "then" , EoL, statement ;
else ::= "else", EoL, statement;
```

#### While loop

```ebnf
whileLoop ::= "while", expression, EoL, (statement)*, "loop" ;
```

#### Do Until loop

```ebnf
doUntilLoop ::= "do", EoL, (statement)*, EoL, "until", expression ;
```

#### For loop

```ebnf
forLoop ::= "for", assignment, "to", expression, { "step", expression }, EoL, (statement)*, EoL, "next";
```

#### Goto and label

```ebnf
label ::= identifier, ":";
goto ::= "goto", label | integer;
```

#### Console IO

> [!NOTE]
> Input is missing some features, like adding question mark to provided string text by placing semicolon after it, or moving cursor position to a specific coordinate with the `TAB(x, y)` function

```ebnf
print ::= "print", expression, ("," | ";", expression)* ;
input ::= "input", (inputExpression)+ ;
inputExpression ::= { string }, identifier, ("," | ";", identifier)* ;
```

### Functions

```ebnf
callFunction ::= identifier, "(", (argumentList)* ")" ;
defineFunction ::= "def", identifier, "(", (argumentList)* ")", EoL ;
argumentList ::= expression, ("," | ";", expression)* ;
```