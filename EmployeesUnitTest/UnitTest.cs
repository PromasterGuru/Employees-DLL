using Microsoft.VisualStudio.TestTools.UnitTesting;
using Library;

namespace EmployeesUnitTest
{
    [TestClass]
    public class EployeesLibraryUnitests
    {
        Employees employee;

        [TestMethod]
        public void Test_Salaries_Must_Be_Valid_Integers()
        {
            try
            {
                employee = new Employees(@"EmployeeTestData1.csv");
            }
            catch (System.IO.InvalidDataException e)
            {
                StringAssert.Contains(e.Message, "Invalid string, salary must be a valid integer number");
            }
        }

        [TestMethod]
        public void Test_One_Employee_Reports_To_One_Manager()
        {
            try
            {
                employee = new Employees(@"EmployeeTestData2.csv");
            }
            catch (System.ArgumentException e)
            {
                StringAssert.Contains(e.Message, "Invalid string, Employee cannot report to more than one manager.");
            }
        }

        [TestMethod]
        public void Test_There_Is_Only_One_CEO()
        {
            try
            {
                employee = new Employees(@"EmployeeTestData3.csv");
            }
            catch (System.IO.InvalidDataException e)
            {
                StringAssert.Contains(e.Message, "Invalid string, there must be only one CEO in the organization...");
            }
        }

        [TestMethod]
        public void Test_No_Circular_Reference()
        {
            try
            {
                employee = new Employees(@"EmployeeTestData4.csv");
            }
            catch (System.IO.InvalidDataException e)
            {
                StringAssert.Contains(e.Message, "Invalid string, there must be no circular reference");
            }
        }

        [TestMethod]
        public void Test_No_Manager_Is_Not_an_Employee()
        {
            try
            {
                employee = new Employees(@"EmployeeTestData5.csv");
            }
            catch (System.IO.InvalidDataException e)
            {
                StringAssert.Contains(e.Message, "Invalid string, every manager must be an employee");
            }
        }

        [TestMethod]
        public void Test_Can_View_Manager_Budget()
        {
            employee = new Employees(@"EmployeeTestData6.csv");
            Assert.AreEqual(3800, employee.SalaryBudget("Employee1"));
            Assert.AreEqual(1800, employee.SalaryBudget("Employee2"));
            Assert.AreEqual(500, employee.SalaryBudget("Employee3"));
            Assert.AreEqual(500, employee.SalaryBudget("Employee4"));
            Assert.AreEqual(500, employee.SalaryBudget("Employee5"));
            Assert.AreEqual(500, employee.SalaryBudget("Employee6"));
        }
    }
}
