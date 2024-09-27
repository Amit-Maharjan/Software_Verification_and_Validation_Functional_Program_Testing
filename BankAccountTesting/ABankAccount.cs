using BankAccountLib;

namespace BankAccountTesting;

public class BankAccountTesting
{
    // Constructor
    [TestCase("123456789", 500, TestName = "ShouldSetAccountNumberAndBalanceWhenConstructed")]
    public void ShouldSetAccountNumberAndBalanceWhenConstructed(string accountNumber, decimal initialBalance)
    {
        var sut = new BankAccount(accountNumber, initialBalance);

        Assert.That(sut.AccountNumber, Is.EqualTo(accountNumber));
        Assert.That(sut.Balance, Is.EqualTo(initialBalance));
    }

    [TestCase(null, 500, TestName = "ShouldThrowArgumentExceptionWhenConstructedForAccountNumberNull")]
    [TestCase("", 500, TestName = "ShouldThrowArgumentExceptionWhenConstructedForAccountNumberEmpty")]
    [TestCase(null, -500, TestName = "ShouldThrowArgumentExceptionWhenConstructedForAccountNumberNullAndNegativeInitialBalance")]
    [TestCase("123456789", -500, TestName = "ShouldThrowArgumentExceptionWhenConstructedForNegativeInitialBalance")]
    [TestCase("", -500, TestName = "ShouldThrowsArgumentExceptionWhenConstructedForAccountNumberEmptyAndNegativeInitialBalance")]
    public void ShouldThrowArgumentExceptionWhenConstructed(string? accountNumber, decimal initialBalance)
    {
        try
        {
            var sut = new BankAccount(accountNumber, initialBalance);
        }
        catch (ArgumentException ex) {
            Assert.Pass();   
        }

        Assert.Fail();
    }

    // Deposit Method
    [TestCase(10, TestName = "ShouldIncreaseBalanceAfterDeposit")]
    public void ShouldIncreaseBalanceAfterDeposit(decimal amount)
    {
        var sut = new BankAccount("123456789", 10m);
        sut.Deposit(amount);

        Assert.That(sut.Balance, Is.EqualTo(20));
    }

    [TestCase(-10, TestName = "ShouldThrowArgumentExceptionWhenDeposit")]
    public void ShouldThrowArgumentExceptionWhenDeposit(decimal amount)
    {
        try
        {
            var sut = new BankAccount("123456789", 10m);
            sut.Deposit(amount);
        }
        catch (ArgumentException ex)
        {
            Assert.Pass();
        }

        Assert.Fail();
    }

    // Withdraw Method
    [TestCase(10, TestName = "ShouldDecreaseBalanceAfterWithdraw")]
    public void ShouldDecreaseBalanceAfterWithdraw(decimal amount)
    {
        var sut = new BankAccount("123456789", 100m);
        sut.Withdraw(amount);

        Assert.That(sut.Balance, Is.EqualTo(90));
    }

    [TestCase(-10, TestName = "ShouldThrowArgumentExceptionWhenWithdrawForNegativeAmount")]
    public void ShouldThrowArgumentExceptionWhenWithdraw(decimal amount)
    {
        try
        {
            var sut = new BankAccount("123456789", 100m);
            sut.Withdraw(amount);
        }
        catch (ArgumentException ex)
        {
            Assert.Pass();
        }

        Assert.Fail();
    }

    [TestCase(150, TestName = "ShouldThrowInvalidOperationExceptionWhenWithdrawForAmountGreaterThanBalance")]
    public void ShouldThrowInvalidOperationExceptionWhenWithdraw(decimal amount)
    {
        try
        {
            var sut = new BankAccount("123456789", 100m);
            sut.Withdraw(amount);
        }
        catch (InvalidOperationException ex)
        {
            Assert.Pass();
        }

        Assert.Fail();
    }

    // GetAccountStatus Method
    [Test]
    public void ShouldReturnAccountStatusAsLow()
    {
        var sut = new BankAccount("123456789", 10);
        string result = sut.GetAccountStatus();

        Assert.That(result, Is.EqualTo("Low"));
    }

    [Test]
    public void ShouldReturnAccountStatusAsNormal()
    {
        var sut = new BankAccount("123456789", 150);
        string result = sut.GetAccountStatus();

        Assert.That(result, Is.EqualTo("Normal"));
    }

    [Test]
    public void ShouldReturnAccountStatusAsHigh()
    {
        var sut = new BankAccount("123456789", 2000);
        string result = sut.GetAccountStatus();

        Assert.That(result, Is.EqualTo("High"));
    }

    // TransferTo Method
    [Test]
    public void ShouldDecreaseBalanceAndIncreaseRecipientBalanceAfterTransfer()
    {
        BankAccount recipient = new BankAccount("1", 10m);
        decimal amount = 50m;
        var sut = new BankAccount("123456789", 100m);

        sut.TransferTo(recipient, amount);

        Assert.That(sut.Balance, Is.EqualTo(50));
        Assert.That(recipient.Balance, Is.EqualTo(60));
    }

    [Test]
    public void ShouldThrowArgumentNullExceptionWhenTransferForPositiveAmount()
    {
        BankAccount recipient = null;
        decimal amount = 50m;
        var sut = new BankAccount("123456789", 100m);

        try
        {
            sut.TransferTo(recipient, amount);
        }
        catch (ArgumentNullException ex)
        {
            Assert.Pass();
        }

        Assert.Fail();
    }

    [Test]
    public void ShouldThrowArgumentNullExceptionWhenTransferForNegativeAmount()
    {
        BankAccount recipient = null;
        decimal amount = -50m;
        var sut = new BankAccount("123456789", 100m);

        try
        {
            sut.TransferTo(recipient, amount);
        }
        catch (ArgumentNullException ex)
        {
            Assert.Pass();
        }

        Assert.Fail();
    }

    [Test]
    public void ShouldThrowArgumentNullExceptionWhenTransferForAmountGreaterThanBalance()
    {
        BankAccount recipient = null;
        decimal amount = 500m;
        var sut = new BankAccount("123456789", 100m);

        try
        {
            sut.TransferTo(recipient, amount);
        }
        catch (ArgumentNullException ex)
        {
            Assert.Pass();
        }

        Assert.Fail();
    }

    [Test]
    public void ShouldThrowArgumentExceptionWhenTransfer()
    {
        BankAccount recipient = new BankAccount("1", 10m);
        decimal amount = -50m;
        var sut = new BankAccount("123456789", 100m);

        try
        {
            sut.TransferTo(recipient, amount);
        }
        catch (ArgumentException ex)
        {
            Assert.Pass();
        }

        Assert.Fail();
    }

    [Test]
    public void ShouldThrowInvalidOperationExceptionWhenTransfer()
    {
        BankAccount recipient = new BankAccount("1", 10m);
        decimal amount = 500m;
        var sut = new BankAccount("123456789", 100m);

        try
        {
            sut.TransferTo(recipient, amount);
        }
        catch (InvalidOperationException ex)
        {
            Assert.Pass();
        }

        Assert.Fail();
    }
}