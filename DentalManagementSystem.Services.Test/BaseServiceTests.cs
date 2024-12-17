namespace DentalManagementSystem.Services.Test;

using DentalManagementSystem.Services.Data;

public class BaseServiceTests
{
    private BaseService baseService;

    [SetUp]
    public void Setup()
    {
        this.baseService = new BaseService();
    }

    [Test]
    public async Task IsGuidValid()
    {
        string id = "a3c8fbd1-774e-4a62-9e5c-4d8c46b567af";
        Guid guid = Guid.Empty;

        bool result = this.baseService.IsGuidValid(id, ref guid);

        Assert.IsTrue(result);
        Assert.AreEqual(Guid.Parse("a3c8fbd1-774e-4a62-9e5c-4d8c46b567af"), guid);
    }

    [Test]
    public async Task IsGuidNullValid()
    {
        string id = null;
        Guid guid = Guid.Empty;

        bool result = this.baseService.IsGuidValid(id, ref guid);

        Assert.IsFalse(result);
        Assert.AreEqual(Guid.Empty, guid);
    }

    [Test]
    public async Task IsGuidEmptyValidNegative()
    {
        string id = String.Empty;
        Guid guid = Guid.Empty;

        bool result = this.baseService.IsGuidValid(id, ref guid);

        Assert.IsFalse(result);
        Assert.AreEqual(Guid.Empty, guid);
    }

    [Test]
    public async Task IsGuidInvalidValidNegative()
    {
        string id = "qwerty";
        Guid guid = Guid.Empty;

        bool result = this.baseService.IsGuidValid(id, ref guid);

        Assert.IsFalse(result);
        Assert.AreEqual(Guid.Empty, guid);
    }
}