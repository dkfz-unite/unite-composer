using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unite.Composer.Search.Engine.Extensions;

namespace Unite.Composer.Tests;

[TestClass]
public class ExpressionExtensionsTests
{
    [TestMethod("'Join' should combine two expressions")]
    public void JoinTest()
    {
        var aModel1 = new SubModelA { BModel = new SubModelB { Value = 1 } };
        var aModel2 = new SubModelA { BModel = new SubModelB { Value = 2 } };
        var models = new Model[]
        {
            new Model { AModel = aModel1 },
            new Model { AModel = aModel2 }
        };

        Expression<Func<Model, SubModelB>> path = model => model.AModel.BModel;

        var data = models.Select(path.Compile()).ToArray();
        var values = models.Select(path.Join(bModel => bModel.Value).Compile()).ToArray();
        var ones = models.Where(path.Join(bModel => bModel.Value == 1).Compile()).ToArray();

        Assert.IsNotNull(data);
        Assert.AreEqual(2, data.Length);
        Assert.AreEqual(3, data[0].Value + data[1].Value);

        Assert.IsNotNull(values);
        Assert.AreEqual(2, values.Length);
        Assert.AreEqual(3, values[0] + values[1]);

        Assert.IsNotNull(ones);
        Assert.AreEqual(1, ones.Length);
        Assert.AreEqual(1, ones[0].AModel.BModel.Value);
    }


    private class Model
    {
        public SubModelA AModel { get; set; }
    }

    private class SubModelA
    {
        public SubModelB BModel { get; set; }
    }

    private class SubModelB
    {
        public int Value { get; set; }
    }
}
