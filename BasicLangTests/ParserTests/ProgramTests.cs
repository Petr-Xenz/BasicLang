namespace BasicLang.Tests.ParserTests;

[TestClass]
public class ProgramTests
{
        [TestMethod]
    public void ProgramStatement()
    {
        var source = """
                        program foo
                            goto 10
                        end
                     """;
        var tokens = new Lexer(source).Lex();

        var tree = new Parser(tokens, source).Parse();
        var root = tree.RootStatement as ProgramStatement;
        Assert.IsNotNull(root, "root");

        var gotoStatement = root.Children.Single() as GotoStatement;
        Assert.IsNotNull(gotoStatement, "goto");
        Assert.AreEqual("10", gotoStatement.LineValue);
    }

    [TestMethod]
    [DynamicData(nameof(ValidSourceCode), DynamicDataSourceType.Method)]
    public void ParsesValidSourceWithoutErrors(string source)
    {
        var tokens = new Lexer(source).Lex().ToArray();

        var tree = new Parser(tokens, source).Parse();

        Assert.IsFalse(tree.Errors.Any());
    }


    private static IEnumerable<object[]> ValidSourceCode()
    {
        yield return [
            """
            PROGRAM product
            ! taken from Chapter 2 of Gould & Tobochnik
            LET m = 2                         ! mass in kilograms
            LET a = 4                         ! acceleration in mks units
            LET force = m*a                   ! force in Newtons
            PRINT force
            END
            """,
        ];
        yield return [
            """
            PROGRAM product2
            INPUT m
            INPUT "acceleration a (mks units) = " a
            LET force = m*a                       ! force in Newton's
            PRINT "force (in Newtons) ="; force
            END
            """
        ];
    }
}