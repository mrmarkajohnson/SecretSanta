using Global.Abstractions.Extensions;
using Xunit;

namespace UnitTests.ExtensionTests;

public class TestStringExtensions
{
    [Fact]
    public void TestListDisplay()
    {
        var list = new List<string>() { "apples", "oranges", "pears", "grapes" };

        string simpleAnd = list.DisplayList();
        Assert.Equal("apples, oranges, pears and grapes", simpleAnd);

        string simpleOr = list.DisplayList(or: true);
        Assert.Equal("apples, oranges, pears or grapes", simpleOr);

        string oxfordAnd = list.DisplayList(oxfordComma : true);
        Assert.Equal("apples, oranges, pears, and grapes", oxfordAnd);

        string oxfordOr = list.DisplayList(or: true, oxfordComma: true);
        Assert.Equal("apples, oranges, pears, or grapes", oxfordOr);
    }
}
