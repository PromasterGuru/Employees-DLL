using System;
using System.Collections.Generic;
using System.IO;

namespace Library
{

    public class Employees
    {
        StreamReader streamReader;
        String line;
        int ceos = 0;
        string ceoId;
        Dictionary<string, Employee> store;
        public Employees(string csv)
        {
            streamReader = new StreamReader(csv);
            store = new Dictionary<string, Employee>();
            try
            {
                while ((line = streamReader.ReadLine()) != null)
                {
                    String[] data = line.Split(',');

                    //Ensure there is only one CEO
                    if(string.IsNullOrEmpty(data[1]) && ceos < 1)
                    {
                        ceoId = data[1];
                        ceos++;
                    }
                    else
                    {
                        throw new InvalidDataException("Invalid string, there must be only one CEO in the organization...");
                    }

                    //Ensure salary is a valid integer number
                    int sal = 0;
                    if(int.TryParse(data[2], out sal))
                    {
                        store.Add(data[0], new Employee(data[0], data[1], sal));
                    }
                    else
                    {
                        throw new InvalidDataException("Invalid string, salary must be a valid integer number");
                    }

                    //Validate circular reference and all managers are mployees
                    foreach(KeyValuePair<string, Employee> kvp in store)
                    {
                        if (!string.IsNullOrEmpty(kvp.Value.Manager))
                        {

                            //Ensure that no manager is not an employee
                            if (!store.ContainsKey(kvp.Value.Manager))
                            {
                                throw new InvalidDataException("Invalid string, every manager must be an employee");
                            }

                            //Ensure there is no circular reference
                            foreach (Employee emp in store.Values)
                            {
                                if (kvp.Key.Equals(emp.Manager) && kvp.Value.Equals(emp.Id))
                                {
                                    throw new InvalidDataException("Invalid string, there must be no circular reference");
                                }
                            }

                        }
                    }                  
                    
                }
            }
            catch(Exception ex)
            {
                throw new InvalidDataException("Invalid string, Employee cannot report to more than one manager.");
            }
    }

        //Instance method to calculate salary budget from a manager
        public long SalaryBudget(string manager)
        {
            var employee = store[manager];
            long salary = 0;

            foreach (var emp in store)
            {
                salary += employee.Salary;
                foreach (var child in store.Values)
                {
                    if (employee.Id == child.Id)
                    {
                        salary += child.Salary;
                    }
                }
                employee = store[employee.Id];
            }
            return 0;
        }

        public class Employee : IComparable<Employee>
        {
            public string Id { get; set; }
            public string Manager { get; set; }
            public int Salary { get; set; }

            public Employee(string empId, string manager, int salary)
            {
                Id = empId;
                Manager = manager;
                Salary = salary;
            }
            public int CompareTo(Employee other)
            {
                if (other == null)
                    return -1;
                return string.Compare(Id, other.Id, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
