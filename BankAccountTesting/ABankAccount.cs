using BankAccountLib;

namespace BankAccountTesting;

public class BankAccountTesting
{
    // Equivalence Partitioning
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

    // Boundary Value Analysis
    // Constructor
    [TestCase(null, 500, TestName = "Constructor_NullAccountNumber")]
    [TestCase("", 500, TestName = "Constructor_EmptyAccountNumber")]
    [TestCase("123456789", -1, TestName = "Constructor_NegativeBalance")]
    [TestCase("123456789", 0, TestName = "Constructor_ZeroBalance")]
    [TestCase("123456789", 1, TestName = "Constructor_PositiveBalance")]
    public void Constructor_BoundaryValueAnalysis(string? accountNumber, decimal initialBalance)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            Assert.Throws<ArgumentException>(
                () => new BankAccount(accountNumber, initialBalance));
        } else if (initialBalance < 0)
        {
            Assert.Throws<ArgumentException>(
                () => new BankAccount(accountNumber, initialBalance));
        } else
        {
            var sut = new BankAccount(accountNumber, initialBalance);
            Assert.That(sut.AccountNumber, Is.EqualTo(accountNumber));
            Assert.That(sut.Balance, Is.EqualTo(initialBalance));
        }
    }

    [Test]
    public void Constructor_VerySmallPositiveValue()
    {
        string accountNumber = "123456789";
        decimal initialBalance = 1E-28M;  // 0.0000000000000000000000000001
        var sut = new BankAccount(accountNumber, initialBalance);

        Assert.That(sut.AccountNumber, Is.EqualTo(accountNumber));
        Assert.That(sut.Balance, Is.EqualTo(initialBalance));
    }

    [Test]
    public void Constructor_VeryLargePositiveValue()
    {
        string accountNumber = "123456789";
        decimal initialBalance = decimal.MaxValue; // 79228162514264337593543950335
        var sut = new BankAccount(accountNumber, initialBalance);

        Assert.That(sut.AccountNumber, Is.EqualTo(accountNumber));
        Assert.That(sut.Balance, Is.EqualTo(initialBalance));
    }

    [Test]
    public void Constructor_VerySmallNegativeValue()
    {
        string accountNumber = "123456789";
        decimal initialBalance = -1E-28M;  // -0.0000000000000000000000000001

        Assert.Throws<ArgumentException>(
                () => new BankAccount(accountNumber, initialBalance));
    }

    [Test]
    public void Constructor_VeryLargeNegativeValue()
    {
        string accountNumber = "123456789";
        decimal initialBalance = decimal.MinValue; // -79228162514264337593543950335

        Assert.Throws<ArgumentException>(
                () => new BankAccount(accountNumber, initialBalance));
    }

    private BankAccount _bankAccount;

    [SetUp]
    public void Setup()
    {
        _bankAccount = new BankAccount("123456789", 10m);
    }

    // Deposit Method
    [TestCase(-0.01, TestName = "DepositMethod_JustBelowZero")]
    [TestCase(0, TestName = "DepositMethod_Zero")]
    [TestCase(0.01, TestName = "DepositMethod_JustAboveZero")]
    public void DepositMethod_BoundaryValueAnalysis(decimal amount)
    {
        if (amount <= 0)
        {
            Assert.Throws<ArgumentException>(
                    () => _bankAccount.Deposit(amount));
        } else {
            decimal initialBalance = _bankAccount.Balance;
            _bankAccount.Deposit(amount);
            Assert.That(_bankAccount.Balance, Is.EqualTo(initialBalance + amount));
        }
    }

    [Test]
    public void DepositMethod_VeryLargePositiveValue()
    {
        decimal amount = decimal.MaxValue - _bankAccount.Balance;
        decimal initialBalance = _bankAccount.Balance;
        _bankAccount.Deposit(amount);
        Assert.That(_bankAccount.Balance, Is.EqualTo(initialBalance + amount));
    }

    [Test]
    public void DepositMethod_VeryLargeNegativeValue()
    {
        decimal amount = decimal.MinValue;
        Assert.Throws<ArgumentException>(
                    () => _bankAccount.Deposit(amount));
    }

    // Withdraw Method
    [TestCase(-0.01, TestName = "WithdrawMethod_JustBelowZero")]
    [TestCase(0, TestName = "WithdrawMethod_Zero")]
    [TestCase(0.01, TestName = "WithdrawMethod_JustAboveZero")]
    [TestCase(9.99, TestName = "WithdrawMethod_JustBelowBalance")]
    [TestCase(10, TestName = "WithdrawMethod_EqualsBalance")]
    [TestCase(10.01, TestName = "WithdrawMethod_JustAboveBalance")]
    public void WithdrawMethod_BoundaryValueAnalysis(decimal amount)
    {
        if (amount <= 0)
        {
            Assert.Throws<ArgumentException>(
                    () => _bankAccount.Withdraw(amount));
        } else if (amount > _bankAccount.Balance)
        {
            Assert.Throws<InvalidOperationException>(
                    () => _bankAccount.Withdraw(amount));
        } else {
            decimal initialBalance = _bankAccount.Balance;
            _bankAccount.Withdraw(amount);
            Assert.That(_bankAccount.Balance, Is.EqualTo(initialBalance - amount));
        }
    }

    [Test]
    public void WithdrawMethod_VeryLargePositiveValue()
    {
        decimal amount = decimal.MaxValue;
        Assert.Throws<InvalidOperationException>(
                    () => _bankAccount.Withdraw(amount));
    }

    [Test]
    public void WithdrawMethod_VeryLargeNegativeValue()
    {
        decimal amount = decimal.MinValue;
        Assert.Throws<ArgumentException>(
                    () => _bankAccount.Withdraw(amount));
    }

    // GetAccountStatus Method
    [TestCase(99.99, TestName = "GetAccountStatus_JustBelowLowThreshold")]
    [TestCase(100, TestName = "GetAccountStatus_MinNormalThreshold")]
    [TestCase(100.01, TestName = "GetAccountStatus_JustAboveMinNormalThreshold")]
    [TestCase(999.99, TestName = "GetAccountStatus_JustBelowHighThreshold")]
    [TestCase(1000, TestName = "GetAccountStatus_MinHighThreshold")]
    [TestCase(1000.01, TestName = "GetAccountStatus_JustAboveMinHighThreshold")]
    public void GetAccountStatus_BoundaryValueAnalysis(decimal Balance)
    {
        var sut = new BankAccount("123456789", Balance);
        string actualStatus = sut.GetAccountStatus();
        string expectedStatus;
        
        if (Balance < 100)
            expectedStatus = "Low";
        else if (Balance < 1000)
            expectedStatus = "Normal";
        else
            expectedStatus = "High";

        Assert.That(actualStatus, Is.EqualTo(expectedStatus));
    }

    // TransferTo Method
    [Test]
    public void TransferTo_BoundaryValueAnalysis_EntireBalance()
    {
        BankAccount recipient = new BankAccount("123456789", 100m);
        BankAccount sut = new BankAccount("987654321", 100m);
        decimal amount = 100m;

        sut.TransferTo(recipient, amount);

        Assert.That(sut.Balance, Is.EqualTo(0));
        Assert.That(recipient.Balance, Is.EqualTo(200));
    }

    [Test]
    public void TransferTo_BoundaryValueAnalysis_SmallAmount()
    {
        BankAccount recipient = new BankAccount("123456789", 100m);
        BankAccount sut = new BankAccount("987654321", 100m);
        decimal amount = 0.01m;

        sut.TransferTo(recipient, amount);

        Assert.That(sut.Balance, Is.EqualTo(99.99));
        Assert.That(recipient.Balance, Is.EqualTo(100.01));
    }

    [Test]
    public void TransferTo_BoundaryValueAnalysis_HighRecipientBalance()
    {
        decimal amount = 10m;
        BankAccount recipient = new BankAccount("123456789", decimal.MaxValue - amount);
        BankAccount sut = new BankAccount("987654321", 100m);

        sut.TransferTo(recipient, amount);

        Assert.That(sut.Balance, Is.EqualTo(90));
        Assert.That(recipient.Balance, Is.EqualTo(decimal.MaxValue));
    }

    [Test]
    public void TransferTo_BoundaryValueAnalysis_ZeroAmount()
    {
        BankAccount sut = new BankAccount("987654321", 100m);
        decimal amount = 0m;

        Assert.Throws<ArgumentException>(
                    () => sut.TransferTo(_bankAccount, amount));
    }

    [Test]
    public void TransferTo_BoundaryValueAnalysis_JustBelowZeroAmount()
    {
        BankAccount sut = new BankAccount("987654321", 100m);
        decimal amount = -0.01m;

        Assert.Throws<ArgumentException>(
                    () => sut.TransferTo(_bankAccount, amount));
    }

    [Test]
    public void TransferTo_BoundaryValueAnalysis_AllAmount()
    {
        BankAccount recipient = new BankAccount("123456789", 100m);
        BankAccount sut = new BankAccount("987654321", 100m);
        decimal amount = 100m;

        sut.TransferTo(recipient, amount);

        Assert.That(sut.Balance, Is.EqualTo(0));
        Assert.That(recipient.Balance, Is.EqualTo(200));
    }

    [Test]
    public void TransferTo_BoundaryValueAnalysis_AmountGreaterThanBalance()
    {
        BankAccount sut = new BankAccount("987654321", 100m);
        decimal amount = 200m;

        Assert.Throws<InvalidOperationException>(
                    () => sut.TransferTo(_bankAccount, amount));
    }

    [Test, Combinatorial]
    public void CombinatorialTesting([Values(1000, 1)] decimal initialBalance,
                                     [Values(10, 100, 500)] decimal depositAmount,
                                     [Values(50, 100, 5)] decimal withdrawAmount)
    {
        var sut = new BankAccount("123456789", initialBalance);
        sut.Deposit(depositAmount);
        sut.Withdraw(withdrawAmount);

        string actualStatus = sut.GetAccountStatus();
        string expectedStatus;
        decimal netBalance = initialBalance + depositAmount - withdrawAmount;

        if (netBalance < 100)
            expectedStatus = "Low";
        else if (netBalance < 1000)
            expectedStatus = "Normal";
        else
            expectedStatus = "High";

        Assert.That(sut.Balance, Is.EqualTo(netBalance));
        Assert.That(actualStatus, Is.EqualTo(expectedStatus));
    }

    [Test, Pairwise]
    public void PairwiseTesting([Values(1000, 1)] decimal initialBalance,
                                     [Values(10, 100, 500)] decimal depositAmount,
                                     [Values(50, 100, 5)] decimal withdrawAmount)
    {
        var sut = new BankAccount("123456789", initialBalance);
        sut.Deposit(depositAmount);
        sut.Withdraw(withdrawAmount);

        string actualStatus = sut.GetAccountStatus();
        string expectedStatus;
        decimal netBalance = initialBalance + depositAmount - withdrawAmount;

        if (netBalance < 100)
            expectedStatus = "Low";
        else if (netBalance < 1000)
            expectedStatus = "Normal";
        else
            expectedStatus = "High";

        Assert.That(sut.Balance, Is.EqualTo(netBalance));
        Assert.That(actualStatus, Is.EqualTo(expectedStatus));
    }
}