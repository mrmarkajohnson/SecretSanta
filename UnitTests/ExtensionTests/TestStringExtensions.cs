using Global.Extensions.System;
using Xunit;

namespace UnitTests.ExtensionTests;

public sealed class TestStringExtensions
{
    [Fact]
    public void TestListDisplay()
    {
        var list = new List<string>() { "apples", "oranges", "pears", "grapes" };

        string simpleAnd = list.DisplayList();
        Assert.Equal("apples, oranges, pears and grapes", simpleAnd);

        string simpleOr = list.DisplayList(or: true);
        Assert.Equal("apples, oranges, pears or grapes", simpleOr);

        string oxfordAnd = list.DisplayList(oxfordComma: true);
        Assert.Equal("apples, oranges, pears, and grapes", oxfordAnd);

        string oxfordOr = list.DisplayList(or: true, oxfordComma: true);
        Assert.Equal("apples, oranges, pears, or grapes", oxfordOr);
    }

    [Fact]
    public void TestSplitPascaleCase()
    {
        string a = "SomethingLikeThis";
        string a2 = a.SplitPascalCase();
        Assert.Equal("Something Like This", a2);

        string b = "ARTHandle";
        string b2 = b.SplitPascalCase();
        Assert.Equal("ART Handle", b2);

        string c = "HandleART";
        string c2 = c.SplitPascalCase();
        Assert.Equal("Handle ART", c2);

        string d = "Tomato";
        string d2 = d.SplitPascalCase();
        Assert.Equal("Tomato", d2);

        string e = "AStringWithARTInIt";
        string e2 = e.SplitPascalCase();
        Assert.Equal("A String With ART In It", e2);
    }
}
