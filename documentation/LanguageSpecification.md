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
float ::=digit, { digit }, ".", digit, { digit } ;
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
array_element ::= identifier | function_call, "[", expression, "]" ;
```
## Comments

Comments are a free text starting with a `!` character until the end of line.

```ebnf
comment ::= "!" , { all characters - "!" }, EoL ;
```

